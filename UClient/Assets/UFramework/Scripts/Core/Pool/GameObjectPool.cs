// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020-06-27 16:40:45
// * @Description:
// --------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UFramework.Core
{
    public class GoPoolIdentity : MonoBehaviour
    {
        public string id;
        public bool isRecycled;
        public void Recycle() => GoPool.Instance.Recycle(this);
    }

    public class GoPoolUnit : IPoolBehaviour
    {
        public bool isRecycled { get; set; }
        public string id { get; private set; }
        public GameObject prefab { get; private set; }
        public Transform transform { get; private set; }

        /// <summary>
        /// 平均分配间隔时间，用于优化释放对象池
        /// </summary>
        public float averageIntervalTime
        {
            get
            {
                var lastInterval = 0f;
                if (_lastTime > 0f)
                    lastInterval = Time.realtimeSinceStartup - _lastTime;
                return (_intervalTimes.Sum() + lastInterval) / (_intervalTimes.Count + 1);
            }
        }

        /// <summary>
        /// 完全释放
        /// </summary>
        public bool isAcquittal => _allocateCount == _recycleCount;

        /// <summary>
        /// 是否自动卸载
        /// </summary>
        public bool isAutoUnload { get; private set; }

        private int _minCount;

        private int _allocateCount;
        private int _recycleCount;

        private AssetRequest _assetRequest;

        private Stack<GoPoolIdentity> _stacks = new Stack<GoPoolIdentity>();
        private List<float> _intervalTimes = new List<float>();
        private float _lastTime;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="resPath">资源路径</param>
        /// <param name="minCount">池最小数量</param>
        /// <param name="autoUnload">是否自动回收卸载</param>
        /// <returns></returns>
        public static GoPoolUnit Allocate(Transform trans, string resPath, int minCount, bool autoUnload)
        {
            return ObjectPool<GoPoolUnit>.Instance.Allocate().Builder(trans, resPath, minCount, autoUnload);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="resPath">资源路径</param>
        /// <param name="targetPrefab">目标资源</param>
        /// <param name="minCount">池最小数量</param>
        /// <param name="autoUnload">是否自动回收卸载</param>
        /// <returns></returns>
        public static GoPoolUnit Allocate(Transform trans, string resPath, GameObject targetPrefab, int minCount,
            bool autoUnload)
        {
            return ObjectPool<GoPoolUnit>.Instance.Allocate()
                .Builder(trans, resPath, targetPrefab, minCount, autoUnload);
        }

        private GoPoolUnit Builder(Transform trans, string resPath, int minCount, bool autoUnload)
        {
            _assetRequest = Assets.LoadAsset(resPath, typeof(GameObject));
            prefab = _assetRequest.asset as GameObject;

            return Builder(trans, resPath, prefab, minCount, autoUnload);
        }

        private GoPoolUnit Builder(Transform trans, string resPath, GameObject targetPrefab, int minCount,
            bool autoUnload)
        {
            id = resPath;
            prefab = targetPrefab;
            transform = trans;
            _minCount = minCount;
            isAutoUnload = autoUnload;
            _allocateCount = 0;
            _recycleCount = 0;
            return this;
        }

        public GoPoolIdentity AllocateGameObject(Transform parent)
        {
            GoPoolIdentity poolIdentity = null;
            if (_stacks.Count > 0)
            {
                poolIdentity = _stacks.Pop();
                poolIdentity.transform.SetParent(parent);
            }
            else
            {
                var newGo = Object.Instantiate<GameObject>(prefab, parent, true);
                poolIdentity = newGo.AddComponent<GoPoolIdentity>();
                poolIdentity.id = id;
            }

            poolIdentity.isRecycled = false;
            poolIdentity.gameObject.SetActive(true);
            _allocateCount++;

            // 间隔时间
            float intervalTime = 0;
            if (_lastTime > 0)
                intervalTime = Time.realtimeSinceStartup - _lastTime;
            _lastTime = Time.realtimeSinceStartup;
            _intervalTimes.Add(intervalTime);

            return poolIdentity;
        }

        public bool RecycleGameObject(GoPoolIdentity poolIdentity)
        {
            if (poolIdentity == null || poolIdentity.isRecycled)
                return false;

            poolIdentity.isRecycled = true;
            poolIdentity.gameObject.SetActive(false);
            poolIdentity.gameObject.transform.SetParent(transform);

            _stacks.Push(poolIdentity);
            _recycleCount++;

            return true;
        }

        public void Recycle()
        {
            ObjectPool<GoPoolUnit>.Instance.Recycle(this);
        }

        public void OnRecycle()
        {
            while (_stacks.Count > 0)
            {
                var poolIdentity = _stacks.Pop();
                poolIdentity.transform.SetParent(null);
                Object.Destroy(poolIdentity.gameObject);
            }

            _allocateCount = 0;
            _recycleCount = 0;

            transform.SetParent(null);
            Object.Destroy(transform.gameObject);
            transform = null;

            prefab = null;

            _assetRequest?.Unload();
            _assetRequest = null;

            _intervalTimes.Clear();
        }

        public void CheckOptimize()
        {
            var num = Mathf.Max((_allocateCount - _recycleCount) / 2, _minCount);
            if (_stacks.Count <= num)
                num = Mathf.Max(_stacks.Count - 1, _minCount);

            while (_stacks.Count > num)
            {
                _allocateCount--;
                _recycleCount--;

                var poolIdentity = _stacks.Pop();
                poolIdentity.transform.SetParent(null);
                Object.Destroy(poolIdentity.gameObject);
            }
        }
    }

    [MonoSingletonPath("UFramework/GoPool")]
    public class GoPool : MonoSingleton<GoPool>
    {
        /// <summary>
        /// 优化间隔时间
        /// </summary>
        private static float optimizeIntervalTime;

        /// <summary>
        /// 自动卸载时间阈值
        /// </summary>
        private static float autoUnloadThresholdValue;

        readonly Dictionary<string, GoPoolUnit> unitDictionary = new Dictionary<string, GoPoolUnit>();
        readonly List<string> removes = new List<string>();

        private float _checkTime;
        private Transform _cacheTransform;

        protected override void OnSingletonStart()
        {
            _cacheTransform = transform;
            var appconfig = Serializer<AppConfig>.Instance;
            optimizeIntervalTime = (float) appconfig.optimizeIntervalTime;
            autoUnloadThresholdValue = (float) appconfig.autoUnloadThresholdValue;
        }

        /// <summary>
        /// 创建对象池
        /// </summary>
        /// <param name="resPath"></param>
        /// <param name="prefab"></param>
        /// <param name="minCount"></param>
        public GoPoolUnit CreatePool(string resPath, GameObject prefab, int minCount = 5, bool autoUnload = true)
        {
            if (!unitDictionary.TryGetValue(resPath, out var unit))
            {
                var unitGo = new GameObject(resPath);
                unitGo.transform.SetParent(_cacheTransform);

                unit = GoPoolUnit.Allocate(unitGo.transform, resPath, prefab, minCount, autoUnload);
                unitDictionary.Add(resPath, unit);
                return unit;
            }

            return null;
        }

        /// <summary>
        /// 创建对象池
        /// </summary>
        /// <param name="resPath"></param>
        /// <param name="minCount"></param>
        public GoPoolUnit CreatePool(string resPath, int minCount = 5, bool autoUnload = true)
        {
            if (!unitDictionary.TryGetValue(resPath, out var unit))
            {
                var unitGo = new GameObject(resPath);
                unitGo.transform.SetParent(_cacheTransform);

                unit = GoPoolUnit.Allocate(unitGo.transform, resPath, minCount, autoUnload);
                unitDictionary.Add(resPath, unit);
                return unit;
            }

            return null;
        }

        /// <summary>
        /// 分配 GameObject
        /// </summary>
        /// <param name="resPath">资源路径-Bundle</param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public GameObject Allocate(string resPath, Transform parent = null)
        {
            if (!unitDictionary.TryGetValue(resPath, out var unit))
                unit = CreatePool(resPath);
            return unit.AllocateGameObject(parent).gameObject;
        }

        /// <summary>
        /// 分配 GameObject
        /// </summary>
        /// <param name="resPath"></param>
        /// <param name="parent"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Allocate<T>(string resPath, Transform parent = null) where T : class
        {
            var go = Allocate(resPath, parent);
            var obj = go.GetComponent<T>();
            if (obj == null)
            {
                Recycle(go);
                return null;
            }

            return (T) obj;
        }

        /// <summary>
        /// 回收 GameObject
        /// </summary>
        /// <param name="go"></param>
        /// <returns></returns>
        public bool Recycle(GameObject go)
        {
            if (go == null)
                return false;
            var poolIdentity = go.GetComponent<GoPoolIdentity>();
            if (poolIdentity == null)
            {
                Logger.Wraning($"对象池回收野对象 {go.name}");
                go.transform.SetParent(null);
                Object.Destroy(go);
                return false;
            }

            return Recycle(poolIdentity);
        }

        /// <summary>
        /// 回收 GameObject
        /// </summary>
        /// <param name="poolIdentity"></param>
        /// <returns></returns>
        public bool Recycle(GoPoolIdentity poolIdentity)
        {
            if (poolIdentity == null)
                return false;

            if (unitDictionary.TryGetValue(poolIdentity.id, out var unit))
                return unit.RecycleGameObject(poolIdentity);

            poolIdentity.transform.SetParent(null);
            DestroyImmediate(poolIdentity);
            return false;
        }

        /// <summary>
        /// 强制回收
        /// </summary>
        /// <param name="id"></param>
        public void ForceRecycle(string id)
        {
            if (unitDictionary.TryGetValue(id, out var unit))
            {
                unit.Recycle();
                unitDictionary.Remove(id);
            }
        }

        protected override void OnSingletonUpdate(float deltaTime)
        {
            if (Time.realtimeSinceStartup - _checkTime >= optimizeIntervalTime)
            {
                _checkTime = Time.realtimeSinceStartup;

                removes.Clear();
                foreach (var item in unitDictionary)
                {
                    var unit = item.Value;
                    unit.CheckOptimize();
                    if (unit.isAutoUnload && unit.isAcquittal && unit.averageIntervalTime >= autoUnloadThresholdValue)
                        removes.Add(item.Key);
                }

                for (var i = 0; i < removes.Count; i++)
                {
                    var key = removes[i];
                    unitDictionary[key].Recycle();
                    unitDictionary.Remove(key);
                }
            }
        }
    }
}
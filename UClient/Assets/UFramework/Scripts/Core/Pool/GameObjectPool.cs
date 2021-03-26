// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020-06-27 16:40:45
// * @Description:
// --------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using UFramework.Core.Pool;
using UnityEngine;

namespace UFramework.Core
{
    public class GoPoolIdentity : MonoBehaviour
    {
        public string id;
        public bool isRecycled;
        public void Recycle() => GammeObjectPool.Instance.Recycle(this);
    }

    public class GammeObjectPoolUnit : IPoolBehaviour
    {
        public bool isRecycled { get; set; }
        public string id { get; private set; }
        public GameObject prefab { get; private set; }

        /// <summary>
        /// 平均分配间隔时间，用于优化释放对象池
        /// </summary>
        public float averageAllocateTime => _intervalTimes.Sum() / Mathf.Max(_intervalTimes.Count, 1);

        /// <summary>
        /// 完全释放
        /// </summary>
        public bool isAcquittal => _allocateCount == _recycleCount;

        private Transform _parentTrans;

        private int _allocateCount;
        private int _recycleCount;

        private AssetRequest _assetRequest;

        private Stack<GoPoolIdentity> _stacks = new Stack<GoPoolIdentity>();
        private List<float> _intervalTimes = new List<float>();

        public static GammeObjectPoolUnit Allocate(string pid, Transform parent)
        {
            return ObjectPool<GammeObjectPoolUnit>.Instance.Allocate().Builder(pid, parent);
        }

        public static GammeObjectPoolUnit Allocate(string pid, GameObject targetPrefab, Transform parent)
        {
            return ObjectPool<GammeObjectPoolUnit>.Instance.Allocate().Builder(pid, targetPrefab, parent);
        }

        private GammeObjectPoolUnit Builder(string pid, Transform parent)
        {
            _assetRequest = Assets.LoadAsset(pid, typeof(GameObject));
            prefab = _assetRequest.asset as GameObject;

            return Builder(pid, prefab, parent);
        }

        private GammeObjectPoolUnit Builder(string pid, GameObject targetPrefab, Transform parent)
        {
            id = pid;
            prefab = prefab;
            _parentTrans = parent;

            _allocateCount = 0;
            _recycleCount = 0;
            return this;
        }

        public GoPoolIdentity AllocateGameObject()
        {
            GoPoolIdentity poolIdentity = null;
            if (_stacks.Count > 0)
            {
                poolIdentity = _stacks.Pop();
                poolIdentity.gameObject.transform.SetParent(_parentTrans);
            }
            else
            {
                var newGo = Object.Instantiate<GameObject>(prefab, _parentTrans, true);
                poolIdentity = newGo.AddComponent<GoPoolIdentity>();
                poolIdentity.id = id;
            }

            poolIdentity.isRecycled = false;
            poolIdentity.gameObject.SetActive(true);
            _allocateCount++;

            // 间隔时间
            float intervalTime = 0;
            if (_intervalTimes.Count > 0)
                intervalTime = Time.realtimeSinceStartup - _intervalTimes[_intervalTimes.Count - 1];
            _intervalTimes.Add(intervalTime);

            return poolIdentity;
        }

        public bool RecycleGameObject(GoPoolIdentity poolIdentity)
        {
            if (poolIdentity == null || poolIdentity.isRecycled)
                return false;

            poolIdentity.isRecycled = true;
            poolIdentity.gameObject.SetActive(false);
            poolIdentity.gameObject.transform.SetParent(_parentTrans);

            _stacks.Push(poolIdentity);
            _recycleCount++;

            return true;
        }

        public void Recycle()
        {
            ObjectPool<GammeObjectPoolUnit>.Instance.Recycle(this);
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

            _parentTrans = null;
            prefab = null;
            _assetRequest?.Unload();
            _assetRequest = null;

            _intervalTimes.Clear();
        }
    }

    public class GammeObjectPool : MonoSingleton<GammeObjectPool>
    {
        /// <summary>
        /// 检测对象池完全
        /// </summary>
        private const float CHECK_TIME = 10;

        /// <summary>
        /// 对象池完全释放时间（大于此时间就完全释放）
        /// </summary>
        private const float ACQUITTAL_TIME_VALUE = 60;

        static readonly Dictionary<string, GammeObjectPoolUnit> unitDictionary = new Dictionary<string, GammeObjectPoolUnit>();
        static readonly List<string> removes = new List<string>();

        private float _checkTime;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resPath">资源路径-Bundle</param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public GameObject Allocate(string resPath, Transform parent)
        {
            if (unitDictionary.TryGetValue(resPath, out var unit))
                return unit.AllocateGameObject().gameObject;

            unit = GammeObjectPoolUnit.Allocate(resPath, parent);
            unitDictionary.Add(resPath, unit);
            return unit.AllocateGameObject().gameObject;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resPath"></param>
        /// <param name="prefab"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public GameObject Allocate(string resPath, GameObject prefab, Transform parent)
        {
            if (unitDictionary.TryGetValue(resPath, out var unit))
                return unit.AllocateGameObject().gameObject;

            unit = GammeObjectPoolUnit.Allocate(resPath, prefab, parent);
            unitDictionary.Add(resPath, unit);
            return unit.AllocateGameObject().gameObject;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="go"></param>
        /// <returns></returns>
        public bool Recycle(GameObject go)
        {
            if (go == null)
                return false;
            return Recycle(go.GetComponent<GoPoolIdentity>());
        }

        /// <summary>
        /// 回收对象
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

        protected override void OnSingletonUpdate(float deltaTime)
        {
            if (Time.realtimeSinceStartup - _checkTime >= CHECK_TIME)
            {
                _checkTime = Time.realtimeSinceStartup;
                
                removes.Clear();
                foreach (var item in unitDictionary)
                {
                    var unit = item.Value;
                    if (unit.isAcquittal && unit.averageAllocateTime >= ACQUITTAL_TIME_VALUE)
                    {
                        removes.Add(item.Key);
                    }
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
/*
 * @Author: fasthro
 * @Date: 2020-06-27 16:40:45
 * @Description: GameObject 对象池
 */

using System.Collections.Generic;
using System.Linq;
using UFramework.Core.Pool;
using UnityEngine;

namespace UFramework.Core
{
    /// <summary>
    /// 对象标识
    /// </summary>
    public class GammeObjectPoolIdentity : MonoBehaviour, IPoolBehaviour
    {
        public string id;
        public bool isRecycled { get; set; }

        public void OnRecycle()
        {
            isRecycled = true;
        }

        public void Recycle()
        {
            GammeObjectPool.Instance.Recycle(this);
        }

        public void OnAllocate()
        {
            isRecycled = false;
        }
    }

    public class GammeObjectPoolUnit : Pool<GammeObjectPoolIdentity>
    {
        /// <summary>
        /// id
        /// </summary>
        public string id { get; private set; }

        /// <summary>
        /// 权重
        /// </summary>
        public int weight {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// prefab
        /// </summary>
        public GameObject prefab { get; private set; }

        /// <summary>
        /// 完全释放
        /// </summary>
        public bool canAcquittal => _allocateCount == _recycleCount;

        private Transform _parentTrans;
        private int _allocateCount;
        private int _recycleCount;
        private List<float> _weightTimes;

        private AssetRequest _assetRequest;

        /// <summary>
        /// Pool Unit
        /// </summary>
        /// <param name="id"></param>
        /// <param name="parent"></param>
        public GammeObjectPoolUnit(string id, Transform parent)
        {
            this.id = id;
            _assetRequest = Assets.LoadAsset(id, typeof(GameObject));
            prefab = _assetRequest.asset as GameObject;
            _parentTrans = parent;

            _weightTimes = new List<float>();
            _allocateCount = 0;
            _recycleCount = 0;
        }

        /// <summary>
        /// Pool Unit
        /// </summary>
        /// <param name="id"></param>
        /// <param name="prefab"></param>
        /// <param name="root"></param>
        public GammeObjectPoolUnit(string id, GameObject prefab, Transform root)
        {
            this.id = id;
            this.prefab = prefab;
            this._parentTrans = root;
        }

        /// <summary>
        /// 分配对象
        /// </summary>
        /// <returns></returns>
        public override GammeObjectPoolIdentity Allocate()
        {
            GammeObjectPoolIdentity poolIdentity = null;
            if (_stacks.Count > 0)
            {
                poolIdentity = _stacks.Pop();
                poolIdentity.gameObject.transform.SetParent(_parentTrans);
            }
            else
            {
                var newGo = Object.Instantiate<GameObject>(prefab, _parentTrans, true);
                poolIdentity = newGo.AddComponent<GammeObjectPoolIdentity>();
                poolIdentity.id = id;
            }

            poolIdentity.OnAllocate();
            poolIdentity.gameObject.SetActive(true);
            _allocateCount++;
            _weightTimes.Add(Time.realtimeSinceStartup);
            return poolIdentity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="poolIdentity"></param>
        /// <returns></returns>
        public override bool Recycle(GammeObjectPoolIdentity poolIdentity)
        {
            if (poolIdentity == null || poolIdentity.isRecycled)
                return false;

            poolIdentity.OnRecycle();
            poolIdentity.gameObject.SetActive(false);
            poolIdentity.gameObject.transform.SetParent(_parentTrans);

            _stacks.Push(poolIdentity);
            _recycleCount++;

            return true;
        }
    }

    public class GammeObjectPool : MonoSingleton<GammeObjectPool>
    {
        /// <summary>
        /// 对象单元字典
        /// </summary>
        static Dictionary<string, GammeObjectPoolUnit> unitDictionary;

        protected override void OnSingletonAwake()
        {
            unitDictionary = new Dictionary<string, GammeObjectPoolUnit>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="res"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public GameObject Allocate(string res, Transform parent)
        {
            if (unitDictionary.TryGetValue(res, out var unit))
                return unit.Allocate().gameObject;

            unit = new GammeObjectPoolUnit(res, parent);
            unitDictionary.Add(res, unit);
            return unit.Allocate().gameObject;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="res"></param>
        /// <param name="prefab"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public GameObject Allocate(string res, GameObject prefab, Transform parent)
        {
            if (unitDictionary.TryGetValue(res, out var unit))
                return unit.Allocate().gameObject;

            unit = new GammeObjectPoolUnit(res, prefab, parent);
            unitDictionary.Add(res, unit);
            return unit.Allocate().gameObject;
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
            return Recycle(go.GetComponent<GammeObjectPoolIdentity>());
        }

        /// <summary>
        /// 回收对象
        /// </summary>
        /// <param name="poolIdentity"></param>
        /// <returns></returns>
        public bool Recycle(GammeObjectPoolIdentity poolIdentity)
        {
            if (poolIdentity == null)
                return false;

            if (unitDictionary.TryGetValue(poolIdentity.id, out var unit))
                return unit.Recycle(poolIdentity);

            poolIdentity.transform.SetParent(null);
            DestroyImmediate(poolIdentity);
            return false;
        }
    }
}
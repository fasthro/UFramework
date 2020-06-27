/*
 * @Author: fasthro
 * @Date: 2020-06-27 16:40:45
 * @Description: GameObject 对象池
 */

using System.Collections.Generic;
using UnityEngine;

namespace UFramework.Pool
{
    /// <summary>
    /// 对象标识
    /// </summary>
    public class GammeObjectPoolIdentity : MonoBehaviour, IPoolObject
    {
        // 回收后60s内没有被唤醒使用，直接通知Unit销毁处理
        readonly static float DISPOSE_SLEEP_TIME = 60f;

        public string id;
        public float sleepTime { get; set; }
        public bool isRecycled { get; set; }

        public void OnAllocate()
        {
            sleepTime = -1;
            isRecycled = false;
        }

        public void OnRecycle()
        {
            sleepTime = Time.time;
            isRecycled = true;
        }

        public void Recycle()
        {
            GammeObjectPool.Instance.Recycle(this);
        }

        public void ResetSleepTime(float time)
        {
            if (isRecycled)
            {
                sleepTime = time;
            }
        }

        public bool IsDispose
        {
            get
            {
                if (isRecycled)
                {
                    if (sleepTime > 0)
                    {
                        if (Time.time - sleepTime > DISPOSE_SLEEP_TIME)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }
    }

    /// <summary>
    /// Pool Unit
    /// </summary>
    public class GammeObjectPoolUnit : Pool<GammeObjectPoolIdentity>
    {
        public string id { get; private set; }
        public GameObject prefab { get; private set; }

        private Transform m_parent;

        // 激活与回收的平衡数量（当平衡数量为0时，说明外界没有使用对象池中的对象）
        private int m_balanceCount;

        // 是否处于空闲状态
        public bool isFree
        {
            get { return m_stacks.Count > 0 && m_balanceCount <= 0; }
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
            this.m_parent = root;
        }

        /// <summary>
        /// 分配对象
        /// </summary>
        /// <returns></returns>
        public override GammeObjectPoolIdentity Allocate()
        {
            GammeObjectPoolIdentity poolIdentity = null;
            if (m_stacks.Count > 0)
            {
                poolIdentity = m_stacks.Pop();
                poolIdentity.gameObject.transform.SetParent(m_parent);
            }
            else
            {
                var newGo = GameObject.Instantiate<GameObject>(prefab);
                newGo.transform.SetParent(m_parent);
                poolIdentity = newGo.AddComponent<GammeObjectPoolIdentity>();
                poolIdentity.id = id;
            }
            poolIdentity.OnAllocate();
            poolIdentity.gameObject.SetActive(true);
            m_balanceCount++;
            return poolIdentity;
        }

        /// <summary>
        /// 回收对象
        /// </summary>
        /// <param name="gameObject"></param>
        public override bool Recycle(GammeObjectPoolIdentity poolIdentity)
        {
            if (poolIdentity == null)
            {
                poolIdentity.gameObject.transform.SetParent(null);
                GameObject.Destroy(poolIdentity.gameObject);
                // TODO WARNING LOG, 不属于对象池中的对象
                return false;
            }

            if (poolIdentity.isRecycled)
            {
                return false;
            }

            poolIdentity.OnRecycle();
            poolIdentity.gameObject.SetActive(false);
            poolIdentity.gameObject.transform.SetParent(m_parent);
            m_stacks.Push(poolIdentity);
            m_balanceCount--;
            return true;
        }

        /// <summary>
        /// 处理销毁长眠物体
        /// </summary>
        /// <param name="poolIdentity"></param>
        public void DisposeSleepGameObject(GammeObjectPoolIdentity poolIdentity)
        {
            var newPoolIdentity = m_stacks.Pop();
            poolIdentity.ResetSleepTime(newPoolIdentity.sleepTime);
            newPoolIdentity.gameObject.transform.SetParent(null);
            GameObject.Destroy(newPoolIdentity.gameObject);
            newPoolIdentity = null;
        }
    }

    /// <summary>
    /// GameObject Pool
    /// </summary>
    public class GammeObjectPool : MonoSingleton<GammeObjectPool>
    {
        private Dictionary<string, GammeObjectPoolUnit> poolUnitDictionary = new Dictionary<string, GammeObjectPoolUnit>();

        /// <summary>
        /// 分配对象
        /// </summary>
        /// <param name="res"></param>
        /// <returns></returns>
        public GameObject Allocate(string res)
        {
            GammeObjectPoolUnit unit = null;
            if (poolUnitDictionary.TryGetValue(res, out unit))
            {
                return unit.Allocate().gameObject;
            }
            // TODO
            unit = new GammeObjectPoolUnit(res, null, null);
            poolUnitDictionary.Add(res, unit);
            return unit.Allocate().gameObject;
        }

        /// <summary>
        /// 回收对象
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public bool Recycle(GameObject gameObject)
        {
            if (gameObject == null)
            {
                return false;
            }
            return Recycle(gameObject.GetComponent<GammeObjectPoolIdentity>());
        }

        /// <summary>
        /// 回收对象
        /// </summary>
        /// <param name="poolIdentity"></param>
        /// <returns></returns>
        public bool Recycle(GammeObjectPoolIdentity poolIdentity)
        {
            if (poolIdentity == null)
            {
                gameObject.transform.SetParent(null);
                GameObject.Destroy(gameObject);
                return false;
            }

            GammeObjectPoolUnit unit = null;
            if (!poolUnitDictionary.TryGetValue(poolIdentity.id, out unit))
            {
                return unit.Recycle(poolIdentity);
            }
            gameObject.transform.SetParent(null);
            GameObject.Destroy(gameObject);
            return false;
        }
    }
}
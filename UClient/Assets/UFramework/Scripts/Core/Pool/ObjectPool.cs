/*
 * @Author: fasthro
 * @Date: 2019-06-19 17:39:24
 * @Description: 对象池
 */

using UnityEngine;
using UFramework.Core.Pool;

namespace UFramework.Core
{
    public class ObjectPool<T> : Pool<T>, ISingleton where T : IPoolBehaviour, new()
    {
        #region Singleton
        public void SingletonAwake() { }

        protected ObjectPool()
        {
            _factory = new ObjectFactory<T>();
        }

        public static ObjectPool<T> Instance
        {
            get { return SingletonProperty<ObjectPool<T>>.Instance; }
        }

        public void Dispose()
        {
            SingletonProperty<ObjectPool<T>>.Dispose();
        }
        #endregion

        // 池最大数量,如果池中数量大于最大数，就移除多余的对象
        public int maxCount
        {
            get { return _maxCount; }
            set
            {
                _maxCount = value;

                if (_stacks != null)
                {
                    if (_maxCount > 0)
                    {
                        if (_maxCount < _stacks.Count)
                        {
                            int removeCount = _stacks.Count - _maxCount;
                            while (removeCount > 0)
                            {
                                _stacks.Pop();
                                --removeCount;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="maxCount">池中对象最大数量</param>
        /// <param name="initCount">池对象初始数量</param>
        public void Initialize(int maxCount, int initCount)
        {
            this.maxCount = maxCount;

            if (maxCount > 0)
            {
                initCount = Mathf.Min(maxCount, initCount);
            }

            if (count < initCount)
            {
                for (var i = count; i < initCount; ++i)
                {
                    Recycle(_factory.Create());
                }
            }
        }

        /// <summary>
        /// 分配对象
        /// </summary>
        /// <returns></returns>
        public override T Allocate()
        {
            var obj = base.Allocate();
            obj.isRecycled = false;
            return obj;
        }

        /// <summary>
        /// 回收
        /// </summary>
        /// <param name="obj"></param>
        public override bool Recycle(T obj)
        {
            if (obj == null || obj.isRecycled)
            {
                return false;
            }

            if (_maxCount > 0)
            {
                if (_stacks.Count >= _maxCount)
                {
                    return false;
                }
            }

            obj.isRecycled = true;
            obj.OnRecycle();
            _stacks.Push(obj);

            return true;
        }
    }
}
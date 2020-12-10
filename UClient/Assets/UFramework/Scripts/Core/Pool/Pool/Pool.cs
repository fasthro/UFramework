/*
 * @Author: fasthro
 * @Date: 2019-06-19 17:39:24
 * @Description: 池的泛型实现
 */

using System.Collections.Generic;

namespace UFramework.Pool
{
    public abstract class Pool<T> : IPool<T>
    {
        // 对象池数量
        public int count { get { return _stacks.Count; } }

        // 对象工厂
        protected IObjectFactory<T> _factory;

        // 池数据
        protected readonly Stack<T> _stacks = new Stack<T>();

        // 池默认最大数量
        protected int _maxCount = 12;

        /// <summary>
        /// 分配对象
        /// </summary>
        public virtual T Allocate()
        {
            return _stacks.Count == 0 ? _factory.Create() : _stacks.Pop();
        }

        /// <summary>
        /// 回收对象
        /// </summary>
        public abstract bool Recycle(T obj);
    }
}
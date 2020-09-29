/*
 * @Author: fasthro
 * @Date: 2019-10-28 16:36:24
 * @Description: 引用计数对象
 */
namespace UFramework.Ref
{
    public abstract class ReferenceObject : IReference
    {
        // 引用数量
        public int refCount { get; private set; }

        // 引用数量是否为空(0)
        public bool isEmptyRef
        {
            get
            {
                return refCount == 0;
            }
        }

        /// <summary>
        /// 增加引用数量
        /// </summary>
        public void Retain()
        {
            refCount++;
        }

        /// <summary>
        /// 减少引用数量
        /// </summary>
        public void Release()
        {
            refCount--;
            if (refCount == 0)
            {
                OnReferenceEmpty();
            }
        }

        protected virtual void OnReferenceEmpty() { }
    }
}
/*
 * @Author: fasthro
 * @Date: 2019-10-28 16:35:14
 * @Description: 引用计数接口
 */
namespace UFramework.Ref
{
    public interface IReference
    {
        int refCount { get; }
        void Retain();
        void Release();
    }
}
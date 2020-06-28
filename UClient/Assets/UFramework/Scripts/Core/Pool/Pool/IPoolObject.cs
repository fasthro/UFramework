/*
 * @Author: fasthro
 * @Date: 2019-08-06 19:47:27
 * @Description: 对象的对象池接口
 */
namespace UFramework.Pool
{
    public interface IPoolObject
    {
        // 回收标识
        bool isRecycled { get; set; }

        void Recycle();

        void OnRecycle();
    }
}
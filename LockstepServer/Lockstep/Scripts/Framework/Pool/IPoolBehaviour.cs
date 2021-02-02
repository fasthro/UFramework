/*
 * @Author: fasthro
 * @Date: 2019-08-06 19:47:27
 * @Description: 对象池行为接口
 */
namespace Lockstep
{
    public interface IPoolBehaviour
    {
        bool isRecycled { get; set; }
        void Recycle();
        void OnRecycle();
    }
}
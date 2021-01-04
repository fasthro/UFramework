/*
 * @Author: fasthro
 * @Date: 2020/12/30 19:38:57
 * @Description:
 */

namespace Lockstep
{
    public interface IViewService : IService
    {
        T CreateView<T>(string path) where T : IView;
    }
}
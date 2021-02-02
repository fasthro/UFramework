// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020/12/29 16:31:41
// * @Description:
// --------------------------------------------------------------------------------
namespace Lockstep
{
    public interface IViewService : IService
    {
        T CreateView<T>(string path) where T : IView;
        T CreateView<T>(string path, int oid) where T : IView;
    }
}
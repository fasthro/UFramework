/*
 * @Author: fasthro
 * @Date: 2020/12/31 15:49:52
 * @Description:
 */

namespace LockstepServer
{
    public interface IModelService : IService
    {
        T GetModel<T>() where T : BaseModel;

        void AddModel<T>(BaseModel model) where T : BaseModel;

        void RemoveModel<T>() where T : BaseModel;
    }
}
// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020/12/18 14:44:00
// * @Description:
// --------------------------------------------------------------------------------

namespace UFramework
{
    public interface IModelManager : IManager
    {
        T GetModel<T>() where T : BaseModel, new();

        void AddModel<T>(BaseModel model) where T : BaseModel;

        void RemoveModel<T>() where T : BaseModel;
    }
}
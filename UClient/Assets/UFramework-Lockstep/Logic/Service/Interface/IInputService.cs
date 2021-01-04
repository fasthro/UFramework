/*
 * @Author: fasthro
 * @Date: 2021-01-04 15:48:35
 * @Description: 
 */
namespace Lockstep.Logic
{
    public interface IInputService : IService
    {
        PlayerInput curentInput { get; }
    }
}
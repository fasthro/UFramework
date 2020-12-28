/*
 * @Author: fasthro
 * @Date: 2020-12-28 15:55:34
 * @Description:
 */

namespace UFramework.Lockstep
{
    public interface IGameService
    {
        GameManager gameManager { get; }
        InputManager inputManager { get; }
        FrameManager frameManager { get; }
        ViewManager viewManager { get; }
    }
}
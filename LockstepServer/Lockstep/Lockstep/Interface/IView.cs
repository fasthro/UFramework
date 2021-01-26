/*
 * @Author: fasthro
 * @Date: 2020/12/29 16:36:58
 * @Description:
 */

namespace Lockstep
{
    public interface IView
    {
        /// <summary>
        /// 位置
        /// </summary>
        LSVector3 position { get; set; }

        /// <summary>
        /// 旋转角度
        /// </summary>
        Fix64 deg { get; set; }

        /// <summary>
        /// 实体
        /// </summary>
        GameEntity entity { get; }

        /// <summary>
        /// 绑定实体
        /// </summary>
        /// <param name="entity"></param>
        void BindEntity(GameEntity entity);

        /// <summary>
        /// 生命周期更新
        /// </summary>
        void Update();
    }
}
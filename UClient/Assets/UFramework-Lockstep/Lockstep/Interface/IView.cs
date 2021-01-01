﻿/*
 * @Author: fasthro
 * @Date: 2020/12/29 16:36:58
 * @Description:
 */

using UnityEngine;

namespace Lockstep
{
    public interface IView
    {
        Vector3 position { get; set; }
        Quaternion rotation { get; set; }

        GameEntity entity { get; }

        void BindEntity(GameEntity entity);
    }
}
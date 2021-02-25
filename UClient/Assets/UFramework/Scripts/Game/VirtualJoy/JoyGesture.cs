// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021-02-23 16:10:08
// * @Description:
// --------------------------------------------------------------------------------

using UnityEngine;

namespace UFramework.Game
{
    public class JoyGesture
    {
        public int fingerIndex;
        public Joy.TouchEvent touchEvent;
        public float virtualRadius;
        public Vector2 virtualCenter;
        public Vector2 touchStartPosition;
        public Vector2 toucheMovePosition;
        public Vector2 touchEndPosition;

        public bool touchMoveing;
        public Vector2 touchMoveDirection;
        public float touchMoveAngle;
    }
}

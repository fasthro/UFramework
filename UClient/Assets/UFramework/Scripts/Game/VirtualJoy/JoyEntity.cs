// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021-02-23 16:10:08
// * @Description:
// --------------------------------------------------------------------------------

using UnityEngine;

namespace UFramework.Game
{
    public abstract class JoyEntity
    {
        protected readonly Joy _joy;
        private bool _isInitialized;
        public JoyEntity(Joy joy)
        {
            _joy = joy;
            
            _joy.joyListener -= OnJoy;
            _joy.joyListener += OnJoy;

            _joy.updateListener -= Update;
            _joy.updateListener += Update;
            
            _isInitialized = false;
        }
        
        public void Initialize()
        {
            _isInitialized = true;
            OnInitialize();
        }
        
        protected virtual void OnInitialize()
        {
            
        }

        private void Update()
        {
            if (!_isInitialized) return;
            
            OnUpdate();
        }

        protected virtual void OnUpdate()
        {
        }

        private void OnJoy(JoyGesture gesture)
        {
            if (!_isInitialized) return;
            
            switch (gesture.touchEvent)
            {
                case Joy.TouchEvent.TouchBegin:
                    OnTouchBegin(gesture);
                    break;
                case Joy.TouchEvent.Moving:
                    OnTouchMove(gesture);
                    break;
                case Joy.TouchEvent.TouchEnd:
                    OnTouchEnd(gesture);
                    break;
            }
        }
        
        protected virtual void OnTouchBegin(JoyGesture gesture)
        {
        }

        protected virtual void OnTouchMove(JoyGesture gesture)
        {
        }

        protected virtual void OnTouchEnd(JoyGesture gesture)
        {
        }
        
        protected static Vector2 CenterToUIPoint(Vector2 center, float uiRadius)
        {
            var uiPoint = Vector2.zero;
            uiPoint.x = center.x - uiRadius;
            uiPoint.y = center.y - uiRadius;
            return uiPoint;
        }
    }
}
// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021-02-23 16:10:08
// * @Description:
// --------------------------------------------------------------------------------

using UnityEngine;

namespace UFramework.Game
{
    [System.Serializable]
    public class Joy
    {
        public delegate void JoyEvent(JoyGesture gesture);

        public delegate void JoyKeyEvent();

        public delegate void JoyUpdateEvent();

        public enum ScreenDirection
        {
            Left,
            Right,
        }

        public enum VirtualShape
        {
            Circle,
            Square,
        }

        public enum TouchEvent
        {
            TouchBegin,
            Moving,
            TouchEnd,
        }

        public string name;
        public bool isMoveJoy;
        public ScreenDirection screenDirection;
        public KeyCode keyCode;
        public VirtualShape virtualShape;
        public JoyParame parame;

        public event JoyEvent joyListener;
        public event JoyUpdateEvent updateListener;
        public event JoyKeyEvent keyDownListener;
        public event JoyKeyEvent keyUpListener;
        
        public Vector2 virtualCenter => _virtualCenter;
        public Vector2 touchBeginPosition => _touchBeginPosition;
        public Vector2 touchMovePosition => _touchMovePosition;
        
        private Vector2 _virtualCenter;
        private int _fingerIndex = -1;
        private Vector2 _touchBeginPosition;
        private Vector2 _touchMovePosition;
        
        private bool _isTouchMoveing;
        private Vector2 _touchMoveDir;
        private float _touchMoveDeg;
        private bool _axis = false;

        private bool _isCheckInJoy;
        
        public void Initialize()
        {
            if (screenDirection == ScreenDirection.Left)
            {
                _virtualCenter.x = parame.boundary.x + parame.radius;
                _virtualCenter.y = Screen.height - parame.boundary.y - parame.radius;
            }
            else if (screenDirection == ScreenDirection.Right)
            {
                _virtualCenter.x = Screen.width - parame.boundary.x - parame.radius;
                _virtualCenter.y = Screen.height - parame.boundary.y - parame.radius;
            }
        }

        // 检查目标点是否在虚拟摇杆范围内
        private bool CheckInJoy(Vector2 pos)
        {
            if (virtualShape == VirtualShape.Circle)
            {
                var distance = Vector2.Distance(_virtualCenter, pos);
                if (distance <= parame.radius)
                    return true;
            }
            else if (virtualShape == VirtualShape.Square)
            {
                if (pos.x >= _virtualCenter.x - parame.radius
                    && pos.x <= _virtualCenter.x + parame.radius
                    && pos.y >= _virtualCenter.y - parame.radius
                    && pos.y <= _virtualCenter.y + parame.radius)
                {
                    return true;
                }
            }

            return false;
        }

        // create gesture
        private JoyGesture CreateJoyGesture(TouchEvent touchEvent, Vector2 endPosition)
        {
            var gesture = new JoyGesture
            {
                fingerIndex = _fingerIndex,
                touchEvent = touchEvent,
                virtualRadius = parame.radius,
                virtualCenter = _virtualCenter,
                touchStartPosition = _touchBeginPosition,
                toucheMovePosition = _touchMovePosition,
                touchEndPosition = endPosition,
                touchMoveing = _isTouchMoveing,
                touchMoveDirection = _touchMoveDir,
                touchMoveAngle = _touchMoveDeg
            };

            return gesture;
        }

        private void BroadcastEvent(TouchEvent @event, Vector2 endPosition)
        {
            joyListener?.Invoke(CreateJoyGesture(@event, endPosition));
        }

        public void OnUpdate()
        {
            updateListener?.Invoke();

            if (!isMoveJoy)
            {
                if (Input.GetKeyDown(keyCode))
                {
                    keyDownListener?.Invoke();
                }

                if (Input.GetKeyUp(keyCode))
                {
                    keyUpListener?.Invoke();
                }
            }

#if UNITY_EDITOR
            if (isMoveJoy && Application.isPlaying)
            {
                var x = Input.GetAxis("Horizontal");
                var y = Input.GetAxis("Vertical") * -1f;

                if ((x != 0 || y != 0) && !_axis)
                {
                    _axis = true;
                    _fingerIndex = 1;
                    _isTouchMoveing = false;
                    _touchBeginPosition = _virtualCenter;

                    BroadcastEvent(TouchEvent.TouchBegin, _touchBeginPosition);
                }
                else if (x == 0 && y == 0 && _axis)
                {
                    _axis = false;
                    _fingerIndex = -1;
                    _isTouchMoveing = false;

                    BroadcastEvent(TouchEvent.TouchEnd, Vector2.zero);
                }
                else if (_axis)
                {
                    _touchMovePosition.x = x * parame.radius;
                    _touchMovePosition.y = y * parame.radius;
                    _touchMovePosition += _virtualCenter;

                    _isTouchMoveing = true;
                    _touchMoveDir = (_touchBeginPosition - _touchMovePosition).normalized;
                    _touchMoveDeg = 180f + Vector2.SignedAngle(_touchMoveDir, Vector2.up);

                    BroadcastEvent(TouchEvent.Moving, _touchMovePosition);
                }
            }
#endif
        }

        public void OnTouchBegin(Gesture gesture)
        {
            var position = ToTouchPoint(gesture.position);

            _isCheckInJoy = CheckInJoy(position);

            if (!_isCheckInJoy)
            {
                _fingerIndex = -1;
                return;
            }

            _fingerIndex = gesture.fingerIndex;
            _isTouchMoveing = false;
            _touchBeginPosition = position;

            BroadcastEvent(TouchEvent.TouchBegin, _touchBeginPosition);
        }

        public void OnTouchEnd(Gesture gesture)
        {
            if (!_isCheckInJoy || _fingerIndex == -1 || gesture.fingerIndex != _fingerIndex)
                return;

            var position = ToTouchPoint(gesture.position);
            _fingerIndex = -1;
            _isTouchMoveing = false;

            BroadcastEvent(TouchEvent.TouchEnd, position);
        }

        public void OnTouchMove(Gesture gesture)
        {
            if (!_isCheckInJoy || _fingerIndex == -1 || gesture.fingerIndex != _fingerIndex)
                return;

            var position = ToTouchPoint(gesture.position);

            _touchMovePosition = position;
            _isTouchMoveing = true;
            _touchMoveDir = (_touchBeginPosition - _touchMovePosition).normalized;
            _touchMoveDeg = 180f + Vector2.SignedAngle(_touchMoveDir, Vector2.up);

            BroadcastEvent(TouchEvent.Moving, _touchMovePosition);
        }

        private Vector2 ToTouchPoint(Vector2 touchPoint)
        {
            var np = Vector2.zero;
            np.x = touchPoint.x;
            np.y = Screen.height - touchPoint.y;
            return np;
        }
    }
}
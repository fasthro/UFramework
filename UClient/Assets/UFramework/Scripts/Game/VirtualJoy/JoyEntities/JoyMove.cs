// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021-02-23 16:10:08
// * @Description:
// --------------------------------------------------------------------------------

using UnityEngine;
using FairyGUI;

namespace UFramework.Game
{
    public class JoyMove : JoyEntity
    {
        public delegate void MoveEvent(Vector2 move, bool isMove);

        public event MoveEvent moveListener;

        #region ui

        public GComponent ui;
        public Transition startTransition;
        public Transition endTransition;
        public GImage slider;

        #endregion

        // ui 半径
        private float _uiRadius;

        // 开始坐标
        private Vector2 _beginPosition;

        // touch 坐标
        private Vector2 _touchPosition;

        // ui 坐标
        private Vector2 _uiPoint;

        // slider 坐标
        private Vector2 _sliderPoint;

        // slider 坐在ui里的初始坐标
        private Vector2 _sliderInitPoint;

        // slider 半径
        private float _sliderRadius;

        // 是否开始移动
        private bool _isBeginMove;

        // 是否已经停止移动
        private bool _isEndMove;

        public JoyMove(Joy joy) : base(joy)
        {
            _isBeginMove = false;
            _isEndMove = false;
        }

        protected override void OnInitialize()
        {
            _uiRadius = ui.width / 2;
            _sliderRadius = slider.width / 2;

            _sliderInitPoint.x = _uiRadius - _sliderRadius;
            _sliderInitPoint.y = _uiRadius - _sliderRadius;
            
            _beginPosition = _joy.virtualCenter;
            _touchPosition = _joy.virtualCenter;

            _uiPoint = CenterToUIPoint(_beginPosition, _uiRadius);
            _sliderPoint = _sliderInitPoint;

            ui.xy = _uiPoint;
            slider.xy = _sliderPoint;

            endTransition.Play();
        }

        protected override void OnTouchBegin(JoyGesture gesture)
        {
            var referenceValue = gesture.virtualRadius - Vector2.Distance(gesture.virtualCenter, gesture.touchStartPosition);
            if (referenceValue > _uiRadius)
            {
                _beginPosition = gesture.touchStartPosition;
            }
            else
            {
                var dir = (gesture.touchStartPosition - gesture.virtualCenter).normalized;
                var len = gesture.virtualRadius - _uiRadius;
                _beginPosition = gesture.virtualCenter + dir * len;
            }

            _touchPosition = gesture.touchStartPosition;

            // ui
            _uiPoint = CenterToUIPoint(_beginPosition, _uiRadius);

            // slider
            _sliderPoint = _sliderInitPoint + (_touchPosition - _beginPosition);

            startTransition.Play();

            _isBeginMove = true;
            _isEndMove = false;
        }

        protected override void OnTouchMove(JoyGesture gesture)
        {
            if (!gesture.touchMoveing)
                return;

            _touchPosition = gesture.toucheMovePosition;

            var maxDis = _uiRadius - _sliderRadius;
            var vector = _touchPosition - _beginPosition;
            var dis = vector.magnitude;
            if (dis > maxDis)
            {
                dis = maxDis;
            }

            // slider
            _sliderPoint = _sliderInitPoint + vector.normalized * dis;
        }

        protected override void OnTouchEnd(JoyGesture gesture)
        {
            _beginPosition = gesture.virtualCenter;
            _touchPosition = gesture.virtualCenter;
            _uiPoint = CenterToUIPoint(_beginPosition, _uiRadius);
            _sliderPoint = _sliderInitPoint;

            endTransition.Play();

            _isEndMove = true;
        }

        protected override void OnUpdate()
        {
            ui.xy = Vector2.Lerp(ui.xy, _uiPoint, Time.deltaTime * 50);
            slider.xy = Vector2.Lerp(slider.xy, _sliderPoint, Time.deltaTime * 50);

            if (moveListener != null)
            {
                var move = (_beginPosition - _touchPosition).normalized;
                move.x *= -1;

                if (_isBeginMove)
                {
                    if (_isEndMove)
                    {
                        _isBeginMove = false;
                        moveListener(move, false);
                    }
                    else
                    {
                        moveListener(move, true);
                    }
                }
            }
        }
    }
}
// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021/03/03 16:23
// * @Description:
// --------------------------------------------------------------------------------

using UFramework.Core;
using UnityEngine;

namespace UFramework.Consoles
{
    public enum TriggerAlignment
    {
        TopLeft = 0,
        TopRight = 1,
        BottomLeft = 2,
        BottomRight = 3,
        CenterLeft = 4,
        CenterRight = 5,
        TopCenter = 6,
        BottomCenter = 7
    }

    public class TriggerService : BaseConsoleService
    {
        private float _areaX;
        private float _areaY;
        private float _areaWidth;
        private float _areaHeight;
        private float _time;

        private float _timeTemp = 0;

        protected override void OnInitialized()
        {
            var consoleConfig = Serializer<ConsoleConfig>.Instance;

            var alignment = consoleConfig.triggerAlignment;
            _areaWidth = Screen.width * consoleConfig.triggerHScreenAspect / 100f;
            _areaHeight = Screen.height * consoleConfig.triggerVScreenAspect / 100f;

            _time = consoleConfig.triggerTime;

            switch (alignment)
            {
                case TriggerAlignment.TopLeft:
                    _areaX = 0;
                    _areaY = 0;
                    break;
                case TriggerAlignment.TopRight:
                    _areaX = Screen.width - _areaWidth;
                    _areaY = 0;
                    break;
                case TriggerAlignment.BottomLeft:
                    _areaX = 0;
                    _areaY = Screen.height - _areaHeight;
                    break;
                case TriggerAlignment.BottomRight:
                    _areaX = Screen.width - _areaWidth;
                    _areaY = Screen.height - _areaHeight;
                    break;
                case TriggerAlignment.CenterLeft:
                    _areaX = 0;
                    _areaY = (Screen.height - _areaHeight) / 2f;
                    break;
                case TriggerAlignment.CenterRight:
                    _areaX = Screen.width - _areaWidth;
                    _areaY = (Screen.height - _areaHeight) / 2f;
                    break;
                case TriggerAlignment.TopCenter:
                    _areaX = (Screen.width - _areaWidth) / 2f;
                    _areaY = 0;
                    break;
                case TriggerAlignment.BottomCenter:
                    _areaX = (Screen.width - _areaWidth) / 2f;
                    _areaY = Screen.height - _areaHeight;
                    break;
            }
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            CheckVisibility();
        }


        private void CheckVisibility()
        {
#if ((!UNITY_ANDROID && !UNITY_IPHONE) || UNITY_EDITOR)
            if (Input.GetMouseButton(0))
            {
                var inputPos = Input.mousePosition;
                inputPos.y = Screen.height - inputPos.y;
#else
            if (Input.touches.Length > 0 && ((Input.GetTouch(0).phase != TouchPhase.Ended) && (Input.GetTouch(0).phase != TouchPhase.Canceled)))
            {
                var inputPos = Input.GetTouch(0).position;
                inputPos.y = Screen.height - inputPos.y;
#endif
                var rect = new Rect(_areaX, _areaY, _areaWidth, _areaHeight);
                if ((inputPos.x > rect.x) && (inputPos.x < rect.x + rect.width) && (inputPos.y > rect.y && inputPos.y < rect.y + rect.height))
                {
                    _timeTemp += Time.deltaTime;
                    if (_timeTemp > _time)
                    {
                        _timeTemp = 0;
                        Console.Instance.TriggerConsolePanel();
                    }
                }
            }
        }
    }
}
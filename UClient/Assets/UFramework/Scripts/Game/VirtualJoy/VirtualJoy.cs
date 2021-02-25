// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021-02-23 16:10:08
// * @Description:
// --------------------------------------------------------------------------------

using UFramework;
using UnityEngine;

namespace UFramework.Game
{
    [RequireComponent(typeof(EasyTouch))]
    [RequireComponent(typeof(VirtualJoyDebug))]
    public class VirtualJoy : MonoSingleton<VirtualJoy>
    {
        public Joy[] joys;
        public bool enableVirtuakJoy = true;

        protected override void OnSingletonStart()
        {
            foreach (var joy in joys)
                joy.Initialize();
        }

        void OnEnable()
        {
            EasyTouch.On_TouchStart += OnTouchBegin;
            EasyTouch.On_TouchUp += OnTouchEnd;
            EasyTouch.On_TouchDown += OnTouchMove;
        }

        void OnDisable()
        {
            EasyTouch.On_TouchStart -= OnTouchBegin;
            EasyTouch.On_TouchUp -= OnTouchEnd;
            EasyTouch.On_TouchDown -= OnTouchMove;
        }

        protected override void OnSingletonUpdate(float deltaTime)
        {
            if (!enableVirtuakJoy) return;

            foreach (var joy in joys)
                joy.OnUpdate();
        }

        void OnTouchBegin(Gesture gesture)
        {
            if (!enableVirtuakJoy) return;

            foreach (var joy in joys)
                joy.OnTouchBegin(gesture);
        }

        void OnTouchEnd(Gesture gesture)
        {
            if (!enableVirtuakJoy) return;

            foreach (var joy in joys)
                joy.OnTouchEnd(gesture);
        }

        void OnTouchMove(Gesture gesture)
        {
            if (!enableVirtuakJoy) return;

            foreach (var joy in joys)
                joy.OnTouchMove(gesture);
        }

        public Joy GetJoyWithName(string joyName)
        {
            foreach (var joy in joys)
            {
                if (joy.name.Equals(joyName))
                    return joy;
            }

            return null;
        }
    }
}
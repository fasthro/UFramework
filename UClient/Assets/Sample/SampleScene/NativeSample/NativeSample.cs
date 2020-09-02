using System.Collections;
using System.Collections.Generic;
using UFramework.Native;
using UnityEngine;

namespace UFramework.Sample
{
    public class NativeSample : SampleScene
    {
        protected override void OnRenderGUI()
        {
            if (GUILayout.Button("Device Id", GUILayout.Width(300), GUILayout.Height(100)))
            {
                Debug.Log("设备唯一标识：" + UNative.device.DeviceId());
            }

            if (GUILayout.Button("Device Country ISO", GUILayout.Width(300), GUILayout.Height(100)))
            {
                Debug.Log("设备所处国家ISO：" + UNative.device.DeviceCountryISO());
            }

            if (GUILayout.Button("Restart App", GUILayout.Width(300), GUILayout.Height(100)))
            {
                UNative.utils.Restart();
            }

            if (GUILayout.Button("Set ClipBoard", GUILayout.Width(300), GUILayout.Height(100)))
            {
                UNative.utils.SetClipBoard("UFramework-" + Random.Range(1, 100));
            }

            if (GUILayout.Button("Get ClipBoard", GUILayout.Width(300), GUILayout.Height(100)))
            {
                Debug.Log("ClipBoard Text: " + UNative.utils.GetClipBoard());
            }
        }
    }
}

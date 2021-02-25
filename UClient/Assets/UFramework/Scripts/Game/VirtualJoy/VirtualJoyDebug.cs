// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021-02-23 16:10:08
// * @Description:
// --------------------------------------------------------------------------------

using UnityEngine;

namespace UFramework.Game
{
    [ExecuteInEditMode]
    public class VirtualJoyDebug : MonoBehaviour
    {
        public delegate void OnGUIDelegate();

        public static OnGUIDelegate OnGUIHandlers;

        void OnGUI()
        {
            OnGUIHandlers?.Invoke();
        }
    }
}
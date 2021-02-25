// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021-02-23 16:10:08
// * @Description:
// --------------------------------------------------------------------------------

using UFramework.Game;
using UnityEngine;
using UnityEditor;

namespace UFramework.GameEditor
{
    [CustomEditor(typeof(VirtualJoy))]
    public class VirtualJoyEditor : UnityEditor.Editor
    {
        static VirtualJoy Target;
        private Texture2D _circleTexture;
        private Texture2D _squarTexture;

        private void OnEnable()
        {
            Target = target as VirtualJoy;

            _circleTexture = AssetDatabase.LoadAssetAtPath("Assets/UFramework/Scripts/Game/VirtualJoy/Editor/GUI/circle.png", typeof(Texture2D)) as Texture2D;
            _squarTexture = AssetDatabase.LoadAssetAtPath("Assets/UFramework/Scripts/Game/VirtualJoy/Editor/GUI/square.png", typeof(Texture2D)) as Texture2D;

            VirtualJoyDebug.OnGUIHandlers -= OnGuiHandler;
            VirtualJoyDebug.OnGUIHandlers += OnGuiHandler;

            UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
        }

        private void OnDisable()
        {
            VirtualJoyDebug.OnGUIHandlers -= OnGuiHandler;
            UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
        }

        private void OnGuiHandler()
        {
            if (Target.joys == null) 
                return;
            
            foreach (var joy in Target.joys)
                GUI.DrawTexture(GetUIRect(joy), GetJoyTexture(joy));
        }

        Texture2D GetJoyTexture(Joy joy)
        {
            return joy.virtualShape == Joy.VirtualShape.Circle ? _circleTexture : _squarTexture;
        }

        private static Rect GetUIRect(Joy joy)
        {
            var uiPoint = GetUIPoint(joy);
            return new Rect(uiPoint.x, uiPoint.y, joy.parame.radius * 2, joy.parame.radius * 2);
        }

        private static Vector2 GetUIPoint(Joy joy)
        {
            var virtualCenter = GetVirtualCenter(joy);
            var uiPoint = Vector2.zero;

            uiPoint.x = virtualCenter.x - joy.parame.radius;
            uiPoint.y = virtualCenter.y - joy.parame.radius;

            return uiPoint;
        }

        private static Vector2 GetVirtualCenter(Joy joy)
        {
            var parame = joy.parame;
            var center = Vector2.zero;
            if (joy.screenDirection == Joy.ScreenDirection.Left)
            {
                center.x = parame.boundary.x + parame.radius;
                center.y = Screen.height - parame.boundary.y - parame.radius;
            }
            else if (joy.screenDirection == Joy.ScreenDirection.Right)
            {
                center.x = Screen.width - parame.boundary.x - parame.radius;
                center.y = Screen.height - parame.boundary.y - parame.radius;
            }

            return center;
        }
    }
}
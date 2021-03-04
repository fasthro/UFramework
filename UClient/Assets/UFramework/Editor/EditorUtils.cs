// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021/03/04 11:40
// * @Description:
// --------------------------------------------------------------------------------

using UnityEditor;
using UnityEngine;

namespace UFramework.Editor
{
    public static class EditorUtils
    {
        public static class Styles
        {
            private static GUIStyle _headerLabel;

            public static GUIStyle headerLabel
            {
                get
                {
                    if (_headerLabel == null)
                    {
                        _headerLabel = new GUIStyle(EditorStyles.largeLabel)
                        {
                            fontSize = 18,
                            fontStyle = FontStyle.Normal,
                            margin = new RectOffset(5, 5, 5, 5)
                        };
                    }

                    return _headerLabel;
                }
            }
        }
    }
}
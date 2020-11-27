/*
 * @Author: fasthro
 * @Date: 2020-09-29 18:00:13
 * @Description: Welcome
 */

using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UFramework.Config;
using UnityEditor;
using UnityEngine;

namespace UFramework.Editor.Preferences
{
    public class WelcomePage : IPage, IPageBar
    {
        public string menuName { get { return "Welcome"; } }
   
        public object GetInstance()
        {
            return this;
        }

        public void OnRenderBefore()
        {
        }

        public void OnPageBarDraw()
        {

        }

        public void OnSaveDescribe()
        {
            
        }
    }
}
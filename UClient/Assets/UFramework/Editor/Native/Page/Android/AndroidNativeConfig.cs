/*
 * @Author: fasthro
 * @Date: 2020-08-29 12:19:07
 * @Description: Android Native Config
 */
using Sirenix.OdinInspector;
using UFramework.Config;
using UnityEditor;

namespace UFramework.Editor.Native
{
    [System.Serializable]
    public class AndroidNativeModule
    {
        [ShowInInspector]
        [HideLabel]
        [ValidateInput("ValidateName", "$validateNameError", InfoMessageType.Error)]
        [HorizontalGroup("Hor")]
        public string name;

        [Button(ButtonSizes.Small)]
        [HorizontalGroup("Hor")]
        [LabelText("Update AAR")]
        public void UpdateReleaseAAR()
        {
            if (string.IsNullOrEmpty(validateNameError))
            {
                var source = IOPath.PathCombine(App.AndroidNativeDirectory, name, "build/outputs/aar/");
                if (!IOPath.DirectoryExists(source))
                {
                    EditorUtility.DisplayDialog("Error Notice", "Android Native Update AAR Failled. [" + name + "] Module AAR File Not Exist.", "OK");
                }
                else
                {
                    AndroidPage.UpdateAAR(false, name);
                }
            }
        }

        #region 名字验证
        private string validateNameError;
        private bool ValidateName(string value)
        {
            validateNameError = null;
            if (string.IsNullOrEmpty(value))
            {
                validateNameError = "error: module is empty.";
                return false;
            }
            var source = IOPath.PathCombine(App.AndroidNativeDirectory, value);
            if (!IOPath.DirectoryExists(source))
            {
                validateNameError = "error: module not exists.";
                return false;
            }
            return true;
        }

        #endregion
    }

    public class AndroidNativeConfig : IConfigObject
    {
        public string name { get { return "AndroidNativeConfig"; } }
        public FileAddress address { get { return FileAddress.Editor; } }

        /// <summary>
        /// Modules
        /// </summary>
        public AndroidNativeModule[] modules;

        public void Save()
        {
            UConfig.Write<AndroidNativeConfig>(this);
        }
    }
}
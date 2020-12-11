/*
 * @Author: fasthro
 * @Date: 2020-08-29 12:19:07
 * @Description: Android Native Config
 */
using System;
using Sirenix.OdinInspector;
using UFramework.Core;
using UnityEditor;

namespace UFramework.Editor.NativePlatform
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
            if (string.IsNullOrEmpty(_validateNameError))
            {
                var source = IOPath.PathCombine(AndroidPage.ProjectPath, name, "build/outputs/aar/");
                if (!IOPath.DirectoryExists(source))
                {
                    EditorUtility.DisplayDialog("Android Native", "Android Native 更新 AAR 失败. [" + name + "] Module AAR 文件不存在！", "确定");
                }
                else
                {
                    AndroidPage.UpdateAAR(false, name);
                }
            }
        }

        #region 名字验证
        private string _validateNameError;
        private bool ValidateName(string value)
        {
            _validateNameError = null;
            if (string.IsNullOrEmpty(value))
            {
                _validateNameError = "error: module is empty.";
                return false;
            }
            var source = IOPath.PathCombine(AndroidPage.ProjectPath, value);
            if (!IOPath.DirectoryExists(source))
            {
                _validateNameError = "error: module not exists.";
                return false;
            }
            return true;
        }

        #endregion
    }

    public class NativePlatform_AndroidConfig : ISerializable
    {
        public SerializableAssigned assigned { get { return SerializableAssigned.Editor; } }

        public AndroidNativeModule[] modules = new AndroidNativeModule[0];

        public void Serialize()
        {
            Serializer<NativePlatform_AndroidConfig>.Serialize(this);
        }
    }
}
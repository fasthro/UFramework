/*
 * @Author: fasthro
 * @Date: 2020-09-16 18:51:28
 * @Description: version
 */

using Sirenix.OdinInspector;
using UFramework.Core;

namespace UFramework.Editor.Tools
{
    public class SerializablePage : IPage, IPageBar
    {
        public string menuName { get { return "Serializable"; } }

        [ShowInInspector, FilePath, HorizontalGroup]
        [HideLabel]
        public string versionFilePath;

        [ShowInInspector, FilePath, HorizontalGroup(150)]
        public void Serializable()
        {
            version = Version.LoadVersion(versionFilePath);
        }

        [ShowInInspector]
        [HideIf("_hideVersion")]
        public Version version;
        private bool _hideVersion { get { return version == null; } }

        public object GetInstance()
        {
            return this;
        }

        public void OnRenderBefore()
        {

        }

        public void OnSaveDescribe()
        {

        }

        public void OnPageBarDraw()
        {

        }
    }
}
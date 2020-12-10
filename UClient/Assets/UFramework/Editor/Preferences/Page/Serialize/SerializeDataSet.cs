/*
 * @Author: fasthro
 * @Date: 2020-12-10 15:44:58
 * @Description: 
 */

using Sirenix.OdinInspector;
using UFramework.Serialize;
namespace UFramework.Editor.Preferences
{
    [System.Serializable]
    public class SerializeItem
    {
        [ShowInInspector, HideLabel, ReadOnly]
        [HorizontalGroup("Config Name")]
        public string name;

        [ShowInInspector, HideLabel, ReadOnly]
        [HorizontalGroup("Address")]
        public SerializableType serializableType;
    }
}
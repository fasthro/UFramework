// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020-12-10 13:51:28
// * @Description:
// --------------------------------------------------------------------------------

using LitJson;
using UnityEngine;

namespace UFramework.Core
{
    public enum SerializableAssigned
    {
        Editor,
        AssetBundle,
        Persistent,
        Resources,
        User,
    }

    public interface ISerializable
    {
        SerializableAssigned assigned { get; }
        void Serialize();
    }

    public class Serializer<T> where T : ISerializable, new()
    {
        static string RootPath = IOPath.PathRelativeAsset(IOPath.PathCombine(UApplication.AssetsDirectory, "Serializable"));
        static string AssetBundlePath = IOPath.PathRelativeAsset(IOPath.PathCombine(RootPath, SerializableAssigned.AssetBundle.ToString()));

        static T _Instance;

        public static T Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = Deserialize();
                }

                return _Instance;
            }
        }

        private static T Deserialize()
        {
            var name = typeof(T).Name;
            var obj = new T();
            var type = obj.GetType();
            string content = null;
            var fileName = name + ".json";
#if UNITY_EDITOR
            content = IOPath.FileReadText(IOPath.PathCombine(RootPath, obj.assigned.ToString(), fileName));
#else
            if (obj.assigned == SerializableAssigned.Resources)
            {
                var asset = Assets.LoadResourceAsset(name, typeof(TextAsset));
                Debug.Log($"{name} {asset}");
                content = asset.GetAsset<TextAsset>().text;
                Debug.Log($"{content}");
                asset.Unload();
            }
            else if (obj.assigned == SerializableAssigned.AssetBundle)
            {
                var asset = Assets.LoadAsset(IOPath.PathCombine(AssetBundlePath, obj.assigned.ToString(), fileName), typeof(TextAsset));
                content = asset.GetAsset<TextAsset>().text;
                asset.Unload();
            }
            else if (obj.assigned == SerializableAssigned.Persistent)
            {
                // TODO
            }
#endif
            if (!string.IsNullOrEmpty(content))
            {
                obj = JsonMapper.ToObject<T>(content);
            }

            return obj;
        }

        public static void Serialize(T obj)
        {
#if UNITY_EDITOR
            var type = typeof(T);
            var name = type.Name;

            var fileName = name + ".json";
            var content = JsonMapper.ToJson(obj);
            var path = IOPath.PathCombine(RootPath, obj.assigned.ToString(), fileName);
            IOPath.FileCreateText(path, content);
#endif
        }
    }
}
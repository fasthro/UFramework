/*
 * @Author: fasthro
 * @Date: 2020-12-10 13:51:28
 * @Description: 序列化
 */

using System;
using LitJson;
using UFramework.Assets;
using UnityEngine;

namespace UFramework.Serialize
{
    public enum SerializableType
    {
        Editor,
        AssetBundle,
        Persistent,
        Resources,
        User,
    }

    public interface ISerializable
    {
        SerializableType serializableType { get; }
        void Serialization();
    }

    public class Serializable<T> where T : ISerializable, new()
    {
        static string RootPath = IOPath.PathRelativeAsset(IOPath.PathCombine(App.AssetsDirectory, "Serdata"));
        static string AssetBundlePath = IOPath.PathRelativeAsset(IOPath.PathCombine(RootPath, SerializableType.AssetBundle.ToString()));

        static T _Instance;

        public static T Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = Deserialization();
                }
                return _Instance;
            }
        }

        public static T Deserialization()
        {
            var name = typeof(T).Name;
            var obj = new T();
            var type = obj.GetType();
            string content = null;
            string fileName = name + ".json";
#if UNITY_EDITOR
            content = IOPath.FileReadText(IOPath.PathCombine(RootPath, obj.serializableType.ToString(), fileName));
#else
            if (obj.serializableType == SerializableType.Resources)
            {
                var asset = Asset.LoadResourceAsset(name, typeof(TextAsset));
                content = asset.GetAsset<TextAsset>().text;
                asset.Unload();
            }
            else if (obj.serializableType == SerializableType.AssetBundle)
            {
                var asset = Asset.LoadAsset(IOPath.PathCombine(AssetBundlePath, obj.serializableType.ToString(), fileName), typeof(TextAsset));
                content = asset.GetAsset<TextAsset>().text;
                asset.Unload();
            }
            else if (obj.serializableType == SerializableType.Persistent)
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

        public static void Serialization(T obj)
        {
#if UNITY_EDITOR
            var type = typeof(T);
            string name = type.Name;

            string fileName = name + ".json";
            string content = JsonMapper.ToJson(obj);
            string path = IOPath.PathCombine(RootPath, obj.serializableType.ToString(), fileName);
            IOPath.FileCreateText(path, content);
#endif
        }
    }
}
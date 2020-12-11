/*
 * @Author: fasthro
 * @Date: 2020-12-10 13:51:28
 * @Description: 序列化
 */

using System;
using LitJson;
using UFramework.Core;
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
            string fileName = name + ".json";
#if UNITY_EDITOR
            content = IOPath.FileReadText(IOPath.PathCombine(RootPath, obj.assigned.ToString(), fileName));
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

        public static void Serialize(T obj)
        {
#if UNITY_EDITOR
            var type = typeof(T);
            string name = type.Name;

            string fileName = name + ".json";
            string content = JsonMapper.ToJson(obj);
            string path = IOPath.PathCombine(RootPath, obj.assigned.ToString(), fileName);
            IOPath.FileCreateText(path, content);
#endif
        }
    }
}
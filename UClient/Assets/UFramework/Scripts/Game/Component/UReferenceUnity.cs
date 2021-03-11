// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021/03/10 15:36
// * @Description:
// --------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Object = UnityEngine.Object;

namespace UFramework.Game
{
    [System.Serializable]
    public class UReferenceUnityData : IComparer<UReferenceUnityData>
    {
        [ShowInInspector] public string key;

        [ShowInInspector] public Object value;

        public int Compare(UReferenceUnityData x, UReferenceUnityData y)
        {
            if (ReferenceEquals(x, y)) return 0;
            if (ReferenceEquals(null, y)) return 1;
            if (ReferenceEquals(null, x)) return -1;
            return string.Compare(x.key, y.key, StringComparison.Ordinal);
        }
    }

    public class UReferenceUnity : MonoBehaviour, ISerializationCallbackReceiver
    {
        [ShowInInspector] [TableList] public List<UReferenceUnityData> objects = new List<UReferenceUnityData>();

        private Dictionary<string, Object> _unityObjectDict = new Dictionary<string, Object>();

#if UNITY_EDITOR
        [HorizontalGroup("btn")]
        [Button]
        private void Save()
        {
            SerializedObject serializedObject = new SerializedObject(this);
            EditorUtility.SetDirty(this);
            serializedObject.ApplyModifiedProperties();
            serializedObject.UpdateIfRequiredOrScript();
        }

        [HorizontalGroup("btn")]
        [Button]
        private void Sort()
        {
            objects.Sort(new UReferenceUnityData());
            Save();
        }

        [HorizontalGroup("btn")]
        [Button]
        private void RemoveAll()
        {
            objects.Clear();
            _unityObjectDict.Clear();
            Save();
        }

        [HorizontalGroup("btn")]
        [Button]
        private void RemoveEmpty()
        {
            var removes = new List<int>();
            for (var i = 0; i < objects.Count; i++)
            {
                if (string.IsNullOrEmpty(objects[i].key) || objects[i].value == null)
                    removes.Add(i);
            }

            for (var i = removes.Count; i >= 0; i--)
                objects.RemoveAt(removes[i]);

            Save();
        }
#endif

        /// <summary>
        /// 泛型获取对象
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Get<T>(string key) where T : class
        {
            if (!_unityObjectDict.TryGetValue(key, out var value))
            {
                return null;
            }

            return value as T;
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Object Get(string key)
        {
            if (!_unityObjectDict.TryGetValue(key, out var value))
            {
                return null;
            }

            return value;
        }

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            _unityObjectDict.Clear();

            foreach (var obj in objects)
            {
                if (!_unityObjectDict.ContainsKey(obj.key))
                    _unityObjectDict.Add(obj.key, obj.value);
            }
        }
    }
}
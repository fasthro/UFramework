/*
 * @Author: fasthro
 * @Date: 2020-12-01 16:51:34
 * @Description: 消息事件
 */

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UFramework.Core
{
    public static class Messenger
    {
        #region Internal variables

        public static Dictionary<int, Delegate> EventDict = new Dictionary<int, Delegate>();

        //Message handlers that should never be removed, regardless of calling Cleanup
        public static List<int> permanentMessages = new List<int>();
        #endregion

        #region Helper methods

        public static void MarkAsPermanent(int eventType)
        {
            permanentMessages.Add(eventType);
        }

        public static void Cleanup()
        {
            List<int> messagesToRemove = new List<int>();

            foreach (KeyValuePair<int, Delegate> pair in EventDict)
            {
                bool wasFound = false;

                foreach (int message in permanentMessages)
                {
                    if (pair.Key == message)
                    {
                        wasFound = true;
                        break;
                    }
                }

                if (!wasFound)
                    messagesToRemove.Add(pair.Key);
            }

            foreach (int message in messagesToRemove)
            {
                EventDict.Remove(message);
            }
        }

        public static void PrintEventTable()
        {
            Logger.Debug("\t\t\t=== MESSENGER PrintEventTable ===");
            foreach (KeyValuePair<int, Delegate> pair in EventDict)
            {
                Logger.Debug("\t\t\t" + pair.Key + "\t\t" + pair.Value);
            }
            Logger.Debug("\n");
        }
        #endregion

        #region Message logging and exception throwing

        static void OnListenerAdding(int eventType, Delegate listenerBeingAdded)
        {
            if (!EventDict.ContainsKey(eventType))
            {
                EventDict.Add(eventType, null);
            }

            Delegate d = EventDict[eventType];
            if (d != null && d.GetType() != listenerBeingAdded.GetType())
            {
                throw new ListenerException(string.Format("Attempting to add listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being added has type {2}", eventType, d.GetType().Name, listenerBeingAdded.GetType().Name));
            }
        }

        static void OnListenerRemoving(int eventType, Delegate listenerBeingRemoved)
        {
            if (EventDict.ContainsKey(eventType))
            {
                Delegate d = EventDict[eventType];

                if (d == null)
                {
                    throw new ListenerException(string.Format("Attempting to remove listener with for event type \"{0}\" but current listener is null.", eventType));
                }
                else if (d.GetType() != listenerBeingRemoved.GetType())
                {
                    throw new ListenerException(string.Format("Attempting to remove listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being removed has type {2}", eventType, d.GetType().Name, listenerBeingRemoved.GetType().Name));
                }
            }
            else
            {
                throw new ListenerException(string.Format("Attempting to remove listener for type \"{0}\" but Messenger doesn't know about this event type.", eventType));
            }
        }

        static void OnListenerRemoved(int eventType)
        {
            if (EventDict[eventType] == null)
            {
                EventDict.Remove(eventType);
            }
        }

        static void OnBroadcasting(int eventType)
        {
            if (!EventDict.ContainsKey(eventType))
            {
                throw new BroadcastException(string.Format("Broadcasting message \"{0}\" but no listener found. Try marking the message with Messenger.MarkAsPermanent.", eventType));
            }
        }

        static BroadcastException CreateBroadcastSignatureException(int eventType)
        {
            return new BroadcastException(string.Format("Broadcasting message \"{0}\" but listeners have a different signature than the broadcaster.", eventType));
        }

        class BroadcastException : Exception
        {
            public BroadcastException(string msg)
                : base(msg)
            {
            }
        }

        class ListenerException : Exception
        {
            public ListenerException(string msg)
                : base(msg)
            {
            }
        }
        #endregion

        #region AddListener

        public static void AddListener(int eventType, UCallback handler)
        {
            OnListenerAdding(eventType, handler);
            EventDict[eventType] = (UCallback)EventDict[eventType] + handler;
        }

        public static void AddListener<T>(int eventType, UCallback<T> handler)
        {
            OnListenerAdding(eventType, handler);
            EventDict[eventType] = (UCallback<T>)EventDict[eventType] + handler;
        }

        public static void AddListener<T, U>(int eventType, UCallback<T, U> handler)
        {
            OnListenerAdding(eventType, handler);
            EventDict[eventType] = (UCallback<T, U>)EventDict[eventType] + handler;
        }

        public static void AddListener<T, U, V>(int eventType, UCallback<T, U, V> handler)
        {
            OnListenerAdding(eventType, handler);
            EventDict[eventType] = (UCallback<T, U, V>)EventDict[eventType] + handler;
        }

        public static void AddListener<T, U, V, Z>(int eventType, UCallback<T, U, V, Z> handler)
        {
            OnListenerAdding(eventType, handler);
            EventDict[eventType] = (UCallback<T, U, V, Z>)EventDict[eventType] + handler;
        }
        #endregion

        #region RemoveListener

        public static void RemoveListener(int eventType, UCallback handler)
        {
            OnListenerRemoving(eventType, handler);
            EventDict[eventType] = (UCallback)EventDict[eventType] - handler;
            OnListenerRemoved(eventType);
        }

        public static void RemoveListener<T>(int eventType, UCallback<T> handler)
        {
            OnListenerRemoving(eventType, handler);
            EventDict[eventType] = (UCallback<T>)EventDict[eventType] - handler;
            OnListenerRemoved(eventType);
        }

        public static void RemoveListener<T, U>(int eventType, UCallback<T, U> handler)
        {
            OnListenerRemoving(eventType, handler);
            EventDict[eventType] = (UCallback<T, U>)EventDict[eventType] - handler;
            OnListenerRemoved(eventType);
        }

        public static void RemoveListener<T, U, V>(int eventType, UCallback<T, U, V> handler)
        {
            OnListenerRemoving(eventType, handler);
            EventDict[eventType] = (UCallback<T, U, V>)EventDict[eventType] - handler;
            OnListenerRemoved(eventType);
        }

        public static void RemoveListener<T, U, V, Z>(int eventType, UCallback<T, U, V, Z> handler)
        {
            OnListenerRemoving(eventType, handler);
            EventDict[eventType] = (UCallback<T, U, V, Z>)EventDict[eventType] - handler;
            OnListenerRemoved(eventType);
        }
        #endregion

        #region Broadcast

        public static void Broadcast(int eventType)
        {
            OnBroadcasting(eventType);

            Delegate d;
            if (EventDict.TryGetValue(eventType, out d))
            {
                UCallback callback = d as UCallback;

                if (callback != null)
                {
                    callback();
                }
                else
                {
                    throw CreateBroadcastSignatureException(eventType);
                }
            }
        }

        public static void Broadcast<T>(int eventType, T arg1)
        {
            OnBroadcasting(eventType);

            Delegate d;
            if (EventDict.TryGetValue(eventType, out d))
            {
                UCallback<T> callback = d as UCallback<T>;

                if (callback != null)
                {
                    callback(arg1);
                }
                else
                {
                    throw CreateBroadcastSignatureException(eventType);
                }
            }
        }

        public static void Broadcast<T, U>(int eventType, T arg1, U arg2)
        {
            OnBroadcasting(eventType);

            Delegate d;
            if (EventDict.TryGetValue(eventType, out d))
            {
                UCallback<T, U> callback = d as UCallback<T, U>;

                if (callback != null)
                {
                    callback(arg1, arg2);
                }
                else
                {
                    throw CreateBroadcastSignatureException(eventType);
                }
            }
        }

        public static void Broadcast<T, U, V>(int eventType, T arg1, U arg2, V arg3)
        {
            OnBroadcasting(eventType);

            Delegate d;
            if (EventDict.TryGetValue(eventType, out d))
            {
                UCallback<T, U, V> callback = d as UCallback<T, U, V>;

                if (callback != null)
                {
                    callback(arg1, arg2, arg3);
                }
                else
                {
                    throw CreateBroadcastSignatureException(eventType);
                }
            }
        }

        public static void Broadcast<T, U, V, Z>(int eventType, T arg1, U arg2, V arg3, Z arg4)
        {
            OnBroadcasting(eventType);

            Delegate d;
            if (EventDict.TryGetValue(eventType, out d))
            {
                UCallback<T, U, V, Z> callback = d as UCallback<T, U, V, Z>;

                if (callback != null)
                {
                    callback(arg1, arg2, arg3, arg4);
                }
                else
                {
                    throw CreateBroadcastSignatureException(eventType);
                }
            }
        }
        #endregion
    }
}
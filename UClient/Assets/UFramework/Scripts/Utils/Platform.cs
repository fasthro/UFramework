/*
 * @Author: fasthro
 * @Date: 2020-10-23 14:38:21
 * @Description: Platform
 */
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace UFramework
{
    public static class Platform
    {
        public const int Unknow = -1; const string STR_Unknow = "Unknow";

        public const int Android = 0; const string STR_Android = "Android";
        public const int iOS = 1; const string STR_iOS = "iOS";
        public const int StandaloneWindows = 2; const string STR_StandaloneWindows = "Windows";
        public const int StandaloneOSX = 3; const string STR_StandaloneOSX = "OSX";


#if UNITY_EDITOR

        public static int BuildTargetCurrent
        {
            get
            {
                switch (EditorUserBuildSettings.activeBuildTarget)
                {
                    case BuildTarget.Android:
                        return Android;
                    case BuildTarget.iOS:
                        return iOS;
                    case BuildTarget.StandaloneWindows:
                    case BuildTarget.StandaloneWindows64:
                        return StandaloneWindows;
                    case BuildTarget.StandaloneOSX:
                        return StandaloneOSX;
                    default:
                        return Unknow;
                }
            }
        }

        public static string BuildTargetCurrentName { get { return ToString(BuildTargetCurrent); } }

        public static bool Equal(BuildTarget target, int platform)
        {
            switch (target)
            {
                case BuildTarget.Android:
                    return platform == Android;
                case BuildTarget.iOS:
                    return platform == iOS;
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    return platform == StandaloneWindows;
                case BuildTarget.StandaloneOSX:
                    return platform == StandaloneOSX;
                default:
                    return false;
            }
        }

        public static string ToString(BuildTarget target)
        {
            switch (target)
            {
                case BuildTarget.Android:
                    return STR_Android;
                case BuildTarget.iOS:
                    return STR_iOS;
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    return STR_StandaloneWindows;
                case BuildTarget.StandaloneOSX:
                    return STR_StandaloneOSX;
                default:
                    return STR_Unknow;
            }
        }
#endif

        public static int RuntimePlatformCurrent
        {
            get
            {
                switch (Application.platform)
                {
                    case RuntimePlatform.Android:
                        return Android;
                    case RuntimePlatform.IPhonePlayer:
                        return iOS;
                    case RuntimePlatform.WindowsPlayer:
                        return StandaloneWindows;
                    case RuntimePlatform.WindowsEditor:
#if UNITY_ANDROID
                        return Android;
#elif UNITY_IOS
                        return iOS;
#else
                        return StandaloneOSX;
#endif
                    case RuntimePlatform.OSXPlayer:
                        return StandaloneOSX;
                    case RuntimePlatform.OSXEditor:
#if UNITY_ANDROID
                        return Android;
#elif UNITY_IOS
                        return iOS;
#else
                        return StandaloneOSX;
#endif
                    default:
                        return Unknow;
                }
            }
        }

        public static string RuntimePlatformCurrentName { get { return ToString(RuntimePlatformCurrent); } }

        public static bool Equal(RuntimePlatform target, int platform)
        {
            switch (target)
            {
                case RuntimePlatform.Android:
                    return platform == Android;
                case RuntimePlatform.IPhonePlayer:
                    return platform == iOS;
                case RuntimePlatform.WindowsPlayer:
                    return platform == StandaloneWindows;
                case RuntimePlatform.WindowsEditor:
#if UNITY_ANDROID
                    return platform == Android;
#elif UNITY_IOS
                    return platform == iOS;
#else
                    return platform == StandaloneWindows;
#endif
                case RuntimePlatform.OSXPlayer:
                    return platform == StandaloneOSX;
                case RuntimePlatform.OSXEditor:
#if UNITY_ANDROID
                    return platform == Android;
#elif UNITY_IOS
                    return platform == iOS;
#else
                    return platform == StandaloneOSX;
#endif
                default:
                    return false;
            }
        }

        public static string ToString(RuntimePlatform target)
        {
            switch (target)
            {
                case RuntimePlatform.Android:
                    return STR_Android;
                case RuntimePlatform.IPhonePlayer:
                    return STR_iOS;
                case RuntimePlatform.WindowsPlayer:
                    return STR_StandaloneWindows;
                case RuntimePlatform.WindowsEditor:
#if UNITY_ANDROID
                    return STR_Android;
#elif UNITY_IOS
                    return STR_iOS;
#else
                    return STR_StandaloneWindows;
#endif
                case RuntimePlatform.OSXPlayer:
                    return STR_StandaloneOSX;
                case RuntimePlatform.OSXEditor:
#if UNITY_ANDROID
                    return STR_Android;
#elif UNITY_IOS
                    return STR_iOS;
#else
                    return STR_StandaloneOSX;
#endif
                default:
                    return STR_Unknow;
            }
        }

        public static string ToString(int platform)
        {
            if (platform == Android) return STR_Android;
            else if (platform == iOS) return STR_iOS;
            else if (platform == StandaloneWindows) return STR_StandaloneWindows;
            else if (platform == StandaloneOSX) return STR_StandaloneOSX;
            else return STR_Unknow;
        }
    }
}
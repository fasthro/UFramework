#define USING_DOTWEENING

using UnityEngine;
using System;
using System.Collections.Generic;
using LuaInterface;
using UnityEditor;

using BindType = ToLuaMenu.BindType;
using UnityEngine.UI;

using System.IO;

public static class CustomSettings
{
    public static string FrameworkPath = Application.dataPath;
    public static string saveDir = FrameworkPath + "/ToLua/Source/Generate/";
    public static string luaDir = FrameworkPath + "/Lua/";
    public static string toluaBaseType = FrameworkPath + "/ToLua/BaseType/";
	public static string baseLuaDir = FrameworkPath + "/ToLua/Lua";
	public static string injectionFilesPath = Application.dataPath + "/ToLua/Injection/";

    //lua print或者error重定向
    public const int PRINTLOGLINE = 208;                //ToLua.Print函数中Debugger.Log位置
    public const int PCALLERRORLINE = 810;              //LuaState.Pcall函数中throw位置
    public const int LUADLLERRORLINE = 803;             //LuaDLL.luaL_argerror函数中throw位置

    public const string LUAJIT_CMD_OPTION = "-b -g";    //luajit.exe 编译命令行参数

    //导出时强制做为静态类的类型(注意customTypeList 还要添加这个类型才能导出)
    //unity 有些类作为sealed class, 其实完全等价于静态类
    public static List<Type> staticClassTypes = new List<Type>
    {        
        typeof(UnityEngine.Application),
        typeof(UnityEngine.Time),
        typeof(UnityEngine.Screen),
        typeof(UnityEngine.SleepTimeout),
        typeof(UnityEngine.Resources),
        typeof(UnityEngine.Graphics),
    };

    //附加导出委托类型(在导出委托时, customTypeList 中牵扯的委托类型都会导出， 无需写在这里)
    public static DelegateType[] customDelegateList = 
    {        
        _DT(typeof(Action)),                
        _DT(typeof(UnityEngine.Events.UnityAction)),
        _DT(typeof(System.Predicate<int>)),
        _DT(typeof(System.Action<int>)),
        _DT(typeof(System.Comparison<int>)),
        _DT(typeof(System.Func<int, int>)),
    };

    //在这里添加你要导出注册到lua的类型列表
    public static BindType[] customTypeList =
    {                
        _GT(typeof(LuaInjectionStation)),
        _GT(typeof(InjectType)),
        _GT(typeof(Debugger)).SetNameSpace(null),          

#if USING_DOTWEENING
        _GT(typeof(DG.Tweening.DOTween)),
        _GT(typeof(DG.Tweening.Tween)).SetBaseType(typeof(System.Object)).AddExtendType(typeof(DG.Tweening.TweenExtensions)),
        _GT(typeof(DG.Tweening.Sequence)).AddExtendType(typeof(DG.Tweening.TweenSettingsExtensions)),
        _GT(typeof(DG.Tweening.Tweener)).AddExtendType(typeof(DG.Tweening.TweenSettingsExtensions)),
        _GT(typeof(DG.Tweening.LoopType)),
        _GT(typeof(DG.Tweening.PathMode)),
        _GT(typeof(DG.Tweening.PathType)),
        _GT(typeof(DG.Tweening.RotateMode)),
#endif
        _GT(typeof(Component)),
        _GT(typeof(Transform)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
        _GT(typeof(Button)),
        _GT(typeof(Slider)),
        _GT(typeof(RectTransform)),
        _GT(typeof(RectTransform.Axis)),
        _GT(typeof(Text)),
        _GT(typeof(Image)),
        _GT(typeof(CanvasGroup)),
        _GT(typeof(ContentSizeFitter)),

        _GT(typeof(Behaviour)),
        _GT(typeof(MonoBehaviour)),        
        _GT(typeof(GameObject)),
        _GT(typeof(Application)),
        _GT(typeof(Time)),        
        _GT(typeof(Texture)),
        _GT(typeof(Texture2D)),
        _GT(typeof(Material)),
        _GT(typeof(Sprite)),
        _GT(typeof(Shader)),        
        _GT(typeof(Renderer)),
        _GT(typeof(Screen)),        
        _GT(typeof(AudioClip)),        
        _GT(typeof(Animator)),
        _GT(typeof(Animation)),        
		_GT(typeof(Resources)),   
        _GT(typeof(Debug)),
        _GT(typeof(PlayerPrefs)),
        _GT(typeof(SpriteRenderer)),
        _GT(typeof(Path)),
        _GT(typeof(Camera)),
    };

    public static List<Type> dynamicList = new List<Type>()
    {
        typeof(MeshRenderer),
        typeof(Animation),
        typeof(AnimationClip),
        typeof(AnimationState),
    };

    //重载函数，相同参数个数，相同位置out参数匹配出问题时, 需要强制匹配解决
    //使用方法参见例子14
    public static List<Type> outList = new List<Type>()
    {
    };
        
    //ngui优化，下面的类没有派生类，可以作为sealed class
    public static List<Type> sealedList = new List<Type>()
    {
        /*typeof(Transform),
        typeof(UIRoot),
        typeof(UICamera),
        typeof(UIViewport),
        typeof(UIPanel),
        typeof(UILabel),
        typeof(UIAnchor),
        typeof(UIAtlas),
        typeof(UIFont),
        typeof(UITexture),
        typeof(UISprite),
        typeof(UIGrid),
        typeof(UITable),
        typeof(UIWrapGrid),
        typeof(UIInput),
        typeof(UIScrollView),
        typeof(UIEventListener),
        typeof(UIScrollBar),
        typeof(UICenterOnChild),
        typeof(UIScrollView),        
        typeof(UIButton),
        typeof(UITextList),
        typeof(UIPlayTween),
        typeof(UIDragScrollView),
        typeof(UISpriteAnimation),
        typeof(UIWrapContent),
        typeof(TweenWidth),
        typeof(TweenAlpha),
        typeof(TweenColor),
        typeof(TweenRotation),
        typeof(TweenPosition),
        typeof(TweenScale),
        typeof(TweenHeight),
        typeof(TypewriterEffect),
        typeof(UIToggle),
        typeof(Localization),*/
    };

    public static BindType _GT(Type t)
    {
        return new BindType(t);
    }

    public static DelegateType _DT(Type t)
    {
        return new DelegateType(t);
    }    


    [MenuItem("Lua/Attach Profiler", false, 151)]
    static void AttachProfiler()
    {
        if (!Application.isPlaying)
        {
            EditorUtility.DisplayDialog("警告", "请在运行时执行此功能", "确定");
            return;
        }

        LuaClient.Instance.AttachProfiler();
    }

    [MenuItem("Lua/Detach Profiler", false, 152)]
    static void DetachProfiler()
    {
        if (!Application.isPlaying)
        {            
            return;
        }

        LuaClient.Instance.DetachProfiler();
    }
}

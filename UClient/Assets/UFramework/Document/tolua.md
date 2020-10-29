
### 升级指南

##### 1.  覆盖以下所有目录

- ToLua
- ToLua\Editor
- ToLua\Plugins
- ToLua\Source
- ToLua\ToLua

##### 2.  注释掉 TuLuaMenu.cs 内代码
```csharp
// static ToLuaMenu()
// {
//     string dir = CustomSettings.saveDir;
//     string[] files = Directory.GetFiles(dir, "*.cs", SearchOption.TopDirectoryOnly);

//     if (files.Length < 3 && beCheck)
//     {
//         if (EditorUtility.DisplayDialog("自动生成", "点击确定自动生成常用类型注册文件， 也可通过菜单逐步完成此功能", "确定", "取消"))
//         {
//             beAutoGen = true;
//             GenLuaDelegates();
//             AssetDatabase.Refresh();
//             GenerateClassWraps();
//             GenLuaBinder();
//             beAutoGen = false;                
//         }

//         beCheck = false;
//     }
// }

```
#### 3.修改 GenLuaAll 方法的访问权限为 public
```csharp
public static void GenLuaAll()
```
#### 4.修改 LuaConst.cs 里面对应路径
```csharp
public static string luaDir = Application.dataPath + "/Scripts/Lua";     //lua逻辑代码目录
public static string toluaDir = Application.dataPath + "/UFramework/3rd/ToLua/ToLua/Lua";        //tolua lua文件目录

```
#### 5.在 tolua.lua 最上面添加
```lua
require("typesys.TypeSystemHeader")
```

#### 6.在 ToLuaExport.cs 得 memberFilter 数组中添加，解决Unity版本API编译报错
```csharp
// UFramework CUSTOM
"MeshRenderer.scaleInLightmap",
"MeshRenderer.receiveGI",
"MeshRenderer.stitchLightmapSeams",
```

#### 7.在 LuaState.cs 中的 InitLuaPath 的函数中进行修改，注释掉路径添加
```csharp
void InitLuaPath()
{
    InitPackagePath();
    
    // lua 路径添加已经在UFramework中实现，不需要在此处进行添加
    //             if (!LuaFileUtils.Instance.beZip)
    //             {
    // #if UNITY_EDITOR
    //                 if (!Directory.Exists(LuaConst.luaDir))
    //                 {
    //                     string msg = string.Format("luaDir path not exists: {0}, configer it in LuaConst.cs", LuaConst.luaDir);
    //                     throw new LuaException(msg);
    //                 }

    //                 if (!Directory.Exists(LuaConst.toluaDir))
    //                 {
    //                     string msg = string.Format("toluaDir path not exists: {0}, configer it in LuaConst.cs", LuaConst.toluaDir);
    //                     throw new LuaException(msg);
    //                 }

    //                 AddSearchPath(LuaConst.toluaDir);
    //                 AddSearchPath(LuaConst.luaDir);
    // #endif
    //                 if (LuaFileUtils.Instance.GetType() == typeof(LuaFileUtils))
    //                 {
    //                     AddSearchPath(LuaConst.luaResDir);
    //                 }
    //             }
}
```

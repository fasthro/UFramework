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
require("TypeSystemHeader")
```

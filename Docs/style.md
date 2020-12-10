### C# 编码风格

```csharp
/*
 * @Author: fasthro
 * @Date: 2020-12-10 11:09:33
 * @Description: 类描述
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSharpStyle
{
    /// <summary>
    /// 常量全大写
    /// </summary>
    public const int MAX_COUNT = 1;

    /// <summary>
    /// 静态变量首字母大写，驼峰格式
    /// </summary>
    public static int MaxCount;

    /// <summary>
    /// 公共变量首字母小写，驼峰格式
    /// </summary>
    public int maxCount;

    /// <summary>
    /// 私有变量下划线开头,驼峰格式
    /// </summary>
    private int _maxCount;

    /// <summary>
    /// 保护变量下划线开头,驼峰格式
    /// </summary>
    protected int _minCount;

    /// <summary>
    /// 数组和List后缀 <s>
    /// </summary>
    public int[] nums = new int[10];
    public List<int> numbers = new List<int>();

    /// <summary>
    /// 字典后缀 <Dict>
    /// </summary>
    public Dictionary<int, int> numDict = new Dictionary<int, int>();
}
```

###Lua 编码风格
```
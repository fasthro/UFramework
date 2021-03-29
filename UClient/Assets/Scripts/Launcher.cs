// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020-12-15 14:35:37
// * @Description:
// --------------------------------------------------------------------------------

using Lockstep.Logic;
using UFramework;
using UFramework.Core;
using UnityEngine;

public class Launcher : AppLauncher
{
    public static Launcher instance { get; private set; }

    private LauncherClient _lockstepClient;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        instance = this;
        Initialize();
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        _lockstepClient = new LauncherClient();
        _lockstepClient.Initialize();

        var count = 10;
        var ts = new GameObject[count];
        for (int i = 0; i < count; i++)
        {
            ts[i] = GoPool.Instance.Allocate("Assets/Arts/Test/Cube.prefab");
        }
        
        for (int i = 0; i < 5; i++)
        {
            GoPool.Instance.Recycle(ts[i]);
        }
    }

    void Update()
    {
        if (!isInitialized)
            return;

        DoUpdate(Time.deltaTime);
        _lockstepClient.Update();
    }

    void OnDestroy()
    {
        if (!isInitialized)
            return;

        DoDispose();
        _lockstepClient.Dispose();
    }
}
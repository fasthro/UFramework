/*
 * @Author: fasthro
 * @Date: 2020-12-15 14:35:37
 * @Description: Lockstep Launcher
 */

using Lockstep.Logic;
using UFramework;
using UnityEngine;

public class Launcher : AppLauncher
{
    public static Launcher instance { get; private set; }

    private LauncherClient _lockstepClient;

    private void Awake()
    {
        instance = this;
        Initialize();
        _lockstepClient = new LauncherClient();
        
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        _lockstepClient.Initialize();
    }

    private void Update()
    {
        DoUpdate(Time.deltaTime);
        _lockstepClient.Update();
    }

    private void OnDestroy()
    {
        DoDispose();
        _lockstepClient.Dispose();
    }
}
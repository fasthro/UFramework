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
    private LauncherClient _lockstepClient;

    private void Awake()
    {
        Initialize();
        _lockstepClient = new LauncherClient();
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
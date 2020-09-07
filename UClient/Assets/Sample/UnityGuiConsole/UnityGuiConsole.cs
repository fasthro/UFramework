using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using System.Collections.Generic;

public class UnityGuiConsole : MonoBehaviour
{
    public static UnityGuiConsole Instance { get; private set; }

    private static readonly int MAX_LOG = 250;
    private static readonly int WND_ID = 0x1435;
    private static readonly float EDGE_X = 350, EDGE_Y = 0;

    public bool Visible = false;

    private readonly string[] logTypeNames_;
    private readonly Queue<string>[] logList_;
    private readonly Vector2[] scrollPos_;

    private UnityGuiConsole()
    {
        this.logTypeNames_ = Enum.GetNames(typeof(LogType));
        for (int i = 0; i < this.logTypeNames_.Length; i++)
        {
            this.logTypeNames_[i] = "\n" + this.logTypeNames_[i] + "\n";
        }
        this.logList_ = new Queue<string>[this.logTypeNames_.Length];
        this.scrollPos_ = new Vector2[this.logTypeNames_.Length];
        for (int i = 0; i < logList_.Length; ++i)
        {
            this.logList_[i] = new Queue<string>(MAX_LOG);
            this.scrollPos_[i] = new Vector2(0, 1);
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        enabled = true;
        Instance = this;
        Application.logMessageReceived += LogCallback;
    }

    private float CoolDown_ = 0;
    void Update()
    {
        if ((Input.touches.Length >= 3 || (Input.GetMouseButton(0) && Input.GetMouseButton(1)))
            && Time.time - CoolDown_ > 2.0f)
        {
            Visible = !Visible;
            CoolDown_ = Time.time;
        }

        checkVisibility();
    }

    private bool _cheatMenuVisible = false;
    private float _menuTimer = 0;
    private void checkVisibility()
    {
#if ((!UNITY_ANDROID && !UNITY_IPHONE) || UNITY_EDITOR)
        if (Input.GetMouseButton(0))
        {
            Vector3 inputPos = Input.mousePosition;
            inputPos.y = Screen.height - inputPos.y;
#else
		if(Input.touches.Length > 0 && 
			((Input.GetTouch(0).phase != TouchPhase.Ended) &&
				(Input.GetTouch(0).phase != TouchPhase.Canceled))) 
		{
			Vector3 inputPos = Input.GetTouch(0).position;
			inputPos.y = Screen.height - inputPos.y;
#endif

            float xPos = Screen.width - 150;
            float yPos = 0;

            Rect rect = new Rect(xPos, yPos, 150, 150);
            if ((inputPos.x > rect.x) && (inputPos.x < rect.x + rect.width)
                && (inputPos.y > rect.y && inputPos.y < rect.y + rect.height))
            {
                _menuTimer += Time.deltaTime;
                if (_menuTimer > 2)
                {
                    _menuTimer = 0;
                    Visible = !Visible;
                }
            }
        }
    }




    private int logTypeChoose_ = (int)LogType.Log;
    private Rect rcWindow_;

    void OnGUI()
    {
        if (!Visible) { return; }
        EventType et = Event.current.type;
        if (et == EventType.Repaint || et == EventType.Layout)
        {
            this.rcWindow_ = new Rect(EDGE_X, EDGE_Y, Screen.width - EDGE_X * 2, Screen.height - EDGE_Y - 8);
            GUI.Window(WND_ID, rcWindow_, WindowFunc, string.Empty);
        }
    }
    GUIStyle fontStyle = new GUIStyle();

    void WindowFunc(int id)
    {
        try
        {
            GUILayout.BeginVertical();
            try
            {
                logTypeChoose_ = GUILayout.Toolbar(logTypeChoose_, this.logTypeNames_);
                var queue = this.logList_[logTypeChoose_];
                if (queue.Count > 0)
                {
                    scrollPos_[logTypeChoose_] = GUILayout.BeginScrollView(scrollPos_[logTypeChoose_]);
                    try
                    {
                        foreach (var s in queue)
                        {
                            fontStyle.fontSize = 20;
                            fontStyle.normal.textColor = new Color(1, 1f, 0.5f);
                            GUILayout.Label(s, fontStyle);
                        }
                    }
                    finally
                    {
                        GUILayout.EndScrollView();
                    }
                }
            }
            finally
            {
                GUILayout.EndVertical();
            }
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogException(ex);
        }
    }

    static void Enqueue(Queue<string> queue, string text, string stackTrace)
    {
        while (queue.Count >= MAX_LOG)
        {
            queue.Dequeue();
        }
        queue.Enqueue(text);
        if (!string.IsNullOrEmpty(stackTrace))
        {
            queue.Enqueue(stackTrace);
        }
    }

    void LogCallback(string condition, string stackTrace, LogType type)
    {
        int index = (int)type;
        var queue = this.logList_[index];
        switch (type)
        {
            case LogType.Exception:
            case LogType.Error:
            case LogType.Warning:
                Enqueue(queue, condition, stackTrace);
                break;
            default:
                Enqueue(queue, condition, null);
                break;
        }
        this.scrollPos_[index] = new Vector2(0, 100000f);
    }


}
/*
 * @Author: fasthro
 * @Date: 2020-09-22 17:46:01
 * @Description: TImer Sample
 */
using System.Collections;
using System.Collections.Generic;
using UFramework.Core;
using UnityEngine;

namespace UFramework.Sample
{
    public class TimerSample : SampleScene
    {
        private string secondText;
        private string completeText;

        private bool isLooped;
        private bool usesRealTime;

        private TimerEntity timer;

        protected override void OnRenderGUI()
        {
            GUILayout.Label("duration:");
            secondText = GUI.TextField(new Rect(55, 105, 50, 20), secondText);
            isLooped = GUILayout.Toggle(isLooped, "isLooped");
            usesRealTime = GUILayout.Toggle(usesRealTime, "usesRealTime");
            if (GUILayout.Button("Wait Seconds", GUILayout.Width(300), GUILayout.Height(100)))
            {
                completeText = "waiting...";
                if (timer != null)
                {
                    timer.Cancel();
                }
                timer = Timer.Add(float.Parse(secondText), OnTimerCompleted, OnTimerUpdate, isLooped, usesRealTime);
            }
            GUILayout.Label(completeText);
        }

        private void OnTimerCompleted()
        {
            completeText = "timer is completed";
        }

        private void OnTimerUpdate(float time)
        {
            Debug.Log("timer update: " + time);
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public static void ClearConsole()
    {
        // This simply does "LogEntries.Clear()" the long way:
        var logEntries = System.Type.GetType("UnityEditor.LogEntries, UnityEditor.dll");
        var clearMethod = logEntries.GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
        clearMethod.Invoke(null, null);
    }


    public static float Approach(float value, float target, float maxMove)
    {
        return value > target ? Mathf.Max(value - maxMove, target) : Mathf.Min(value + maxMove, target);
    }

    public static float Transpose(float currentValue, float currentMin, float currentMax, float newMin, float newMax)
    {
        return (currentValue - currentMin) / (currentMax - currentMin) * (newMax - newMin) + newMin;
    }

    public static void DelayedExecution(MonoBehaviour context, float delay, Action onExecute)
    {
        IEnumerator DelayRoutine(float d, Action a)
        {
            yield return new WaitForSeconds(d);
            a?.Invoke();
        }
        context.StartCoroutine(DelayRoutine(delay, onExecute));
    }

    public static void CallDelay(MonoBehaviour context, float delay, Action onExecute)
    {
        Utils.DelayedExecution(context, delay, onExecute);
    }
}

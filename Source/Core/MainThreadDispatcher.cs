using BrilliantSkies.Core.Timing.Internal;
using BrilliantSkies.PlayerProfiles;
using MTMTVFX.Core;
using MTMTVFX.Effects;
using MTMTVFX.Effects.Muzzle;
using MTMTVFX.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainThreadDispatcher : MonoBehaviour
{
    public static MainThreadDispatcher Instance => _instance;
    private static MainThreadDispatcher _instance;
    private static readonly Queue<Action> _actions = new Queue<Action>();
    private static readonly object _lock = new object();

    private static void Init()
    {
        if (_instance == null)
        {
            GameObject go = new GameObject("MTMT MainThreadDispatcher");
            _instance = go.AddComponent<MainThreadDispatcher>();
            DontDestroyOnLoad(go);
        }
    }

    /// <summary>Enqueue an action to run on the main thread.</summary>
    public static void Enqueue(Action action)
    {
        Init();
        if (action == null) return;

        lock (_lock)
        {
            _actions.Enqueue(action);
        }
    }

    private void FixedUpdate()
    {
        lock (_lock)
        {
            while (_actions.Count > 0)
            {
                _actions.Dequeue()?.Invoke();
            }
        }

        OnFixedUpdate();
    }

    private void OnFixedUpdate()
    {
        if (Utils.GetConfig().E_CONTINUOUS) LaserVFXPatchContinuous.UpdateContBeams();
    }

    private IEnumerator CR(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);
        action.Invoke();
        yield break;
    }
}
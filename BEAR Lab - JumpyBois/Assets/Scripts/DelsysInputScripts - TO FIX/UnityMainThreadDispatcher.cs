using System;
using System.Collections.Generic;
using UnityEngine;

public class UnityMainThreadDispatcher : MonoBehaviour {
    private static UnityMainThreadDispatcher instance;
    private static readonly Queue<Action> actionQueue = new Queue<Action>();

    private void Awake() {
        if (instance != null) {
            Destroy(gameObject);
            return;
        }

        instance = this;
        // DontDestroyOnLoad(gameObject);
    }

    private void Update() {
        lock (actionQueue) {
            while (actionQueue.Count > 0) {
                Action action = actionQueue.Dequeue();
                action.Invoke();
            }
        }
    }

    public static void ExecuteOnMainThread(Action action) {
        lock (actionQueue) {
            actionQueue.Enqueue(action);
        }
    }
}

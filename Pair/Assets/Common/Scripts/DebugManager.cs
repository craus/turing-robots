using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using RSG;

public class DebugManager : MonoBehaviour
{
    void Awake() {
        Promise.EnablePromiseTracking = true;
        Promise.UnhandledException += (object sender, ExceptionEventArgs e) => {
            Debug.LogErrorFormat("Unhandled exception from promises: {0}, {1}", sender, e.Exception);
        };
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            Debug.LogFormat("Pending promises: {0}", Promise.pendingPromises);
        }
    }
}

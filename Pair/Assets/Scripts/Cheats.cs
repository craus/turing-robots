using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Cheats : MonoBehaviour
{
    public static bool on = false;
    public static bool hacked = false;

    public Text cheatingText;

    public const string cheat = "hack";
    public int score = 0;

    void Start() {
        Enable(on);
    }

    void Enable(bool on) {
        Debug.Log(string.Format("Cheats {0}", on ? "on" : "off"));
        Cheats.on = on;
		if (cheatingText != null) {
			cheatingText.enabled = false;
		}
    }

    void Update() {
        bool correctKey = false;

        if (Debug.isDebugBuild && !hacked) {
            hacked = true;
        }

        if (score == cheat.Length && !hacked) {
            on = true;
            hacked = true;
            score = 0;
        } else {
            if (Input.GetKeyDown(KeyCode.H) && cheat[score] == 'h') { 
                score++;
                correctKey = true;
            } 
            if (Input.GetKeyDown(KeyCode.A) && cheat[score] == 'a') { 
                score++;
                correctKey = true;
            } 
            if (Input.GetKeyDown(KeyCode.C) && cheat[score] == 'c') { 
                score++;
                correctKey = true;
            } 
            if (Input.GetKeyDown(KeyCode.K) && cheat[score] == 'k') { 
                score++;
                correctKey = true;
            } 
            if (Input.anyKeyDown && !correctKey) {
                score = 0;
            }
        }
        if (hacked && Input.GetKeyDown(KeyCode.ScrollLock)) {
            Enable(!on);
        }

		if (on) {
			if (Input.GetKeyDown(KeyCode.R)) {
				//Board.instance.Generate();
			}
		}
    }
}
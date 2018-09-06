using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class CommandMachine : MonoBehaviour {
	public static CommandMachine instance;

	public bool playing = false;
	public bool launched = false;
	public bool finished = false;

	public float lastStep = float.NegativeInfinity;
	public float period = 0.25f;

	public float basePeriod = 0.25f;
	public float fasterPeriod = 0.05f;

	public int steps = 0;

	public List<CommandGraphInterpreter> interpreters;

	public CommandGraphInterpreter currentInterpreter;

	public void Awake() {
		instance = this;
	}

	public void Step() {
		if (finished) {
			return;
		}

		launched = true;
		++steps;

		if (currentInterpreter == null) {
			currentInterpreter = interpreters.First();
		}

		currentInterpreter.Step();
		currentInterpreter = interpreters.CyclicNext(currentInterpreter);
	}

	public void Play() {
		playing = true;
		period = basePeriod;
		Step();
		lastStep = Time.time;
	}

	public void Faster() {
		Play();
		period = fasterPeriod;
	}

	public void Pause() {
		playing = false;
	}

	public void Reset() {
		launched = false;
		playing = false;
		finished = false;
		steps = 0;
	}

	public void Update() {
		if (Input.GetButtonDown("Step")) {
			Step();
		}
		if (Input.GetButtonDown("Pause")) {
			Pause();
		}
		if (Input.GetButtonDown("Play")) {
			Play();
		}	
		if (Input.GetButtonDown("Faster")) {
			Faster();
		}	
		if (playing) {
			for (int i = 0; i < 100; i++) {
				if (playing && Time.time > lastStep + period) {
					Step();
					lastStep += period;
				}
			}
		}
	}
}

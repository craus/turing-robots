﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class Exit : MonoBehaviour {
	public void Activate(Figure figure) {
		var interpreter = figure.GetComponentInChildren<CommandGraphInterpreter>();
		if (interpreter != null) {
			interpreter.finished = true;
			if (MazeProblem.instance.current.interpreters.All(i => i.finished)) {
				MazeProblem.instance.CompleteTest();
			}
		}
	}
}

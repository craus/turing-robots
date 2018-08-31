using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public abstract class Command : MonoBehaviour {
	public virtual string Run() {
		return "does nothing";
	}
}

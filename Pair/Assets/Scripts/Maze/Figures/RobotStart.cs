using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using UnityEngine.Events;

public class RobotStart : MonoBehaviour {
	public Figure robotPrefab;

	public Figure Spawn() {
		return Instantiate(robotPrefab).Place(GetComponent<Figure>().position);
	}
}

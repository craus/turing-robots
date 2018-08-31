using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public abstract class FigureProvider : MonoBehaviour, Provider<Figure> {
	public abstract Figure Get();
}

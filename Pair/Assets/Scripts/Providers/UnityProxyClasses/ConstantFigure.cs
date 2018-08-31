using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class ConstantFigure : FigureProvider {
	public Figure value;

	public override Figure Get() {
		return value;
	}
}

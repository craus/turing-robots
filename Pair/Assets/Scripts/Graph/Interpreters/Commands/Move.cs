using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class Move : Command {
	public FigureProvider figure;

	public int dx;
	public int dy;

	public CommandGraphInterpreter interpreter;

	public override string Run() {
		if (figure.Get() == null) {
			return "destroyed";
		}

		var location = figure.Get().position.Shifted(dx, dy);
		if (location == null || location.figures.Any(f => f.FigureType() == Library.instance.wallType)) {
			interpreter.collided = true;
			return "collided";
		} else {
			interpreter.collided = false;
			figure.Get().Place(location);
			return "moved by ({0}, {1})".i(dx, dy);
		}
	}
}

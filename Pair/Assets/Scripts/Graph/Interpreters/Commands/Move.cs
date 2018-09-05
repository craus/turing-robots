using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class Move : Command {
	public int dx;
	public int dy;

	public override string Run(CommandGraphInterpreter interpreter) {
		var location = interpreter.robot.position.Shifted(dx, dy);
		if (location == null || location.figures.Any(f => f.FigureType() == Library.instance.wallType)) {
			interpreter.collided = true;
			return "collided";
		} else {
			interpreter.collided = false;
			interpreter.robot.Place(location);
			return "moved by ({0}, {1})".i(dx, dy);
		}
	}
}

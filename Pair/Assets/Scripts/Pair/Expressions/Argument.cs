﻿using System.Collections.Generic;

public class Argument : Expression
{
	public string name;

	public int index;

	public Argument(string name, int index) {
		this.name = name;
		this.index = index;
	}

	public override Calculatable Evaluate(params Calculatable[] argumentValues) {
		return argumentValues[index];
	}

	public override string ToString() {
		return name;
	}
}
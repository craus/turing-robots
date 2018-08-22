using System.Collections.Generic;

public abstract class Expression
{
	public abstract Calculatable Evaluate(params Calculatable[] argumentValues);
}
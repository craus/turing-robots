using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class PairError : Exception
{
	public PairError(string message) 
		:base(message)
	{
	}
}

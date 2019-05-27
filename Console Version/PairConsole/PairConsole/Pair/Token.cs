using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Token
{
	public string text;
	public CodePosition position;

	public Token(string text, CodePosition position) {
		this.text = text;
		this.position = position;
	}

	public static implicit operator string(Token t) {
		return t.text;
	}

	public override string ToString() {
		return "{0} ({1})".i(text, position);
	}
}
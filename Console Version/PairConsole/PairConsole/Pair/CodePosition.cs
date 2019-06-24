using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class CodePosition
{
	public string file;
	public int line;
	public int character;

	public CodePosition(string file, int line, int character) {
		this.file = file;
		this.line = line;
		this.character = character;
	}

	public override string ToString() {
		return "{0} {1}:{2}".i(file, line, character);
	}
}

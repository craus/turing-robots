using System;

namespace Pair
{

	public class CommandObject : Object
	{
		public Func<Func<CommandObject>> command;

		public CommandObject(Func<Func<CommandObject>> command) {
			this.command = command;
		}

		public override string Structure() {
			return string.Format("command");
		}
	}
}
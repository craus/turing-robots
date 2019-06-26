using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pair
{
	public class ConsoleIO
	{
		public Buffer inputBuffer;
		public Buffer outputBuffer;

		public Calculatable one = new Calculatable(new PairObject(null, null));
		public Calculatable zero = new Calculatable(null);

		public void Bin(Action<Calculatable> callback) {
			bool bit = inputBuffer.ReadBit();
			callback(bit ? one : zero);
		}

		public void Eof(Action<Calculatable> callback) {
			bool bit = inputBuffer.Eof();
			callback( bit ? one : zero);
		}

		public void Bout(bool bit, Action callback) {
			outputBuffer.WriteBit(bit);
			callback();
		}

	}
}
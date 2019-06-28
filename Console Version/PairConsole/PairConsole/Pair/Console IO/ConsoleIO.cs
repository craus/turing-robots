using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pair
{
	public class ConsoleIO
	{
		public Buffer inputBuffer = new Buffer();
		public Buffer outputBuffer = new Buffer();

		public static Calculatable zero = new Calculatable(null);
		public static Calculatable one = new Calculatable(new PairObject(zero, zero));

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
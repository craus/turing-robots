using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pair
{
	public class Buffer
	{
		public int c = 0;
		public int cnt = -1;

		public int wcnt = 0;

		public bool eof = false;

		public bool ReadBit() {
			if (cnt == -1) {
				c = Console.Read();
				cnt = 7;
			}
			var result = (c & (1 << cnt)) != 0;
			--cnt;
			return result;
		}

		public bool Eof() {
			return c == -1;
		}

		public void WriteBit(bool bit) {
			c = (c << 1);
			if (bit) {
				c |= 1;
			}
			++wcnt;
			if (wcnt == 8) {
				Console.Write((char)c);
				wcnt = 0;
				c = 0;
			}
		}

	}

}
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
			//Debug.LogFormat("ReadBit");
			if (cnt == -1) {
				c = Console.Read();
				cnt = 7;
			}
			var result = (c & (1 << cnt)) != 0;
			--cnt;
			return result;
		}

		public bool Eof() {
			//Debug.LogFormat("Eof");
			return c == -1;
		}

		public void WriteBit(bool bit) {
			//Debug.LogFormat("WriteBit {0}", bit ? 1 : 0);
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
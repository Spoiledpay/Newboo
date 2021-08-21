using System;
using System.Collections;

namespace antlr.collections.impl
{
	public class BitSet : ICloneable
	{
		protected internal const int BITS = 64;

		protected internal const int NIBBLE = 4;

		protected internal const int LOG_BITS = 6;

		protected internal static readonly int MOD_MASK = 63;

		protected internal long[] dataBits;

		public BitSet()
			: this(64)
		{
		}

		public BitSet(long[] bits_)
		{
			dataBits = bits_;
		}

		public BitSet(int nbits)
		{
			dataBits = new long[(nbits - 1 >> 6) + 1];
		}

		public virtual void add(int el)
		{
			int num = wordNumber(el);
			if (num >= dataBits.Length)
			{
				growToInclude(el);
			}
			dataBits[num] |= bitMask(el);
		}

		public virtual BitSet and(BitSet a)
		{
			BitSet bitSet = (BitSet)Clone();
			bitSet.andInPlace(a);
			return bitSet;
		}

		public virtual void andInPlace(BitSet a)
		{
			int num = Math.Min(dataBits.Length, a.dataBits.Length);
			for (int num2 = num - 1; num2 >= 0; num2--)
			{
				dataBits[num2] &= a.dataBits[num2];
			}
			for (int num2 = num; num2 < dataBits.Length; num2++)
			{
				dataBits[num2] = 0L;
			}
		}

		private static long bitMask(int bitNumber)
		{
			int num = bitNumber & MOD_MASK;
			return 1L << num;
		}

		public virtual void clear()
		{
			for (int num = dataBits.Length - 1; num >= 0; num--)
			{
				dataBits[num] = 0L;
			}
		}

		public virtual void clear(int el)
		{
			int num = wordNumber(el);
			if (num >= dataBits.Length)
			{
				growToInclude(el);
			}
			dataBits[num] &= ~bitMask(el);
		}

		public virtual object Clone()
		{
			BitSet bitSet;
			try
			{
				bitSet = new BitSet();
				bitSet.dataBits = new long[dataBits.Length];
				Array.Copy(dataBits, 0, bitSet.dataBits, 0, dataBits.Length);
			}
			catch
			{
				throw new ApplicationException();
			}
			return bitSet;
		}

		public virtual int degree()
		{
			int num = 0;
			for (int num2 = dataBits.Length - 1; num2 >= 0; num2--)
			{
				long num3 = dataBits[num2];
				if (num3 != 0)
				{
					for (int num4 = 63; num4 >= 0; num4--)
					{
						if ((num3 & (1L << num4)) != 0)
						{
							num++;
						}
					}
				}
			}
			return num;
		}

		public override int GetHashCode()
		{
			return dataBits.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (obj != null && obj is BitSet)
			{
				BitSet bitSet = (BitSet)obj;
				int num = Math.Min(dataBits.Length, bitSet.dataBits.Length);
				int num2 = num;
				while (num2-- > 0)
				{
					if (dataBits[num2] != bitSet.dataBits[num2])
					{
						return false;
					}
				}
				if (dataBits.Length > num)
				{
					num2 = dataBits.Length;
					while (num2-- > num)
					{
						if (dataBits[num2] != 0)
						{
							return false;
						}
					}
				}
				else if (bitSet.dataBits.Length > num)
				{
					num2 = bitSet.dataBits.Length;
					while (num2-- > num)
					{
						if (bitSet.dataBits[num2] != 0)
						{
							return false;
						}
					}
				}
				return true;
			}
			return false;
		}

		public virtual void growToInclude(int bit)
		{
			int num = Math.Max(dataBits.Length << 1, numWordsToHold(bit));
			long[] destinationArray = new long[num];
			Array.Copy(dataBits, 0, destinationArray, 0, dataBits.Length);
			dataBits = destinationArray;
		}

		public virtual bool member(int el)
		{
			int num = wordNumber(el);
			if (num >= dataBits.Length)
			{
				return false;
			}
			return (dataBits[num] & bitMask(el)) != 0;
		}

		public virtual bool nil()
		{
			for (int num = dataBits.Length - 1; num >= 0; num--)
			{
				if (dataBits[num] != 0)
				{
					return false;
				}
			}
			return true;
		}

		public virtual BitSet not()
		{
			BitSet bitSet = (BitSet)Clone();
			bitSet.notInPlace();
			return bitSet;
		}

		public virtual void notInPlace()
		{
			for (int num = dataBits.Length - 1; num >= 0; num--)
			{
				dataBits[num] = ~dataBits[num];
			}
		}

		public virtual void notInPlace(int maxBit)
		{
			notInPlace(0, maxBit);
		}

		public virtual void notInPlace(int minBit, int maxBit)
		{
			growToInclude(maxBit);
			for (int i = minBit; i <= maxBit; i++)
			{
				int num = wordNumber(i);
				dataBits[num] ^= bitMask(i);
			}
		}

		private int numWordsToHold(int el)
		{
			return (el >> 6) + 1;
		}

		public static BitSet of(int el)
		{
			BitSet bitSet = new BitSet(el + 1);
			bitSet.add(el);
			return bitSet;
		}

		public virtual BitSet or(BitSet a)
		{
			BitSet bitSet = (BitSet)Clone();
			bitSet.orInPlace(a);
			return bitSet;
		}

		public virtual void orInPlace(BitSet a)
		{
			if (a.dataBits.Length > dataBits.Length)
			{
				setSize(a.dataBits.Length);
			}
			int num = Math.Min(dataBits.Length, a.dataBits.Length);
			for (int num2 = num - 1; num2 >= 0; num2--)
			{
				dataBits[num2] |= a.dataBits[num2];
			}
		}

		public virtual void remove(int el)
		{
			int num = wordNumber(el);
			if (num >= dataBits.Length)
			{
				growToInclude(el);
			}
			dataBits[num] &= ~bitMask(el);
		}

		private void setSize(int nwords)
		{
			long[] destinationArray = new long[nwords];
			int length = Math.Min(nwords, dataBits.Length);
			Array.Copy(dataBits, 0, destinationArray, 0, length);
			dataBits = destinationArray;
		}

		public virtual int size()
		{
			return dataBits.Length << 6;
		}

		public virtual int lengthInLongWords()
		{
			return dataBits.Length;
		}

		public virtual bool subset(BitSet a)
		{
			if (a == null)
			{
				return false;
			}
			return and(a).Equals(this);
		}

		public virtual void subtractInPlace(BitSet a)
		{
			if (a != null)
			{
				for (int i = 0; i < dataBits.Length && i < a.dataBits.Length; i++)
				{
					dataBits[i] &= ~a.dataBits[i];
				}
			}
		}

		public virtual int[] toArray()
		{
			int[] array = new int[degree()];
			int num = 0;
			for (int i = 0; i < dataBits.Length << 6; i++)
			{
				if (member(i))
				{
					array[num++] = i;
				}
			}
			return array;
		}

		public virtual long[] toPackedArray()
		{
			return dataBits;
		}

		public override string ToString()
		{
			return ToString(",");
		}

		public virtual string ToString(string separator)
		{
			string text = "";
			for (int i = 0; i < dataBits.Length << 6; i++)
			{
				if (member(i))
				{
					if (text.Length > 0)
					{
						text += separator;
					}
					text += i;
				}
			}
			return text;
		}

		public virtual string ToString(string separator, ArrayList vocabulary)
		{
			if (vocabulary == null)
			{
				return ToString(separator);
			}
			string text = "";
			for (int i = 0; i < dataBits.Length << 6; i++)
			{
				if (member(i))
				{
					if (text.Length > 0)
					{
						text += separator;
					}
					if (i >= vocabulary.Count)
					{
						object obj = text;
						text = string.Concat(obj, "<bad element ", i, ">");
					}
					else if (vocabulary[i] == null)
					{
						object obj = text;
						text = string.Concat(obj, "<", i, ">");
					}
					else
					{
						text += (string)vocabulary[i];
					}
				}
			}
			return text;
		}

		public virtual string toStringOfHalfWords()
		{
			string text = new string("".ToCharArray());
			for (int i = 0; i < dataBits.Length; i++)
			{
				if (i != 0)
				{
					text += ", ";
				}
				long num = dataBits[i];
				num &= 0xFFFFFFFFu;
				text = text + num + "UL";
				text += ", ";
				num = SupportClass.URShift(dataBits[i], 32);
				num &= 0xFFFFFFFFu;
				text = text + num + "UL";
			}
			return text;
		}

		public virtual string toStringOfWords()
		{
			string text = new string("".ToCharArray());
			for (int i = 0; i < dataBits.Length; i++)
			{
				if (i != 0)
				{
					text += ", ";
				}
				text = text + dataBits[i] + "L";
			}
			return text;
		}

		private static int wordNumber(int bit)
		{
			return bit >> 6;
		}
	}
}

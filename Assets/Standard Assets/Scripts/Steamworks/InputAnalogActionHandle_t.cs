// Decompile from assembly: Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
	[Serializable]
	public struct InputAnalogActionHandle_t : IEquatable<InputAnalogActionHandle_t>, IComparable<InputAnalogActionHandle_t>
	{
		public ulong m_InputAnalogActionHandle;

		public InputAnalogActionHandle_t(ulong value)
		{
			this.m_InputAnalogActionHandle = value;
		}

		public override string ToString()
		{
			return this.m_InputAnalogActionHandle.ToString();
		}

		public override bool Equals(object other)
		{
			return other is InputAnalogActionHandle_t && this == (InputAnalogActionHandle_t)other;
		}

		public override int GetHashCode()
		{
			return this.m_InputAnalogActionHandle.GetHashCode();
		}

		public static bool operator ==(InputAnalogActionHandle_t x, InputAnalogActionHandle_t y)
		{
			return x.m_InputAnalogActionHandle == y.m_InputAnalogActionHandle;
		}

		public static bool operator !=(InputAnalogActionHandle_t x, InputAnalogActionHandle_t y)
		{
			return !(x == y);
		}

		public static explicit operator InputAnalogActionHandle_t(ulong value)
		{
			return new InputAnalogActionHandle_t(value);
		}

		public static explicit operator ulong(InputAnalogActionHandle_t that)
		{
			return that.m_InputAnalogActionHandle;
		}

		public bool Equals(InputAnalogActionHandle_t other)
		{
			return this.m_InputAnalogActionHandle == other.m_InputAnalogActionHandle;
		}

		public int CompareTo(InputAnalogActionHandle_t other)
		{
			return this.m_InputAnalogActionHandle.CompareTo(other.m_InputAnalogActionHandle);
		}
	}
}

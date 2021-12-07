// Decompile from assembly: Assembly-CSharp.dll

using System;

[Serializable]
public struct EquipmentID
{
	public long id;

	public EquipmentID(long id)
	{
		this.id = id;
	}

	public EquipmentID(ulong id)
	{
		this.id = (long)id;
	}

	public bool IsNull()
	{
		return this.id == 0L;
	}

	public override bool Equals(object other)
	{
		return other is EquipmentID && (EquipmentID)other == this;
	}

	public override int GetHashCode()
	{
		int num = 17;
		return num * 7 + ((this.id != 0L) ? this.id.GetHashCode() : 0);
	}

	public static bool operator ==(EquipmentID a, EquipmentID b)
	{
		return a.id == b.id;
	}

	public static bool operator !=(EquipmentID a, EquipmentID b)
	{
		return !(a == b);
	}

	public override string ToString()
	{
		return this.id.ToString();
	}
}

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Beebyte.Obfuscator;
using UnityEngine;

// Token: 0x02000882 RID: 2178
[Serializable]
public class RollbackStateContainer
{
	// Token: 0x060036B0 RID: 14000 RVA: 0x000FC13A File Offset: 0x000FA53A
	public RollbackStateContainer(bool cloneMode)
	{
		this.cloneMode = cloneMode;
	}

	// Token: 0x060036B1 RID: 14001 RVA: 0x000FC14C File Offset: 0x000FA54C
	public bool WriteState(RollbackState state)
	{
		if (this.states == null)
		{
			this.states = new List<RollbackState>(512);
		}
		if (Debug.isDebugBuild && !state.GetType().IsSerializable)
		{
			Debug.LogError("RollbackState " + state.GetType() + " must be made serializable");
		}
		if (state is ICopyable && this.cloneMode)
		{
			this.states.Add(state.Clone() as RollbackState);
		}
		else
		{
			this.states.Add(state);
		}
		return true;
	}

	// Token: 0x060036B2 RID: 14002 RVA: 0x000FC1E8 File Offset: 0x000FA5E8
	private T getReadState<T>() where T : RollbackState
	{
		if (this.states == null || this.states.Count == 0)
		{
			Debug.LogError("Attempted to read state of type " + typeof(T) + ", when there were none left! This is not good");
			throw new Exception("NoReadState");
		}
		if (this.states.Count <= this.headIndex)
		{
			Debug.LogError(string.Concat(new object[]
			{
				"Attempted to read too many elements, index: ",
				this.headIndex,
				" count: ",
				this.states.Count
			}));
			throw new Exception("TooManyReads");
		}
		RollbackState rollbackState = this.states[this.headIndex];
		this.headIndex++;
		if (!(rollbackState is T))
		{
			Debug.LogError(string.Concat(new object[]
			{
				"Attempted to read a state ",
				rollbackState.GetType(),
				" of incorrect type ",
				typeof(T)
			}));
			throw new Exception("IncorrectType");
		}
		return rollbackState as T;
	}

	// Token: 0x060036B3 RID: 14003 RVA: 0x000FC314 File Offset: 0x000FA714
	public bool ReadState<T>(ref T theObject) where T : RollbackStateTyped<T>
	{
		T readState = this.getReadState<T>();
		readState.CopyTo(theObject);
		return true;
	}

	// Token: 0x060036B4 RID: 14004 RVA: 0x000FC33C File Offset: 0x000FA73C
	public T ReadState<T>() where T : RollbackState
	{
		RollbackState rollbackState = this.getReadState<T>();
		if (rollbackState is ICopyable)
		{
			throw new Exception("Incorrect read state");
		}
		return rollbackState.Clone() as T;
	}

	// Token: 0x060036B5 RID: 14005 RVA: 0x000FC37B File Offset: 0x000FA77B
	public void ResetIndex()
	{
		this.headIndex = 0;
	}

	// Token: 0x060036B6 RID: 14006 RVA: 0x000FC384 File Offset: 0x000FA784
	public void Clear()
	{
		this.headIndex = 0;
		if (this.states != null)
		{
			this.states.Clear();
		}
	}

	// Token: 0x17000D4A RID: 3402
	// (get) Token: 0x060036B7 RID: 14007 RVA: 0x000FC3A3 File Offset: 0x000FA7A3
	public int Count
	{
		get
		{
			return (this.states != null) ? this.states.Count : 0;
		}
	}

	// Token: 0x060036B8 RID: 14008 RVA: 0x000FC3C1 File Offset: 0x000FA7C1
	public RollbackState GetState(int index)
	{
		return (this.Count <= index) ? null : this.states[index];
	}

	// Token: 0x060036B9 RID: 14009 RVA: 0x000FC3E4 File Offset: 0x000FA7E4
	[SkipRename]
	public short GetMemberwiseHashCode()
	{
		int num = 0;
		for (int i = 0; i < this.states.Count; i++)
		{
			num ^= this.states[i].GetMemberwiseHashCode();
		}
		return (short)num;
	}

	// Token: 0x060036BA RID: 14010 RVA: 0x000FC428 File Offset: 0x000FA828
	[SkipRename]
	public string DebugMemberwiseHashCode()
	{
		int num = 0;
		string text = "CODE DEBUG:\n";
		if (this.states != null)
		{
			for (int i = 0; i < this.states.Count; i++)
			{
				int memberwiseHashCode = this.states[i].GetMemberwiseHashCode();
				num ^= memberwiseHashCode;
				string text2 = text;
				text = string.Concat(new object[]
				{
					text2,
					this.states[i],
					",",
					memberwiseHashCode,
					"\n"
				});
			}
		}
		return text + " Final: " + (short)num;
	}

	// Token: 0x060036BB RID: 14011 RVA: 0x000FC4CC File Offset: 0x000FA8CC
	public void LogOutArray(StringBuilder stringBuilder, Array arrayList)
	{
		for (int i = 0; i < arrayList.Length; i++)
		{
			object value = arrayList.GetValue(i);
			if (value != null)
			{
				Type type = value.GetType();
				if (type.IsValueType || type == typeof(string))
				{
					stringBuilder.Append(value.ToString());
					stringBuilder.Append(",");
				}
				else if (type.IsArray && !CloneUtil.IsDictionary(value))
				{
					stringBuilder.AppendFormat("\n{0}{1}", type, i);
					this.LogOutArray(stringBuilder, (Array)value);
				}
				else if (type.IsSerializable)
				{
					stringBuilder.Append("\n");
					foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
					{
						stringBuilder.AppendFormat("{0}:", fieldInfo.Name);
						object value2 = fieldInfo.GetValue(value);
						if (value2 != null)
						{
							if (fieldInfo.FieldType.IsValueType || fieldInfo.FieldType == typeof(string))
							{
								stringBuilder.AppendFormat("{0},", value2.ToString());
							}
							else
							{
								foreach (FieldInfo fieldInfo2 in fieldInfo.FieldType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
								{
									if (fieldInfo2.FieldType.IsValueType || fieldInfo2.FieldType == typeof(string))
									{
										object value3 = fieldInfo2.GetValue(value2);
										string arg = (value3 != null) ? value3.ToString() : "<k0>";
										stringBuilder.AppendFormat("{0}.", arg);
									}
								}
							}
						}
						else
						{
							stringBuilder.Append("<j0>, ");
						}
					}
				}
				else
				{
					stringBuilder.Append(value.ToString());
					stringBuilder.Append(",");
				}
			}
		}
	}

	// Token: 0x060036BC RID: 14012 RVA: 0x000FC6E0 File Offset: 0x000FAAE0
	public string LogOutHashes(StringBuilder stringBuilder)
	{
		for (int i = 0; i < this.states.Count; i++)
		{
			int memberwiseHashCode = this.states[i].GetMemberwiseHashCode();
			stringBuilder.AppendFormat("State:{0}::Hash:{1}\n", this.states[i], memberwiseHashCode);
			Type type = this.states[i].GetType();
			foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				stringBuilder.AppendFormat("\tField: {0}({1}):", fieldInfo.Name, fieldInfo.FieldType.Name);
				object value = fieldInfo.GetValue(this.states[i]);
				if (value == null)
				{
					stringBuilder.Append("<0>");
				}
				else if (fieldInfo.FieldType.IsValueType || fieldInfo.FieldType == typeof(string))
				{
					stringBuilder.Append(value.ToString());
				}
				else if (fieldInfo.FieldType.IsArray && !CloneUtil.IsDictionary(value))
				{
					this.LogOutArray(stringBuilder, (Array)value);
				}
				else if (fieldInfo.FieldType.IsSerializable && !CloneUtil.IsDictionary(value))
				{
					foreach (FieldInfo fieldInfo2 in fieldInfo.FieldType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
					{
						if (fieldInfo2.Name != "_version")
						{
							stringBuilder.AppendFormat("\n\t\t{0}({1}):", fieldInfo2.Name, fieldInfo2.FieldType.Name);
							object value2 = fieldInfo2.GetValue(value);
							if (value2 != null)
							{
								if (fieldInfo2.FieldType.IsValueType || fieldInfo2.FieldType == typeof(string))
								{
									stringBuilder.AppendFormat("{0},", value2.ToString());
								}
								else if (fieldInfo2.FieldType.IsArray && !CloneUtil.IsDictionary(value))
								{
									this.LogOutArray(stringBuilder, (Array)value2);
								}
								else
								{
									foreach (FieldInfo fieldInfo3 in fieldInfo2.FieldType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
									{
										if (fieldInfo3.FieldType.IsValueType || fieldInfo3.FieldType == typeof(string))
										{
											if (fieldInfo3.Name != "_version")
											{
												object value3 = fieldInfo3.GetValue(value2);
												string arg = (value3 != null) ? value3.ToString() : "<k0>";
												stringBuilder.AppendFormat("{0}|{1}.", fieldInfo3.Name, arg);
											}
										}
										else
										{
											stringBuilder.AppendFormat("Object[{0}].", fieldInfo3.Name);
										}
									}
								}
							}
							else
							{
								stringBuilder.Append("<j0>, ");
							}
						}
					}
				}
				else
				{
					stringBuilder.Append((value != null) ? value.ToString() : "<0>");
				}
				stringBuilder.Append("\n");
			}
		}
		return stringBuilder.ToString();
	}

	// Token: 0x060036BD RID: 14013 RVA: 0x000FCA3C File Offset: 0x000FAE3C
	public int GetSubHash(int count)
	{
		int num = 0;
		int num2 = Mathf.Min(this.states.Count, count);
		for (int i = 0; i < num2; i++)
		{
			num ^= this.states[i].GetMemberwiseHashCode();
		}
		return num;
	}

	// Token: 0x060036BE RID: 14014 RVA: 0x000FCA84 File Offset: 0x000FAE84
	public bool TestHash(RollbackStateContainer other, ref string error, bool detailedDelta = false)
	{
		if (other.GetMemberwiseHashCode() == this.GetMemberwiseHashCode())
		{
			return true;
		}
		if (other.Count != this.Count)
		{
			string text = error;
			error = string.Concat(new object[]
			{
				text,
				"Length mismatch: ",
				this.Count,
				" != ",
				other.Count
			});
			return false;
		}
		bool result = false;
		for (int i = 0; i < this.Count; i++)
		{
			RollbackState rollbackState = this.states[i];
			RollbackState rollbackState2 = other.states[i];
			if (rollbackState.GetMemberwiseHashCode() != rollbackState2.GetMemberwiseHashCode())
			{
				result = true;
				error = error + "Hash mismatch: " + rollbackState.ToString() + "\n";
				if (detailedDelta)
				{
					for (int j = 1; j < 100; j++)
					{
						if (rollbackState.GetSubHash(j) != rollbackState2.GetSubHash(j))
						{
							string text = error;
							error = string.Concat(new string[]
							{
								text,
								"\t",
								rollbackState.GetMemberStringByIndex(j - 1),
								" != ",
								rollbackState2.GetMemberStringByIndex(j - 1),
								"\n"
							});
							break;
						}
					}
				}
			}
		}
		return result;
	}

	// Token: 0x04002538 RID: 9528
	[SerializeField]
	protected List<RollbackState> states;

	// Token: 0x04002539 RID: 9529
	protected int headIndex;

	// Token: 0x0400253A RID: 9530
	public bool cloneMode;
}

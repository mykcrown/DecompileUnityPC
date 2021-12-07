// Decompile from assembly: Assembly-CSharp.dll

using Beebyte.Obfuscator;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

[Serializable]
public class RollbackStateContainer
{
	[SerializeField]
	protected List<RollbackState> states;

	protected int headIndex;

	public bool cloneMode;

	public int Count
	{
		get
		{
			return (this.states != null) ? this.states.Count : 0;
		}
	}

	public RollbackStateContainer(bool cloneMode)
	{
		this.cloneMode = cloneMode;
	}

	public bool WriteState(RollbackState state)
	{
		if (this.states == null)
		{
			this.states = new List<RollbackState>(512);
		}
		if (Debug.isDebugBuild && !state.GetType().IsSerializable)
		{
			UnityEngine.Debug.LogError("RollbackState " + state.GetType() + " must be made serializable");
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

	private T getReadState<T>() where T : RollbackState
	{
		if (this.states == null || this.states.Count == 0)
		{
			UnityEngine.Debug.LogError("Attempted to read state of type " + typeof(T) + ", when there were none left! This is not good");
			throw new Exception("NoReadState");
		}
		if (this.states.Count <= this.headIndex)
		{
			UnityEngine.Debug.LogError(string.Concat(new object[]
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
			UnityEngine.Debug.LogError(string.Concat(new object[]
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

	public bool ReadState<T>(ref T theObject) where T : RollbackStateTyped<T>
	{
		T readState = this.getReadState<T>();
		readState.CopyTo(theObject);
		return true;
	}

	public T ReadState<T>() where T : RollbackState
	{
		RollbackState rollbackState = this.getReadState<T>();
		if (rollbackState is ICopyable)
		{
			throw new Exception("Incorrect read state");
		}
		return rollbackState.Clone() as T;
	}

	public void ResetIndex()
	{
		this.headIndex = 0;
	}

	public void Clear()
	{
		this.headIndex = 0;
		if (this.states != null)
		{
			this.states.Clear();
		}
	}

	public RollbackState GetState(int index)
	{
		return (this.Count <= index) ? null : this.states[index];
	}

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
					FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					for (int j = 0; j < fields.Length; j++)
					{
						FieldInfo fieldInfo = fields[j];
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
								FieldInfo[] fields2 = fieldInfo.FieldType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
								for (int k = 0; k < fields2.Length; k++)
								{
									FieldInfo fieldInfo2 = fields2[k];
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

	public string LogOutHashes(StringBuilder stringBuilder)
	{
		for (int i = 0; i < this.states.Count; i++)
		{
			int memberwiseHashCode = this.states[i].GetMemberwiseHashCode();
			stringBuilder.AppendFormat("State:{0}::Hash:{1}\n", this.states[i], memberwiseHashCode);
			Type type = this.states[i].GetType();
			FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			for (int j = 0; j < fields.Length; j++)
			{
				FieldInfo fieldInfo = fields[j];
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
					FieldInfo[] fields2 = fieldInfo.FieldType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					for (int k = 0; k < fields2.Length; k++)
					{
						FieldInfo fieldInfo2 = fields2[k];
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
									FieldInfo[] fields3 = fieldInfo2.FieldType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
									for (int l = 0; l < fields3.Length; l++)
									{
										FieldInfo fieldInfo3 = fields3[l];
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
}

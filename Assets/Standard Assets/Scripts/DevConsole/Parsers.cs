// Decompile from assembly: Assembly-CSharp-firstpass.dll

using SickDev.CommandSystem;
using System;
using UnityEngine;

namespace DevConsole
{
	public static class Parsers
	{
		[Parser(typeof(Vector2))]
		private static Vector2 ParseVector2(string value)
		{
			string[] array = value.Split(new char[]
			{
				' '
			});
			if (array.Length != 2)
			{
				throw new InvalidArgumentFormatException<Vector2>(value);
			}
			float[] array2 = new float[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				if (!float.TryParse(array[i].Trim(), out array2[i]))
				{
					throw new InvalidArgumentFormatException<Vector2>(value);
				}
			}
			return new Vector2(array2[0], array2[1]);
		}

		[Parser(typeof(Vector3))]
		private static Vector3 ParseVector3(string value)
		{
			string[] array = value.Split(new char[]
			{
				' '
			});
			if (array.Length < 2 || array.Length > 3)
			{
				throw new InvalidArgumentFormatException<Vector3>(value);
			}
			float[] array2 = new float[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				if (!float.TryParse(array[i].Trim(), out array2[i]))
				{
					throw new InvalidArgumentFormatException<Vector3>(value);
				}
			}
			return new Vector3(array2[0], array2[1], (array.Length <= 2) ? 0f : array2[2]);
		}

		[Parser(typeof(Vector4))]
		private static Vector4 ParseVector4(string value)
		{
			string[] array = value.Split(new char[]
			{
				' '
			});
			if (array.Length < 2 || array.Length > 4)
			{
				throw new InvalidArgumentFormatException<Vector4>(value);
			}
			float[] array2 = new float[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				if (!float.TryParse(array[i].Trim(), out array2[i]))
				{
					throw new InvalidArgumentFormatException<Vector4>(value);
				}
			}
			return new Vector4(array2[0], array2[1], (array.Length <= 2) ? 0f : array2[2], (array.Length <= 3) ? 0f : array2[3]);
		}

		[Parser(typeof(Quaternion))]
		private static Quaternion ParseQuaternion(string value)
		{
			string[] array = value.Split(new char[]
			{
				' '
			});
			if (array.Length != 4)
			{
				throw new InvalidArgumentFormatException<Quaternion>(value);
			}
			float[] array2 = new float[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				if (!float.TryParse(array[i].Trim(), out array2[i]))
				{
					throw new InvalidArgumentFormatException<Quaternion>(value);
				}
			}
			return new Quaternion(array2[0], array2[1], array2[2], array2[3]);
		}

		[Parser(typeof(Color))]
		private static Color ParseColor(string value)
		{
			string[] array = value.Split(new char[]
			{
				' '
			});
			if (array.Length < 3 || array.Length > 4)
			{
				throw new InvalidArgumentFormatException<Color>(value);
			}
			float[] array2 = new float[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				if (!float.TryParse(array[i].Trim(), out array2[i]))
				{
					throw new InvalidArgumentFormatException<Color>(value);
				}
			}
			return new Color(array2[0], array2[1], array2[2], (array.Length <= 3) ? 1f : array2[3]);
		}

		[Parser(typeof(Rect))]
		private static Rect ParseRect(string value)
		{
			string[] array = value.Split(new char[]
			{
				' '
			});
			if (array.Length != 4)
			{
				throw new InvalidArgumentFormatException<Rect>(value);
			}
			float[] array2 = new float[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				if (!float.TryParse(array[i].Trim(), out array2[i]))
				{
					throw new InvalidArgumentFormatException<Rect>(value);
				}
			}
			return new Rect(array2[0], array2[1], array2[2], array2[3]);
		}

		[Parser(typeof(GameObject))]
		private static GameObject ParseGameObject(string value)
		{
			return GameObject.Find(value);
		}
	}
}

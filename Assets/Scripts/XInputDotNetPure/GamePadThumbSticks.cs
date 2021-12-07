// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

namespace XInputDotNetPure
{
	public struct GamePadThumbSticks
	{
		public struct StickValue
		{
			private Vector2 vector;

			public float X
			{
				get
				{
					return this.vector.x;
				}
			}

			public float Y
			{
				get
				{
					return this.vector.y;
				}
			}

			public Vector2 Vector
			{
				get
				{
					return this.vector;
				}
			}

			internal StickValue(float x, float y)
			{
				this.vector = new Vector2(x, y);
			}
		}

		private GamePadThumbSticks.StickValue left;

		private GamePadThumbSticks.StickValue right;

		public GamePadThumbSticks.StickValue Left
		{
			get
			{
				return this.left;
			}
		}

		public GamePadThumbSticks.StickValue Right
		{
			get
			{
				return this.right;
			}
		}

		internal GamePadThumbSticks(GamePadThumbSticks.StickValue left, GamePadThumbSticks.StickValue right)
		{
			this.left = left;
			this.right = right;
		}
	}
}

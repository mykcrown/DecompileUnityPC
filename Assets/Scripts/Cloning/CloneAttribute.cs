// Decompile from assembly: Assembly-CSharp.dll

using System;

namespace Cloning
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public class CloneAttribute : Attribute
	{
		private CloneType _clonetype;

		public CloneType CloneType
		{
			get
			{
				return this._clonetype;
			}
			set
			{
				this._clonetype = value;
			}
		}
	}
}

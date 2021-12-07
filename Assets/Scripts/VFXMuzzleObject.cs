// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class VFXMuzzleObject : MonoBehaviour
{
	private sealed class _Start_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal VFXMuzzleObject _this;

		internal object _current;

		internal bool _disposing;

		internal int _PC;

		object IEnumerator<object>.Current
		{
			get
			{
				return this._current;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return this._current;
			}
		}

		public _Start_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._current = new WaitForSeconds(this._this.MuzzleLife);
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			case 1u:
				UnityEngine.Object.Destroy(this._this.gameObject);
				this._PC = -1;
				break;
			}
			return false;
		}

		public void Dispose()
		{
			this._disposing = true;
			this._PC = -1;
		}

		public void Reset()
		{
			throw new NotSupportedException();
		}
	}

	public float MuzzleLife;

	private IEnumerator Start()
	{
		VFXMuzzleObject._Start_c__Iterator0 _Start_c__Iterator = new VFXMuzzleObject._Start_c__Iterator0();
		_Start_c__Iterator._this = this;
		return _Start_c__Iterator;
	}
}

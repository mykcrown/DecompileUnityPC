// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ObjectForMaterialQualitySwitcher : MonoBehaviour
{
	private sealed class _Start_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal ObjectForMaterialQualitySwitcher _this;

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
				StaticInject.Inject(this._this);
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this._this.userVideoSettingsModel == null)
			{
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			if (this._this.lowQuality)
			{
				this._this.lowQuality.SetActive(this._this.userVideoSettingsModel.MaterialQuality == ThreeTierQualityLevel.Low);
			}
			if (this._this.highQuality)
			{
				this._this.highQuality.SetActive(this._this.userVideoSettingsModel.MaterialQuality != ThreeTierQualityLevel.Low);
			}
			this._PC = -1;
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

	public GameObject lowQuality;

	public GameObject highQuality;

	[Inject]
	public IUserVideoSettingsModel userVideoSettingsModel
	{
		get;
		set;
	}

	private IEnumerator Start()
	{
		ObjectForMaterialQualitySwitcher._Start_c__Iterator0 _Start_c__Iterator = new ObjectForMaterialQualitySwitcher._Start_c__Iterator0();
		_Start_c__Iterator._this = this;
		return _Start_c__Iterator;
	}
}

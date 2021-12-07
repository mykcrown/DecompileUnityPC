// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LandingSpotController : MonoBehaviour
{
	private sealed class _InstantLandOnStart_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal float delay;

		internal LandingSpotController _this;

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

		public _InstantLandOnStart_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._current = new WaitForSeconds(this.delay);
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			case 1u:
				for (int i = 0; i < this._this._thisT.childCount; i++)
				{
					if (this._this._thisT.GetChild(i).GetComponent<LandingSpot>() != null)
					{
						LandingSpot component = this._this._thisT.GetChild(i).GetComponent<LandingSpot>();
						component.InstantLand();
					}
				}
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

	private sealed class _InstantLand_c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal float delay;

		internal LandingSpotController _this;

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

		public _InstantLand_c__Iterator1()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._current = new WaitForSeconds(this.delay);
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			case 1u:
				for (int i = 0; i < this._this._thisT.childCount; i++)
				{
					if (this._this._thisT.GetChild(i).GetComponent<LandingSpot>() != null)
					{
						LandingSpot component = this._this._thisT.GetChild(i).GetComponent<LandingSpot>();
						component.InstantLand();
					}
				}
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

	public bool _randomRotate = true;

	public Vector2 _autoCatchDelay = new Vector2(10f, 20f);

	public Vector2 _autoDismountDelay = new Vector2(10f, 20f);

	public float _maxBirdDistance = 20f;

	public float _minBirdDistance = 5f;

	public bool _takeClosest;

	public FlockController _flock;

	public bool _landOnStart;

	public bool _soarLand = true;

	public bool _onlyBirdsAbove;

	public float _landingSpeedModifier = 0.5f;

	public float _landingTurnSpeedModifier = 5f;

	public Transform _featherPS;

	public Transform _thisT;

	public int _activeLandingSpots;

	public float _snapLandDistance = 0.1f;

	public float _landedRotateSpeed = 0.01f;

	public float _gizmoSize = 0.2f;

	public void Start()
	{
		if (this._thisT == null)
		{
			this._thisT = base.transform;
		}
		if (this._flock == null)
		{
			this._flock = (FlockController)UnityEngine.Object.FindObjectOfType(typeof(FlockController));
			UnityEngine.Debug.Log(this + " has no assigned FlockController, a random FlockController has been assigned");
		}
		if (this._landOnStart)
		{
			base.StartCoroutine(this.InstantLandOnStart(0.1f));
		}
	}

	public void ScareAll()
	{
		this.ScareAll(0f, 1f);
	}

	public void ScareAll(float minDelay, float maxDelay)
	{
		for (int i = 0; i < this._thisT.childCount; i++)
		{
			if (this._thisT.GetChild(i).GetComponent<LandingSpot>() != null)
			{
				LandingSpot component = this._thisT.GetChild(i).GetComponent<LandingSpot>();
				component.Invoke("ReleaseFlockChild", UnityEngine.Random.Range(minDelay, maxDelay));
			}
		}
	}

	public void LandAll()
	{
		for (int i = 0; i < this._thisT.childCount; i++)
		{
			if (this._thisT.GetChild(i).GetComponent<LandingSpot>() != null)
			{
				LandingSpot component = this._thisT.GetChild(i).GetComponent<LandingSpot>();
				base.StartCoroutine(component.GetFlockChild(0f, 2f));
			}
		}
	}

	public IEnumerator InstantLandOnStart(float delay)
	{
		LandingSpotController._InstantLandOnStart_c__Iterator0 _InstantLandOnStart_c__Iterator = new LandingSpotController._InstantLandOnStart_c__Iterator0();
		_InstantLandOnStart_c__Iterator.delay = delay;
		_InstantLandOnStart_c__Iterator._this = this;
		return _InstantLandOnStart_c__Iterator;
	}

	public IEnumerator InstantLand(float delay)
	{
		LandingSpotController._InstantLand_c__Iterator1 _InstantLand_c__Iterator = new LandingSpotController._InstantLand_c__Iterator1();
		_InstantLand_c__Iterator.delay = delay;
		_InstantLand_c__Iterator._this = this;
		return _InstantLand_c__Iterator;
	}
}

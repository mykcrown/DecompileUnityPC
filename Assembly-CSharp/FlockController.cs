using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000008 RID: 8
public class FlockController : MonoBehaviour
{
	// Token: 0x0600002E RID: 46 RVA: 0x00003B64 File Offset: 0x00001F64
	public void Start()
	{
		this._thisT = base.transform;
		if (this._positionSphereDepth == -1f)
		{
			this._positionSphereDepth = this._positionSphere;
		}
		if (this._spawnSphereDepth == -1f)
		{
			this._spawnSphereDepth = this._spawnSphere;
		}
		this._posBuffer = this._thisT.position + this._startPosOffset;
		if (!this._slowSpawn)
		{
			this.AddChild(this._childAmount);
		}
		if (this._randomPositionTimer > 0f)
		{
			base.InvokeRepeating("SetFlockRandomPosition", this._randomPositionTimer, this._randomPositionTimer);
		}
	}

	// Token: 0x0600002F RID: 47 RVA: 0x00003C10 File Offset: 0x00002010
	public void AddChild(int amount)
	{
		if (this._groupChildToNewTransform)
		{
			this.InstantiateGroup();
		}
		for (int i = 0; i < amount; i++)
		{
			FlockChild flockChild = UnityEngine.Object.Instantiate<FlockChild>(this._childPrefab);
			flockChild._spawner = this;
			this._roamers.Add(flockChild);
			this.AddChildToParent(flockChild.transform);
		}
	}

	// Token: 0x06000030 RID: 48 RVA: 0x00003C6B File Offset: 0x0000206B
	public void AddChildToParent(Transform obj)
	{
		if (this._groupChildToFlock)
		{
			obj.parent = base.transform;
			return;
		}
		if (this._groupChildToNewTransform)
		{
			obj.parent = this._groupTransform;
			return;
		}
	}

	// Token: 0x06000031 RID: 49 RVA: 0x00003CA0 File Offset: 0x000020A0
	public void RemoveChild(int amount)
	{
		for (int i = 0; i < amount; i++)
		{
			FlockChild flockChild = this._roamers[this._roamers.Count - 1];
			this._roamers.RemoveAt(this._roamers.Count - 1);
			UnityEngine.Object.Destroy(flockChild.gameObject);
		}
	}

	// Token: 0x06000032 RID: 50 RVA: 0x00003CFC File Offset: 0x000020FC
	public void Update()
	{
		if (this._activeChildren > 0f)
		{
			if (this._updateDivisor > 1)
			{
				this._updateCounter++;
				this._updateCounter %= this._updateDivisor;
				this._newDelta = Time.deltaTime * (float)this._updateDivisor;
			}
			else
			{
				this._newDelta = Time.deltaTime;
			}
		}
		this.UpdateChildAmount();
	}

	// Token: 0x06000033 RID: 51 RVA: 0x00003D70 File Offset: 0x00002170
	public void InstantiateGroup()
	{
		if (this._groupTransform != null)
		{
			return;
		}
		GameObject gameObject = new GameObject();
		this._groupTransform = gameObject.transform;
		this._groupTransform.position = this._thisT.position;
		if (this._groupName != string.Empty)
		{
			gameObject.name = this._groupName;
			return;
		}
		gameObject.name = this._thisT.name + " Fish Container";
	}

	// Token: 0x06000034 RID: 52 RVA: 0x00003DF4 File Offset: 0x000021F4
	public void UpdateChildAmount()
	{
		if (this._childAmount >= 0 && this._childAmount < this._roamers.Count)
		{
			this.RemoveChild(1);
			return;
		}
		if (this._childAmount > this._roamers.Count)
		{
			this.AddChild(1);
		}
	}

	// Token: 0x06000035 RID: 53 RVA: 0x00003E48 File Offset: 0x00002248
	public void OnDrawGizmos()
	{
		if (this._thisT == null)
		{
			this._thisT = base.transform;
		}
		if (!Application.isPlaying && this._posBuffer != this._thisT.position + this._startPosOffset)
		{
			this._posBuffer = this._thisT.position + this._startPosOffset;
		}
		if (this._positionSphereDepth == -1f)
		{
			this._positionSphereDepth = this._positionSphere;
		}
		if (this._spawnSphereDepth == -1f)
		{
			this._spawnSphereDepth = this._spawnSphere;
		}
		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube(this._posBuffer, new Vector3(this._spawnSphere * 2f, this._spawnSphereHeight * 2f, this._spawnSphereDepth * 2f));
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireCube(this._thisT.position, new Vector3(this._positionSphere * 2f + this._spawnSphere * 2f, this._positionSphereHeight * 2f + this._spawnSphereHeight * 2f, this._positionSphereDepth * 2f + this._spawnSphereDepth * 2f));
	}

	// Token: 0x06000036 RID: 54 RVA: 0x00003FA0 File Offset: 0x000023A0
	public void SetFlockRandomPosition()
	{
		Vector3 zero = Vector3.zero;
		zero.x = UnityEngine.Random.Range(-this._positionSphere, this._positionSphere) + this._thisT.position.x;
		zero.z = UnityEngine.Random.Range(-this._positionSphereDepth, this._positionSphereDepth) + this._thisT.position.z;
		zero.y = UnityEngine.Random.Range(-this._positionSphereHeight, this._positionSphereHeight) + this._thisT.position.y;
		this._posBuffer = zero;
		if (this._forceChildWaypoints)
		{
			for (int i = 0; i < this._roamers.Count; i++)
			{
				this._roamers[i].Wander(UnityEngine.Random.value * this._forcedRandomDelay);
			}
		}
	}

	// Token: 0x06000037 RID: 55 RVA: 0x0000408C File Offset: 0x0000248C
	public void destroyBirds()
	{
		for (int i = 0; i < this._roamers.Count; i++)
		{
			UnityEngine.Object.Destroy(this._roamers[i].gameObject);
		}
		this._childAmount = 0;
		this._roamers.Clear();
	}

	// Token: 0x04000044 RID: 68
	public FlockChild _childPrefab;

	// Token: 0x04000045 RID: 69
	public int _childAmount = 250;

	// Token: 0x04000046 RID: 70
	public bool _slowSpawn;

	// Token: 0x04000047 RID: 71
	public float _spawnSphere = 3f;

	// Token: 0x04000048 RID: 72
	public float _spawnSphereHeight = 3f;

	// Token: 0x04000049 RID: 73
	public float _spawnSphereDepth = -1f;

	// Token: 0x0400004A RID: 74
	public float _minSpeed = 6f;

	// Token: 0x0400004B RID: 75
	public float _maxSpeed = 10f;

	// Token: 0x0400004C RID: 76
	public float _minScale = 0.7f;

	// Token: 0x0400004D RID: 77
	public float _maxScale = 1f;

	// Token: 0x0400004E RID: 78
	public float _soarFrequency;

	// Token: 0x0400004F RID: 79
	public string _soarAnimation = "Soar";

	// Token: 0x04000050 RID: 80
	public string _flapAnimation = "Flap";

	// Token: 0x04000051 RID: 81
	public string _idleAnimation = "Idle";

	// Token: 0x04000052 RID: 82
	public float _diveValue = 7f;

	// Token: 0x04000053 RID: 83
	public float _diveFrequency = 0.5f;

	// Token: 0x04000054 RID: 84
	public float _minDamping = 1f;

	// Token: 0x04000055 RID: 85
	public float _maxDamping = 2f;

	// Token: 0x04000056 RID: 86
	public float _waypointDistance = 1f;

	// Token: 0x04000057 RID: 87
	public float _minAnimationSpeed = 2f;

	// Token: 0x04000058 RID: 88
	public float _maxAnimationSpeed = 4f;

	// Token: 0x04000059 RID: 89
	public float _randomPositionTimer = 10f;

	// Token: 0x0400005A RID: 90
	public float _positionSphere = 25f;

	// Token: 0x0400005B RID: 91
	public float _positionSphereHeight = 25f;

	// Token: 0x0400005C RID: 92
	public float _positionSphereDepth = -1f;

	// Token: 0x0400005D RID: 93
	public bool _childTriggerPos;

	// Token: 0x0400005E RID: 94
	public bool _forceChildWaypoints;

	// Token: 0x0400005F RID: 95
	public float _forcedRandomDelay = 1.5f;

	// Token: 0x04000060 RID: 96
	public bool _flatFly;

	// Token: 0x04000061 RID: 97
	public bool _flatSoar;

	// Token: 0x04000062 RID: 98
	public bool _birdAvoid;

	// Token: 0x04000063 RID: 99
	public int _birdAvoidHorizontalForce = 1000;

	// Token: 0x04000064 RID: 100
	public bool _birdAvoidDown;

	// Token: 0x04000065 RID: 101
	public bool _birdAvoidUp;

	// Token: 0x04000066 RID: 102
	public int _birdAvoidVerticalForce = 300;

	// Token: 0x04000067 RID: 103
	public float _birdAvoidDistanceMax = 4.5f;

	// Token: 0x04000068 RID: 104
	public float _birdAvoidDistanceMin = 5f;

	// Token: 0x04000069 RID: 105
	public float _soarMaxTime;

	// Token: 0x0400006A RID: 106
	public LayerMask _avoidanceMask = -1;

	// Token: 0x0400006B RID: 107
	public List<FlockChild> _roamers;

	// Token: 0x0400006C RID: 108
	public Vector3 _posBuffer;

	// Token: 0x0400006D RID: 109
	public int _updateDivisor = 1;

	// Token: 0x0400006E RID: 110
	public float _newDelta;

	// Token: 0x0400006F RID: 111
	public int _updateCounter;

	// Token: 0x04000070 RID: 112
	public float _activeChildren;

	// Token: 0x04000071 RID: 113
	public bool _groupChildToNewTransform;

	// Token: 0x04000072 RID: 114
	public Transform _groupTransform;

	// Token: 0x04000073 RID: 115
	public string _groupName = string.Empty;

	// Token: 0x04000074 RID: 116
	public bool _groupChildToFlock;

	// Token: 0x04000075 RID: 117
	public Vector3 _startPosOffset;

	// Token: 0x04000076 RID: 118
	public Transform _thisT;
}

// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using MemberwiseEquality;
using System;

[Serializable]
public class InputState : MemberwiseEqualityObject, ICloneable
{
	private InputValue value = new InputValue();

	public int framesHeldDown;

	public bool tapped;

	public bool justTapped;

	public Fixed axis
	{
		get
		{
			return this.value.axis.Value;
		}
	}

	public bool button
	{
		get
		{
			return this.value.button;
		}
	}

	public IntegerAxis IntAxis
	{
		get
		{
			return this.value.axis;
		}
	}

	public void Load(InputState state)
	{
		if (state == null)
		{
			this.Clear();
		}
		else
		{
			this.framesHeldDown = state.framesHeldDown;
			this.tapped = state.tapped;
			this.justTapped = state.justTapped;
			this.value.Load(state.value);
		}
	}

	public void Clear()
	{
		this.framesHeldDown = 0;
		this.tapped = false;
		this.justTapped = false;
	}

	public void UpdateTapped(bool tapped, int currentFrame)
	{
		this.justTapped = (tapped && !this.tapped);
		this.tapped = tapped;
	}

	public void LoadValue(InputValue value)
	{
		if (this.framesHeldDown > 0 && (!value.button || value.axis.RawIntegerValue == 0 || !MathUtil.SignsMatch(value.axis.Value, this.axis)))
		{
			this.framesHeldDown = 0;
		}
		this.value.Load(value);
		if (this.axis != 0 || this.button)
		{
			this.framesHeldDown++;
		}
		else
		{
			this.framesHeldDown = 0;
		}
	}

	public object Clone()
	{
		return base.MemberwiseClone();
	}
}

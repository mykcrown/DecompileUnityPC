// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections.Generic;

[Serializable]
public class InputButtonsData : CloneableObject, ICopyable<InputButtonsData>, ICopyable
{
	public static readonly InputButtonsData EmptyInput = new InputButtonsData();

	[IgnoreCopyValidation, IsClonedManually]
	public List<ButtonPress> movementButtonsPressed = new List<ButtonPress>(32);

	[IgnoreCopyValidation, IsClonedManually]
	public List<ButtonPress> moveButtonsPressed = new List<ButtonPress>(32);

	[IgnoreCopyValidation, IsClonedManually]
	public List<ButtonPress> buttonsHeld = new List<ButtonPress>(32);

	public int currentFrame;

	public Fixed horizontalAxisValue = 0;

	public Fixed verticalAxisValue = 0;

	public HorizontalDirection facing;

	public HorizontalDirection HorizontalDirection
	{
		get
		{
			HorizontalDirection result = HorizontalDirection.None;
			if (this.horizontalAxisValue > 0)
			{
				result = HorizontalDirection.Right;
			}
			else if (this.horizontalAxisValue < 0)
			{
				result = HorizontalDirection.Left;
			}
			return result;
		}
	}

	public void InvertFacing()
	{
		this.facing = ((this.facing != HorizontalDirection.Left) ? HorizontalDirection.Left : HorizontalDirection.Right);
		this.invertHorizontalButtons(this.moveButtonsPressed);
		this.invertHorizontalButtons(this.movementButtonsPressed);
		this.invertHorizontalButtons(this.buttonsHeld);
	}

	public void CopyTo(InputButtonsData target)
	{
		target.currentFrame = this.currentFrame;
		target.horizontalAxisValue = this.horizontalAxisValue;
		target.verticalAxisValue = this.verticalAxisValue;
		target.facing = this.facing;
		this.copyList(this.moveButtonsPressed, target.moveButtonsPressed);
		this.copyList(this.movementButtonsPressed, target.movementButtonsPressed);
		this.copyList(this.buttonsHeld, target.buttonsHeld);
	}

	private void copyList(List<ButtonPress> source, List<ButtonPress> target)
	{
		target.Clear();
		foreach (ButtonPress current in source)
		{
			target.Add(current);
		}
	}

	public override object Clone()
	{
		InputButtonsData inputButtonsData = new InputButtonsData();
		this.CopyTo(inputButtonsData);
		return inputButtonsData;
	}

	private void invertHorizontalButtons(List<ButtonPress> buttons)
	{
		for (int i = 0; i < buttons.Count; i++)
		{
			if (InputUtils.IsHorizontal(buttons[i]))
			{
				buttons[i] = InputUtils.GetOppositeHorizontalButton(buttons[i]);
			}
		}
	}

	public bool IsAnyInput()
	{
		return this.horizontalAxisValue != 0 || this.verticalAxisValue != 0 || this.buttonsHeld.Count > 0 || this.movementButtonsPressed.Count > 0 || this.moveButtonsPressed.Count > 0;
	}

	public void AddButtonPressed(ButtonPress press, bool omitMovement = false)
	{
		if (!omitMovement)
		{
			this.movementButtonsPressed.Add(press);
		}
		this.moveButtonsPressed.Add(press);
	}

	public void Clear()
	{
		this.movementButtonsPressed.Clear();
		this.moveButtonsPressed.Clear();
		this.buttonsHeld.Clear();
		this.horizontalAxisValue = 0;
		this.verticalAxisValue = 0;
		this.currentFrame = 0;
		this.facing = HorizontalDirection.None;
	}
}

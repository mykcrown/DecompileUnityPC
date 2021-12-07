// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

[Serializable]
public class BufferedPlayerInput : CloneableObject, ICopyable<BufferedPlayerInput>, ICopyable
{
	[IgnoreCopyValidation, IsClonedManually]
	public InputButtonsData inputButtonsData = new InputButtonsData();

	public void CopyTo(BufferedPlayerInput target)
	{
		this.inputButtonsData.CopyTo(target.inputButtonsData);
	}

	public override object Clone()
	{
		BufferedPlayerInput bufferedPlayerInput = new BufferedPlayerInput();
		this.CopyTo(bufferedPlayerInput);
		return bufferedPlayerInput;
	}

	public void Load(InputButtonsData inputButtonsData, List<ButtonPress> moveButtonsPressed)
	{
		this.inputButtonsData.Clear();
		this.inputButtonsData.currentFrame = inputButtonsData.currentFrame;
		this.inputButtonsData.moveButtonsPressed.AddRange(moveButtonsPressed);
		this.inputButtonsData.movementButtonsPressed.AddRange(inputButtonsData.movementButtonsPressed);
		this.inputButtonsData.buttonsHeld.AddRange(inputButtonsData.buttonsHeld);
		this.inputButtonsData.horizontalAxisValue = inputButtonsData.horizontalAxisValue;
		this.inputButtonsData.verticalAxisValue = inputButtonsData.verticalAxisValue;
		this.inputButtonsData.facing = inputButtonsData.facing;
	}

	public void AdditiveLoad(InputButtonsData inputButtonsData, List<ButtonPress> moveButtonsPressed)
	{
		this.inputButtonsData.currentFrame = inputButtonsData.currentFrame;
		foreach (ButtonPress current in moveButtonsPressed)
		{
			if (!this.inputButtonsData.moveButtonsPressed.Contains(current))
			{
				this.inputButtonsData.moveButtonsPressed.Add(current);
			}
		}
		foreach (ButtonPress current2 in inputButtonsData.movementButtonsPressed)
		{
			if (!this.inputButtonsData.movementButtonsPressed.Contains(current2))
			{
				this.inputButtonsData.movementButtonsPressed.Add(current2);
			}
		}
		foreach (ButtonPress current3 in inputButtonsData.buttonsHeld)
		{
			if (!this.inputButtonsData.buttonsHeld.Contains(current3))
			{
				this.inputButtonsData.buttonsHeld.Add(current3);
			}
		}
		if (inputButtonsData.horizontalAxisValue != 0)
		{
			this.inputButtonsData.horizontalAxisValue = inputButtonsData.horizontalAxisValue;
		}
		if (inputButtonsData.verticalAxisValue != 0)
		{
			this.inputButtonsData.verticalAxisValue = inputButtonsData.verticalAxisValue;
		}
		if (inputButtonsData.facing != HorizontalDirection.None)
		{
			this.inputButtonsData.facing = inputButtonsData.facing;
		}
	}

	public void Clear()
	{
		this.inputButtonsData.Clear();
	}
}

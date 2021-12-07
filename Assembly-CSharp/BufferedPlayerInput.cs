using System;
using System.Collections.Generic;

// Token: 0x02000688 RID: 1672
[Serializable]
public class BufferedPlayerInput : CloneableObject, ICopyable<BufferedPlayerInput>, ICopyable
{
	// Token: 0x06002962 RID: 10594 RVA: 0x000DCE8E File Offset: 0x000DB28E
	public void CopyTo(BufferedPlayerInput target)
	{
		this.inputButtonsData.CopyTo(target.inputButtonsData);
	}

	// Token: 0x06002963 RID: 10595 RVA: 0x000DCEA4 File Offset: 0x000DB2A4
	public override object Clone()
	{
		BufferedPlayerInput bufferedPlayerInput = new BufferedPlayerInput();
		this.CopyTo(bufferedPlayerInput);
		return bufferedPlayerInput;
	}

	// Token: 0x06002964 RID: 10596 RVA: 0x000DCEC0 File Offset: 0x000DB2C0
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

	// Token: 0x06002965 RID: 10597 RVA: 0x000DCF5C File Offset: 0x000DB35C
	public void AdditiveLoad(InputButtonsData inputButtonsData, List<ButtonPress> moveButtonsPressed)
	{
		this.inputButtonsData.currentFrame = inputButtonsData.currentFrame;
		foreach (ButtonPress item in moveButtonsPressed)
		{
			if (!this.inputButtonsData.moveButtonsPressed.Contains(item))
			{
				this.inputButtonsData.moveButtonsPressed.Add(item);
			}
		}
		foreach (ButtonPress item2 in inputButtonsData.movementButtonsPressed)
		{
			if (!this.inputButtonsData.movementButtonsPressed.Contains(item2))
			{
				this.inputButtonsData.movementButtonsPressed.Add(item2);
			}
		}
		foreach (ButtonPress item3 in inputButtonsData.buttonsHeld)
		{
			if (!this.inputButtonsData.buttonsHeld.Contains(item3))
			{
				this.inputButtonsData.buttonsHeld.Add(item3);
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

	// Token: 0x06002966 RID: 10598 RVA: 0x000DD120 File Offset: 0x000DB520
	public void Clear()
	{
		this.inputButtonsData.Clear();
	}

	// Token: 0x04001DE9 RID: 7657
	[IsClonedManually]
	[IgnoreCopyValidation]
	public InputButtonsData inputButtonsData = new InputButtonsData();
}

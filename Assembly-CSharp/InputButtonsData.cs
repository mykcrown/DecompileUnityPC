using System;
using System.Collections.Generic;
using FixedPoint;

// Token: 0x02000698 RID: 1688
[Serializable]
public class InputButtonsData : CloneableObject, ICopyable<InputButtonsData>, ICopyable
{
	// Token: 0x17000A39 RID: 2617
	// (get) Token: 0x060029A2 RID: 10658 RVA: 0x000DDD7C File Offset: 0x000DC17C
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

	// Token: 0x060029A3 RID: 10659 RVA: 0x000DDDB7 File Offset: 0x000DC1B7
	public void InvertFacing()
	{
		this.facing = ((this.facing != HorizontalDirection.Left) ? HorizontalDirection.Left : HorizontalDirection.Right);
		this.invertHorizontalButtons(this.moveButtonsPressed);
		this.invertHorizontalButtons(this.movementButtonsPressed);
		this.invertHorizontalButtons(this.buttonsHeld);
	}

	// Token: 0x060029A4 RID: 10660 RVA: 0x000DDDF8 File Offset: 0x000DC1F8
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

	// Token: 0x060029A5 RID: 10661 RVA: 0x000DDE6C File Offset: 0x000DC26C
	private void copyList(List<ButtonPress> source, List<ButtonPress> target)
	{
		target.Clear();
		foreach (ButtonPress item in source)
		{
			target.Add(item);
		}
	}

	// Token: 0x060029A6 RID: 10662 RVA: 0x000DDECC File Offset: 0x000DC2CC
	public override object Clone()
	{
		InputButtonsData inputButtonsData = new InputButtonsData();
		this.CopyTo(inputButtonsData);
		return inputButtonsData;
	}

	// Token: 0x060029A7 RID: 10663 RVA: 0x000DDEE8 File Offset: 0x000DC2E8
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

	// Token: 0x060029A8 RID: 10664 RVA: 0x000DDF30 File Offset: 0x000DC330
	public bool IsAnyInput()
	{
		return this.horizontalAxisValue != 0 || this.verticalAxisValue != 0 || this.buttonsHeld.Count > 0 || this.movementButtonsPressed.Count > 0 || this.moveButtonsPressed.Count > 0;
	}

	// Token: 0x060029A9 RID: 10665 RVA: 0x000DDF9D File Offset: 0x000DC39D
	public void AddButtonPressed(ButtonPress press, bool omitMovement = false)
	{
		if (!omitMovement)
		{
			this.movementButtonsPressed.Add(press);
		}
		this.moveButtonsPressed.Add(press);
	}

	// Token: 0x060029AA RID: 10666 RVA: 0x000DDFC0 File Offset: 0x000DC3C0
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

	// Token: 0x04001DEF RID: 7663
	public static readonly InputButtonsData EmptyInput = new InputButtonsData();

	// Token: 0x04001DF0 RID: 7664
	[IsClonedManually]
	[IgnoreCopyValidation]
	public List<ButtonPress> movementButtonsPressed = new List<ButtonPress>(32);

	// Token: 0x04001DF1 RID: 7665
	[IsClonedManually]
	[IgnoreCopyValidation]
	public List<ButtonPress> moveButtonsPressed = new List<ButtonPress>(32);

	// Token: 0x04001DF2 RID: 7666
	[IsClonedManually]
	[IgnoreCopyValidation]
	public List<ButtonPress> buttonsHeld = new List<ButtonPress>(32);

	// Token: 0x04001DF3 RID: 7667
	public int currentFrame;

	// Token: 0x04001DF4 RID: 7668
	public Fixed horizontalAxisValue = 0;

	// Token: 0x04001DF5 RID: 7669
	public Fixed verticalAxisValue = 0;

	// Token: 0x04001DF6 RID: 7670
	public HorizontalDirection facing;
}

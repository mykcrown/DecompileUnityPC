using System;
using UnityEngine.EventSystems;

// Token: 0x02000915 RID: 2325
public interface ICursorInputDelegate
{
	// Token: 0x06003C4C RID: 15436
	void OnCancelPressed(IPlayerCursor cursor);

	// Token: 0x06003C4D RID: 15437
	void OnStartPressed(IPlayerCursor cursor);

	// Token: 0x06003C4E RID: 15438
	void OnSubmitPressed(PointerEventData eventData);

	// Token: 0x06003C4F RID: 15439
	void OnAltCancelPressed(IPlayerCursor cursor);

	// Token: 0x06003C50 RID: 15440
	void OnAdvance1Pressed(IPlayerCursor cursor);

	// Token: 0x06003C51 RID: 15441
	void OnPrevious1Pressed(IPlayerCursor cursor);

	// Token: 0x06003C52 RID: 15442
	void OnAdvance2Pressed(IPlayerCursor cursor);

	// Token: 0x06003C53 RID: 15443
	void OnPrevious2Pressed(IPlayerCursor cursor);

	// Token: 0x06003C54 RID: 15444
	void OnRightStickUpPressed(IPlayerCursor cursor);

	// Token: 0x06003C55 RID: 15445
	void OnRightStickDownPressed(IPlayerCursor cursor);

	// Token: 0x06003C56 RID: 15446
	void OnAltSubmitPressed(IPlayerCursor cursor);

	// Token: 0x06003C57 RID: 15447
	void OnMouseMode();

	// Token: 0x06003C58 RID: 15448
	void OnControllerMode();
}

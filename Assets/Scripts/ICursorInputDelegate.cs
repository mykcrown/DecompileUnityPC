// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine.EventSystems;

public interface ICursorInputDelegate
{
	void OnCancelPressed(IPlayerCursor cursor);

	void OnStartPressed(IPlayerCursor cursor);

	void OnSubmitPressed(PointerEventData eventData);

	void OnAltCancelPressed(IPlayerCursor cursor);

	void OnAdvance1Pressed(IPlayerCursor cursor);

	void OnPrevious1Pressed(IPlayerCursor cursor);

	void OnAdvance2Pressed(IPlayerCursor cursor);

	void OnPrevious2Pressed(IPlayerCursor cursor);

	void OnRightStickUpPressed(IPlayerCursor cursor);

	void OnRightStickDownPressed(IPlayerCursor cursor);

	void OnAltSubmitPressed(IPlayerCursor cursor);

	void OnMouseMode();

	void OnControllerMode();
}

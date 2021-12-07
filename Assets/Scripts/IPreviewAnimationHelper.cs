// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IPreviewAnimationHelper
{
	void PlayBasicAnimation(IUISceneCharacter characterDisplay, CharacterDefinition characterDef, EquippableItem item, int selectedCharacterIndex);

	void PlayVictoryPose(IUISceneCharacter characterDisplay, CharacterDefinition characterDef, EquippableItem item);
}

using System;

// Token: 0x02000A20 RID: 2592
public interface IPreviewAnimationHelper
{
	// Token: 0x06004B63 RID: 19299
	void PlayBasicAnimation(IUISceneCharacter characterDisplay, CharacterDefinition characterDef, EquippableItem item, int selectedCharacterIndex);

	// Token: 0x06004B64 RID: 19300
	void PlayVictoryPose(IUISceneCharacter characterDisplay, CharacterDefinition characterDef, EquippableItem item);
}

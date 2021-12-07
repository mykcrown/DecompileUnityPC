using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace IconsTool
{
	// Token: 0x020006F7 RID: 1783
	public class StaticDataItemList
	{
		// Token: 0x04001F93 RID: 8083
		[XmlElement("StaticItemSkin")]
		public List<StaticItemSkin> m_itemSkinList = new List<StaticItemSkin>();

		// Token: 0x04001F94 RID: 8084
		[XmlElement("StaticItemEmote")]
		public List<StaticItemEmote> m_itemEmoteList = new List<StaticItemEmote>();

		// Token: 0x04001F95 RID: 8085
		[XmlElement("StaticItemHologram")]
		public List<StaticItemHologram> m_itemHologramList = new List<StaticItemHologram>();

		// Token: 0x04001F96 RID: 8086
		[XmlElement("StaticItemVoiceLine")]
		public List<StaticItemVoiceLine> m_itemVoiceLineList = new List<StaticItemVoiceLine>();

		// Token: 0x04001F97 RID: 8087
		[XmlElement("StaticItemVictoryPose")]
		public List<StaticItemVictoryPose> m_itemVictoryPoseList = new List<StaticItemVictoryPose>();

		// Token: 0x04001F98 RID: 8088
		[XmlElement("StaticItemRespawnPlatform")]
		public List<StaticItemRespawnPlatform> m_itemRespawnPlatformList = new List<StaticItemRespawnPlatform>();

		// Token: 0x04001F99 RID: 8089
		[XmlElement("StaticItemNetsuke")]
		public List<StaticItemNetsuke> m_itemNetsukeList = new List<StaticItemNetsuke>();

		// Token: 0x04001F9A RID: 8090
		[XmlElement("StaticItemToken")]
		public List<StaticItemToken> m_itemTokenList = new List<StaticItemToken>();

		// Token: 0x04001F9B RID: 8091
		[XmlElement("StaticItemPlayerImage")]
		public List<StaticItemPlayerImage> m_itemPlayerImageList = new List<StaticItemPlayerImage>();

		// Token: 0x04001F9C RID: 8092
		[XmlElement("StaticItemBlastZone")]
		public List<StaticItemBlastZone> m_itemBlastZoneList = new List<StaticItemBlastZone>();

		// Token: 0x04001F9D RID: 8093
		[XmlElement("StaticItemCharacter")]
		public List<StaticItemCharacter> m_itemCharacterList = new List<StaticItemCharacter>();

		// Token: 0x04001F9E RID: 8094
		[XmlElement("StaticItemPremiumAccount")]
		public List<StaticItemPremiumAccount> m_itemPremiumAccountList = new List<StaticItemPremiumAccount>();

		// Token: 0x04001F9F RID: 8095
		[XmlElement("StaticItemUnlockToken")]
		public List<StaticItemUnlockToken> m_itemUnlockTokenList = new List<StaticItemUnlockToken>();

		// Token: 0x04001FA0 RID: 8096
		[XmlElement("StaticItemCurrency")]
		public List<StaticItemCurrency> m_itemCurrencyList = new List<StaticItemCurrency>();
	}
}

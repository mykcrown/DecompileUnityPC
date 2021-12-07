// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace IconsTool
{
	public class StaticDataItemList
	{
		[XmlElement("StaticItemSkin")]
		public List<StaticItemSkin> m_itemSkinList = new List<StaticItemSkin>();

		[XmlElement("StaticItemEmote")]
		public List<StaticItemEmote> m_itemEmoteList = new List<StaticItemEmote>();

		[XmlElement("StaticItemHologram")]
		public List<StaticItemHologram> m_itemHologramList = new List<StaticItemHologram>();

		[XmlElement("StaticItemVoiceLine")]
		public List<StaticItemVoiceLine> m_itemVoiceLineList = new List<StaticItemVoiceLine>();

		[XmlElement("StaticItemVictoryPose")]
		public List<StaticItemVictoryPose> m_itemVictoryPoseList = new List<StaticItemVictoryPose>();

		[XmlElement("StaticItemRespawnPlatform")]
		public List<StaticItemRespawnPlatform> m_itemRespawnPlatformList = new List<StaticItemRespawnPlatform>();

		[XmlElement("StaticItemNetsuke")]
		public List<StaticItemNetsuke> m_itemNetsukeList = new List<StaticItemNetsuke>();

		[XmlElement("StaticItemToken")]
		public List<StaticItemToken> m_itemTokenList = new List<StaticItemToken>();

		[XmlElement("StaticItemPlayerImage")]
		public List<StaticItemPlayerImage> m_itemPlayerImageList = new List<StaticItemPlayerImage>();

		[XmlElement("StaticItemBlastZone")]
		public List<StaticItemBlastZone> m_itemBlastZoneList = new List<StaticItemBlastZone>();

		[XmlElement("StaticItemCharacter")]
		public List<StaticItemCharacter> m_itemCharacterList = new List<StaticItemCharacter>();

		[XmlElement("StaticItemPremiumAccount")]
		public List<StaticItemPremiumAccount> m_itemPremiumAccountList = new List<StaticItemPremiumAccount>();

		[XmlElement("StaticItemUnlockToken")]
		public List<StaticItemUnlockToken> m_itemUnlockTokenList = new List<StaticItemUnlockToken>();

		[XmlElement("StaticItemCurrency")]
		public List<StaticItemCurrency> m_itemCurrencyList = new List<StaticItemCurrency>();
	}
}

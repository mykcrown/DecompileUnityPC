using System;
using UnityEngine;

// Token: 0x02000AD0 RID: 2768
public class HologramDisplayCommand : GameEvent
{
	// Token: 0x060050B1 RID: 20657 RVA: 0x001505FF File Offset: 0x0014E9FF
	public HologramDisplayCommand(IPlayerDelegate playerDelegate, ParticleData hologramParticle, ParticleData lineParticle, Texture2D hologramTexture)
	{
		this.playerDelegate = playerDelegate;
		this.hologramParticle = hologramParticle;
		this.lineParticle = lineParticle;
		this.hologramTexture = hologramTexture;
	}

	// Token: 0x040033F6 RID: 13302
	public IPlayerDelegate playerDelegate;

	// Token: 0x040033F7 RID: 13303
	public ParticleData hologramParticle;

	// Token: 0x040033F8 RID: 13304
	public ParticleData lineParticle;

	// Token: 0x040033F9 RID: 13305
	public Texture2D hologramTexture;
}

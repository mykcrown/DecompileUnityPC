// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class HologramDisplayCommand : GameEvent
{
	public IPlayerDelegate playerDelegate;

	public ParticleData hologramParticle;

	public ParticleData lineParticle;

	public Texture2D hologramTexture;

	public HologramDisplayCommand(IPlayerDelegate playerDelegate, ParticleData hologramParticle, ParticleData lineParticle, Texture2D hologramTexture)
	{
		this.playerDelegate = playerDelegate;
		this.hologramParticle = hologramParticle;
		this.lineParticle = lineParticle;
		this.hologramTexture = hologramTexture;
	}
}

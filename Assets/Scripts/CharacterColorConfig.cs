// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class CharacterColorConfig : ScriptableObject
{
	public AnimatingColor chargingEmission = new AnimatingColor();

	public AnimatingColor dazedEmission = new AnimatingColor();

	public AnimatingColor invincibleEmission = new AnimatingColor();

	public AnimatingColor invincibleEmissionMed = new AnimatingColor();

	public AnimatingColor invincibleEmissionSlow = new AnimatingColor();

	public AnimatingColor helplessColor = new AnimatingColor();

	public AnimatingColor regrabPreventionColor = new AnimatingColor();

	public AnimatingColor ledgegrabVulnerable = new AnimatingColor();

	public bool useImpactEmission;

	public int impactEmissionMaxFrames = 10;

	public AnimatingColor impactEmission = new AnimatingColor();

	public Color tumblingEmission;

	public Color emissionColorOffset;

	public Color inactiveEmission;

	public Color inactiveColorOffset = Color.white;
}

// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

namespace AI
{
	public interface IAICalculator
	{
		int reactionFrames
		{
			get;
		}

		int buttonSpamFrames
		{
			get;
		}

		bool TestOffstage(PlayerController player);

		bool TestReachedEdgeOfStage(PlayerController player, int direction);

		int RandomizeReactionSpeed(int baseReactionSpeed);

		int RandomizeButtonSpam(int baseButtonSpamFrames);

		Fixed GenerateRandomNumber();

		PlayerReference FindClosestEnemy(PlayerController player);
	}
}

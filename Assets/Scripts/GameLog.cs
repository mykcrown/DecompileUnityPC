// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class GameLog
{
	private IEvents events;

	private bool haltOnFatalError;

	public GameLog(IEvents events)
	{
		this.events = events;
	}

	public void Debug(string message)
	{
		UnityEngine.Debug.Log(message);
	}

	public void Error(string message)
	{
		UnityEngine.Debug.LogError(message);
	}

	public void FatalError(string message)
	{
		UnityEngine.Debug.LogError(message);
	}
}

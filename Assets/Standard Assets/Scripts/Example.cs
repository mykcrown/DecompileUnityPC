// Decompile from assembly: Assembly-CSharp-firstpass.dll

using DevConsole;
using SickDev.CommandSystem;
using System;
using UnityEngine;

public class Example : MonoBehaviour
{
	[Command]
	private static void TimeScale(float value)
	{
		Time.timeScale = value;
		DevConsole.Console.Log("Change successful", Color.green);
	}

	[Command]
	private static void ShowTime()
	{
		DevConsole.Console.Log(Time.time.ToString());
	}

	[Command]
	private static void SetGravity(Vector3 value)
	{
		Physics.gravity = value;
	}
}

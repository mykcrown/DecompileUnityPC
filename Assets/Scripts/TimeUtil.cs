// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Text;

public class TimeUtil
{
	private static string[] intToString;

	public static void Init()
	{
		TimeUtil.intToString = new string[1000];
		for (int i = 0; i < TimeUtil.intToString.Length; i++)
		{
			TimeUtil.intToString[i] = i.ToString();
		}
	}

	public static void FormatTimeByFrames(int frames, StringBuilder builder)
	{
		float seconds = (float)(frames / 60);
		TimeUtil.FormatTime(seconds, builder, 0, false);
	}

	public static void FormatClockDisplay(DateTime time, ref StaticString staticString)
	{
		int num = time.Hour;
		int minute = time.Minute;
		bool flag = false;
		if (num >= 12)
		{
			flag = true;
		}
		if (num > 12)
		{
			num -= 12;
		}
		if (num == 0)
		{
			num = 12;
		}
		staticString.Append(TimeUtil.intToString[num]);
		staticString.Append(':');
		if (minute < 10)
		{
			staticString.Append('0');
		}
		staticString.Append(TimeUtil.intToString[minute]);
		staticString.Append(' ');
		if (flag)
		{
			staticString.Append('P');
		}
		else
		{
			staticString.Append('A');
		}
		staticString.Append('M');
	}

	public static void FormatTime(float seconds, ref StaticString staticString, int displayDecimalAtReducedSize = 0, bool fixedWidthMinutes = false)
	{
		int num = (int)seconds / 60;
		int num2 = (int)seconds % 60;
		int num3 = (int)((seconds - (float)(60 * num) - (float)num2) * 100f);
		if (fixedWidthMinutes && num < 10)
		{
			staticString.Append('0');
		}
		staticString.Append(TimeUtil.intToString[num]);
		staticString.Append(':');
		if (num2 < 10)
		{
			staticString.Append('0');
		}
		staticString.Append(TimeUtil.intToString[num2]);
		if (displayDecimalAtReducedSize != 0)
		{
			staticString.Append(" <size=");
			if (displayDecimalAtReducedSize > 0)
			{
				staticString.Append(TimeUtil.intToString[displayDecimalAtReducedSize]);
			}
			else
			{
				staticString.Append("-");
				staticString.Append(TimeUtil.intToString[-displayDecimalAtReducedSize]);
			}
			staticString.Append("> ");
			if (num3 < 10)
			{
				staticString.Append('0');
			}
			staticString.Append(TimeUtil.intToString[num3]);
			staticString.Append("</size>");
		}
	}

	public static void FormatTime(float seconds, StringBuilder builder, int displayDecimalAtReducedSize = 0, bool fixedWidthMinutes = false)
	{
		int num = (int)seconds / 60;
		int num2 = (int)seconds % 60;
		int num3 = (int)((seconds - (float)(60 * num) - (float)num2) * 100f);
		if (fixedWidthMinutes)
		{
			builder.Append((num >= 10) ? string.Empty : "0");
		}
		builder.Append(TimeUtil.intToString[num]);
		builder.Append(":");
		builder.Append((num2 >= 10) ? string.Empty : "0");
		builder.Append(TimeUtil.intToString[num2]);
		if (displayDecimalAtReducedSize != 0)
		{
			builder.Append(" <size=");
			if (displayDecimalAtReducedSize > 0)
			{
				builder.Append(TimeUtil.intToString[displayDecimalAtReducedSize]);
			}
			else
			{
				builder.Append("-");
				builder.Append(TimeUtil.intToString[-displayDecimalAtReducedSize]);
			}
			builder.Append("> ");
			builder.Append((num3 >= 10) ? string.Empty : "0");
			builder.Append(TimeUtil.intToString[num3]);
			builder.Append("</size>");
		}
	}

	public static StringBuilder FormatLongCountdown(TimeSpan remainingTime)
	{
		StringBuilder result = new StringBuilder();
		TimeUtil.FormatLongCountdown(remainingTime, result);
		return result;
	}

	public static void FormatLongCountdown(TimeSpan remainingTime, StringBuilder result)
	{
		int num = (int)remainingTime.TotalHours;
		if (num > 0)
		{
			result.Append(num).Append(":");
		}
		if (remainingTime.Minutes > 0 && remainingTime.Minutes < 10 && num > 0)
		{
			result.Append("0");
		}
		result.Append(remainingTime.Minutes);
		result.Append(":");
		if (remainingTime.Seconds < 10)
		{
			result.Append("0");
		}
		result.Append(remainingTime.Seconds);
	}
}

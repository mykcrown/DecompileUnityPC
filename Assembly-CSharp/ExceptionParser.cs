using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;

// Token: 0x02000AB9 RID: 2745
public class ExceptionParser : IExceptionParser
{
	// Token: 0x06005075 RID: 20597 RVA: 0x0014F9B4 File Offset: 0x0014DDB4
	public string GetShortString(Exception e)
	{
		StackTrace stackTrace = new StackTrace(e, true);
		StringBuilder stringBuilder = new StringBuilder();
		if (e.GetType() == typeof(Exception))
		{
			stringBuilder.Append(e.Message);
		}
		else
		{
			string text = e.GetType().ToString();
			string[] array = text.Split(new char[]
			{
				'.'
			});
			stringBuilder.Append(array[array.Length - 1]);
		}
		StackFrame[] frames = stackTrace.GetFrames();
		for (int i = 0; i < frames.Length; i++)
		{
			MethodBase method = frames[i].GetMethod();
			string text2 = method.DeclaringType.FullName;
			if (!string.IsNullOrEmpty(text2) && text2.Length >= 6)
			{
				text2 = text2.Substring(0, 6);
			}
			string text3 = method.Name;
			if (!string.IsNullOrEmpty(text3) && text3.Length >= 6)
			{
				text3 = text3.Substring(0, 6);
			}
			stringBuilder.AppendFormat("\n{0}:{1}", text2, text3);
		}
		return stringBuilder.ToString();
	}
}

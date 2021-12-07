using System;

// Token: 0x02000AB8 RID: 2744
public class EventUtil
{
	// Token: 0x06005073 RID: 20595 RVA: 0x0014F940 File Offset: 0x0014DD40
	public static void AddOnce(Action action, Action<Action> subscribe, Action<Action> unsubscribe)
	{
		Action myDelegate = null;
		myDelegate = delegate()
		{
			action();
			unsubscribe(myDelegate);
		};
		subscribe(myDelegate);
	}
}

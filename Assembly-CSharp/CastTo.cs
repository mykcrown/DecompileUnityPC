using System;
using System.Linq.Expressions;

// Token: 0x02000AA3 RID: 2723
public static class CastTo<T>
{
	// Token: 0x06005003 RID: 20483 RVA: 0x0014E0A8 File Offset: 0x0014C4A8
	public static T From<S>(S s)
	{
		return CastTo<T>.Cache<S>.caster(s);
	}

	// Token: 0x02000AA4 RID: 2724
	private static class Cache<S>
	{
		// Token: 0x06005004 RID: 20484 RVA: 0x0014E0B8 File Offset: 0x0014C4B8
		private static Func<S, T> Get()
		{
			ParameterExpression parameterExpression = Expression.Parameter(typeof(S), "t");
			UnaryExpression body = Expression.ConvertChecked(parameterExpression, typeof(T));
			return Expression.Lambda<Func<S, T>>(body, new ParameterExpression[]
			{
				parameterExpression
			}).Compile();
		}

		// Token: 0x040033AE RID: 13230
		public static readonly Func<S, T> caster = CastTo<T>.Cache<S>.Get();
	}
}

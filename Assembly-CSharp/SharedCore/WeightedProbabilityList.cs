using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SharedCore
{
	// Token: 0x02000B82 RID: 2946
	[Serializable]
	public class WeightedProbabilityList<T>
	{
		// Token: 0x060054F9 RID: 21753 RVA: 0x001B4252 File Offset: 0x001B2652
		public WeightedProbabilityList()
		{
		}

		// Token: 0x060054FA RID: 21754 RVA: 0x001B4268 File Offset: 0x001B2668
		public WeightedProbabilityList(T[] values, int[] weights)
		{
			if (values.Length != weights.Length)
			{
				throw new ArgumentException("WeightedProbabilityList was supplied two lists of differing sizes!");
			}
			for (int i = 0; i < values.Length; i++)
			{
				this.Add(values[i], weights[i]);
			}
		}

		// Token: 0x17001392 RID: 5010
		public T this[int key]
		{
			get
			{
				return this.Items[key].Value;
			}
			set
			{
				this.Items[key].Value = value;
			}
		}

		// Token: 0x17001393 RID: 5011
		// (get) Token: 0x060054FD RID: 21757 RVA: 0x001B42E7 File Offset: 0x001B26E7
		public int Count
		{
			get
			{
				return this.Items.Count;
			}
		}

		// Token: 0x060054FE RID: 21758 RVA: 0x001B42F4 File Offset: 0x001B26F4
		public void Add(T value, int weight = 1)
		{
			if (weight < 1)
			{
				weight = 1;
			}
			this.Items.Add(new WeightedValue<T>(value, weight));
			this.TotalWeight += weight;
		}

		// Token: 0x060054FF RID: 21759 RVA: 0x001B4320 File Offset: 0x001B2720
		public void Clear()
		{
			this.TotalWeight = 0;
			this.Items.Clear();
		}

		// Token: 0x06005500 RID: 21760 RVA: 0x001B4334 File Offset: 0x001B2734
		public T GetRandomItem(System.Random generator)
		{
			return this.GetRandomItem(generator.Next(0, this.TotalWeight));
		}

		// Token: 0x06005501 RID: 21761 RVA: 0x001B4349 File Offset: 0x001B2749
		public T GetRandomItem()
		{
			return this.GetRandomItem(UnityEngine.Random.Range(0, this.TotalWeight));
		}

		// Token: 0x06005502 RID: 21762 RVA: 0x001B435D File Offset: 0x001B275D
		public T RemoveRandomItem(System.Random generator)
		{
			return this.RemoveRandomItem(generator.Next(0, this.TotalWeight));
		}

		// Token: 0x06005503 RID: 21763 RVA: 0x001B4372 File Offset: 0x001B2772
		public T RemoveRandomItem()
		{
			return this.RemoveRandomItem(UnityEngine.Random.Range(0, this.TotalWeight));
		}

		// Token: 0x06005504 RID: 21764 RVA: 0x001B4386 File Offset: 0x001B2786
		public List<T> ToList()
		{
			return (from item in this.Items
			select item.Value).ToList<T>();
		}

		// Token: 0x06005505 RID: 21765 RVA: 0x001B43B5 File Offset: 0x001B27B5
		public List<int> GetWeights()
		{
			return (from item in this.Items
			select item.Weight).ToList<int>();
		}

		// Token: 0x06005506 RID: 21766 RVA: 0x001B43E4 File Offset: 0x001B27E4
		public WeightedProbabilityList<T> MatchingItems(Func<T, int, bool> predicate)
		{
			WeightedProbabilityList<T> weightedProbabilityList = new WeightedProbabilityList<T>();
			foreach (WeightedValue<T> weightedValue in from t in this.Items
			where predicate(t.Value, t.Weight)
			select t)
			{
				weightedProbabilityList.Add(weightedValue.Value, weightedValue.Weight);
			}
			return weightedProbabilityList;
		}

		// Token: 0x06005507 RID: 21767 RVA: 0x001B4470 File Offset: 0x001B2870
		public WeightedProbabilityList<T> MatchingItems(Func<T, bool> predicate)
		{
			return this.MatchingItems((T item, int weight) => predicate(item));
		}

		// Token: 0x06005508 RID: 21768 RVA: 0x001B449C File Offset: 0x001B289C
		public static T GetRandomItem(IEnumerable<WeightedValue<T>> items, System.Random generator = null)
		{
			int num = 0;
			foreach (WeightedValue<T> weightedValue in items)
			{
				num += weightedValue.Weight;
			}
			int num2 = (generator == null) ? UnityEngine.Random.Range(0, num) : generator.Next(0, num);
			foreach (WeightedValue<T> weightedValue2 in items)
			{
				if (num2 < weightedValue2.Weight)
				{
					return weightedValue2.Value;
				}
				num2 -= weightedValue2.Weight;
			}
			return default(T);
		}

		// Token: 0x06005509 RID: 21769 RVA: 0x001B4584 File Offset: 0x001B2984
		public static T GetRandomItem(IEnumerable<WeightedValue<T>> items, int totalWeight, System.Random generator = null)
		{
			int num = (generator == null) ? UnityEngine.Random.Range(0, totalWeight) : generator.Next(0, totalWeight);
			foreach (WeightedValue<T> weightedValue in items)
			{
				if (num < weightedValue.Weight)
				{
					return weightedValue.Value;
				}
				num -= weightedValue.Weight;
			}
			return default(T);
		}

		// Token: 0x0600550A RID: 21770 RVA: 0x001B461C File Offset: 0x001B2A1C
		private T GetRandomItem(int resultWeightIndex)
		{
			foreach (WeightedValue<T> weightedValue in this.Items)
			{
				if (resultWeightIndex < weightedValue.Weight)
				{
					return weightedValue.Value;
				}
				resultWeightIndex -= weightedValue.Weight;
			}
			return default(T);
		}

		// Token: 0x0600550B RID: 21771 RVA: 0x001B46A0 File Offset: 0x001B2AA0
		private T RemoveRandomItem(int resultWeightIndex)
		{
			for (int i = 0; i < this.Items.Count; i++)
			{
				if (resultWeightIndex < this.Items[i].Weight)
				{
					T value = this.Items[i].Value;
					this.TotalWeight -= this.Items[i].Weight;
					this.Items.RemoveAt(i);
					return value;
				}
				resultWeightIndex -= this.Items[i].Weight;
			}
			return default(T);
		}

		// Token: 0x0400360D RID: 13837
		public List<WeightedValue<T>> Items = new List<WeightedValue<T>>();

		// Token: 0x0400360E RID: 13838
		public int TotalWeight;
	}
}

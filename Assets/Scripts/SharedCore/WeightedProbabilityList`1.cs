// Decompile from assembly: Assembly-CSharp.dll

using SharedCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SharedCore
{
	[Serializable]
	public class WeightedProbabilityList<T>
	{
		private sealed class _MatchingItems_c__AnonStorey0
		{
			internal Func<T, int, bool> predicate;

			internal bool __m__0(WeightedValue<T> t)
			{
				return this.predicate(t.Value, t.Weight);
			}
		}

		private sealed class _MatchingItems_c__AnonStorey1
		{
			internal Func<T, bool> predicate;

			internal bool __m__0(T item, int weight)
			{
				return this.predicate(item);
			}
		}

		public List<WeightedValue<T>> Items = new List<WeightedValue<T>>();

		public int TotalWeight;

		private static Func<WeightedValue<T>, T> __f__am_cache0;

		private static Func<WeightedValue<T>, int> __f__am_cache1;

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

		public int Count
		{
			get
			{
				return this.Items.Count;
			}
		}

		public WeightedProbabilityList()
		{
		}

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

		public void Add(T value, int weight = 1)
		{
			if (weight < 1)
			{
				weight = 1;
			}
			this.Items.Add(new WeightedValue<T>(value, weight));
			this.TotalWeight += weight;
		}

		public void Clear()
		{
			this.TotalWeight = 0;
			this.Items.Clear();
		}

		public T GetRandomItem(System.Random generator)
		{
			return this.GetRandomItem(generator.Next(0, this.TotalWeight));
		}

		public T GetRandomItem()
		{
			return this.GetRandomItem(UnityEngine.Random.Range(0, this.TotalWeight));
		}

		public T RemoveRandomItem(System.Random generator)
		{
			return this.RemoveRandomItem(generator.Next(0, this.TotalWeight));
		}

		public T RemoveRandomItem()
		{
			return this.RemoveRandomItem(UnityEngine.Random.Range(0, this.TotalWeight));
		}

		public List<T> ToList()
		{
			IEnumerable<WeightedValue<T>> arg_23_0 = this.Items;
			if (WeightedProbabilityList<T>.__f__am_cache0 == null)
			{
				WeightedProbabilityList<T>.__f__am_cache0 = new Func<WeightedValue<T>, T>(WeightedProbabilityList<T>._ToList_m__0);
			}
			return arg_23_0.Select(WeightedProbabilityList<T>.__f__am_cache0).ToList<T>();
		}

		public List<int> GetWeights()
		{
			IEnumerable<WeightedValue<T>> arg_23_0 = this.Items;
			if (WeightedProbabilityList<T>.__f__am_cache1 == null)
			{
				WeightedProbabilityList<T>.__f__am_cache1 = new Func<WeightedValue<T>, int>(WeightedProbabilityList<T>._GetWeights_m__1);
			}
			return arg_23_0.Select(WeightedProbabilityList<T>.__f__am_cache1).ToList<int>();
		}

		public WeightedProbabilityList<T> MatchingItems(Func<T, int, bool> predicate)
		{
			WeightedProbabilityList<T>._MatchingItems_c__AnonStorey0 _MatchingItems_c__AnonStorey = new WeightedProbabilityList<T>._MatchingItems_c__AnonStorey0();
			_MatchingItems_c__AnonStorey.predicate = predicate;
			WeightedProbabilityList<T> weightedProbabilityList = new WeightedProbabilityList<T>();
			foreach (WeightedValue<T> current in this.Items.Where(new Func<WeightedValue<T>, bool>(_MatchingItems_c__AnonStorey.__m__0)))
			{
				weightedProbabilityList.Add(current.Value, current.Weight);
			}
			return weightedProbabilityList;
		}

		public WeightedProbabilityList<T> MatchingItems(Func<T, bool> predicate)
		{
			WeightedProbabilityList<T>._MatchingItems_c__AnonStorey1 _MatchingItems_c__AnonStorey = new WeightedProbabilityList<T>._MatchingItems_c__AnonStorey1();
			_MatchingItems_c__AnonStorey.predicate = predicate;
			return this.MatchingItems(new Func<T, int, bool>(_MatchingItems_c__AnonStorey.__m__0));
		}

		public static T GetRandomItem(IEnumerable<WeightedValue<T>> items, System.Random generator = null)
		{
			int num = 0;
			foreach (WeightedValue<T> current in items)
			{
				num += current.Weight;
			}
			int num2 = (generator == null) ? UnityEngine.Random.Range(0, num) : generator.Next(0, num);
			foreach (WeightedValue<T> current2 in items)
			{
				if (num2 < current2.Weight)
				{
					return current2.Value;
				}
				num2 -= current2.Weight;
			}
			return default(T);
		}

		public static T GetRandomItem(IEnumerable<WeightedValue<T>> items, int totalWeight, System.Random generator = null)
		{
			int num = (generator == null) ? UnityEngine.Random.Range(0, totalWeight) : generator.Next(0, totalWeight);
			foreach (WeightedValue<T> current in items)
			{
				if (num < current.Weight)
				{
					return current.Value;
				}
				num -= current.Weight;
			}
			return default(T);
		}

		private T GetRandomItem(int resultWeightIndex)
		{
			foreach (WeightedValue<T> current in this.Items)
			{
				if (resultWeightIndex < current.Weight)
				{
					return current.Value;
				}
				resultWeightIndex -= current.Weight;
			}
			return default(T);
		}

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

		private static T _ToList_m__0(WeightedValue<T> item)
		{
			return item.Value;
		}

		private static int _GetWeights_m__1(WeightedValue<T> item)
		{
			return item.Weight;
		}
	}
}

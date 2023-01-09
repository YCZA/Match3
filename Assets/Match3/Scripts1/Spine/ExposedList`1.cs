using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Match3.Scripts1.Spine
{
	// Token: 0x0200020F RID: 527
	[DebuggerDisplay("Count={Count}")]
	[Serializable]
	public class ExposedList<T> : IEnumerable<T>, IEnumerable
	{
		// Token: 0x06000FE8 RID: 4072 RVA: 0x00026227 File Offset: 0x00024627
		public ExposedList()
		{
			this.Items = ExposedList<T>.EmptyArray;
		}

		// Token: 0x06000FE9 RID: 4073 RVA: 0x0002623C File Offset: 0x0002463C
		public ExposedList(IEnumerable<T> collection)
		{
			this.CheckCollection(collection);
			ICollection<T> collection2 = collection as ICollection<T>;
			if (collection2 == null)
			{
				this.Items = ExposedList<T>.EmptyArray;
				this.AddEnumerable(collection);
			}
			else
			{
				this.Items = new T[collection2.Count];
				this.AddCollection(collection2);
			}
		}

		// Token: 0x06000FEA RID: 4074 RVA: 0x00026292 File Offset: 0x00024692
		public ExposedList(int capacity)
		{
			if (capacity < 0)
			{
				throw new ArgumentOutOfRangeException("capacity");
			}
			this.Items = new T[capacity];
		}

		// Token: 0x06000FEB RID: 4075 RVA: 0x000262B8 File Offset: 0x000246B8
		internal ExposedList(T[] data, int size)
		{
			this.Items = data;
			this.Count = size;
		}

		// Token: 0x06000FEC RID: 4076 RVA: 0x000262D0 File Offset: 0x000246D0
		public void Add(T item)
		{
			if (this.Count == this.Items.Length)
			{
				this.GrowIfNeeded(1);
			}
			this.Items[this.Count++] = item;
			this.version++;
		}

		// Token: 0x06000FED RID: 4077 RVA: 0x00026324 File Offset: 0x00024724
		public void GrowIfNeeded(int newCount)
		{
			int num = this.Count + newCount;
			if (num > this.Items.Length)
			{
				this.Capacity = Math.Max(Math.Max(this.Capacity * 2, 4), num);
			}
		}

		// Token: 0x06000FEE RID: 4078 RVA: 0x00026362 File Offset: 0x00024762
		private void CheckRange(int idx, int count)
		{
			if (idx < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (idx + count > this.Count)
			{
				throw new ArgumentException("index and count exceed length of list");
			}
		}

		// Token: 0x06000FEF RID: 4079 RVA: 0x000263A4 File Offset: 0x000247A4
		private void AddCollection(ICollection<T> collection)
		{
			int count = collection.Count;
			if (count == 0)
			{
				return;
			}
			this.GrowIfNeeded(count);
			collection.CopyTo(this.Items, this.Count);
			this.Count += count;
		}

		// Token: 0x06000FF0 RID: 4080 RVA: 0x000263E8 File Offset: 0x000247E8
		private void AddEnumerable(IEnumerable<T> enumerable)
		{
			foreach (T item in enumerable)
			{
				this.Add(item);
			}
		}

		// Token: 0x06000FF1 RID: 4081 RVA: 0x0002643C File Offset: 0x0002483C
		public void AddRange(IEnumerable<T> collection)
		{
			this.CheckCollection(collection);
			ICollection<T> collection2 = collection as ICollection<T>;
			if (collection2 != null)
			{
				this.AddCollection(collection2);
			}
			else
			{
				this.AddEnumerable(collection);
			}
			this.version++;
		}

		// Token: 0x06000FF2 RID: 4082 RVA: 0x0002647E File Offset: 0x0002487E
		public int BinarySearch(T item)
		{
			return Array.BinarySearch<T>(this.Items, 0, this.Count, item);
		}

		// Token: 0x06000FF3 RID: 4083 RVA: 0x00026493 File Offset: 0x00024893
		public int BinarySearch(T item, IComparer<T> comparer)
		{
			return Array.BinarySearch<T>(this.Items, 0, this.Count, item, comparer);
		}

		// Token: 0x06000FF4 RID: 4084 RVA: 0x000264A9 File Offset: 0x000248A9
		public int BinarySearch(int index, int count, T item, IComparer<T> comparer)
		{
			this.CheckRange(index, count);
			return Array.BinarySearch<T>(this.Items, index, count, item, comparer);
		}

		// Token: 0x06000FF5 RID: 4085 RVA: 0x000264C3 File Offset: 0x000248C3
		public void Clear(bool clearArray = true)
		{
			if (clearArray)
			{
				Array.Clear(this.Items, 0, this.Items.Length);
			}
			this.Count = 0;
			this.version++;
		}

		// Token: 0x06000FF6 RID: 4086 RVA: 0x000264F4 File Offset: 0x000248F4
		public bool Contains(T item)
		{
			return Array.IndexOf<T>(this.Items, item, 0, this.Count) != -1;
		}

		// Token: 0x06000FF7 RID: 4087 RVA: 0x00026510 File Offset: 0x00024910
		public ExposedList<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter)
		{
			if (converter == null)
			{
				throw new ArgumentNullException("converter");
			}
			ExposedList<TOutput> exposedList = new ExposedList<TOutput>(this.Count);
			for (int i = 0; i < this.Count; i++)
			{
				exposedList.Items[i] = converter(this.Items[i]);
			}
			exposedList.Count = this.Count;
			return exposedList;
		}

		// Token: 0x06000FF8 RID: 4088 RVA: 0x0002657C File Offset: 0x0002497C
		public void CopyTo(T[] array)
		{
			Array.Copy(this.Items, 0, array, 0, this.Count);
		}

		// Token: 0x06000FF9 RID: 4089 RVA: 0x00026592 File Offset: 0x00024992
		public void CopyTo(T[] array, int arrayIndex)
		{
			Array.Copy(this.Items, 0, array, arrayIndex, this.Count);
		}

		// Token: 0x06000FFA RID: 4090 RVA: 0x000265A8 File Offset: 0x000249A8
		public void CopyTo(int index, T[] array, int arrayIndex, int count)
		{
			this.CheckRange(index, count);
			Array.Copy(this.Items, index, array, arrayIndex, count);
		}

		// Token: 0x06000FFB RID: 4091 RVA: 0x000265C3 File Offset: 0x000249C3
		public bool Exists(Predicate<T> match)
		{
			ExposedList<T>.CheckMatch(match);
			return this.GetIndex(0, this.Count, match) != -1;
		}

		// Token: 0x06000FFC RID: 4092 RVA: 0x000265E0 File Offset: 0x000249E0
		public T Find(Predicate<T> match)
		{
			ExposedList<T>.CheckMatch(match);
			int index = this.GetIndex(0, this.Count, match);
			return (index == -1) ? default(T) : this.Items[index];
		}

		// Token: 0x06000FFD RID: 4093 RVA: 0x00026623 File Offset: 0x00024A23
		private static void CheckMatch(Predicate<T> match)
		{
			if (match == null)
			{
				throw new ArgumentNullException("match");
			}
		}

		// Token: 0x06000FFE RID: 4094 RVA: 0x00026636 File Offset: 0x00024A36
		public ExposedList<T> FindAll(Predicate<T> match)
		{
			ExposedList<T>.CheckMatch(match);
			return this.FindAllList(match);
		}

		// Token: 0x06000FFF RID: 4095 RVA: 0x00026648 File Offset: 0x00024A48
		private ExposedList<T> FindAllList(Predicate<T> match)
		{
			ExposedList<T> exposedList = new ExposedList<T>();
			for (int i = 0; i < this.Count; i++)
			{
				if (match(this.Items[i]))
				{
					exposedList.Add(this.Items[i]);
				}
			}
			return exposedList;
		}

		// Token: 0x06001000 RID: 4096 RVA: 0x0002669C File Offset: 0x00024A9C
		public int FindIndex(Predicate<T> match)
		{
			ExposedList<T>.CheckMatch(match);
			return this.GetIndex(0, this.Count, match);
		}

		// Token: 0x06001001 RID: 4097 RVA: 0x000266B2 File Offset: 0x00024AB2
		public int FindIndex(int startIndex, Predicate<T> match)
		{
			ExposedList<T>.CheckMatch(match);
			this.CheckIndex(startIndex);
			return this.GetIndex(startIndex, this.Count - startIndex, match);
		}

		// Token: 0x06001002 RID: 4098 RVA: 0x000266D1 File Offset: 0x00024AD1
		public int FindIndex(int startIndex, int count, Predicate<T> match)
		{
			ExposedList<T>.CheckMatch(match);
			this.CheckRange(startIndex, count);
			return this.GetIndex(startIndex, count, match);
		}

		// Token: 0x06001003 RID: 4099 RVA: 0x000266EC File Offset: 0x00024AEC
		private int GetIndex(int startIndex, int count, Predicate<T> match)
		{
			int num = startIndex + count;
			for (int i = startIndex; i < num; i++)
			{
				if (match(this.Items[i]))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06001004 RID: 4100 RVA: 0x0002672C File Offset: 0x00024B2C
		public T FindLast(Predicate<T> match)
		{
			ExposedList<T>.CheckMatch(match);
			int lastIndex = this.GetLastIndex(0, this.Count, match);
			return (lastIndex != -1) ? this.Items[lastIndex] : default(T);
		}

		// Token: 0x06001005 RID: 4101 RVA: 0x0002676F File Offset: 0x00024B6F
		public int FindLastIndex(Predicate<T> match)
		{
			ExposedList<T>.CheckMatch(match);
			return this.GetLastIndex(0, this.Count, match);
		}

		// Token: 0x06001006 RID: 4102 RVA: 0x00026785 File Offset: 0x00024B85
		public int FindLastIndex(int startIndex, Predicate<T> match)
		{
			ExposedList<T>.CheckMatch(match);
			this.CheckIndex(startIndex);
			return this.GetLastIndex(0, startIndex + 1, match);
		}

		// Token: 0x06001007 RID: 4103 RVA: 0x000267A0 File Offset: 0x00024BA0
		public int FindLastIndex(int startIndex, int count, Predicate<T> match)
		{
			ExposedList<T>.CheckMatch(match);
			int num = startIndex - count + 1;
			this.CheckRange(num, count);
			return this.GetLastIndex(num, count, match);
		}

		// Token: 0x06001008 RID: 4104 RVA: 0x000267CC File Offset: 0x00024BCC
		private int GetLastIndex(int startIndex, int count, Predicate<T> match)
		{
			int num = startIndex + count;
			while (num != startIndex)
			{
				if (match(this.Items[--num]))
				{
					return num;
				}
			}
			return -1;
		}

		// Token: 0x06001009 RID: 4105 RVA: 0x00026808 File Offset: 0x00024C08
		public void ForEach(Action<T> action)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			for (int i = 0; i < this.Count; i++)
			{
				action(this.Items[i]);
			}
		}

		// Token: 0x0600100A RID: 4106 RVA: 0x0002684F File Offset: 0x00024C4F
		public ExposedList<T>.Enumerator GetEnumerator()
		{
			return new ExposedList<T>.Enumerator(this);
		}

		// Token: 0x0600100B RID: 4107 RVA: 0x00026858 File Offset: 0x00024C58
		public ExposedList<T> GetRange(int index, int count)
		{
			this.CheckRange(index, count);
			T[] array = new T[count];
			Array.Copy(this.Items, index, array, 0, count);
			return new ExposedList<T>(array, count);
		}

		// Token: 0x0600100C RID: 4108 RVA: 0x0002688A File Offset: 0x00024C8A
		public int IndexOf(T item)
		{
			return Array.IndexOf<T>(this.Items, item, 0, this.Count);
		}

		// Token: 0x0600100D RID: 4109 RVA: 0x0002689F File Offset: 0x00024C9F
		public int IndexOf(T item, int index)
		{
			this.CheckIndex(index);
			return Array.IndexOf<T>(this.Items, item, index, this.Count - index);
		}

		// Token: 0x0600100E RID: 4110 RVA: 0x000268C0 File Offset: 0x00024CC0
		public int IndexOf(T item, int index, int count)
		{
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (index + count > this.Count)
			{
				throw new ArgumentOutOfRangeException("index and count exceed length of list");
			}
			return Array.IndexOf<T>(this.Items, item, index, count);
		}

		// Token: 0x0600100F RID: 4111 RVA: 0x00026918 File Offset: 0x00024D18
		private void Shift(int start, int delta)
		{
			if (delta < 0)
			{
				start -= delta;
			}
			if (start < this.Count)
			{
				Array.Copy(this.Items, start, this.Items, start + delta, this.Count - start);
			}
			this.Count += delta;
			if (delta < 0)
			{
				Array.Clear(this.Items, this.Count, -delta);
			}
		}

		// Token: 0x06001010 RID: 4112 RVA: 0x00026982 File Offset: 0x00024D82
		private void CheckIndex(int index)
		{
			if (index < 0 || index > this.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
		}

		// Token: 0x06001011 RID: 4113 RVA: 0x000269A4 File Offset: 0x00024DA4
		public void Insert(int index, T item)
		{
			this.CheckIndex(index);
			if (this.Count == this.Items.Length)
			{
				this.GrowIfNeeded(1);
			}
			this.Shift(index, 1);
			this.Items[index] = item;
			this.version++;
		}

		// Token: 0x06001012 RID: 4114 RVA: 0x000269F5 File Offset: 0x00024DF5
		private void CheckCollection(IEnumerable<T> collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
		}

		// Token: 0x06001013 RID: 4115 RVA: 0x00026A08 File Offset: 0x00024E08
		public void InsertRange(int index, IEnumerable<T> collection)
		{
			this.CheckCollection(collection);
			this.CheckIndex(index);
			if (collection == this)
			{
				T[] array = new T[this.Count];
				this.CopyTo(array, 0);
				this.GrowIfNeeded(this.Count);
				this.Shift(index, array.Length);
				Array.Copy(array, 0, this.Items, index, array.Length);
			}
			else
			{
				ICollection<T> collection2 = collection as ICollection<T>;
				if (collection2 != null)
				{
					this.InsertCollection(index, collection2);
				}
				else
				{
					this.InsertEnumeration(index, collection);
				}
			}
			this.version++;
		}

		// Token: 0x06001014 RID: 4116 RVA: 0x00026A9C File Offset: 0x00024E9C
		private void InsertCollection(int index, ICollection<T> collection)
		{
			int count = collection.Count;
			this.GrowIfNeeded(count);
			this.Shift(index, count);
			collection.CopyTo(this.Items, index);
		}

		// Token: 0x06001015 RID: 4117 RVA: 0x00026ACC File Offset: 0x00024ECC
		private void InsertEnumeration(int index, IEnumerable<T> enumerable)
		{
			foreach (T item in enumerable)
			{
				this.Insert(index++, item);
			}
		}

		// Token: 0x06001016 RID: 4118 RVA: 0x00026B28 File Offset: 0x00024F28
		public int LastIndexOf(T item)
		{
			return Array.LastIndexOf<T>(this.Items, item, this.Count - 1, this.Count);
		}

		// Token: 0x06001017 RID: 4119 RVA: 0x00026B44 File Offset: 0x00024F44
		public int LastIndexOf(T item, int index)
		{
			this.CheckIndex(index);
			return Array.LastIndexOf<T>(this.Items, item, index, index + 1);
		}

		// Token: 0x06001018 RID: 4120 RVA: 0x00026B60 File Offset: 0x00024F60
		public int LastIndexOf(T item, int index, int count)
		{
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", index, "index is negative");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", count, "count is negative");
			}
			if (index - count + 1 < 0)
			{
				throw new ArgumentOutOfRangeException("count", count, "count is too large");
			}
			return Array.LastIndexOf<T>(this.Items, item, index, count);
		}

		// Token: 0x06001019 RID: 4121 RVA: 0x00026BD8 File Offset: 0x00024FD8
		public bool Remove(T item)
		{
			int num = this.IndexOf(item);
			if (num != -1)
			{
				this.RemoveAt(num);
			}
			return num != -1;
		}

		// Token: 0x0600101A RID: 4122 RVA: 0x00026C04 File Offset: 0x00025004
		public int RemoveAll(Predicate<T> match)
		{
			ExposedList<T>.CheckMatch(match);
			int i;
			for (i = 0; i < this.Count; i++)
			{
				if (match(this.Items[i]))
				{
					break;
				}
			}
			if (i == this.Count)
			{
				return 0;
			}
			this.version++;
			int j;
			for (j = i + 1; j < this.Count; j++)
			{
				if (!match(this.Items[j]))
				{
					this.Items[i++] = this.Items[j];
				}
			}
			if (j - i > 0)
			{
				Array.Clear(this.Items, i, j - i);
			}
			this.Count = i;
			return j - i;
		}

		// Token: 0x0600101B RID: 4123 RVA: 0x00026CD8 File Offset: 0x000250D8
		public void RemoveAt(int index)
		{
			if (index < 0 || index >= this.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			this.Shift(index, -1);
			Array.Clear(this.Items, this.Count, 1);
			this.version++;
		}

		// Token: 0x0600101C RID: 4124 RVA: 0x00026D2B File Offset: 0x0002512B
		public void RemoveRange(int index, int count)
		{
			this.CheckRange(index, count);
			if (count > 0)
			{
				this.Shift(index, -count);
				Array.Clear(this.Items, this.Count, count);
				this.version++;
			}
		}

		// Token: 0x0600101D RID: 4125 RVA: 0x00026D65 File Offset: 0x00025165
		public void Reverse()
		{
			Array.Reverse(this.Items, 0, this.Count);
			this.version++;
		}

		// Token: 0x0600101E RID: 4126 RVA: 0x00026D87 File Offset: 0x00025187
		public void Reverse(int index, int count)
		{
			this.CheckRange(index, count);
			Array.Reverse(this.Items, index, count);
			this.version++;
		}

		// Token: 0x0600101F RID: 4127 RVA: 0x00026DAC File Offset: 0x000251AC
		public void Sort()
		{
			Array.Sort<T>(this.Items, 0, this.Count, Comparer<T>.Default);
			this.version++;
		}

		// Token: 0x06001020 RID: 4128 RVA: 0x00026DD3 File Offset: 0x000251D3
		public void Sort(IComparer<T> comparer)
		{
			Array.Sort<T>(this.Items, 0, this.Count, comparer);
			this.version++;
		}

		// Token: 0x06001021 RID: 4129 RVA: 0x00026DF6 File Offset: 0x000251F6
		public void Sort(Comparison<T> comparison)
		{
			Array.Sort<T>(this.Items, comparison);
			this.version++;
		}

		// Token: 0x06001022 RID: 4130 RVA: 0x00026E12 File Offset: 0x00025212
		public void Sort(int index, int count, IComparer<T> comparer)
		{
			this.CheckRange(index, count);
			Array.Sort<T>(this.Items, index, count, comparer);
			this.version++;
		}

		// Token: 0x06001023 RID: 4131 RVA: 0x00026E38 File Offset: 0x00025238
		public T[] ToArray()
		{
			T[] array = new T[this.Count];
			Array.Copy(this.Items, array, this.Count);
			return array;
		}

		// Token: 0x06001024 RID: 4132 RVA: 0x00026E64 File Offset: 0x00025264
		public void TrimExcess()
		{
			this.Capacity = this.Count;
		}

		// Token: 0x06001025 RID: 4133 RVA: 0x00026E74 File Offset: 0x00025274
		public bool TrueForAll(Predicate<T> match)
		{
			ExposedList<T>.CheckMatch(match);
			for (int i = 0; i < this.Count; i++)
			{
				if (!match(this.Items[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06001026 RID: 4134 RVA: 0x00026EB8 File Offset: 0x000252B8
		// (set) Token: 0x06001027 RID: 4135 RVA: 0x00026EC2 File Offset: 0x000252C2
		public int Capacity
		{
			get
			{
				return this.Items.Length;
			}
			set
			{
				if (value < this.Count)
				{
					throw new ArgumentOutOfRangeException();
				}
				Array.Resize<T>(ref this.Items, value);
			}
		}

		// Token: 0x06001028 RID: 4136 RVA: 0x00026EE2 File Offset: 0x000252E2
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06001029 RID: 4137 RVA: 0x00026EEF File Offset: 0x000252EF
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04004109 RID: 16649
		public T[] Items;

		// Token: 0x0400410A RID: 16650
		public int Count;

		// Token: 0x0400410B RID: 16651
		private const int DefaultCapacity = 4;

		// Token: 0x0400410C RID: 16652
		private static readonly T[] EmptyArray = new T[0];

		// Token: 0x0400410D RID: 16653
		private int version;

		// Token: 0x02000210 RID: 528
		[Serializable]
		public struct Enumerator : IEnumerator<T>, IDisposable, IEnumerator
		{
			// Token: 0x0600102B RID: 4139 RVA: 0x00026F09 File Offset: 0x00025309
			internal Enumerator(ExposedList<T> l)
			{
				this = default(ExposedList<T>.Enumerator);
				this.l = l;
				this.ver = l.version;
			}

			// Token: 0x0600102C RID: 4140 RVA: 0x00026F25 File Offset: 0x00025325
			public void Dispose()
			{
				this.l = null;
			}

			// Token: 0x0600102D RID: 4141 RVA: 0x00026F30 File Offset: 0x00025330
			private void VerifyState()
			{
				if (this.l == null)
				{
					throw new ObjectDisposedException(base.GetType().FullName);
				}
				if (this.ver != this.l.version)
				{
					throw new InvalidOperationException("Collection was modified; enumeration operation may not execute.");
				}
			}

			// Token: 0x0600102E RID: 4142 RVA: 0x00026F84 File Offset: 0x00025384
			public bool MoveNext()
			{
				this.VerifyState();
				if (this.next < 0)
				{
					return false;
				}
				if (this.next < this.l.Count)
				{
					this.current = this.l.Items[this.next++];
					return true;
				}
				this.next = -1;
				return false;
			}

			// Token: 0x17000216 RID: 534
			// (get) Token: 0x0600102F RID: 4143 RVA: 0x00026FEC File Offset: 0x000253EC
			public T Current
			{
				get
				{
					return this.current;
				}
			}

			// Token: 0x06001030 RID: 4144 RVA: 0x00026FF4 File Offset: 0x000253F4
			void IEnumerator.Reset()
			{
				this.VerifyState();
				this.next = 0;
			}

			// Token: 0x17000215 RID: 533
			// (get) Token: 0x06001031 RID: 4145 RVA: 0x00027003 File Offset: 0x00025403
			object IEnumerator.Current
			{
				get
				{
					this.VerifyState();
					if (this.next <= 0)
					{
						throw new InvalidOperationException();
					}
					return this.current;
				}
			}

			// Token: 0x0400410E RID: 16654
			private ExposedList<T> l;

			// Token: 0x0400410F RID: 16655
			private int next;

			// Token: 0x04004110 RID: 16656
			private int ver;

			// Token: 0x04004111 RID: 16657
			private T current;
		}
	}
}

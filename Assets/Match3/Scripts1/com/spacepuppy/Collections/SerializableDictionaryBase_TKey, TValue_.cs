using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Match3.Scripts1.com.spacepuppy.Collections
{
	// Token: 0x02000ABF RID: 2751
	[Serializable]
	public class SerializableDictionaryBase<TKey, TValue> : DrawableDictionary, IDictionary<TKey, TValue>, ISerializationCallbackReceiver, IEnumerable, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>
	{
		// Token: 0x17000972 RID: 2418
		// (get) Token: 0x06004157 RID: 16727 RVA: 0x000079CE File Offset: 0x00005DCE
		public int Count
		{
			get
			{
				return (this._dict == null) ? 0 : this._dict.Count;
			}
		}

		// Token: 0x06004158 RID: 16728 RVA: 0x000079EC File Offset: 0x00005DEC
		public void Add(TKey key, TValue value)
		{
			if (this._dict == null)
			{
				this._dict = new Dictionary<TKey, TValue>();
			}
			this._dict.Add(key, value);
		}

		// Token: 0x06004159 RID: 16729 RVA: 0x00007A11 File Offset: 0x00005E11
		public bool ContainsKey(TKey key)
		{
			return this._dict != null && this._dict.ContainsKey(key);
		}

		// Token: 0x17000973 RID: 2419
		// (get) Token: 0x0600415A RID: 16730 RVA: 0x00007A2C File Offset: 0x00005E2C
		public ICollection<TKey> Keys
		{
			get
			{
				if (this._dict == null)
				{
					this._dict = new Dictionary<TKey, TValue>();
				}
				return this._dict.Keys;
			}
		}

		// Token: 0x0600415B RID: 16731 RVA: 0x00007A4F File Offset: 0x00005E4F
		public bool Remove(TKey key)
		{
			return this._dict != null && this._dict.Remove(key);
		}

		// Token: 0x0600415C RID: 16732 RVA: 0x00007A6C File Offset: 0x00005E6C
		public bool TryGetValue(TKey key, out TValue value)
		{
			if (this._dict == null)
			{
				value = default(TValue);
				return false;
			}
			return this._dict.TryGetValue(key, out value);
		}

		// Token: 0x17000974 RID: 2420
		// (get) Token: 0x0600415D RID: 16733 RVA: 0x00007AA2 File Offset: 0x00005EA2
		public ICollection<TValue> Values
		{
			get
			{
				if (this._dict == null)
				{
					this._dict = new Dictionary<TKey, TValue>();
				}
				return this._dict.Values;
			}
		}

		// Token: 0x17000975 RID: 2421
		public TValue this[TKey key]
		{
			get
			{
				if (this._dict == null)
				{
					throw new KeyNotFoundException();
				}
				return this._dict[key];
			}
			set
			{
				if (this._dict == null)
				{
					this._dict = new Dictionary<TKey, TValue>();
				}
				this._dict[key] = value;
			}
		}

		// Token: 0x06004160 RID: 16736 RVA: 0x00007B09 File Offset: 0x00005F09
		public void Clear()
		{
			if (this._dict != null)
			{
				this._dict.Clear();
			}
		}

		// Token: 0x06004161 RID: 16737 RVA: 0x00007B21 File Offset: 0x00005F21
		void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
		{
			if (this._dict == null)
			{
				this._dict = new Dictionary<TKey, TValue>();
			}
			((ICollection<KeyValuePair<TKey, TValue>>)this._dict).Add(item);
		}

		// Token: 0x06004162 RID: 16738 RVA: 0x00007B45 File Offset: 0x00005F45
		bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
		{
			return this._dict != null && ((ICollection<KeyValuePair<TKey, TValue>>)this._dict).Contains(item);
		}

		// Token: 0x06004163 RID: 16739 RVA: 0x00007B60 File Offset: 0x00005F60
		void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			if (this._dict == null)
			{
				return;
			}
			((ICollection<KeyValuePair<TKey, TValue>>)this._dict).CopyTo(array, arrayIndex);
		}

		// Token: 0x06004164 RID: 16740 RVA: 0x00007B7B File Offset: 0x00005F7B
		bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
		{
			return this._dict != null && ((ICollection<KeyValuePair<TKey, TValue>>)this._dict).Remove(item);
		}

		// Token: 0x17000971 RID: 2417
		// (get) Token: 0x06004165 RID: 16741 RVA: 0x00007B96 File Offset: 0x00005F96
		bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06004166 RID: 16742 RVA: 0x00007B9C File Offset: 0x00005F9C
		public Dictionary<TKey, TValue>.Enumerator GetEnumerator()
		{
			if (this._dict == null)
			{
				return default(Dictionary<TKey, TValue>.Enumerator);
			}
			return this._dict.GetEnumerator();
		}

		// Token: 0x06004167 RID: 16743 RVA: 0x00007BC9 File Offset: 0x00005FC9
		IEnumerator IEnumerable.GetEnumerator()
		{
			if (this._dict == null)
			{
				return Enumerable.Empty<KeyValuePair<TKey, TValue>>().GetEnumerator();
			}
			return this._dict.GetEnumerator();
		}

		// Token: 0x06004168 RID: 16744 RVA: 0x00007BF1 File Offset: 0x00005FF1
		IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
		{
			if (this._dict == null)
			{
				return Enumerable.Empty<KeyValuePair<TKey, TValue>>().GetEnumerator();
			}
			return this._dict.GetEnumerator();
		}

		// Token: 0x06004169 RID: 16745 RVA: 0x00007C1C File Offset: 0x0000601C
		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			if (this._keys != null && this._values != null)
			{
				if (this._dict == null)
				{
					this._dict = new Dictionary<TKey, TValue>(this._keys.Length);
				}
				else
				{
					this._dict.Clear();
				}
				for (int i = 0; i < this._keys.Length; i++)
				{
					if (i < this._values.Length)
					{
						this._dict[this._keys[i]] = this._values[i];
					}
					else
					{
						this._dict[this._keys[i]] = default(TValue);
					}
				}
			}
			this._keys = null;
			this._values = null;
		}

		// Token: 0x0600416A RID: 16746 RVA: 0x00007CEC File Offset: 0x000060EC
		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			if (this._dict == null || this._dict.Count == 0)
			{
				this._keys = null;
				this._values = null;
			}
			else
			{
				int count = this._dict.Count;
				this._keys = new TKey[count];
				this._values = new TValue[count];
				int num = 0;
				Dictionary<TKey, TValue>.Enumerator enumerator = this._dict.GetEnumerator();
				while (enumerator.MoveNext())
				{
					TKey[] keys = this._keys;
					int num2 = num;
					KeyValuePair<TKey, TValue> keyValuePair = enumerator.Current;
					keys[num2] = keyValuePair.Key;
					TValue[] values = this._values;
					int num3 = num;
					KeyValuePair<TKey, TValue> keyValuePair2 = enumerator.Current;
					values[num3] = keyValuePair2.Value;
					num++;
				}
			}
		}

		// Token: 0x04006AEA RID: 27370
		[NonSerialized]
		private Dictionary<TKey, TValue> _dict;

		// Token: 0x04006AEB RID: 27371
		[SerializeField]
		private TKey[] _keys;

		// Token: 0x04006AEC RID: 27372
		[SerializeField]
		private TValue[] _values;
	}
}

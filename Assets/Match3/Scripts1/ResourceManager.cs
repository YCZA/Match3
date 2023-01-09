using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Match3.Scripts1
{
	// Token: 0x02000B1C RID: 2844
	public abstract class ResourceManager<T> : MonoBehaviour where T : global::UnityEngine.Object
	{
		// Token: 0x170009A0 RID: 2464
		// (get) Token: 0x060042D4 RID: 17108 RVA: 0x000C0CED File Offset: 0x000BF0ED
		public int Count
		{
			get
			{
				return this.m_lookup.Count;
			}
		}

		// Token: 0x170009A1 RID: 2465
		public T this[string key]
		{
			get
			{
				if (!this.IsInitialized)
				{
					this.Initialize();
				}
				T t;
				if (!this.m_lookup.TryGetValue(key, out t) && ResourceManager<T>.s_regex.IsMatch(key))
				{
					key = ResourceManager<T>.s_regex.Replace(key, ".0");
					if (this.m_lookup.TryGetValue(key, out t))
					{
						return t;
					}
				}
				if (t == null && this.resourceNameOverrides != null)
				{
					global::UnityEngine.Object @object;
					this.resourceNameOverrides.TryGetValue(key, out @object);
					t = (T)((object)@object);
				}
				return (!t) ? this.fallback : t;
			}
			set
			{
				if (!this.IsInitialized)
				{
					this.Initialize();
				}
				if (this.m_lookup.ContainsKey(key))
				{
					this.m_lookup[key] = value;
				}
			}
		}

		// Token: 0x060042D7 RID: 17111 RVA: 0x000C0DE3 File Offset: 0x000BF1E3
		public bool HasItem(string key)
		{
			if (!this.IsInitialized)
			{
				this.Initialize();
			}
			return this.m_lookup.ContainsKey(key);
		}

		// Token: 0x060042D8 RID: 17112 RVA: 0x000C0E04 File Offset: 0x000BF204
		public bool TryGetValue(string key, out T value)
		{
			if (!this.IsInitialized)
			{
				this.Initialize();
			}
			if (!this.m_lookup.TryGetValue(key, out value))
			{
				value = this.fallback;
				WoogaDebug.LogWarningFormatted("Didn't find resource for key {0}.", new object[]
				{
					key
				});
				return false;
			}
			return true;
		}

		// Token: 0x060042D9 RID: 17113 RVA: 0x000C0E57 File Offset: 0x000BF257
		public T GetSimilar(string substring)
		{
			return this.GetSimilar(substring, true);
		}

		// Token: 0x060042DA RID: 17114 RVA: 0x000C0E64 File Offset: 0x000BF264
		public T GetSimilar(string substring, bool allowFallback)
		{
			if (!this.IsInitialized)
			{
				this.Initialize();
			}
			foreach (KeyValuePair<string, T> keyValuePair in this.m_lookup)
			{
				if (keyValuePair.Key.Contains(substring, StringComparison.OrdinalIgnoreCase))
				{
					return keyValuePair.Value;
				}
			}
			if (this.resourceNameOverrides.ContainsKey(substring))
			{
				return (T)((object)this.resourceNameOverrides[substring]);
			}
			foreach (string text in this.resourceNameOverrides.Keys)
			{
				if (text.Contains(substring, StringComparison.OrdinalIgnoreCase))
				{
					return (T)((object)this.resourceNameOverrides[text]);
				}
			}
			if (allowFallback)
			{
				WoogaDebug.LogWarningFormatted("Didn't find resource for substring {0}.", new object[]
				{
					substring
				});
				return this.fallback;
			}
			return (T)((object)null);
		}

		// Token: 0x060042DB RID: 17115 RVA: 0x000C0FA8 File Offset: 0x000BF3A8
		public virtual void Initialize()
		{
			this.m_lookup = new Dictionary<string, T>(StringComparer.OrdinalIgnoreCase);
			foreach (T t in this.resources)
			{
				if (t)
				{
					this.m_lookup[t.name] = t;
				}
			}
		}

		// Token: 0x170009A2 RID: 2466
		// (get) Token: 0x060042DC RID: 17116 RVA: 0x000C1016 File Offset: 0x000BF416
		private bool IsInitialized
		{
			get
			{
				return this.m_lookup != null;
			}
		}

		// Token: 0x060042DD RID: 17117 RVA: 0x000C1024 File Offset: 0x000BF424
		public T GetRandom()
		{
			if (this.resources.Length <= 0)
			{
				return (T)((object)null);
			}
			return this.resources[global::UnityEngine.Random.Range(0, this.resources.Length)];
		}

		// Token: 0x04006B99 RID: 27545
		protected Dictionary<string, T> m_lookup;

		// Token: 0x04006B9A RID: 27546
		private static readonly Regex s_regex = new Regex("\\.\\d+$");

		// Token: 0x04006B9B RID: 27547
		public T fallback;

		// Token: 0x04006B9C RID: 27548
		public T[] resources;

		// Token: 0x04006B9D RID: 27549
		// [HideInInspector]
		public ResourceItemDictionary resourceNameOverrides;
	}

	public abstract class ResourceManager<T1, T2> : ResourceManager<T1> where T1 : global::UnityEngine.Object where T2 : IConvertibleTo<T1>
	{
		// Token: 0x060042E1 RID: 17121
		public abstract void Convert(out T1 result);
	}
}
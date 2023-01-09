namespace Match3.Scripts1.com.adjust.sdk
{
	// Token: 0x02000007 RID: 7
	internal class JSONLazyCreator : JSONNode
	{
		// Token: 0x06000056 RID: 86 RVA: 0x00003E5B File Offset: 0x0000225B
		public JSONLazyCreator(JSONNode aNode)
		{
			this.m_Node = aNode;
			this.m_Key = null;
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00003E71 File Offset: 0x00002271
		public JSONLazyCreator(JSONNode aNode, string aKey)
		{
			this.m_Node = aNode;
			this.m_Key = aKey;
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00003E87 File Offset: 0x00002287
		private void Set(JSONNode aVal)
		{
			if (this.m_Key == null)
			{
				this.m_Node.Add(aVal);
			}
			else
			{
				this.m_Node.Add(this.m_Key, aVal);
			}
			this.m_Node = null;
		}

		// Token: 0x17000016 RID: 22
		public override JSONNode this[int aIndex]
		{
			get
			{
				return new JSONLazyCreator(this);
			}
			set
			{
				this.Set(new JSONArray
				{
					value
				});
			}
		}

		// Token: 0x17000017 RID: 23
		public override JSONNode this[string aKey]
		{
			get
			{
				return new JSONLazyCreator(this, aKey);
			}
			set
			{
				this.Set(new JSONClass
				{
					{
						aKey,
						value
					}
				});
			}
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00003F18 File Offset: 0x00002318
		public override void Add(JSONNode aItem)
		{
			this.Set(new JSONArray
			{
				aItem
			});
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00003F3C File Offset: 0x0000233C
		public override void Add(string aKey, JSONNode aItem)
		{
			this.Set(new JSONClass
			{
				{
					aKey,
					aItem
				}
			});
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00003F5E File Offset: 0x0000235E
		public static bool operator ==(JSONLazyCreator a, object b)
		{
			return b == null || object.ReferenceEquals(a, b);
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00003F6F File Offset: 0x0000236F
		public static bool operator !=(JSONLazyCreator a, object b)
		{
			return !(a == b);
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00003F7B File Offset: 0x0000237B
		public override bool Equals(object obj)
		{
			return obj == null || object.ReferenceEquals(this, obj);
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00003F8C File Offset: 0x0000238C
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00003F94 File Offset: 0x00002394
		public override string ToString()
		{
			return string.Empty;
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00003F9B File Offset: 0x0000239B
		public override string ToString(string aPrefix)
		{
			return string.Empty;
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000065 RID: 101 RVA: 0x00003FA4 File Offset: 0x000023A4
		// (set) Token: 0x06000066 RID: 102 RVA: 0x00003FC0 File Offset: 0x000023C0
		public override int AsInt
		{
			get
			{
				JSONData aVal = new JSONData(0);
				this.Set(aVal);
				return 0;
			}
			set
			{
				JSONData aVal = new JSONData(value);
				this.Set(aVal);
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000067 RID: 103 RVA: 0x00003FDC File Offset: 0x000023DC
		// (set) Token: 0x06000068 RID: 104 RVA: 0x00004000 File Offset: 0x00002400
		public override float AsFloat
		{
			get
			{
				JSONData aVal = new JSONData(0f);
				this.Set(aVal);
				return 0f;
			}
			set
			{
				JSONData aVal = new JSONData(value);
				this.Set(aVal);
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000069 RID: 105 RVA: 0x0000401C File Offset: 0x0000241C
		// (set) Token: 0x0600006A RID: 106 RVA: 0x00004048 File Offset: 0x00002448
		public override double AsDouble
		{
			get
			{
				JSONData aVal = new JSONData(0.0);
				this.Set(aVal);
				return 0.0;
			}
			set
			{
				JSONData aVal = new JSONData(value);
				this.Set(aVal);
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600006B RID: 107 RVA: 0x00004064 File Offset: 0x00002464
		// (set) Token: 0x0600006C RID: 108 RVA: 0x00004080 File Offset: 0x00002480
		public override bool AsBool
		{
			get
			{
				JSONData aVal = new JSONData(false);
				this.Set(aVal);
				return false;
			}
			set
			{
				JSONData aVal = new JSONData(value);
				this.Set(aVal);
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600006D RID: 109 RVA: 0x0000409C File Offset: 0x0000249C
		public override JSONArray AsArray
		{
			get
			{
				JSONArray jsonarray = new JSONArray();
				this.Set(jsonarray);
				return jsonarray;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600006E RID: 110 RVA: 0x000040B8 File Offset: 0x000024B8
		public override JSONClass AsObject
		{
			get
			{
				JSONClass jsonclass = new JSONClass();
				this.Set(jsonclass);
				return jsonclass;
			}
		}

		// Token: 0x0400000C RID: 12
		private JSONNode m_Node;

		// Token: 0x0400000D RID: 13
		private string m_Key;
	}
}

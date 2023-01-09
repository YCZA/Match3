using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Match3.Scripts1.com.adjust.sdk
{
	// Token: 0x02000004 RID: 4
	public class JSONArray : JSONNode, IEnumerable
	{
		// Token: 0x1700000D RID: 13
		public override JSONNode this[int aIndex]
		{
			get
			{
				if (aIndex < 0 || aIndex >= this.m_List.Count)
				{
					return new JSONLazyCreator(this);
				}
				return this.m_List[aIndex];
			}
			set
			{
				if (aIndex < 0 || aIndex >= this.m_List.Count)
				{
					this.m_List.Add(value);
				}
				else
				{
					this.m_List[aIndex] = value;
				}
			}
		}

		// Token: 0x1700000E RID: 14
		public override JSONNode this[string aKey]
		{
			get
			{
				return new JSONLazyCreator(this);
			}
			set
			{
				this.m_List.Add(value);
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000034 RID: 52 RVA: 0x00002FCA File Offset: 0x000013CA
		public override int Count
		{
			get
			{
				return this.m_List.Count;
			}
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002FD7 File Offset: 0x000013D7
		public override void Add(string aKey, JSONNode aItem)
		{
			this.m_List.Add(aItem);
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002FE8 File Offset: 0x000013E8
		public override JSONNode Remove(int aIndex)
		{
			if (aIndex < 0 || aIndex >= this.m_List.Count)
			{
				return null;
			}
			JSONNode result = this.m_List[aIndex];
			this.m_List.RemoveAt(aIndex);
			return result;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00003029 File Offset: 0x00001429
		public override JSONNode Remove(JSONNode aNode)
		{
			this.m_List.Remove(aNode);
			return aNode;
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000038 RID: 56 RVA: 0x0000303C File Offset: 0x0000143C
		public override IEnumerable<JSONNode> Childs
		{
			get
			{
				foreach (JSONNode N in this.m_List)
				{
					yield return N;
				}
				yield break;
			}
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00003060 File Offset: 0x00001460
		public IEnumerator GetEnumerator()
		{
			foreach (JSONNode N in this.m_List)
			{
				yield return N;
			}
			yield break;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x0000307C File Offset: 0x0000147C
		public override string ToString()
		{
			string text = "[ ";
			foreach (JSONNode jsonnode in this.m_List)
			{
				if (text.Length > 2)
				{
					text += ", ";
				}
				text += jsonnode.ToString();
			}
			text += " ]";
			return text;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x0000310C File Offset: 0x0000150C
		public override string ToString(string aPrefix)
		{
			string text = "[ ";
			foreach (JSONNode jsonnode in this.m_List)
			{
				if (text.Length > 3)
				{
					text += ", ";
				}
				text = text + "\n" + aPrefix + "   ";
				text += jsonnode.ToString(aPrefix + "   ");
			}
			text = text + "\n" + aPrefix + "]";
			return text;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x000031BC File Offset: 0x000015BC
		public override void Serialize(BinaryWriter aWriter)
		{
			aWriter.Write(1);
			aWriter.Write(this.m_List.Count);
			for (int i = 0; i < this.m_List.Count; i++)
			{
				this.m_List[i].Serialize(aWriter);
			}
		}

		// Token: 0x04000009 RID: 9
		private List<JSONNode> m_List = new List<JSONNode>();
	}
}

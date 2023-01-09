using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Match3.Scripts1.com.adjust.sdk
{
	// Token: 0x02000005 RID: 5
	public class JSONClass : JSONNode, IEnumerable
	{
		// Token: 0x17000011 RID: 17
		public override JSONNode this[string aKey]
		{
			get
			{
				if (this.m_Dict.ContainsKey(aKey))
				{
					return this.m_Dict[aKey];
				}
				return new JSONLazyCreator(this, aKey);
			}
			set
			{
				if (this.m_Dict.ContainsKey(aKey))
				{
					this.m_Dict[aKey] = value;
				}
				else
				{
					this.m_Dict.Add(aKey, value);
				}
			}
		}

		// Token: 0x17000012 RID: 18
		public override JSONNode this[int aIndex]
		{
			get
			{
				if (aIndex < 0 || aIndex >= this.m_Dict.Count)
				{
					return null;
				}
				return this.m_Dict.ElementAt(aIndex).Value;
			}
			set
			{
				if (aIndex < 0 || aIndex >= this.m_Dict.Count)
				{
					return;
				}
				string key = this.m_Dict.ElementAt(aIndex).Key;
				this.m_Dict[key] = value;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000042 RID: 66 RVA: 0x000035DC File Offset: 0x000019DC
		public override int Count
		{
			get
			{
				return this.m_Dict.Count;
			}
		}

		// Token: 0x06000043 RID: 67 RVA: 0x000035EC File Offset: 0x000019EC
		public override void Add(string aKey, JSONNode aItem)
		{
			if (!string.IsNullOrEmpty(aKey))
			{
				if (this.m_Dict.ContainsKey(aKey))
				{
					this.m_Dict[aKey] = aItem;
				}
				else
				{
					this.m_Dict.Add(aKey, aItem);
				}
			}
			else
			{
				this.m_Dict.Add(Guid.NewGuid().ToString(), aItem);
			}
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00003658 File Offset: 0x00001A58
		public override JSONNode Remove(string aKey)
		{
			if (!this.m_Dict.ContainsKey(aKey))
			{
				return null;
			}
			JSONNode result = this.m_Dict[aKey];
			this.m_Dict.Remove(aKey);
			return result;
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00003694 File Offset: 0x00001A94
		public override JSONNode Remove(int aIndex)
		{
			if (aIndex < 0 || aIndex >= this.m_Dict.Count)
			{
				return null;
			}
			KeyValuePair<string, JSONNode> keyValuePair = this.m_Dict.ElementAt(aIndex);
			this.m_Dict.Remove(keyValuePair.Key);
			return keyValuePair.Value;
		}

		// Token: 0x06000046 RID: 70 RVA: 0x000036E4 File Offset: 0x00001AE4
		public override JSONNode Remove(JSONNode aNode)
		{
			JSONNode result;
			try
			{
				KeyValuePair<string, JSONNode> keyValuePair = (from k in this.m_Dict
				where k.Value == aNode
				select k).First<KeyValuePair<string, JSONNode>>();
				this.m_Dict.Remove(keyValuePair.Key);
				result = aNode;
			}
			catch
			{
				result = null;
			}
			return result;
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000047 RID: 71 RVA: 0x00003754 File Offset: 0x00001B54
		public override IEnumerable<JSONNode> Childs
		{
			get
			{
				foreach (KeyValuePair<string, JSONNode> N in this.m_Dict)
				{
					yield return N.Value;
				}
				yield break;
			}
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00003778 File Offset: 0x00001B78
		public IEnumerator GetEnumerator()
		{
			foreach (KeyValuePair<string, JSONNode> N in this.m_Dict)
			{
				yield return N;
			}
			yield break;
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00003794 File Offset: 0x00001B94
		public override string ToString()
		{
			string text = "{";
			foreach (KeyValuePair<string, JSONNode> keyValuePair in this.m_Dict)
			{
				if (text.Length > 2)
				{
					text += ", ";
				}
				string text2 = text;
				text = string.Concat(new string[]
				{
					text2,
					"\"",
					JSONNode.Escape(keyValuePair.Key),
					"\":",
					keyValuePair.Value.ToString()
				});
			}
			text += "}";
			return text;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00003854 File Offset: 0x00001C54
		public override string ToString(string aPrefix)
		{
			string text = "{ ";
			foreach (KeyValuePair<string, JSONNode> keyValuePair in this.m_Dict)
			{
				if (text.Length > 3)
				{
					text += ", ";
				}
				text = text + "\n" + aPrefix + "   ";
				string text2 = text;
				text = string.Concat(new string[]
				{
					text2,
					"\"",
					JSONNode.Escape(keyValuePair.Key),
					"\" : ",
					keyValuePair.Value.ToString(aPrefix + "   ")
				});
			}
			text = text + "\n" + aPrefix + "}";
			return text;
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00003938 File Offset: 0x00001D38
		public override void Serialize(BinaryWriter aWriter)
		{
			aWriter.Write(2);
			aWriter.Write(this.m_Dict.Count);
			foreach (string text in this.m_Dict.Keys)
			{
				aWriter.Write(text);
				this.m_Dict[text].Serialize(aWriter);
			}
		}

		// Token: 0x0400000A RID: 10
		private Dictionary<string, JSONNode> m_Dict = new Dictionary<string, JSONNode>();
	}
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Match3.Scripts1.com.adjust.sdk
{
	// Token: 0x02000003 RID: 3
	public class JSONNode
	{
		// Token: 0x06000002 RID: 2 RVA: 0x0000234A File Offset: 0x0000074A
		public virtual void Add(string aKey, JSONNode aItem)
		{
		}

		// Token: 0x17000001 RID: 1
		public virtual JSONNode this[int aIndex]
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x17000002 RID: 2
		public virtual JSONNode this[string aKey]
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000007 RID: 7 RVA: 0x00002356 File Offset: 0x00000756
		// (set) Token: 0x06000008 RID: 8 RVA: 0x0000235D File Offset: 0x0000075D
		public virtual string Value
		{
			get
			{
				return string.Empty;
			}
			set
			{
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000009 RID: 9 RVA: 0x0000235F File Offset: 0x0000075F
		public virtual int Count
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002362 File Offset: 0x00000762
		public virtual void Add(JSONNode aItem)
		{
			this.Add(string.Empty, aItem);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002370 File Offset: 0x00000770
		public virtual JSONNode Remove(string aKey)
		{
			return null;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002373 File Offset: 0x00000773
		public virtual JSONNode Remove(int aIndex)
		{
			return null;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002376 File Offset: 0x00000776
		public virtual JSONNode Remove(JSONNode aNode)
		{
			return aNode;
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000E RID: 14 RVA: 0x0000237C File Offset: 0x0000077C
		public virtual IEnumerable<JSONNode> Childs
		{
			get
			{
				yield break;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000F RID: 15 RVA: 0x00002398 File Offset: 0x00000798
		public IEnumerable<JSONNode> DeepChilds
		{
			get
			{
				foreach (JSONNode C in this.Childs)
				{
					foreach (JSONNode D in C.DeepChilds)
					{
						yield return D;
					}
				}
				yield break;
			}
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000023BB File Offset: 0x000007BB
		public override string ToString()
		{
			return "JSONNode";
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000023C2 File Offset: 0x000007C2
		public virtual string ToString(string aPrefix)
		{
			return "JSONNode";
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000012 RID: 18 RVA: 0x000023CC File Offset: 0x000007CC
		// (set) Token: 0x06000013 RID: 19 RVA: 0x000023F0 File Offset: 0x000007F0
		public virtual int AsInt
		{
			get
			{
				int result = 0;
				if (int.TryParse(this.Value, out result))
				{
					return result;
				}
				return 0;
			}
			set
			{
				this.Value = value.ToString();
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000014 RID: 20 RVA: 0x00002408 File Offset: 0x00000808
		// (set) Token: 0x06000015 RID: 21 RVA: 0x00002434 File Offset: 0x00000834
		public virtual float AsFloat
		{
			get
			{
				float result = 0f;
				if (float.TryParse(this.Value, out result))
				{
					return result;
				}
				return 0f;
			}
			set
			{
				this.Value = value.ToString();
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000016 RID: 22 RVA: 0x0000244C File Offset: 0x0000084C
		// (set) Token: 0x06000017 RID: 23 RVA: 0x00002480 File Offset: 0x00000880
		public virtual double AsDouble
		{
			get
			{
				double result = 0.0;
				if (double.TryParse(this.Value, out result))
				{
					return result;
				}
				return 0.0;
			}
			set
			{
				this.Value = value.ToString();
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000018 RID: 24 RVA: 0x00002498 File Offset: 0x00000898
		// (set) Token: 0x06000019 RID: 25 RVA: 0x000024C9 File Offset: 0x000008C9
		public virtual bool AsBool
		{
			get
			{
				bool result = false;
				if (bool.TryParse(this.Value, out result))
				{
					return result;
				}
				return !string.IsNullOrEmpty(this.Value);
			}
			set
			{
				this.Value = ((!value) ? "false" : "true");
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600001A RID: 26 RVA: 0x000024E6 File Offset: 0x000008E6
		public virtual JSONArray AsArray
		{
			get
			{
				return this as JSONArray;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600001B RID: 27 RVA: 0x000024EE File Offset: 0x000008EE
		public virtual JSONClass AsObject
		{
			get
			{
				return this as JSONClass;
			}
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000024F6 File Offset: 0x000008F6
		public static implicit operator JSONNode(string s)
		{
			return new JSONData(s);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000024FE File Offset: 0x000008FE
		public static implicit operator string(JSONNode d)
		{
			return (!(d == null)) ? d.Value : null;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002518 File Offset: 0x00000918
		public static bool operator ==(JSONNode a, object b)
		{
			return (b == null && a is JSONLazyCreator) || object.ReferenceEquals(a, b);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002534 File Offset: 0x00000934
		public static bool operator !=(JSONNode a, object b)
		{
			return !(a == b);
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002540 File Offset: 0x00000940
		public override bool Equals(object obj)
		{
			return object.ReferenceEquals(this, obj);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002549 File Offset: 0x00000949
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002554 File Offset: 0x00000954
		internal static string Escape(string aText)
		{
			string text = string.Empty;
			foreach (char c in aText)
			{
				switch (c)
				{
				case '\b':
					text += "\\b";
					break;
				case '\t':
					text += "\\t";
					break;
				case '\n':
					text += "\\n";
					break;
				default:
					if (c != '"')
					{
						if (c != '\\')
						{
							text += c;
						}
						else
						{
							text += "\\\\";
						}
					}
					else
					{
						text += "\\\"";
					}
					break;
				case '\f':
					text += "\\f";
					break;
				case '\r':
					text += "\\r";
					break;
				}
			}
			return text;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002648 File Offset: 0x00000A48
		public static JSONNode Parse(string aJSON)
		{
			Stack<JSONNode> stack = new Stack<JSONNode>();
			JSONNode jsonnode = null;
			int i = 0;
			string text = string.Empty;
			string text2 = string.Empty;
			bool flag = false;
			while (i < aJSON.Length)
			{
				char c = aJSON[i];
				switch (c)
				{
				case '\t':
					goto IL_333;
				case '\n':
				case '\r':
					break;
				default:
					switch (c)
					{
					case '[':
						if (flag)
						{
							text += aJSON[i];
							goto IL_45C;
						}
						stack.Push(new JSONArray());
						if (jsonnode != null)
						{
							text2 = text2.Trim();
							if (jsonnode is JSONArray)
							{
								jsonnode.Add(stack.Peek());
							}
							else if (text2 != string.Empty)
							{
								jsonnode.Add(text2, stack.Peek());
							}
						}
						text2 = string.Empty;
						text = string.Empty;
						jsonnode = stack.Peek();
						goto IL_45C;
					case '\\':
						i++;
						if (flag)
						{
							char c2 = aJSON[i];
							switch (c2)
							{
							case 'r':
								text += '\r';
								break;
							default:
								if (c2 != 'b')
								{
									if (c2 != 'f')
									{
										if (c2 != 'n')
										{
											text += c2;
										}
										else
										{
											text += '\n';
										}
									}
									else
									{
										text += '\f';
									}
								}
								else
								{
									text += '\b';
								}
								break;
							case 't':
								text += '\t';
								break;
							case 'u':
							{
								string s = aJSON.Substring(i + 1, 4);
								text += (char)int.Parse(s, NumberStyles.AllowHexSpecifier);
								i += 4;
								break;
							}
							}
						}
						goto IL_45C;
					case ']':
						break;
					default:
						switch (c)
						{
						case ' ':
							goto IL_333;
						default:
							switch (c)
							{
							case '{':
								if (flag)
								{
									text += aJSON[i];
									goto IL_45C;
								}
								stack.Push(new JSONClass());
								if (jsonnode != null)
								{
									text2 = text2.Trim();
									if (jsonnode is JSONArray)
									{
										jsonnode.Add(stack.Peek());
									}
									else if (text2 != string.Empty)
									{
										jsonnode.Add(text2, stack.Peek());
									}
								}
								text2 = string.Empty;
								text = string.Empty;
								jsonnode = stack.Peek();
								goto IL_45C;
							default:
								if (c != ',')
								{
									if (c != ':')
									{
										text += aJSON[i];
										goto IL_45C;
									}
									if (flag)
									{
										text += aJSON[i];
										goto IL_45C;
									}
									text2 = text;
									text = string.Empty;
									goto IL_45C;
								}
								else
								{
									if (flag)
									{
										text += aJSON[i];
										goto IL_45C;
									}
									if (text != string.Empty)
									{
										if (jsonnode is JSONArray)
										{
											jsonnode.Add(text);
										}
										else if (text2 != string.Empty)
										{
											jsonnode.Add(text2, text);
										}
									}
									text2 = string.Empty;
									text = string.Empty;
									goto IL_45C;
								}
								break;
							case '}':
								break;
							}
							break;
						case '"':
							flag ^= true;
							goto IL_45C;
						}
						break;
					}
					if (flag)
					{
						text += aJSON[i];
					}
					else
					{
						if (stack.Count == 0)
						{
							throw new Exception("JSON Parse: Too many closing brackets");
						}
						stack.Pop();
						if (text != string.Empty)
						{
							text2 = text2.Trim();
							if (jsonnode is JSONArray)
							{
								jsonnode.Add(text);
							}
							else if (text2 != string.Empty)
							{
								jsonnode.Add(text2, text);
							}
						}
						text2 = string.Empty;
						text = string.Empty;
						if (stack.Count > 0)
						{
							jsonnode = stack.Peek();
						}
					}
					break;
				}
				IL_45C:
				i++;
				continue;
				IL_333:
				if (flag)
				{
					text += aJSON[i];
				}
				goto IL_45C;
			}
			if (flag)
			{
				throw new Exception("JSON Parse: Quotation marks seems to be messed up.");
			}
			return jsonnode;
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002AD4 File Offset: 0x00000ED4
		public virtual void Serialize(BinaryWriter aWriter)
		{
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002AD8 File Offset: 0x00000ED8
		public void SaveToStream(Stream aData)
		{
			BinaryWriter aWriter = new BinaryWriter(aData);
			this.Serialize(aWriter);
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002AF3 File Offset: 0x00000EF3
		public void SaveToCompressedStream(Stream aData)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002AFF File Offset: 0x00000EFF
		public void SaveToCompressedFile(string aFileName)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002B0B File Offset: 0x00000F0B
		public string SaveToCompressedBase64()
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002B18 File Offset: 0x00000F18
		public static JSONNode Deserialize(BinaryReader aReader)
		{
			JSONBinaryTag jsonbinaryTag = (JSONBinaryTag)aReader.ReadByte();
			switch (jsonbinaryTag)
			{
			case JSONBinaryTag.Array:
			{
				int num = aReader.ReadInt32();
				JSONArray jsonarray = new JSONArray();
				for (int i = 0; i < num; i++)
				{
					jsonarray.Add(JSONNode.Deserialize(aReader));
				}
				return jsonarray;
			}
			case JSONBinaryTag.Class:
			{
				int num2 = aReader.ReadInt32();
				JSONClass jsonclass = new JSONClass();
				for (int j = 0; j < num2; j++)
				{
					string aKey = aReader.ReadString();
					JSONNode aItem = JSONNode.Deserialize(aReader);
					jsonclass.Add(aKey, aItem);
				}
				return jsonclass;
			}
			case JSONBinaryTag.Value:
				return new JSONData(aReader.ReadString());
			case JSONBinaryTag.IntValue:
				return new JSONData(aReader.ReadInt32());
			case JSONBinaryTag.DoubleValue:
				return new JSONData(aReader.ReadDouble());
			case JSONBinaryTag.BoolValue:
				return new JSONData(aReader.ReadBoolean());
			case JSONBinaryTag.FloatValue:
				return new JSONData(aReader.ReadSingle());
			default:
				throw new Exception("Error deserializing JSON. Unknown tag: " + jsonbinaryTag);
			}
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002C17 File Offset: 0x00001017
		public static JSONNode LoadFromCompressedFile(string aFileName)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002C23 File Offset: 0x00001023
		public static JSONNode LoadFromCompressedStream(Stream aData)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002C2F File Offset: 0x0000102F
		public static JSONNode LoadFromCompressedBase64(string aBase64)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002C3C File Offset: 0x0000103C
		public static JSONNode LoadFromStream(Stream aData)
		{
			JSONNode result;
			using (BinaryReader binaryReader = new BinaryReader(aData))
			{
				result = JSONNode.Deserialize(binaryReader);
			}
			return result;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002C7C File Offset: 0x0000107C
		public static JSONNode LoadFromBase64(string aBase64)
		{
			byte[] buffer = Convert.FromBase64String(aBase64);
			return JSONNode.LoadFromStream(new MemoryStream(buffer)
			{
				Position = 0L
			});
		}
	}
}

using System.IO;

namespace Match3.Scripts1.com.adjust.sdk
{
	// Token: 0x02000006 RID: 6
	public class JSONData : JSONNode
	{
		// Token: 0x0600004C RID: 76 RVA: 0x00003CC3 File Offset: 0x000020C3
		public JSONData(string aData)
		{
			this.m_Data = aData;
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00003CD2 File Offset: 0x000020D2
		public JSONData(float aData)
		{
			this.AsFloat = aData;
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00003CE1 File Offset: 0x000020E1
		public JSONData(double aData)
		{
			this.AsDouble = aData;
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00003CF0 File Offset: 0x000020F0
		public JSONData(bool aData)
		{
			this.AsBool = aData;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00003CFF File Offset: 0x000020FF
		public JSONData(int aData)
		{
			this.AsInt = aData;
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000051 RID: 81 RVA: 0x00003D0E File Offset: 0x0000210E
		// (set) Token: 0x06000052 RID: 82 RVA: 0x00003D16 File Offset: 0x00002116
		public override string Value
		{
			get
			{
				return this.m_Data;
			}
			set
			{
				this.m_Data = value;
			}
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00003D1F File Offset: 0x0000211F
		public override string ToString()
		{
			return "\"" + JSONNode.Escape(this.m_Data) + "\"";
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00003D3B File Offset: 0x0000213B
		public override string ToString(string aPrefix)
		{
			return "\"" + JSONNode.Escape(this.m_Data) + "\"";
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00003D58 File Offset: 0x00002158
		public override void Serialize(BinaryWriter aWriter)
		{
			JSONData jsondata = new JSONData(string.Empty);
			jsondata.AsInt = this.AsInt;
			if (jsondata.m_Data == this.m_Data)
			{
				aWriter.Write(4);
				aWriter.Write(this.AsInt);
				return;
			}
			jsondata.AsFloat = this.AsFloat;
			if (jsondata.m_Data == this.m_Data)
			{
				aWriter.Write(7);
				aWriter.Write(this.AsFloat);
				return;
			}
			jsondata.AsDouble = this.AsDouble;
			if (jsondata.m_Data == this.m_Data)
			{
				aWriter.Write(5);
				aWriter.Write(this.AsDouble);
				return;
			}
			jsondata.AsBool = this.AsBool;
			if (jsondata.m_Data == this.m_Data)
			{
				aWriter.Write(6);
				aWriter.Write(this.AsBool);
				return;
			}
			aWriter.Write(3);
			aWriter.Write(this.m_Data);
		}

		// Token: 0x0400000B RID: 11
		private string m_Data;
	}
}

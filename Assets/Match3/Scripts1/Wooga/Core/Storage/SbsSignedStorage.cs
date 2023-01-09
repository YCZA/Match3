using System;
using Match3.Scripts1.Wooga.Core.Data;
using Match3.Scripts1.Wooga.Core.Exceptions;
using Match3.Scripts1.Wooga.Core.Network;
using Wooga.Core.Utilities;
using Wooga.Foundation.Json;
using Wooga.Foundation.Json.Mutable;

namespace Wooga.Core.Storage
{
	// Token: 0x0200039A RID: 922
	public class SbsSignedStorage : ISbsStorage
	{
		// Token: 0x06001BF5 RID: 7157 RVA: 0x0007B6C6 File Offset: 0x00079AC6
		public SbsSignedStorage(ISbsStorage storage, string hashingKey)
		{
			this._storage = storage;
			this.hashingKey = hashingKey;
		}

		// Token: 0x06001BF6 RID: 7158 RVA: 0x0007B6DC File Offset: 0x00079ADC
		public SbsRichData Load(string filename)
		{
			SbsRichData sbsRichData = this._storage.Load(filename);
			if (sbsRichData != null)
			{
				string signature = sbsRichData.MetaData.Signature;
				try
				{
					// eli key point: 签名
					// if (signature == null || !this.IsSignatureValid(signature, sbsRichData.Data))
					// {
						// throw new SbsInvalidSignatureException(filename, sbsRichData.Data, signature);
					// }
				}
				catch (Exception)
				{
					sbsRichData.Data = this.GetOldStrippedContents(sbsRichData.Data);
				}
			}
			return sbsRichData;
		}

		// Token: 0x06001BF7 RID: 7159 RVA: 0x0007B75C File Offset: 0x00079B5C
		public void Save(string filename, string contents, int formatVersion)
		{
			SbsRichData data = new SbsRichData(contents, formatVersion);
			this.Save(filename, data);
		}

		// Token: 0x06001BF8 RID: 7160 RVA: 0x0007B779 File Offset: 0x00079B79
		public void Save(string filename, SbsRichData data)
		{
			data.MetaData.Signature = this.GetSignature(data);
			this._storage.Save(filename, data);
		}

		// Token: 0x06001BF9 RID: 7161 RVA: 0x0007B79A File Offset: 0x00079B9A
		public void Delete(string name)
		{
			this._storage.Delete(name);
		}

		// Token: 0x06001BFA RID: 7162 RVA: 0x0007B7A8 File Offset: 0x00079BA8
		public bool Exists(string name)
		{
			return this._storage.Exists(name);
		}

		// Token: 0x06001BFB RID: 7163 RVA: 0x0007B7B6 File Offset: 0x00079BB6
		protected virtual string GetSignature(SbsRichData data)
		{
			return CryptoUtils.GetMD5String(data.Data + this.hashingKey);
		}

		// Token: 0x06001BFC RID: 7164 RVA: 0x0007B7D0 File Offset: 0x00079BD0
		protected virtual string GetOldStrippedContents(string contents)
		{
			string result;
			try
			{
				JSONNode jsonnode = JSON.Deserialize(contents);
				if (jsonnode == null)
				{
					result = null;
				}
				else
				{
					if (!this.IsOldHashValid(jsonnode))
					{
						string failedSignature = "<missing>";
						if (jsonnode.HasKey("__SIGNATURE__"))
						{
							failedSignature = jsonnode.GetString("__SIGNATURE__", null);
						}
						throw new SbsInvalidSignatureException("<oldstrippedcontent>", contents, failedSignature);
					}
					jsonnode.Remove("__SIGNATURE__");
					result = JSON.Serialize(jsonnode, false, 1, ' ');
				}
			}
			catch (Exception)
			{
				throw new SbsInvalidSignatureException("<oldstrippedcontent>", contents, "<unknown>");
			}
			return result;
		}

		// Token: 0x06001BFD RID: 7165 RVA: 0x0007B86C File Offset: 0x00079C6C
		private bool IsOldHashValid(JSONNode dictionary)
		{
			if (!dictionary.HasKey("__SIGNATURE__"))
			{
				return false;
			}
			string a = (string)dictionary["__SIGNATURE__"];
			string b = this.BuildSignature(dictionary, this.hashingKey);
			return a == b;
		}

		// Token: 0x06001BFE RID: 7166 RVA: 0x0007B8B4 File Offset: 0x00079CB4
		private string BuildSignature(JSONNode dictionary, string password)
		{
			string stringToSign = this.BuildStringToSign(dictionary);
			return SbsSigningHandler.BuildSignature(password, stringToSign);
		}

		// Token: 0x06001BFF RID: 7167 RVA: 0x0007B8D4 File Offset: 0x00079CD4
		private string BuildStringToSign(JSONNode dictionary)
		{
			JSONNode jsonnode = JSON.Clone(dictionary);
			jsonnode.Remove("__SIGNATURE__");
			return JSON.Serialize(jsonnode, false, 1, ' ');
		}

		// Token: 0x06001C00 RID: 7168 RVA: 0x0007B900 File Offset: 0x00079D00
		private bool IsSignatureValid(string signature, string contents)
		{
			string md5String = CryptoUtils.GetMD5String(contents + this.hashingKey);
			return md5String == signature;
		}

		// Token: 0x04004979 RID: 18809
		public string hashingKey;

		// Token: 0x0400497A RID: 18810
		protected ISbsStorage _storage;

		// Token: 0x0400497B RID: 18811
		private const string SIGNATURE = "__SIGNATURE__";
	}
}

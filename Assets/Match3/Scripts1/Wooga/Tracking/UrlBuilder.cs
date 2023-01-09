using System;
using System.Collections.Generic;
using System.Text;

namespace Match3.Scripts1.Wooga.Tracking
{
	// Token: 0x0200046C RID: 1132
	public class UrlBuilder
	{
		// Token: 0x060020DC RID: 8412 RVA: 0x0008A9B6 File Offset: 0x00088DB6
		public UrlBuilder(string host)
		{
			this._hostName = host;
		}

		// Token: 0x060020DD RID: 8413 RVA: 0x0008A9C8 File Offset: 0x00088DC8
		public string BuildUrl(string callName, Dictionary<string, object> data)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(string.Format("https://{0}/{1}/?", this._hostName, callName));
			string text = string.Empty;
			if (data != null)
			{
				foreach (KeyValuePair<string, object> keyValuePair in data)
				{
					stringBuilder.Append(string.Format("{0}{1}={2}", text, keyValuePair.Key, this.EncodeParameterValue(keyValuePair.Value)));
					if (text == string.Empty)
					{
						text = "&";
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060020DE RID: 8414 RVA: 0x0008AA84 File Offset: 0x00088E84
		public string EncodeParameterValue(object value)
		{
			string result;
			if (value is bool)
			{
				result = ((!(bool)value) ? "0" : "1");
			}
			else if (value == null)
			{
				result = "null";
			}
			else
			{
				result = Uri.EscapeDataString(value.ToString());
			}
			return result;
		}

		// Token: 0x04004BA8 RID: 19368
		private readonly string _hostName;
	}
}

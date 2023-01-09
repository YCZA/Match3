using System;
using UnityEngine;

namespace Match3.Scripts1.Wooga.Services.ErrorAnalytics.Internal
{
	// Token: 0x020003F3 RID: 1011
	[Serializable]
	public class ExceptionWithMetaData<TData> : Exception where TData : new()
	{
		// Token: 0x06001E4B RID: 7755 RVA: 0x00080860 File Offset: 0x0007EC60
		public ExceptionWithMetaData(TData data) : base(ExceptionWithMetaData<TData>.FormatMessage(data))
		{
			this.data = data;
		}

		// Token: 0x06001E4C RID: 7756 RVA: 0x00080875 File Offset: 0x0007EC75
		private static string FormatMessage(TData data)
		{
			return string.Format("[[{0}]]{1}", data.GetType().AssemblyQualifiedName, JsonUtility.ToJson(data));
		}

		// Token: 0x04004A27 RID: 18983
		[SerializeField]
		protected TData data;
	}
}

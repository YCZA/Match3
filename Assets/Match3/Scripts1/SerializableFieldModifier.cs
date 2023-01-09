using System;

namespace Match3.Scripts1
{
	// Token: 0x020004FB RID: 1275
	[Serializable]
	public struct SerializableFieldModifier
	{
		// Token: 0x06002316 RID: 8982 RVA: 0x0009B924 File Offset: 0x00099D24
		public SerializableFieldModifier(IFieldModifier modifier)
		{
			this.type = modifier.GetType().FullName;
			this.count = modifier.Count;
		}

		// Token: 0x04004ED1 RID: 20177
		public string type;

		// Token: 0x04004ED2 RID: 20178
		public int count;
	}
}

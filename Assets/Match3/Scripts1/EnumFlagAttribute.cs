using UnityEngine;

// Token: 0x02000B31 RID: 2865
namespace Match3.Scripts1
{
	public class EnumFlagAttribute : PropertyAttribute
	{
		// Token: 0x0600433B RID: 17211 RVA: 0x00157CE1 File Offset: 0x001560E1
		public EnumFlagAttribute()
		{
		}

		// Token: 0x0600433C RID: 17212 RVA: 0x00157CE9 File Offset: 0x001560E9
		public EnumFlagAttribute(string name)
		{
			this.enumName = name;
		}

		// Token: 0x04006BD9 RID: 27609
		public string enumName;
	}
}

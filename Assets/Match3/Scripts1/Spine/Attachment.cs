using System;

namespace Match3.Scripts1.Spine
{
	// Token: 0x02000201 RID: 513
	public abstract class Attachment
	{
		// Token: 0x06000EDF RID: 3807 RVA: 0x00024766 File Offset: 0x00022B66
		public Attachment(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name cannot be null.");
			}
			this.Name = name;
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x06000EE0 RID: 3808 RVA: 0x00024786 File Offset: 0x00022B86
		// (set) Token: 0x06000EE1 RID: 3809 RVA: 0x0002478E File Offset: 0x00022B8E
		public string Name { get; private set; }

		// Token: 0x06000EE2 RID: 3810 RVA: 0x00024797 File Offset: 0x00022B97
		public override string ToString()
		{
			return this.Name;
		}

		// Token: 0x04004076 RID: 16502
		public Attachment.Type type;

		// Token: 0x02000202 RID: 514
		public enum Type
		{
			// Token: 0x04004078 RID: 16504
			unknown,
			// Token: 0x04004079 RID: 16505
			basic,
			// Token: 0x0400407A RID: 16506
			ground
		}
	}
}

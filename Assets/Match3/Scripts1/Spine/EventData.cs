using System;

namespace Match3.Scripts1.Spine
{
	// Token: 0x0200020E RID: 526
	public class EventData
	{
		// Token: 0x06000FDF RID: 4063 RVA: 0x000261C4 File Offset: 0x000245C4
		public EventData(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name cannot be null.");
			}
			this.name = name;
		}

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x06000FE0 RID: 4064 RVA: 0x000261E4 File Offset: 0x000245E4
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x06000FE1 RID: 4065 RVA: 0x000261EC File Offset: 0x000245EC
		// (set) Token: 0x06000FE2 RID: 4066 RVA: 0x000261F4 File Offset: 0x000245F4
		public int Int { get; set; }

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x06000FE3 RID: 4067 RVA: 0x000261FD File Offset: 0x000245FD
		// (set) Token: 0x06000FE4 RID: 4068 RVA: 0x00026205 File Offset: 0x00024605
		public float Float { get; set; }

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x06000FE5 RID: 4069 RVA: 0x0002620E File Offset: 0x0002460E
		// (set) Token: 0x06000FE6 RID: 4070 RVA: 0x00026216 File Offset: 0x00024616
		public string String { get; set; }

		// Token: 0x06000FE7 RID: 4071 RVA: 0x0002621F File Offset: 0x0002461F
		public override string ToString()
		{
			return this.Name;
		}

		// Token: 0x04004105 RID: 16645
		internal string name;
	}
}

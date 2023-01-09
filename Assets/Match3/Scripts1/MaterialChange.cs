namespace Match3.Scripts1
{
	// Token: 0x020007ED RID: 2029
	public struct MaterialChange
	{
		// Token: 0x06003246 RID: 12870 RVA: 0x000ECA57 File Offset: 0x000EAE57
		public MaterialChange(string name, int before, int after)
		{
			this.name = name;
			this.before = before;
			this.after = after;
		}

		// Token: 0x170007E7 RID: 2023
		// (get) Token: 0x06003247 RID: 12871 RVA: 0x000ECA6E File Offset: 0x000EAE6E
		public int Delta
		{
			get
			{
				return this.after - this.before;
			}
		}

		// Token: 0x04005ABA RID: 23226
		public string name;

		// Token: 0x04005ABB RID: 23227
		public int before;

		// Token: 0x04005ABC RID: 23228
		public int after;
	}
}

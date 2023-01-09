namespace Match3.Scripts1
{
	// Token: 0x020007DB RID: 2011
	public struct LocaParam
	{
		// Token: 0x060031A0 RID: 12704 RVA: 0x000E9595 File Offset: 0x000E7995
		public LocaParam(string name, object value)
		{
			this.name = string.Format("[[{0}]]", name);
			this.value = ((value != null) ? value.ToString() : string.Empty);
		}

		// Token: 0x04005A1E RID: 23070
		private const string paramPattern = "[[{0}]]";

		// Token: 0x04005A1F RID: 23071
		public readonly string name;

		// Token: 0x04005A20 RID: 23072
		public readonly string value;
	}
}

namespace Match3.Scripts1.Wooga.Services.ConfigServiceV2
{
	// Token: 0x02000333 RID: 819
	public class PersistedConfigData
	{
		// Token: 0x0600194E RID: 6478 RVA: 0x00072B23 File Offset: 0x00070F23
		public PersistedConfigData(string json, string hash, string abTests = null, string personalizationString = null)
		{
			this.hash = hash;
			this.json = json;
			this.abTests = abTests;
			this.personalizationString = personalizationString;
		}

		// Token: 0x04004806 RID: 18438
		public string hash;

		// Token: 0x04004807 RID: 18439
		public string json;

		// Token: 0x04004808 RID: 18440
		public string abTests;

		// Token: 0x04004809 RID: 18441
		public string personalizationString;
	}
}

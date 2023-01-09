namespace Match3.Scripts1.Wooga.Services.ConfigServiceV2
{
	// Token: 0x0200033E RID: 830
	public interface IReadableConfigSource
	{
		// Token: 0x06001981 RID: 6529
		PersistedConfigData Read();

		// Token: 0x17000404 RID: 1028
		// (get) Token: 0x06001982 RID: 6530
		string Hash { get; }

		// Token: 0x17000405 RID: 1029
		// (get) Token: 0x06001983 RID: 6531
		string AbTests { get; }

		// Token: 0x17000406 RID: 1030
		// (get) Token: 0x06001984 RID: 6532
		string PersonalizationString { get; }
	}
}

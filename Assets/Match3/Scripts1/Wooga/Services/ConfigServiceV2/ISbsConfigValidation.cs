namespace Match3.Scripts1.Wooga.Services.ConfigServiceV2
{
	// Token: 0x02000331 RID: 817
	public interface ISbsConfigValidation
	{
		// Token: 0x0600194B RID: 6475
		bool VerifySBSResponse(string body, string signature, string signatureAlgo, string publicKey);
	}
}

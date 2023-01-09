namespace Match3.Scripts1.Wooga.Services.ConfigServiceV2
{
	// Token: 0x02000332 RID: 818
	public class NullSbsConfigValidation : ISbsConfigValidation
	{
		// Token: 0x0600194D RID: 6477 RVA: 0x00072B20 File Offset: 0x00070F20
		public bool VerifySBSResponse(string body, string signature, string signatureAlgo, string publicKey)
		{
			return true;
		}
	}
}

using System.Security.Cryptography.X509Certificates;

namespace Match3.Scripts1.Wooga.Core.Network.SSL
{
	// Token: 0x02000369 RID: 873
	public class SbsCertificateValidator : ISbsCertificateValidator
	{
		// Token: 0x06001A67 RID: 6759 RVA: 0x00076348 File Offset: 0x00074748
		public bool IsCertificateValid(X509Certificate certificate)
		{
			return true;
		}
	}
}

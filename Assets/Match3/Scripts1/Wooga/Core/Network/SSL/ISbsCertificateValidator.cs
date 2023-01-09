using System.Security.Cryptography.X509Certificates;

namespace Match3.Scripts1.Wooga.Core.Network.SSL
{
	// Token: 0x02000368 RID: 872
	public interface ISbsCertificateValidator
	{
		// Token: 0x06001A65 RID: 6757
		bool IsCertificateValid(X509Certificate certificate);
	}
}

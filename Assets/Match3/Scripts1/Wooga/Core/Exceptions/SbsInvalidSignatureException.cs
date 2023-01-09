using System;

namespace Match3.Scripts1.Wooga.Core.Exceptions
{
	// Token: 0x0200035B RID: 859
	public class SbsInvalidSignatureException : Exception
	{
		// Token: 0x06001A04 RID: 6660 RVA: 0x00074F6C File Offset: 0x0007336C
		public SbsInvalidSignatureException(string fileName, string content, string failedSignature) : base("The expected signature was invalid. " + fileName)
		{
			this.fileName = fileName;
			this.content = content;
			this.failedSignature = failedSignature;
		}

		// Token: 0x0400485A RID: 18522
		public string fileName;

		// Token: 0x0400485B RID: 18523
		public string content;

		// Token: 0x0400485C RID: 18524
		public string failedSignature;
	}
}

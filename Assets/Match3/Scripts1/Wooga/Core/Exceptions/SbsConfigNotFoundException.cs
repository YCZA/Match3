using System;

namespace Match3.Scripts1.Wooga.Core.Exceptions
{
	// Token: 0x02000359 RID: 857
	public class SbsConfigNotFoundException : Exception
	{
		// Token: 0x06001A02 RID: 6658 RVA: 0x00074F50 File Offset: 0x00073350
		public SbsConfigNotFoundException(string configName) : base(string.Format("The configuration {0} could not be loaded. It either does not exist, or the local version has an invalid signature", configName))
		{
		}
	}
}

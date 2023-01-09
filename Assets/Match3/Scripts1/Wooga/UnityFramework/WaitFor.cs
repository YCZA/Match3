using System;

namespace Wooga.UnityFramework
{
	// Token: 0x02000B64 RID: 2916
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public class WaitFor : Attribute
	{
		// Token: 0x06004440 RID: 17472 RVA: 0x0015C202 File Offset: 0x0015A602
		public WaitFor(bool hasToBeInitialized = true, bool hasToBeRegistered = true)
		{
			this.hasToBeInitialized = hasToBeInitialized;
			this.hasToBeRegistered = hasToBeRegistered;
		}

		// Token: 0x04006C70 RID: 27760
		public readonly bool hasToBeInitialized;

		// Token: 0x04006C71 RID: 27761
		public readonly bool hasToBeRegistered;
	}
}

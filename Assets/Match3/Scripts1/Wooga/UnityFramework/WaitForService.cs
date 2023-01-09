using System;

namespace Wooga.UnityFramework
{
	// Token: 0x02000B6D RID: 2925
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public sealed class WaitForService : WaitFor
	{
		// Token: 0x06004465 RID: 17509 RVA: 0x0015C7BC File Offset: 0x0015ABBC
		public WaitForService(bool hasToBeInitialized = true, bool hasToBeRegistered = true) : base(hasToBeInitialized, hasToBeRegistered)
		{
		}
	}
}

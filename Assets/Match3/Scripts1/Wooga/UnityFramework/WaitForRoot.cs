using System;

namespace Wooga.UnityFramework
{
	// Token: 0x02000B65 RID: 2917
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public sealed class WaitForRoot : WaitFor
	{
		// Token: 0x06004441 RID: 17473 RVA: 0x0015C218 File Offset: 0x0015A618
		public WaitForRoot(bool hasToBeInitialized = false, bool destroyWithParent = false) : base(hasToBeInitialized, false)
		{
			this.followLifeCycle = destroyWithParent;
		}

		// Token: 0x04006C72 RID: 27762
		public readonly bool followLifeCycle;
	}
}

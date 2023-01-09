using System.Collections.Generic;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x02000741 RID: 1857
	public abstract class APreloadTrigger : IPreloadTrigger
	{
		// Token: 0x06002DFB RID: 11771 RVA: 0x000D6173 File Offset: 0x000D4573
		public APreloadTrigger()
		{
		}

		// Token: 0x06002DFC RID: 11772 RVA: 0x000D617B File Offset: 0x000D457B
		public APreloadTrigger(IEnumerable<string> bundleNames)
		{
			this.BundleNames = bundleNames;
		}

		// Token: 0x17000720 RID: 1824
		// (get) Token: 0x06002DFD RID: 11773 RVA: 0x000D618A File Offset: 0x000D458A
		// (set) Token: 0x06002DFE RID: 11774 RVA: 0x000D6192 File Offset: 0x000D4592
		public IEnumerable<string> BundleNames { get; protected set; }

		// Token: 0x06002DFF RID: 11775
		public abstract bool ShouldPreload();

		// Token: 0x02000742 RID: 1858
		public enum Type
		{
			// Token: 0x0400577D RID: 22397
			UnlockedLevel
		}

		// Token: 0x02000743 RID: 1859
		public class Config
		{
			// Token: 0x0400577E RID: 22398
			public APreloadTrigger.Type type;

			// Token: 0x0400577F RID: 22399
			public string value;

			// Token: 0x04005780 RID: 22400
			public List<string> bundleNames;
		}
	}
}

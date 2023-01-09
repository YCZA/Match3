using System.Collections.Generic;

namespace Match3.Scripts1.Wooga.Services.ConfigServiceV2
{
	// Token: 0x02000337 RID: 823
	public class AbTestConfig
	{
		// Token: 0x04004813 RID: 18451
		public List<AbTestConfig.AbTest> ab_tests;

		// Token: 0x02000338 RID: 824
		public class AbTestGroup
		{
			// Token: 0x04004814 RID: 18452
			public string name;

			// Token: 0x04004815 RID: 18453
			public string key;
		}

		// Token: 0x02000339 RID: 825
		public class AbTest
		{
			// Token: 0x04004816 RID: 18454
			public string name;

			// Token: 0x04004817 RID: 18455
			public int position;

			// Token: 0x04004818 RID: 18456
			public List<AbTestConfig.AbTestGroup> groups;
		}
	}
}

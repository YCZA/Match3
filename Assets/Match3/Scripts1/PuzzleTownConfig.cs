using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

// Token: 0x0200049B RID: 1179
namespace Match3.Scripts1
{
	public static class PuzzleTownConfig
	{
		// Token: 0x0600216A RID: 8554 RVA: 0x0008C5D2 File Offset: 0x0008A9D2
		public static string ToAssetsPath(string fileName)
		{
			return Path.Combine(PuzzleTownConfig.JsonConfigAssetsPath, fileName).Replace('\\', '/');
		}

		// Token: 0x17000528 RID: 1320
		// (get) Token: 0x0600216B RID: 8555 RVA: 0x0008C5E8 File Offset: 0x0008A9E8
		public static string JsonConfigAssetsPath
		{
			get
			{
				string[] array = new string[]
				{
					"Assets",
					"Puzzletown",
					"Config",
					"Json"
				};
				IEnumerable<string> source = array;
				if (PuzzleTownConfig._003C_003Ef__mg_0024cache0 == null)
				{
					PuzzleTownConfig._003C_003Ef__mg_0024cache0 = new Func<string, string, string>(Path.Combine);
				}
				return source.Aggregate(PuzzleTownConfig._003C_003Ef__mg_0024cache0);
			}
		}

		// Token: 0x17000529 RID: 1321
		// (get) Token: 0x0600216C RID: 8556 RVA: 0x0008C640 File Offset: 0x0008AA40
		public static string JsonConfigPath
		{
			get
			{
				string[] array = new string[]
				{
					Directory.GetCurrentDirectory(),
					PuzzleTownConfig.JsonConfigAssetsPath
				};
				IEnumerable<string> source = array;
				if (PuzzleTownConfig._003C_003Ef__mg_0024cache1 == null)
				{
					PuzzleTownConfig._003C_003Ef__mg_0024cache1 = new Func<string, string, string>(Path.Combine);
				}
				return source.Aggregate(PuzzleTownConfig._003C_003Ef__mg_0024cache1);
			}
		}

		// Token: 0x04004C80 RID: 19584
		[CompilerGenerated]
		private static Func<string, string, string> _003C_003Ef__mg_0024cache0;

		// Token: 0x04004C81 RID: 19585
		[CompilerGenerated]
		private static Func<string, string, string> _003C_003Ef__mg_0024cache1;
	}
}

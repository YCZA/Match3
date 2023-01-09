using System.IO;
using UnityEngine;

namespace Match3.Scripts1.Wooga.Core.Tests
{
	// Token: 0x0200039B RID: 923
	public static class TestTools
	{
		// Token: 0x06001C01 RID: 7169 RVA: 0x0007B928 File Offset: 0x00079D28
		public static string GetTempPath()
		{
			return Application.temporaryCachePath;
		}

		// Token: 0x06001C02 RID: 7170 RVA: 0x0007B93C File Offset: 0x00079D3C
		public static string CreateNewTemporaryDirectory()
		{
			string text = Path.Combine(TestTools.GetTempPath(), Path.GetRandomFileName());
			Directory.CreateDirectory(text);
			return text;
		}
	}
}

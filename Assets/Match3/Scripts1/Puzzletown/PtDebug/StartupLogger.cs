using System;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.PtDebug
{
	// Token: 0x020004AB RID: 1195
	public static class StartupLogger
	{
		// Token: 0x060021A3 RID: 8611 RVA: 0x0008CF18 File Offset: 0x0008B318
		public static void LogInitialized(object obj)
		{
			if (obj == null)
			{
				return;
			}
			Type type = obj.GetType();
			EAHelper.AddBreadcrumb("Initialized: " + type.Name);
			WoogaDebug.Log(new object[]
			{
				"Initialized",
				type.Name,
				Time.realtimeSinceStartup
			});
		}
	}
}

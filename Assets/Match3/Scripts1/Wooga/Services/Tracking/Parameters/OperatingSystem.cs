using UnityEngine;

namespace Match3.Scripts1.Wooga.Services.Tracking.Parameters
{
	// Token: 0x0200044A RID: 1098
	public static class OperatingSystem
	{
		// Token: 0x170004E4 RID: 1252
		// (get) Token: 0x06001FDF RID: 8159 RVA: 0x00085EF0 File Offset: 0x000842F0
		public static string version
		{
			get
			{
				string operatingSystem = SystemInfo.operatingSystem;
				string[] array = operatingSystem.Split(new char[]
				{
					' ',
					'_'
				});
				string result = operatingSystem;
				foreach (string text in array)
				{
					if (text.Length > 0)
					{
						bool flag = true;
						foreach (char c in text)
						{
							if (!char.IsDigit(c) && !char.IsPunctuation(c))
							{
								flag = false;
								break;
							}
						}
						if (flag)
						{
							result = text;
							break;
						}
					}
				}
				return result;
			}
		}
	}
}

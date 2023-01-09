using UnityEngine;

namespace Match3.Scripts1.Shared.Build
{
	// Token: 0x02000AAE RID: 2734
	[CreateAssetMenu(fileName = "DeviceInfo", menuName = "Puzzletown/DeviceInfo")]
	public class DeviceInfo : ScriptableObject
	{
		// Token: 0x060040EB RID: 16619 RVA: 0x001517A8 File Offset: 0x0014FBA8
		public static DeviceInfo Load()
		{
			return Resources.Load<DeviceInfo>("Config/DeviceInfo");
		}

		// Token: 0x060040EC RID: 16620 RVA: 0x001517C4 File Offset: 0x0014FBC4
		public static void ChangeResolution(DeviceInfo.Resolution res)
		{
			DeviceInfo deviceInfo = DeviceInfo.Load();
			deviceInfo.resolution = res;
			deviceInfo.Save();
		}

		// Token: 0x060040ED RID: 16621 RVA: 0x001517E4 File Offset: 0x0014FBE4
		public void Save()
		{
		}

		// Token: 0x04006ABD RID: 27325
		private const string pathInResources = "Config/DeviceInfo";

		// Token: 0x04006ABE RID: 27326
		public DeviceInfo.Resolution resolution;

		// Token: 0x02000AAF RID: 2735
		public enum Resolution
		{
			// Token: 0x04006AC0 RID: 27328
			none,
			// Token: 0x04006AC1 RID: 27329
			hd,
			// Token: 0x04006AC2 RID: 27330
			sd
		}
	}
}

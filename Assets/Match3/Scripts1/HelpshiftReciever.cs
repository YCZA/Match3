using Match3.Scripts1.Puzzletown.Services;
using UnityEngine;

// Token: 0x020007C2 RID: 1986
namespace Match3.Scripts1
{
	public class HelpshiftReciever : MonoBehaviour
	{
		// Token: 0x060030E4 RID: 12516 RVA: 0x000E5743 File Offset: 0x000E3B43
		public void Init(HelpshiftService helpshiftService)
		{
			this.helpshiftService = helpshiftService;
		}

		// Token: 0x060030E5 RID: 12517 RVA: 0x000E574C File Offset: 0x000E3B4C
		public void didReceiveNotificationCount(string count)
		{
			global::UnityEngine.Debug.Log("Notification async count : " + count);
			int count2 = 0;
			if (int.TryParse(count, out count2))
			{
				this.helpshiftService.receivedNotificationCount(count2);
			}
		}

		// Token: 0x060030E6 RID: 12518 RVA: 0x000E5784 File Offset: 0x000E3B84
		public void updateMetaData()
		{
			this.helpshiftService.UpdateMetaData();
		}

		// Token: 0x04005998 RID: 22936
		private HelpshiftService helpshiftService;
	}
}

using Match3.Scripts1.Puzzletown.Services;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020009B4 RID: 2484
namespace Match3.Scripts1
{
	[RequireComponent(typeof(Text))]
	public class CheatProgressionDisplay : MonoBehaviour
	{
		// Token: 0x06003C45 RID: 15429 RVA: 0x0012C6DB File Offset: 0x0012AADB
		public void Init(ProgressionDataService.Service progressionService, ContentUnlockService contentUnlockService, QuestService questService)
		{
			this.progressionService = progressionService;
			this.contentUnlockService = contentUnlockService;
			this.questService = questService;
		}

		// Token: 0x06003C46 RID: 15430 RVA: 0x0012C6F2 File Offset: 0x0012AAF2
		private void OnEnable()
		{
			this.Refresh();
		}

		// Token: 0x06003C47 RID: 15431 RVA: 0x0012C6FA File Offset: 0x0012AAFA
		public void OverrideText(string txt)
		{
			base.GetComponent<Text>().text = txt;
		}

		// Token: 0x06003C48 RID: 15432 RVA: 0x0012C708 File Offset: 0x0012AB08
		public void Refresh()
		{
			if (this.progressionService != null && this.contentUnlockService != null)
			{
				base.GetComponent<Text>().text = string.Format("Area: {0}  Level: {1} Content Unlock Enabled: {2}", this.questService.UnlockedAreaWithQuestAndEndOfContent, this.progressionService.UnlockedLevel, !this.contentUnlockService.IsUnlockDisabled);
			}
		}

		// Token: 0x04006460 RID: 25696
		private ProgressionDataService.Service progressionService;

		// Token: 0x04006461 RID: 25697
		private ContentUnlockService contentUnlockService;

		// Token: 0x04006462 RID: 25698
		private QuestService questService;
	}
}

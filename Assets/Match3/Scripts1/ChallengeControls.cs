using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000904 RID: 2308
namespace Match3.Scripts1
{
	public class ChallengeControls : MonoBehaviour
	{
		// Token: 0x06003852 RID: 14418 RVA: 0x001134CC File Offset: 0x001118CC
		public void Refresh(bool isChallengeActive)
		{
			if (this.buttonPlay.IsActive() != isChallengeActive)
			{
				if (this.buttonPlay != null)
				{
					this.buttonPlay.gameObject.SetActive(isChallengeActive);
				}
				if (this.buttonCancel != null)
				{
					this.buttonCancel.gameObject.SetActive(!isChallengeActive);
				}
				if (this.buttonAddTime != null)
				{
					this.buttonAddTime.gameObject.SetActive(!isChallengeActive);
				}
			}
		}

		// Token: 0x0400605B RID: 24667
		[SerializeField]
		private Button buttonPlay;

		// Token: 0x0400605C RID: 24668
		[SerializeField]
		private Button buttonAddTime;

		// Token: 0x0400605D RID: 24669
		[SerializeField]
		private Button buttonCancel;
	}
}

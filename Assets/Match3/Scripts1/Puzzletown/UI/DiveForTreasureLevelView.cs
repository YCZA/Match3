using Match3.Scripts1.Puzzletown.Flows;
using Match3.Scripts1.Puzzletown.Match3;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x020004AE RID: 1198
	public class DiveForTreasureLevelView : MonoBehaviour
	{
		// Token: 0x060021B0 RID: 8624 RVA: 0x0008D314 File Offset: 0x0008B714
		public void SetupView(int level, bool hasRewardItem, bool timerExpired)
		{
			this.level = level;
			this.timerExpired = timerExpired;
			this.levelLabel.text = level.ToString();
			if (this.rewardItemView != null)
			{
				this.rewardItemView.SetActive(hasRewardItem);
			}
			if (this.animation != null)
			{
				this.animation["ItemFloatingLoop"].time = global::UnityEngine.Random.Range(0f, this.animation["ItemFloatingLoop"].length);
			}
			if (this.button != null)
			{
				this.button.onClick.AddListener(new UnityAction(this.HandlePlayButton));
			}
		}

		// Token: 0x060021B1 RID: 8625 RVA: 0x0008D3D6 File Offset: 0x0008B7D6
		public void HandlePlayButton()
		{
			if (!this.timerExpired)
			{
				new CoreGameFlow().Start(new CoreGameFlow.Input(this.level, false, null, LevelPlayMode.DiveForTreasure));
			}
		}

		// Token: 0x04004CDB RID: 19675
		private const string ANIMATION_NAME = "ItemFloatingLoop";

		// Token: 0x04004CDC RID: 19676
		[SerializeField]
		private TextMeshProUGUI levelLabel;

		// Token: 0x04004CDD RID: 19677
		[SerializeField]
		private GameObject rewardItemView;

		// Token: 0x04004CDE RID: 19678
		[SerializeField]
		private Animation animation;

		// Token: 0x04004CDF RID: 19679
		[SerializeField]
		private Button button;

		// Token: 0x04004CE0 RID: 19680
		private int level;

		// Token: 0x04004CE1 RID: 19681
		private bool timerExpired;
	}
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x020008B3 RID: 2227
	public class ProgressBarUI : MonoBehaviour
	{
		// Token: 0x06003652 RID: 13906 RVA: 0x001072F9 File Offset: 0x001056F9
		private void OnValidate()
		{
		}

		// Token: 0x06003653 RID: 13907 RVA: 0x001072FC File Offset: 0x001056FC
		public void SetProgress(float current, float target)
		{
			if (this.label != null)
			{
				this.SetLabel((int)current, (int)target, true);
			}
			float num = (target <= 0f) ? 1f : (current / target);
			this.progressBar.fillAmount = num;
			if (this.completionMarker != null)
			{
				this.completionMarker.gameObject.SetActive(num >= 1f);
			}
		}

		// Token: 0x06003654 RID: 13908 RVA: 0x00107376 File Offset: 0x00105776
		protected void SetLabel(int current, int target, bool clampCurrent = true)
		{
			if (clampCurrent && current > target)
			{
				current = target;
			}
			this.label.text = string.Format("{0}/{1}", current, target);
		}

		// Token: 0x04005E5D RID: 24157
		public TextMeshProUGUI label;

		// Token: 0x04005E5E RID: 24158
		public Image progressBar;

		// Token: 0x04005E5F RID: 24159
		public GameObject completionMarker;
	}
}

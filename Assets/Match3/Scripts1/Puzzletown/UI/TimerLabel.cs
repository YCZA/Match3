using System;
using TMPro;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x020007D5 RID: 2005
	[Obsolete("Use CountdownTimer with TimeSpanView instead.")]
	public class TimerLabel : MonoBehaviour
	{
		// Token: 0x06003176 RID: 12662 RVA: 0x000E89F6 File Offset: 0x000E6DF6
		private void OnValidate()
		{
			if (this.splitLabel)
			{
			}
		}

		// Token: 0x06003177 RID: 12663 RVA: 0x000E8A08 File Offset: 0x000E6E08
		public void Refresh(int remainingSeconds, bool show = true)
		{
			if (this.disableWhenNotShown != null)
			{
				this.disableWhenNotShown.SetActive(show);
			}
			int num = remainingSeconds / 3600;
			remainingSeconds -= num * 3600;
			int num2 = remainingSeconds / 60;
			remainingSeconds -= num2 * 60;
			if (!this.splitLabel)
			{
				this.label.text = string.Format("{0:D2}:{1:D2}:{2:D2}", num, num2, remainingSeconds);
			}
			else
			{
				this.hoursLabel.text = string.Format("{0:D2}", num);
				this.minutesLabel.text = string.Format("{0:D2}", num2);
				this.secondsLabel.text = string.Format("{0:D2}", remainingSeconds);
			}
		}

		// Token: 0x04005A08 RID: 23048
		public bool splitLabel;

		// Token: 0x04005A09 RID: 23049
		public TextMeshProUGUI label;

		// Token: 0x04005A0A RID: 23050
		public TextMeshProUGUI hoursLabel;

		// Token: 0x04005A0B RID: 23051
		public TextMeshProUGUI minutesLabel;

		// Token: 0x04005A0C RID: 23052
		public TextMeshProUGUI secondsLabel;

		// Token: 0x04005A0D RID: 23053
		public GameObject disableWhenNotShown;
	}
}

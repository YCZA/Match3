using System;
using System.Collections;
using System.Text;
using Wooga.UnityFramework;
using TMPro;
using UnityEngine;

// Token: 0x02000A25 RID: 2597
namespace Match3.Scripts1
{
	public class TimeSpanView : MonoBehaviour
	{
		// Token: 0x06003E54 RID: 15956 RVA: 0x0013C118 File Offset: 0x0013A518
		private IEnumerator Start()
		{
			yield return ServiceLocator.Instance.Inject(this);
			yield break;
		}

		// Token: 0x06003E55 RID: 15957 RVA: 0x0013C134 File Offset: 0x0013A534
		public IEnumerator SetTimeSpan(TimeSpan timeSpan, bool isLowTime = false)
		{
			Color labelColor = (!isLowTime) ? this.normalColor : this.lowTimeColor;
			if (this.singleLabel != null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				bool showReducedTimer = this.enableReducedView && this.MoreThanOneDayLeft(timeSpan);
				if (showReducedTimer)
				{
					while (this.localizationService == null)
					{
						yield return null;
					}
					stringBuilder.Append(this.GetDays(timeSpan));
					stringBuilder.Append(this.localizationService.GetText("ui.timer.days", new LocaParam[0]));
					stringBuilder.Append(" ");
					stringBuilder.Append(this.GetHours(timeSpan));
					stringBuilder.Append(this.localizationService.GetText("ui.timer.hours", new LocaParam[0]));
				}
				else
				{
					if (this.daysField)
					{
						stringBuilder.Append(this.GetDays(timeSpan));
						stringBuilder.Append(":");
					}
					if (this.hoursField)
					{
						stringBuilder.Append(this.GetHours(timeSpan));
						stringBuilder.Append(":");
					}
					if (this.minutesField)
					{
						stringBuilder.Append(this.GetMinutes(timeSpan));
						stringBuilder.Append(":");
					}
					if (this.secondsField)
					{
						stringBuilder.Append(this.GetSeconds(timeSpan));
						stringBuilder.Append(":");
					}
					if (stringBuilder[stringBuilder.Length - 1] == ':')
					{
						stringBuilder.Remove(stringBuilder.Length - 1, 1);
					}
				}
				this.singleLabel.text = stringBuilder.ToString();
				this.singleLabel.color = labelColor;
			}
			else
			{
				if (this.daysLabel != null)
				{
					this.daysLabel.text = this.GetDays(timeSpan);
					this.daysLabel.color = labelColor;
				}
				if (this.hoursLabel != null)
				{
					this.hoursLabel.text = this.GetHours(timeSpan);
					this.hoursLabel.color = labelColor;
				}
				if (this.minutesLabel != null)
				{
					this.minutesLabel.text = this.GetMinutes(timeSpan);
					this.minutesLabel.color = labelColor;
				}
				if (this.secondsLabel != null)
				{
					this.secondsLabel.text = this.GetSeconds(timeSpan);
					this.secondsLabel.color = labelColor;
				}
			}
			yield break;
		}

		// Token: 0x06003E56 RID: 15958 RVA: 0x0013C15D File Offset: 0x0013A55D
		private string GetDays(TimeSpan timeSpan)
		{
			return this.FormatTime(Math.Max(0, (int)timeSpan.TotalDays));
		}

		// Token: 0x06003E57 RID: 15959 RVA: 0x0013C174 File Offset: 0x0013A574
		private string GetHours(TimeSpan timeSpan)
		{
			if (this.enableReducedView && this.MoreThanOneDayLeft(timeSpan))
			{
				return this.FormatTime(Math.Max(0, timeSpan.Hours));
			}
			return this.FormatTime(Math.Max(0, this.daysField ? timeSpan.Hours : ((int)timeSpan.TotalHours)));
		}

		// Token: 0x06003E58 RID: 15960 RVA: 0x0013C1D7 File Offset: 0x0013A5D7
		private string GetMinutes(TimeSpan timeSpan)
		{
			return this.FormatTime(Math.Max(0, this.hoursField ? timeSpan.Minutes : ((int)timeSpan.TotalMinutes)));
		}

		// Token: 0x06003E59 RID: 15961 RVA: 0x0013C204 File Offset: 0x0013A604
		private string GetSeconds(TimeSpan timeSpan)
		{
			return this.FormatTime(Math.Max(0, this.minutesField ? timeSpan.Seconds : ((int)timeSpan.TotalSeconds)));
		}

		// Token: 0x06003E5A RID: 15962 RVA: 0x0013C231 File Offset: 0x0013A631
		private string FormatTime(int timeDelta)
		{
			return timeDelta.ToString(string.Format("D{0}", this.numberOfDigits));
		}

		// Token: 0x06003E5B RID: 15963 RVA: 0x0013C24F File Offset: 0x0013A64F
		private bool MoreThanOneDayLeft(TimeSpan timeSpan)
		{
			return timeSpan.TotalDays >= 1.0;
		}

		// Token: 0x04006761 RID: 26465
		[WaitForService(true, true)]
		private ILocalizationService localizationService;

		// Token: 0x04006762 RID: 26466
		[SerializeField]
		public bool singleField;

		// Token: 0x04006763 RID: 26467
		[SerializeField]
		public bool multipleFields = true;

		// Token: 0x04006764 RID: 26468
		[SerializeField]
		public bool daysField;

		// Token: 0x04006765 RID: 26469
		[SerializeField]
		public bool hoursField = true;

		// Token: 0x04006766 RID: 26470
		[SerializeField]
		public bool minutesField = true;

		// Token: 0x04006767 RID: 26471
		[SerializeField]
		public bool secondsField = true;

		// Token: 0x04006768 RID: 26472
		[SerializeField]
		public bool enableReducedView;

		// Token: 0x04006769 RID: 26473
		[SerializeField]
		public TextMeshProUGUI singleLabel;

		// Token: 0x0400676A RID: 26474
		[SerializeField]
		public TextMeshProUGUI daysLabel;

		// Token: 0x0400676B RID: 26475
		[SerializeField]
		public TextMeshProUGUI hoursLabel;

		// Token: 0x0400676C RID: 26476
		[SerializeField]
		public TextMeshProUGUI minutesLabel;

		// Token: 0x0400676D RID: 26477
		[SerializeField]
		public TextMeshProUGUI secondsLabel;

		// Token: 0x0400676E RID: 26478
		[SerializeField]
		public int numberOfDigits = 2;

		// Token: 0x0400676F RID: 26479
		[SerializeField]
		public Color normalColor = Color.white;

		// Token: 0x04006770 RID: 26480
		[SerializeField]
		public Color lowTimeColor = Color.white;
	}
}

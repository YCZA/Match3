using System;
using Match3.Scripts1.Wooga.UI;

// Token: 0x0200002E RID: 46
namespace Match3.Scripts1
{
	public class CountDownTimerDataView : CountdownTimer, IDataView<DateTime>
	{
		// Token: 0x06000175 RID: 373 RVA: 0x00008DBE File Offset: 0x000071BE
		public void Show(DateTime date)
		{
			base.SetTargetTime(date, date.Kind == DateTimeKind.Utc, null);
		}
	}
}

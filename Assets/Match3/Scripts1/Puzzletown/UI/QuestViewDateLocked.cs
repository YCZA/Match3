namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x02000A0D RID: 2573
	public class QuestViewDateLocked : QuestView
	{
		// Token: 0x17000923 RID: 2339
		// (get) Token: 0x06003DEE RID: 15854 RVA: 0x00139E9C File Offset: 0x0013829C
		public override QuestView.CellType questStatus
		{
			get
			{
				return QuestView.CellType.dateLocked;
			}
		}

		// Token: 0x06003DEF RID: 15855 RVA: 0x00139E9F File Offset: 0x0013829F
		public override void Show(QuestUIData data)
		{
			base.Show(data);
			if (data.unlockDate != null)
			{
				this.countdownTimer.SetTargetTime(data.unlockDate.Value, false, null);
			}
		}

		// Token: 0x040066D5 RID: 26325
		public CountdownTimer countdownTimer;
	}
}

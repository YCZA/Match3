namespace Match3.Scripts1.Helpshift
{
	// Token: 0x020001D8 RID: 472
	public interface HelpshiftInboxMessage
	{
		// Token: 0x06000DE1 RID: 3553
		string GetIdentifier();

		// Token: 0x06000DE2 RID: 3554
		string GetCoverImageFilePath();

		// Token: 0x06000DE3 RID: 3555
		string GetIconImageFilePath();

		// Token: 0x06000DE4 RID: 3556
		string GetTitle();

		// Token: 0x06000DE5 RID: 3557
		string GetTitleColor();

		// Token: 0x06000DE6 RID: 3558
		string GetBody();

		// Token: 0x06000DE7 RID: 3559
		string GetBodyColor();

		// Token: 0x06000DE8 RID: 3560
		string GetBackgroundColor();

		// Token: 0x06000DE9 RID: 3561
		double GetCreatedAt();

		// Token: 0x06000DEA RID: 3562
		double GetExpiryTimeStamp();

		// Token: 0x06000DEB RID: 3563
		bool HasExpiryTimestamp();

		// Token: 0x06000DEC RID: 3564
		bool GetReadStatus();

		// Token: 0x06000DED RID: 3565
		bool GetSeenStatus();

		// Token: 0x06000DEE RID: 3566
		int GetCountOfActions();

		// Token: 0x06000DEF RID: 3567
		string GetActionTitle(int index);

		// Token: 0x06000DF0 RID: 3568
		string GetActionTitleColor(int index);

		// Token: 0x06000DF1 RID: 3569
		bool IsActionGoalCompletion(int index);

		// Token: 0x06000DF2 RID: 3570
		void ExecuteAction(int index, object activity);

		// Token: 0x06000DF3 RID: 3571
		HelpshiftInboxMessageActionType GetActionType(int index);

		// Token: 0x06000DF4 RID: 3572
		string GetActionData(int index);
	}
}

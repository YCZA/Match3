namespace Match3.Scripts1.UnityEngine.UI.Extensions.Tweens
{
	// Token: 0x02000B83 RID: 2947
	internal interface ITweenValue
	{
		// Token: 0x060044EE RID: 17646
		void TweenValue(float floatPercentage);

		// Token: 0x170009E3 RID: 2531
		// (get) Token: 0x060044EF RID: 17647
		bool ignoreTimeScale { get; }

		// Token: 0x170009E4 RID: 2532
		// (get) Token: 0x060044F0 RID: 17648
		float duration { get; }

		// Token: 0x060044F1 RID: 17649
		bool ValidTarget();

		// Token: 0x060044F2 RID: 17650
		void Finished();
	}
}

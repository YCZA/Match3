namespace Match3.Scripts1.Spine.Unity
{
	// Token: 0x0200022C RID: 556
	public interface ISkeletonAnimation
	{
		// Token: 0x14000009 RID: 9
		// (add) Token: 0x06001186 RID: 4486
		// (remove) Token: 0x06001187 RID: 4487
		event UpdateBonesDelegate UpdateLocal;

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x06001188 RID: 4488
		// (remove) Token: 0x06001189 RID: 4489
		event UpdateBonesDelegate UpdateWorld;

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x0600118A RID: 4490
		// (remove) Token: 0x0600118B RID: 4491
		event UpdateBonesDelegate UpdateComplete;

		// Token: 0x0600118C RID: 4492
		void LateUpdate();

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x0600118D RID: 4493
		Skeleton Skeleton { get; }
	}
}

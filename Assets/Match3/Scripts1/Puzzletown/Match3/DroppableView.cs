using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020006A6 RID: 1702
	public class DroppableView : AAnimatedGemOverlayView, ITintableView
	{
		// Token: 0x06002A70 RID: 10864 RVA: 0x000C244D File Offset: 0x000C084D
		protected override int GetRevealBlinkingTimeRangeInMs()
		{
			return 10000;
		}

		// Token: 0x06002A71 RID: 10865 RVA: 0x000C2454 File Offset: 0x000C0854
		protected override void Awake()
		{
			base.Awake();
			this.meshRenderer = base.GetComponent<MeshRenderer>();
		}

		// Token: 0x06002A72 RID: 10866 RVA: 0x000C2468 File Offset: 0x000C0868
		private void Start()
		{
			this.materialPropertyBlock = new MaterialPropertyBlock();
		}

		// Token: 0x06002A73 RID: 10867 RVA: 0x000C2478 File Offset: 0x000C0878
		public void ApplyTintColor(Color tint)
		{
			if (this.meshRenderer != null && this.materialPropertyBlock != null)
			{
				this.meshRenderer.GetPropertyBlock(this.materialPropertyBlock);
				this.materialPropertyBlock.SetColor("_Tint", tint);
				this.meshRenderer.SetPropertyBlock(this.materialPropertyBlock);
			}
		}

		// Token: 0x040053C4 RID: 21444
		private MeshRenderer meshRenderer;

		// Token: 0x040053C5 RID: 21445
		private MaterialPropertyBlock materialPropertyBlock;

		// Token: 0x040053C6 RID: 21446
		private const int REVEAL_BLINKING_TIME_RANGE_MS = 10000;
	}
}

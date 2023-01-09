using Shared.Pooling;
using UnityEngine;
using UnityEngine.Rendering;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000696 RID: 1686
	public class CannonView : AReleasable, IGemModifierView, IUpdatableGemModifierView, ITintableView
	{
		// Token: 0x170006BD RID: 1725
		// (get) Token: 0x06002A13 RID: 10771 RVA: 0x000C0B31 File Offset: 0x000BEF31
		private bool IsFullyCharged
		{
			get
			{
				return this.parameterShown >= 45;
			}
		}

		// Token: 0x06002A14 RID: 10772 RVA: 0x000C0B40 File Offset: 0x000BEF40
		public void ApplyTintColor(Color tint)
		{
			Color color = this.originalColor * tint;
			this.cannonSprite.color = tint;
			this.colorBackgroundSprite.color = color;
			this.colorizer.SetColor(color);
		}

		// Token: 0x06002A15 RID: 10773 RVA: 0x000C0B80 File Offset: 0x000BEF80
		public void ShowModifier(Gem gem)
		{
			this.cannonSprite.gameObject.SetActive(true);
			this.originalColor = this.colorsMap.GetColorFromGem(gem);
			this.colorizer.InitializeWithColor(this.originalColor);
			this.colorBackgroundSprite.color = this.originalColor;
			this.parameterShown = gem.parameter;
			this.UpdateChargeView(this.GetProgressRateFromZeroToOne());
		}

		// Token: 0x06002A16 RID: 10774 RVA: 0x000C0BEB File Offset: 0x000BEFEB
		public void UpdateWithDelta(int delta)
		{
			this.cannonSprite.gameObject.SetActive(true);
			this.parameterShown += delta;
			this.UpdateChargeView(this.GetProgressRateFromZeroToOne());
			this.PlayChargeEffect();
		}

		// Token: 0x06002A17 RID: 10775 RVA: 0x000C0C1E File Offset: 0x000BF01E
		public int ParameterShown()
		{
			return this.parameterShown;
		}

		// Token: 0x06002A18 RID: 10776 RVA: 0x000C0C26 File Offset: 0x000BF026
		private void UpdateChargeView(float progressRate)
		{
			// Debug.LogError("update charge view:" + progressRate);
			this.chargeCover.transform.localPosition = this.GetChargeCoverPosition(progressRate);
			if (this.IsFullyCharged)
			{
				this.colorizer.StartGlowing();
			}
		}

		// Token: 0x06002A19 RID: 10777 RVA: 0x000C0C58 File Offset: 0x000BF058
		private Vector3 GetChargeCoverPosition(float progressRate)
		{
			Vector3 zero = Vector3.zero;
			float y = this.minYPos + this.chargeRange * progressRate;
			zero.y = y;
			return zero;
		}

		// Token: 0x06002A1A RID: 10778 RVA: 0x000C0C84 File Offset: 0x000BF084
		private float GetProgressRateFromZeroToOne()
		{
			return (float)this.parameterShown / 45f;
		}

		// Token: 0x06002A1B RID: 10779 RVA: 0x000C0C93 File Offset: 0x000BF093
		public override void Release(float delay = 0f)
		{
			this.parameterShown = CannonView.NOT_INITIALIZED;
			base.Release(delay);
		}

		// Token: 0x06002A1C RID: 10780 RVA: 0x000C0CA7 File Offset: 0x000BF0A7
		public void PlayPreExplosionEffect()
		{
			this.sorting.sortingLayerName = "VFX";
			this.animator.Play("PreFire", -1);
		}

		// Token: 0x06002A1D RID: 10781 RVA: 0x000C0CCA File Offset: 0x000BF0CA
		private void PlayChargeEffect()
		{
			this.animator.Play("PartialCharge", -1);
		}

		// Token: 0x0400537D RID: 21373
		public SpriteRenderer cannonSprite;

		// Token: 0x0400537E RID: 21374
		public SpriteRenderer colorBackgroundSprite;

		// Token: 0x0400537F RID: 21375
		public SpriteRenderer chargeCover;

		// Token: 0x04005380 RID: 21376
		public CannonChargeColorizer colorizer;

		// Token: 0x04005381 RID: 21377
		public GemColorToColor colorsMap;

		// Token: 0x04005382 RID: 21378
		public static int NOT_INITIALIZED = -1;

		// Token: 0x04005383 RID: 21379
		[SerializeField]
		private Animator animator;

		// Token: 0x04005384 RID: 21380
		[SerializeField]
		private SortingGroup sorting;

		// Token: 0x04005385 RID: 21381
		private Color originalColor;

		// Token: 0x04005386 RID: 21382
		private int parameterShown = CannonView.NOT_INITIALIZED;

		// Token: 0x04005387 RID: 21383
		private float minYPos = -0.237f;

		// Token: 0x04005388 RID: 21384
		private float chargeRange = 0.427f;
	}
}

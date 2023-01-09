using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020006B8 RID: 1720
	public class IceView : MonoBehaviour, IGemModifierView, ITintableView
	{
		// Token: 0x06002AF0 RID: 10992 RVA: 0x000C450F File Offset: 0x000C290F
		public void ApplyTintColor(Color tint)
		{
			this.coverSprite.color = tint;
			this.backgroundSprite.color = tint;
		}

		// Token: 0x06002AF1 RID: 10993 RVA: 0x000C452C File Offset: 0x000C292C
		public void ShowModifier(Gem gem)
		{
			bool isIced = gem.IsIced;
			this.coverSprite.gameObject.SetActive(isIced);
			this.backgroundSprite.gameObject.SetActive(isIced);
			if (isIced)
			{
				this.coverSprite.sprite = this.coverIceSpriteManager.GetSimilar(gem.modifier.ToString());
			}
			else
			{
				base.transform.localScale = Vector3.one;
				this.coverSprite.transform.localScale = Vector3.one;
				this.backgroundSprite.transform.localScale = Vector3.one;
			}
		}

		// Token: 0x04005438 RID: 21560
		public SpriteRenderer coverSprite;

		// Token: 0x04005439 RID: 21561
		public SpriteRenderer backgroundSprite;

		// Token: 0x0400543A RID: 21562
		public SpriteManager coverIceSpriteManager;
	}
}

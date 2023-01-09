using Match3.Scripts1.Wooga.UI;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020006EF RID: 1775
	public class M3_LevelSelectionTierView : ATableViewReusableCell<M3_LevelSelectionTier.State>, IDataView<M3_LevelSelectionTier>
	{
		// Token: 0x06002C19 RID: 11289 RVA: 0x000CB23C File Offset: 0x000C963C
		public void Show(M3_LevelSelectionTier data)
		{
			if (this.image)
			{
				this.image.sprite = this.tierSprites.GetSimilar(M3_LevelSelectionTierView.s_tierNames[(int)data.tier]);
			}
		}

		// Token: 0x04005548 RID: 21832
		public Image image;

		// Token: 0x04005549 RID: 21833
		public SpriteManager tierSprites;

		// Token: 0x0400554A RID: 21834
		private static readonly string[] s_tierNames = new string[]
		{
			"_x1",
			"_x2",
			"_x3"
		};
	}
}

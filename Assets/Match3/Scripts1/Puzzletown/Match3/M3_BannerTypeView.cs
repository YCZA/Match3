using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Puzzletown.Tutorials;
using Match3.Scripts1.Puzzletown.UI;
using Match3.Scripts1.UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000705 RID: 1797
	public class M3_BannerTypeView : UiSimpleView<M3_BannerType>
	{
		// Token: 0x06002C82 RID: 11394 RVA: 0x000CCFCC File Offset: 0x000CB3CC
		public override void Show(M3_BannerType data)
		{
			base.Show(data);
			M3_BannersRoot componentInParent = base.GetComponentInParent<M3_BannersRoot>();
			if (this.CheckMatch(data))
			{
				if (data != M3_BannerType.LevelStart)
				{
					if (data != M3_BannerType.OutOfMoves && data != M3_BannerType.OutOfMovesDiscount && data != M3_BannerType.OutOfMovesFree)
					{
						if (data == M3_BannerType.OutOfMovesNoDiamonds)
						{
							// if (componentInParent.iapData != null && componentInParent.iapData.storeProduct != null)
							// {
							// 	string text = componentInParent.loc.GetText("ui.monthly.sale.popup.button", new LocaParam[0]);
							// 	this.label.text = text + " " + componentInParent.iapData.storeProduct.metadata.localizedPriceString;
							// }
						}
					}
					else
					{
						BoosterData boosterData = componentInParent.config.general.boosters.Find((BoosterData e) => e.type == "moves");
						MaterialAmount[] ie = new MaterialAmount[]
						{
							new MaterialAmount("moves", boosterData.amount, MaterialAmountUsage.Income, 0),
							new MaterialAmount("diamonds", boosterData.cost, MaterialAmountUsage.Price, boosterData.offer_price)
						};
						ie.ForEach(delegate(MaterialAmount mat)
						{
							this.ShowOnChildren(mat, true, true);
						});
					}
				}
				else if (componentInParent.level != null && componentInParent.level.LevelCollectionConfig != null && !componentInParent.level.IsEditMode)
				{
					string objective = componentInParent.level.LevelCollectionConfig.objective;
					this.taskImage.sprite = this.taskImageManager.GetSimilar(objective);
					this.label.text = componentInParent.loc.GetText("ui.m3.objective." + objective, new LocaParam[0]);
				}
			}
		}

		// Token: 0x040055CD RID: 21965
		public TMP_Text label;

		// Token: 0x040055CE RID: 21966
		public Image taskImage;

		// Token: 0x040055CF RID: 21967
		public SpriteManager taskImageManager;

		// Token: 0x040055D0 RID: 21968
		public TutorialSpeechBubble offerSpeechBubble;
	}
}

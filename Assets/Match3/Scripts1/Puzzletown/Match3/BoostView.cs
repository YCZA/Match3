using Match3.Scripts1.UnityEngine;
using Match3.Scripts1.Wooga.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000724 RID: 1828
	public class BoostView : ATableViewReusableCell<BoostState>, IDataView<BoostViewData>, IHandler<BoostOperation>
	{
		// Token: 0x1700070F RID: 1807
		// (get) Token: 0x06002D33 RID: 11571 RVA: 0x000D1DA3 File Offset: 0x000D01A3
		public Boosts Type
		{
			get
			{
				return this.data.Type;
			}
		}

		// Token: 0x06002D34 RID: 11572 RVA: 0x000D1DB0 File Offset: 0x000D01B0
		public void Reset()
		{
			this.Show(this.data);
		}

		// Token: 0x06002D35 RID: 11573 RVA: 0x000D1DC0 File Offset: 0x000D01C0
		public void Show(BoostViewData data)
		{
			this.data = data;
			if (this.boostIcon)
			{
				this.boostIcon.sprite = this.boosterIcons.GetSimilar(data.name);
			}
			if (this.labelAmount)
			{
				this.labelAmount.text = data.amount.ToString();
			}
			if (this.waterAllFieldIndicator)
			{
				bool waterAllFields = data.waterAllFields;
				// this.waterAllFieldIndicator.gameObject.SetActive(waterAllFields);
				// 锤子上不显示蜂蜜
				this.waterAllFieldIndicator.gameObject.SetActive(false);
			}
			BoostState state = data.state;
			if (state != BoostState.Selected)
			{
				this.ShowOnChildren((data.amount <= 0) ? BoostOperation.Add : BoostOperation.Use, true, true);
			}
			else
			{
				this.ShowOnChildren(BoostOperation.Cancel, true, true);
			}
			Boosts type = data.Type;
			if (type != Boosts.boost_pre_rainbow)
			{
				if (type != Boosts.boost_pre_bomb_linegem)
				{
					if (type == Boosts.boost_pre_double_fish)
					{
						base.gameObject.tag = "Tutorial - Town - PreGameDoubleFish";
					}
				}
				else
				{
					base.gameObject.tag = "Tutorial - Town - PreGameBombLine";
				}
			}
			else
			{
				base.gameObject.tag = "Tutorial - Town - PreGameRainbow";
			}
		}

		// Token: 0x06002D36 RID: 11574 RVA: 0x000D1EF7 File Offset: 0x000D02F7
		public void Handle(BoostOperation op)
		{
			this.HandleOnParent(this.data, op);
		}

		// Token: 0x040056C6 RID: 22214
		private const string TUTORIAL_PRE_RAINBOW_TAG = "Tutorial - Town - PreGameRainbow";

		// Token: 0x040056C7 RID: 22215
		private const string TUTORIAL_PRE_BOMB_LINE_TAG = "Tutorial - Town - PreGameBombLine";

		// Token: 0x040056C8 RID: 22216
		private const string TUTORIAL_PRE_DOUBLE_FISH_TAG = "Tutorial - Town - PreGameDoubleFish";

		// Token: 0x040056C9 RID: 22217
		[SerializeField]
		private SpriteManager boosterIcons;

		// Token: 0x040056CA RID: 22218
		[SerializeField]
		private Image boostIcon;

		// Token: 0x040056CB RID: 22219
		[SerializeField]
		private TextMeshProUGUI labelAmount;

		// Token: 0x040056CC RID: 22220
		[SerializeField]
		private Image waterAllFieldIndicator;

		// Token: 0x040056CD RID: 22221
		private BoostViewData data;
	}
}

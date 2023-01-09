using System.Collections.Generic;
using System.Linq;
using Match3.Scripts1.UnityEngine;
using Match3.Scripts1.Wooga.UI;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020006FD RID: 1789
	public class M3LevelSelectionItemView : AVisibleGameObject, IDataView<LevelSelectionData>, IDataView<IFacebookFriendsList>, IHandler<PopupOperation>
	{
		// Token: 0x06002C5D RID: 11357 RVA: 0x000CC6A0 File Offset: 0x000CAAA0
		public void Show(LevelSelectionData data)
		{
			int value = (!data.isSingleTier) ? data.tier : 0;
			this._level = new Level(data.setConfig.level, Mathf.Clamp(value, 0, 2));
			this._itemState = data.itemState;
			this._lockReason = data.lockReason;
			this._isHollow = data.isHollow;
			this._isSeparator = data.isSeparator;
			this.Show();
			this.Show(data.facebookFriends);
			if (this.foreshadowing != null)
			{
				bool flag = (this._level.level >= 17) ? (this._level.tier > 0) : (!data.isLatest && !data.isLocked);
				if (this._isSeparator || flag || !data.featureSwitchesConfig.foreshadowing_enabled)
				{
					this.foreshadowing.gameObject.SetActive(false);
				}
				else
				{
					this.foreshadowing.SetVisible(data.foreshadowingConfigs, this._level.level, !string.IsNullOrEmpty(data.collectable), !data.isLocked);
				}
			}
			// 隐藏关卡显示上的New
			// #if REVIEW_VERSION
				// foreshadowing.gameObject.SetActive(false);
			// #endif
		}

		// Token: 0x06002C5E RID: 11358 RVA: 0x000CC7EC File Offset: 0x000CABEC
		public void Handle(PopupOperation op)
		{
			if (op == PopupOperation.OK)
			{
				M3_LevelMapItemState itemState = this._itemState;
				if (itemState != M3_LevelMapItemState.ActiveSingleTier)
				{
					if (itemState != M3_LevelMapItemState.Inactive)
					{
						if (!this._isSeparator)
						{
							this.HandleOnParent(this._level);
						}
					}
					else if (this._lockReason != LevelSelectionData.LockReason.NotReached)
					{
						this.HandleOnParent(this._level);
					}
				}
			}
		}

		// Token: 0x06002C5F RID: 11359 RVA: 0x000CC864 File Offset: 0x000CAC64
		public void Show(IFacebookFriendsList friends)
		{
			if (this._isHollow || this._isSeparator)
			{
				this.friendsDataSource.Show(null);
				return;
			}
			IEnumerable<TownOptionsFriendData> facebookFriends = friends.FacebookFriends;
			this.friendsDataSource.Show((from f in facebookFriends
			where f.level == this._level.level
			select f).Take(4).Reverse<TownOptionsFriendData>());
		}

		// Token: 0x040055A6 RID: 21926
		private const int MAX_FRIENDS = 4;

		// Token: 0x040055A7 RID: 21927
		private const int FIRST_TIERED_LEVEL = 17;

		// Token: 0x040055A8 RID: 21928
		public M3_LevelSelectionFriendsDataSource friendsDataSource;

		// Token: 0x040055A9 RID: 21929
		public M3_LevelSelectionForeshadowing foreshadowing;

		// Token: 0x040055AA RID: 21930
		private M3_LevelMapItemState _itemState;

		// Token: 0x040055AB RID: 21931
		private LevelSelectionData.LockReason _lockReason;

		// Token: 0x040055AC RID: 21932
		private Level _level;

		// Token: 0x040055AD RID: 21933
		private bool _isHollow;

		// Token: 0x040055AE RID: 21934
		private bool _isSeparator;
	}
}

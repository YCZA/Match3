using System;
using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Puzzletown.Match3;

namespace Match3.Scripts1
{
	public struct LevelSelectionData
	{
		public enum State
		{
			Inactive,
			Focus,
			Active
		}

		public enum LockReason
		{
			NotLocked,
			NotReached,
			ComingSoon,
			Quest,
			DateLocked
		}

		public enum LevelOrder
		{
			Normal,
			Latest,
			NextLatest
		}

		public enum LevelMode
		{
			Normal,
			SingleTier
		}

		public LevelSelectionData.LockReason lockReason;

		public ALevelCollectionConfig setConfig;

		public LevelSelectionData.LevelOrder levelOrder;

		public LevelSelectionData.LevelMode levelMode;

		public int tier;

		public string collectable;

		public bool isHollow;

		public bool isSeparator;

		public IFacebookFriendsList facebookFriends;

		public List<LevelForeshadowingConfig.ForeshadowingLevelConfig> foreshadowingConfigs;

		public FeatureSwitchesConfig featureSwitchesConfig;

		public DateTime? unlockDate;

		public LevelSelectionData.State CurrentState
		{
			get
			{
				if (this.isLocked)
				{
					return LevelSelectionData.State.Inactive;
				}
				if (this.isLatest)
				{
					return LevelSelectionData.State.Focus;
				}
				return LevelSelectionData.State.Active;
			}
		}

		public LevelSelectionData.State VisualState
		{
			get
			{
				if (this.isLatest)
				{
					return LevelSelectionData.State.Focus;
				}
				if (this.isLocked)
				{
					return LevelSelectionData.State.Inactive;
				}
				return LevelSelectionData.State.Active;
			}
		}

		public M3_LevelMapItemState itemState
		{
			get
			{
				if (this.isSeparator)
				{
					return M3_LevelMapItemState.Separator;
				}
				if (this.isHollow)
				{
					return M3_LevelMapItemState.None;
				}
				if (this.isLocked)
				{
					return M3_LevelMapItemState.Inactive;
				}
				if (this.isLatest)
				{
					return M3_LevelMapItemState.FocusMode;
				}
				if (this.isSingleTier)
				{
					return M3_LevelMapItemState.ActiveSingleTier;
				}
				if (this.isMastered)
				{
					return M3_LevelMapItemState.Mastered;
				}
				return M3_LevelMapItemState.Active;
			}
		}

		public bool isLocked
		{
			get
			{
				return this.lockReason != LevelSelectionData.LockReason.NotLocked;
			}
		}

		public bool isLatest
		{
			get
			{
				return this.levelOrder == LevelSelectionData.LevelOrder.Latest;
			}
		}

		public bool isSingleTier
		{
			get
			{
				return this.levelMode == LevelSelectionData.LevelMode.SingleTier;
			}
		}

		public bool isMastered
		{
			get
			{
				return this.tier > 2;
			}
		}

		public LevelSelectionData(ALevelCollectionConfig setConfig, int tier, bool isHollow, bool isSeparator, string collectable, LevelSelectionData.LockReason lockReason, LevelSelectionData.LevelOrder levelOrder, LevelSelectionData.LevelMode levelMode, IFacebookFriendsList facebookFriends, List<LevelForeshadowingConfig.ForeshadowingLevelConfig> foreshadowingConfigs, FeatureSwitchesConfig featureSwitchesConfig, DateTime? unlockDate)
		{
			this.setConfig = setConfig;
			this.tier = tier;
			this.lockReason = lockReason;
			this.levelOrder = levelOrder;
			this.levelMode = levelMode;
			this.isHollow = isHollow;
			this.collectable = collectable;
			this.isSeparator = isSeparator;
			this.facebookFriends = facebookFriends;
			this.foreshadowingConfigs = foreshadowingConfigs;
			this.featureSwitchesConfig = featureSwitchesConfig;
			this.unlockDate = unlockDate;
		}
	}
}

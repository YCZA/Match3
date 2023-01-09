using Match3.Scripts1.Puzzletown.Config;
using UnityEngine;

// Token: 0x02000A58 RID: 2648
namespace Match3.Scripts1
{
	public class TournamentRewardSpriteManager : SpriteManager
	{
		// Token: 0x06003F70 RID: 16240 RVA: 0x00144984 File Offset: 0x00142D84
		public void GetSprite(TournamentRewardCategory rewardCategory, out Sprite reward, out Sprite shadow)
		{
			if (this.resources == null || this.resources.Length < 1 || rewardCategory == TournamentRewardCategory.None)
			{
				reward = null;
				shadow = null;
				return;
			}
			int num = this.resources.Length / 2;
			int num2 = Mathf.Min(rewardCategory - TournamentRewardCategory.Gold, num - 1);
			reward = this.resources[num2];
			shadow = this.resources[num + num2];
		}
	}
}

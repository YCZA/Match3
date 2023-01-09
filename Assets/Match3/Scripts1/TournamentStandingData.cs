using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Wooga.Signals;
using UnityEngine;

// Token: 0x02000A5A RID: 2650
namespace Match3.Scripts1
{
	public class TournamentStandingData
	{
		// Token: 0x06003F81 RID: 16257 RVA: 0x001456BC File Offset: 0x00143ABC
		public TournamentStandingData(string facebookID, string name)
		{
			this.fbData = new FacebookData.Friend(facebookID, name);
			this.name = name;
		}

		// Token: 0x0400691B RID: 26907
		public readonly FacebookData.Friend fbData;

		// Token: 0x0400691C RID: 26908
		public string name;

		// Token: 0x0400691D RID: 26909
		public int rank;

		// Token: 0x0400691E RID: 26910
		public int points;

		// Token: 0x0400691F RID: 26911
		public bool isSelf;

		// Token: 0x04006920 RID: 26912
		public Sprite avatar;

		// Token: 0x04006921 RID: 26913
		public bool isOnline;

		// Token: 0x04006922 RID: 26914
		public readonly Signal<string, Sprite> OnAvatarAvailable = new Signal<string, Sprite>();

		// Token: 0x04006923 RID: 26915
		public Sprite taskSprite;

		// Token: 0x04006924 RID: 26916
		public Sprite trophySprite;

		// Token: 0x04006925 RID: 26917
		public Sprite rewardSprite;

		// Token: 0x04006926 RID: 26918
		public Sprite rewardShadowSprite;

		// Token: 0x04006927 RID: 26919
		public TournamentType tournamentType;
	}
}

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020006E3 RID: 1763
	public class LevelOfDayFriendView : MonoBehaviour
	{
		// Token: 0x06002BC9 RID: 11209 RVA: 0x000C9094 File Offset: 0x000C7494
		public void UpdateFacebookFriends(IEnumerable<TownOptionsFriendData> friends)
		{
			this.friendsDataSource.Show((from f in friends
			where f.level == this.day
			select f).Take(4).Reverse<TownOptionsFriendData>());
		}

		// Token: 0x040054E6 RID: 21734
		private const int MAX_FRIENDS = 4;

		// Token: 0x040054E7 RID: 21735
		public int day;

		// Token: 0x040054E8 RID: 21736
		public M3_LevelSelectionFriendsDataSource friendsDataSource;
	}
}

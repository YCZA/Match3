using System.Collections.Generic;
using System.Linq;
using Match3.Scripts1.Puzzletown.Match3;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x020004AD RID: 1197
	public class DiveForTreasureFriendView : MonoBehaviour
	{
		// Token: 0x060021AD RID: 8621 RVA: 0x0008D2D1 File Offset: 0x0008B6D1
		public void UpdateFacebookFriends(IEnumerable<TownOptionsFriendData> friends)
		{
			this.friendsDataSource.Show((from f in friends
			where f.level == this.level
			select f).Take(4).Reverse<TownOptionsFriendData>());
		}

		// Token: 0x04004CD8 RID: 19672
		private const int MAX_FRIENDS = 4;

		// Token: 0x04004CD9 RID: 19673
		public int level;

		// Token: 0x04004CDA RID: 19674
		public M3_LevelSelectionFriendsDataSource friendsDataSource;
	}
}

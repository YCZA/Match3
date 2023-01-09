using Match3.Scripts1.Wooga.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020006EA RID: 1770
	public class M3_LevelSelectionFriendView : ATableViewReusableCell<TownOptionsFriendData.Type>, IDataView<TownOptionsFriendData>
	{
		// Token: 0x06002C12 RID: 11282 RVA: 0x000CB183 File Offset: 0x000C9583
		public void Show(TownOptionsFriendData data)
		{
			this.data = data;
			this.SetAvatarSprite(data.avatar);
			data.OnAvatarAvailable.RemoveAllListeners();
			data.OnAvatarAvailable.AddListener(delegate(string id, Sprite spr)
			{
				if (id == this.data.friend.ID)
				{
					this.SetAvatarSprite(spr);
				}
			});
		}

		// Token: 0x06002C13 RID: 11283 RVA: 0x000CB1BA File Offset: 0x000C95BA
		private void SetAvatarSprite(Sprite sprite)
		{
			if (this.image && sprite)
			{
				this.image.sprite = sprite;
			}
		}

		// Token: 0x04005540 RID: 21824
		private TownOptionsFriendData data;

		// Token: 0x04005541 RID: 21825
		public Image image;
	}
}

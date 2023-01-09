using Match3.Scripts1.UnityEngine;
using Match3.Scripts1.Wooga.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020008B2 RID: 2226
namespace Match3.Scripts1
{
	public class MultiFriendsSelectorView : ATableViewReusableCell, IDataView<MultiFriendsSelectorFriendData>, IEditorDescription
	{
		// Token: 0x0600364D RID: 13901 RVA: 0x001071AF File Offset: 0x001055AF
		private void setPictureSprite(Sprite nSprite)
		{
			this.PictureSil.gameObject.SetActive(nSprite == null);
			this.Picture.gameObject.SetActive(nSprite != null);
			this.Picture.sprite = nSprite;
		}

		// Token: 0x0600364E RID: 13902 RVA: 0x001071EB File Offset: 0x001055EB
		public string GetEditorDescription()
		{
			return this.State.ToString();
		}

		// Token: 0x0600364F RID: 13903 RVA: 0x00107200 File Offset: 0x00105600
		public void Show(MultiFriendsSelectorFriendData data)
		{
			this.Name.text = data.friend.Name;
			this.setPictureSprite(data.avatar);
			this.FriendButton.onClick.RemoveAllListeners();
			this.FriendButton.onClick.AddListener(delegate()
			{
				this.HandleOnParent(data);
			});
			data.OnAvatarAvailable.RemoveAllListeners();
			data.OnAvatarAvailable.AddListener(delegate(string id, Sprite spr)
			{
				if (id == data.friend.ID)
				{
					this.setPictureSprite(spr);
				}
			});
		}

		// Token: 0x17000852 RID: 2130
		// (get) Token: 0x06003650 RID: 13904 RVA: 0x001072A5 File Offset: 0x001056A5
		public override int reusableId
		{
			get
			{
				return (int)this.State;
			}
		}

		// Token: 0x04005E58 RID: 24152
		[SerializeField]
		private Image PictureSil;

		// Token: 0x04005E59 RID: 24153
		[SerializeField]
		private Image Picture;

		// Token: 0x04005E5A RID: 24154
		[SerializeField]
		private TMP_Text Name;

		// Token: 0x04005E5B RID: 24155
		[SerializeField]
		private Button FriendButton;

		// Token: 0x04005E5C RID: 24156
		public MultiFriendsSelectorFriendData.State State;
	}
}

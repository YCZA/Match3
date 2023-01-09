using Match3.Scripts1.UnityEngine;
using Match3.Scripts1.Wooga.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020009E7 RID: 2535
namespace Match3.Scripts1
{
	public class TownOptionsInboxView : ATableViewReusableCell, IDataView<TownOptionsInboxData>, IHandler<TownOptionsInboxOperation>
	{
		// Token: 0x06003D2D RID: 15661 RVA: 0x00134B4F File Offset: 0x00132F4F
		private void setAvatarSprite(Sprite nSprite)
		{
			this.silAvatar.gameObject.SetActive(nSprite == null);
			this.userAvatar.gameObject.SetActive(nSprite != null);
			this.userAvatar.sprite = nSprite;
		}

		// Token: 0x06003D2E RID: 15662 RVA: 0x00134B8C File Offset: 0x00132F8C
		public void Show(TownOptionsInboxData data)
		{
			this.data = data;
			this.userName.text = data.friend.Name;
			this.setAvatarSprite(data.avatar);
			this.AcceptAndSendButton.SetActive(data.enableSend);
			this.AcceptButton.SetActive(!data.enableSend);
			data.OnAvatarAvailable.RemoveAllListeners();
			data.OnAvatarAvailable.AddListener(delegate(string id, Sprite spr)
			{
				if (id == this.data.friend.ID)
				{
					this.setAvatarSprite(spr);
				}
			});
		}

		// Token: 0x17000918 RID: 2328
		// (get) Token: 0x06003D2F RID: 15663 RVA: 0x00134C09 File Offset: 0x00133009
		public override int reusableId
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x06003D30 RID: 15664 RVA: 0x00134C0C File Offset: 0x0013300C
		public void Handle(TownOptionsInboxOperation op)
		{
			this.HandleOnParent(op, this.data.requestId);
		}

		// Token: 0x040065EC RID: 26092
		private TownOptionsInboxData data;

		// Token: 0x040065ED RID: 26093
		[SerializeField]
		private Image userAvatar;

		// Token: 0x040065EE RID: 26094
		[SerializeField]
		private Image silAvatar;

		// Token: 0x040065EF RID: 26095
		[SerializeField]
		private TMP_Text userName;

		// Token: 0x040065F0 RID: 26096
		[SerializeField]
		private GameObject AcceptAndSendButton;

		// Token: 0x040065F1 RID: 26097
		[SerializeField]
		private GameObject AcceptButton;
	}
}

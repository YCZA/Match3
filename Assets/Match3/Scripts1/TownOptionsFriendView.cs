using System;
using Match3.Scripts1.UnityEngine;
using Match3.Scripts1.Wooga.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020009E2 RID: 2530
namespace Match3.Scripts1
{
	public class TownOptionsFriendView : ATableViewReusableCell, IDataView<TownOptionsFriendData>, IHandler<TownOptionsFriendOperation>
	{
		// Token: 0x06003D21 RID: 15649 RVA: 0x001348FC File Offset: 0x00132CFC
		private void setAvatarSprite(Sprite nSprite)
		{
			this.silAvatar.gameObject.SetActive(nSprite == null);
			this.userAvatar.gameObject.SetActive(nSprite != null);
			this.userAvatar.sprite = nSprite;
		}

		// Token: 0x06003D22 RID: 15650 RVA: 0x00134938 File Offset: 0x00132D38
		public void Show(TownOptionsFriendData data)
		{
			this.data = data;
			this.userName.text = data.friend.Name;
			this.levelNum.text = data.level.ToString();
			this.number.text = data.index.ToString();
			this.nextLifeAvailable = data.nextLifeAvailable;
			this.setAvatarSprite(data.avatar);
			data.OnAvatarAvailable.RemoveAllListeners();
			data.OnAvatarAvailable.AddListener(delegate(string id, Sprite spr)
			{
				if (id == this.data.friend.ID)
				{
					this.setAvatarSprite(spr);
				}
			});
			this.UpdateSendLife();
			this.VisitFriendButton.SetActive(this.data.enableSend);
		}

		// Token: 0x17000917 RID: 2327
		// (get) Token: 0x06003D23 RID: 15651 RVA: 0x001349F0 File Offset: 0x00132DF0
		public override int reusableId
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x06003D24 RID: 15652 RVA: 0x001349F3 File Offset: 0x00132DF3
		public void Handle(TownOptionsFriendOperation op)
		{
			this.HandleOnParent(op, this.data.friend.ID);
		}

		// Token: 0x06003D25 RID: 15653 RVA: 0x00134A0C File Offset: 0x00132E0C
		public void UpdateSendLife()
		{
			bool flag = DateTime.UtcNow > this.nextLifeAvailable;
			if (this.data != null)
			{
				this.SendLifeButton.SetActive(flag && this.data.enableSend);
				this.LifeCooldownButton.SetActive(!flag && this.data.enableSend);
			}
		}

		// Token: 0x06003D26 RID: 15654 RVA: 0x00134A73 File Offset: 0x00132E73
		public void Update()
		{
			this.UpdateSendLife();
		}

		// Token: 0x040065D6 RID: 26070
		private TownOptionsInboxData data;

		// Token: 0x040065D7 RID: 26071
		[SerializeField]
		private Image silAvatar;

		// Token: 0x040065D8 RID: 26072
		[SerializeField]
		private Image userAvatar;

		// Token: 0x040065D9 RID: 26073
		[SerializeField]
		private TMP_Text userName;

		// Token: 0x040065DA RID: 26074
		[SerializeField]
		private TMP_Text levelNum;

		// Token: 0x040065DB RID: 26075
		[SerializeField]
		private TMP_Text number;

		// Token: 0x040065DC RID: 26076
		[SerializeField]
		private GameObject SendLifeButton;

		// Token: 0x040065DD RID: 26077
		[SerializeField]
		private GameObject LifeCooldownButton;

		// Token: 0x040065DE RID: 26078
		[SerializeField]
		private GameObject VisitFriendButton;

		// Token: 0x040065DF RID: 26079
		private DateTime nextLifeAvailable;
	}
}

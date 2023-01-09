using Match3.Scripts1.Wooga.UI;
using TMPro;
using UnityEngine;

// Token: 0x020008A7 RID: 2215
namespace Match3.Scripts1
{
	public class MultiFriendsSelectMetaPanel : MonoBehaviour, IDataView<MultiFriendsSelectorRoot.ViewData>
	{
		// Token: 0x0600362A RID: 13866 RVA: 0x0010658A File Offset: 0x0010498A
		public void Show(MultiFriendsSelectorRoot.ViewData data)
		{
			this.friendsSelectedCounter.text = data.SelectedCount.ToString() + "/" + data.TotalCount.ToString();
		}

		// Token: 0x04005E29 RID: 24105
		[SerializeField]
		private TMP_Text friendsSelectedCounter;
	}
}

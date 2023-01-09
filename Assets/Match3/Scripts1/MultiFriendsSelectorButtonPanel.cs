using Match3.Scripts1.UnityEngine;
using Match3.Scripts1.Wooga.UI;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020008AA RID: 2218
namespace Match3.Scripts1
{
	public class MultiFriendsSelectorButtonPanel : MonoBehaviour, IDataView<MultiFriendsSelectorRoot.ViewData>
	{
		// Token: 0x0600362E RID: 13870 RVA: 0x001065E8 File Offset: 0x001049E8
		public void Show(MultiFriendsSelectorRoot.ViewData data)
		{
			MultiFriendsSelectOperation multiFriendsSelectOperation = MultiFriendsSelectOperation.AskAnyone;
			multiFriendsSelectOperation |= ((data.SelectedCount <= 0) ? MultiFriendsSelectOperation.AskSelectedDisabled : MultiFriendsSelectOperation.AskSelected);
			this.ShowOnChildren(multiFriendsSelectOperation, true, true);
		}

		// Token: 0x04005E30 RID: 24112
		[SerializeField]
		private Button SendButton;

		// Token: 0x04005E31 RID: 24113
		[SerializeField]
		private Button SendButtonDisabled;
	}
}

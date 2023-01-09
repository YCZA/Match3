using UnityEngine;
using UnityEngine.EventSystems;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020006E9 RID: 1769
	public class M3_LevelSelectionFriendsDataSource : ArrayDataSource<M3_LevelSelectionFriendView, TownOptionsFriendData>, IPointerUpHandler, IPointerDownHandler, IEventSystemHandler
	{
		// Token: 0x170006F2 RID: 1778
		// (get) Token: 0x06002C0A RID: 11274 RVA: 0x000CB11D File Offset: 0x000C951D
		private Animator expander
		{
			get
			{
				return base.GetComponent<Animator>();
			}
		}

		// Token: 0x170006F3 RID: 1779
		// (get) Token: 0x06002C0B RID: 11275 RVA: 0x000CB125 File Offset: 0x000C9525
		// (set) Token: 0x06002C0C RID: 11276 RVA: 0x000CB137 File Offset: 0x000C9537
		public bool isExpanded
		{
			get
			{
				return this.expander.GetBool(M3_LevelSelectionFriendsDataSource.EXPAND);
			}
			set
			{
				this.expander.SetBool(M3_LevelSelectionFriendsDataSource.EXPAND, value);
			}
		}

		// Token: 0x06002C0D RID: 11277 RVA: 0x000CB14A File Offset: 0x000C954A
		public void OnPointerUp(PointerEventData evt)
		{
			this.isExpanded = false;
		}

		// Token: 0x06002C0E RID: 11278 RVA: 0x000CB153 File Offset: 0x000C9553
		public void OnPointerDown(PointerEventData evt)
		{
			this.isExpanded = true;
		}

		// Token: 0x06002C0F RID: 11279 RVA: 0x000CB15C File Offset: 0x000C955C
		public override int GetReusableIdForIndex(int index)
		{
			return (int)this.GetDataForIndex(index).type;
		}

		// Token: 0x0400553F RID: 21823
		private static readonly int EXPAND = Animator.StringToHash("Expand");
	}
}

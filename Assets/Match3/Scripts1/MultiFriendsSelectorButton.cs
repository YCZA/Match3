

// Token: 0x020008A9 RID: 2217
namespace Match3.Scripts1
{
	public class MultiFriendsSelectorButton : AParentedHideableControlButton<MultiFriendsSelectOperation>
	{
		// Token: 0x0600362C RID: 13868 RVA: 0x001065CB File Offset: 0x001049CB
		protected override bool CanShow(MultiFriendsSelectOperation op)
		{
			return (op & this.operation) == this.operation;
		}
	}
}

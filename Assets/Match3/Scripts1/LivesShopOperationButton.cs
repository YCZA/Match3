

// Token: 0x020008A2 RID: 2210
namespace Match3.Scripts1
{
	public class LivesShopOperationButton : AParentedHideableControlButton<LivesShopOperation>
	{
		// Token: 0x06003608 RID: 13832 RVA: 0x001052AF File Offset: 0x001036AF
		protected override bool CanShow(LivesShopOperation op)
		{
			return (op & this.operation) == this.operation;
		}
	}
}

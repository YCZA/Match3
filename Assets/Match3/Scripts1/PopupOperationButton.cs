

// Token: 0x02000A23 RID: 2595
namespace Match3.Scripts1
{
	public class PopupOperationButton : AParentedHideableControlButton<PopupOperation>
	{
		// Token: 0x06003E4F RID: 15951 RVA: 0x0013BFAE File Offset: 0x0013A3AE
		protected override bool CanShow(PopupOperation op)
		{
			return (op & this.operation) == this.operation;
		}
	}
}
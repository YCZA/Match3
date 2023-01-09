

// Token: 0x02000A1F RID: 2591
namespace Match3.Scripts1
{
	public class AdSpinOperationButton : AParentedHideableControlButton<AdSpinOperation>
	{
		// Token: 0x06003E40 RID: 15936 RVA: 0x0013BEBE File Offset: 0x0013A2BE
		protected override bool CanShow(AdSpinOperation op)
		{
			return (op & this.operation) == this.operation;
		}
	}
}

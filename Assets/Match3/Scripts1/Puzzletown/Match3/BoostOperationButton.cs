namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x0200071A RID: 1818
	public class BoostOperationButton : AParentedHideableControlButton<BoostOperation>
	{
		// Token: 0x06002CFE RID: 11518 RVA: 0x000D0C7A File Offset: 0x000CF07A
		protected override bool CanShow(BoostOperation op)
		{
			return (op & this.operation) == this.operation;
		}
	}
}

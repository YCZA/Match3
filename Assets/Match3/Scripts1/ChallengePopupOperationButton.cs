

// Token: 0x0200075E RID: 1886
namespace Match3.Scripts1
{
	public class ChallengePopupOperationButton : AParentedHideableControlButton<ChallengeOperation>
	{
		// Token: 0x06002EBB RID: 11963 RVA: 0x000DA23E File Offset: 0x000D863E
		protected override bool CanShow(ChallengeOperation op)
		{
			return (op & this.operation) == this.operation;
		}
	}
}

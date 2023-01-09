using Match3.Scripts1.Puzzletown.UI;

// Token: 0x02000A18 RID: 2584
namespace Match3.Scripts1
{
	public class TownResourceView : UiSimpleView<TownResourceElement>
	{
		// Token: 0x06003E28 RID: 15912 RVA: 0x0013B31C File Offset: 0x0013971C
		public override bool CheckMatch(TownResourceElement data)
		{
			return (data & this.state) == this.state;
		}

		// Token: 0x04006710 RID: 26384
		public const TownResourceElement AllResources = TownResourceElement.Diamonds | TownResourceElement.Lives | TownResourceElement.Harmony | TownResourceElement.Coins | TownResourceElement.Seasonal;
	}
}

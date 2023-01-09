using Match3.Scripts1.Puzzletown.Town;
using Match3.Scripts1.Town;

// Token: 0x02000985 RID: 2437
namespace Match3.Scripts1
{
	public abstract class ABuildingUiStateCallout : ABuildingUi, IHandler<BuildingOperation>
	{
		// Token: 0x06003B68 RID: 15208 RVA: 0x001277F5 File Offset: 0x00125BF5
		public void AssignTownOverheadUiRoot(TownOverheadUiRoot townOverheadUiRoot)
		{
			this.townOverheadUiRoot = townOverheadUiRoot;
		}

		// Token: 0x06003B69 RID: 15209 RVA: 0x001277FE File Offset: 0x00125BFE
		public override void Show(BuildingInstance building)
		{
			base.Show(building);
			this.DoShow(building);
		}

		// Token: 0x06003B6A RID: 15210
		public abstract void DoShow(BuildingInstance building);

		// Token: 0x06003B6B RID: 15211
		public abstract void Refresh(BuildingInstance building);

		// Token: 0x06003B6C RID: 15212
		public abstract void Handle(BuildingOperation op);

		// Token: 0x0400637F RID: 25471
		protected TownOverheadUiRoot townOverheadUiRoot;
	}
}

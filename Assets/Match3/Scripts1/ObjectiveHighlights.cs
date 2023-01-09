using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1
{
	// Token: 0x020005DC RID: 1500
	public struct ObjectiveHighlights : IMatchGroup, IMatchResult
	{
		// Token: 0x060026E0 RID: 9952 RVA: 0x000AD934 File Offset: 0x000ABD34
		public ObjectiveHighlights(Group group)
		{
			this.group = group;
		}

		// Token: 0x17000614 RID: 1556
		// (get) Token: 0x060026E1 RID: 9953 RVA: 0x000AD93D File Offset: 0x000ABD3D
		public Group Group
		{
			get
			{
				return this.group;
			}
		}

		// Token: 0x17000615 RID: 1557
		// (get) Token: 0x060026E2 RID: 9954 RVA: 0x000AD945 File Offset: 0x000ABD45
		public bool ShouldHitAdjacentFields
		{
			get
			{
				return true;
			}
		}

		// Token: 0x04005198 RID: 20888
		private readonly Group group;
	}
}

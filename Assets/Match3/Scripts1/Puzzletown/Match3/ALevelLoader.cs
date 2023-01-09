using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020006C4 RID: 1732
	public abstract class ALevelLoader : MonoBehaviour
	{
		// Token: 0x06002B20 RID: 11040
		public abstract void LoadBoard(Fields fields);

		// Token: 0x170006CE RID: 1742
		// (get) Token: 0x06002B21 RID: 11041
		public abstract Fields Fields { get; }

		// Token: 0x06002B22 RID: 11042
		public abstract APTBoardFactory CreateBoardFactory();

		// Token: 0x04005448 RID: 21576
		[SerializeField]
		protected BoardView boardView;

		// Token: 0x04005449 RID: 21577
		[SerializeField]
		protected TextAsset testState;

		// Token: 0x0400544A RID: 21578
		[SerializeField]
		protected LastHurray lastHurray;

		// Token: 0x0400544B RID: 21579
		public TextAsset testConfig;
	}
}

using Match3.Scripts1.Puzzletown.Config;
using Shared.Pooling;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020006D0 RID: 1744
	public class TournamentItemCollectedViewFactory : MonoBehaviour
	{
		// Token: 0x06002B77 RID: 11127 RVA: 0x000C757C File Offset: 0x000C597C
		public TournamentItemCollectedView CreateTournamentCollectedItem(Vector3 position, TournamentType type)
		{
			TournamentItemCollectedView tournamentItemCollectedView = this.prefabTournamentCollectedLine;
			switch (type)
			{
			case TournamentType.Bomb:
				tournamentItemCollectedView = this.prefabTournamentCollectedBomb;
				break;
			case TournamentType.Butterfly:
				tournamentItemCollectedView = this.prefabTournamentCollectedButterfly;
				break;
			case TournamentType.Line:
				tournamentItemCollectedView = this.prefabTournamentCollectedLine;
				break;
			case TournamentType.Strawberry:
				tournamentItemCollectedView = this.prefabTournamentCollectedStrawberry;
				break;
			case TournamentType.Banana:
				tournamentItemCollectedView = this.prefabTournamentCollectedBanana;
				break;
			case TournamentType.Plum:
				tournamentItemCollectedView = this.prefabTournamentCollectedPlum;
				break;
			case TournamentType.Apple:
				tournamentItemCollectedView = this.prefabTournamentCollectedApple;
				break;
			case TournamentType.Starfruit:
				tournamentItemCollectedView = this.prefabTournamentCollectedStarfruit;
				break;
			case TournamentType.Grape:
				tournamentItemCollectedView = this.prefabTournamentCollectedGrape;
				break;
			default:
				global::UnityEngine.Debug.LogWarning("Trying to create undefined tournament collected item view. Default will be used");
				break;
			}
			TournamentItemCollectedView component = this.pool.Get(tournamentItemCollectedView.gameObject).GetComponent<TournamentItemCollectedView>();
			component.transform.SetParent(base.transform);
			component.transform.localScale = Vector3.one;
			component.transform.localPosition = position;
			return component;
		}

		// Token: 0x0400548B RID: 21643
		[SerializeField]
		private ObjectPool pool;

		// Token: 0x0400548C RID: 21644
		[SerializeField]
		private TournamentItemCollectedView prefabTournamentCollectedLine;

		// Token: 0x0400548D RID: 21645
		[SerializeField]
		private TournamentItemCollectedView prefabTournamentCollectedBomb;

		// Token: 0x0400548E RID: 21646
		[SerializeField]
		private TournamentItemCollectedView prefabTournamentCollectedButterfly;

		// Token: 0x0400548F RID: 21647
		[SerializeField]
		private TournamentItemCollectedView prefabTournamentCollectedStrawberry;

		// Token: 0x04005490 RID: 21648
		[SerializeField]
		private TournamentItemCollectedView prefabTournamentCollectedBanana;

		// Token: 0x04005491 RID: 21649
		[SerializeField]
		private TournamentItemCollectedView prefabTournamentCollectedPlum;

		// Token: 0x04005492 RID: 21650
		[SerializeField]
		private TournamentItemCollectedView prefabTournamentCollectedApple;

		// Token: 0x04005493 RID: 21651
		[SerializeField]
		private TournamentItemCollectedView prefabTournamentCollectedStarfruit;

		// Token: 0x04005494 RID: 21652
		[SerializeField]
		private TournamentItemCollectedView prefabTournamentCollectedGrape;
	}
}

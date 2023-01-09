using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Shared.M3Engine;
using UnityEngine;

namespace Match3.Scripts1
{
	// Token: 0x020005EA RID: 1514
	public struct TournamentScoreMatch : ITournamentScoreMatch, IMatchResult
	{
		// Token: 0x060026F5 RID: 9973 RVA: 0x000AD948 File Offset: 0x000ABD48
		public TournamentScoreMatch(TournamentType type, IntVector2 position, Vector3 releasePosition)
		{
			this.type = type;
			this.position = position;
			this.releasePosition = releasePosition;
		}

		// Token: 0x17000626 RID: 1574
		// (get) Token: 0x060026F6 RID: 9974 RVA: 0x000AD95F File Offset: 0x000ABD5F
		public TournamentType Type
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x17000627 RID: 1575
		// (get) Token: 0x060026F7 RID: 9975 RVA: 0x000AD967 File Offset: 0x000ABD67
		public IntVector2 Position
		{
			get
			{
				return this.position;
			}
		}

		// Token: 0x17000628 RID: 1576
		// (get) Token: 0x060026F8 RID: 9976 RVA: 0x000AD96F File Offset: 0x000ABD6F
		public Vector3 ReleasePosition
		{
			get
			{
				return this.releasePosition;
			}
		}

		// Token: 0x04005199 RID: 20889
		private readonly TournamentType type;

		// Token: 0x0400519A RID: 20890
		private readonly IntVector2 position;

		// Token: 0x0400519B RID: 20891
		private readonly Vector3 releasePosition;
	}
}

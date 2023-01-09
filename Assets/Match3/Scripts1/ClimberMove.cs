using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1
{
	// Token: 0x020005CF RID: 1487
	public struct ClimberMove : IFinalMovement, IMatchResult
	{
		// Token: 0x060026A4 RID: 9892 RVA: 0x000AD3D1 File Offset: 0x000AB7D1
		public ClimberMove(Move move)
		{
			this.move = move;
		}

		// Token: 0x170005ED RID: 1517
		// (get) Token: 0x060026A5 RID: 9893 RVA: 0x000AD3DA File Offset: 0x000AB7DA
		// (set) Token: 0x060026A6 RID: 9894 RVA: 0x000AD3E7 File Offset: 0x000AB7E7
		public bool IsFinal
		{
			get
			{
				return this.move.IsFinal;
			}
			set
			{
				this.move.IsFinal = value;
			}
		}

		// Token: 0x170005EE RID: 1518
		// (get) Token: 0x060026A7 RID: 9895 RVA: 0x000AD3F5 File Offset: 0x000AB7F5
		// (set) Token: 0x060026A8 RID: 9896 RVA: 0x000AD402 File Offset: 0x000AB802
		public IntVector2 Position
		{
			get
			{
				return this.move.Position;
			}
			private set
			{
			}
		}

		// Token: 0x060026A9 RID: 9897 RVA: 0x000AD404 File Offset: 0x000AB804
		public static ClimberMove FromPortal(IntVector2 from, IntVector2 to)
		{
			Move move = Move.FromPortal(from, to);
			return new ClimberMove(move);
		}

		// Token: 0x060026AA RID: 9898 RVA: 0x000AD41F File Offset: 0x000AB81F
		public override string ToString()
		{
			return string.Format("[ClimberMove]{0}{1}", this.move.from, this.move.to);
		}

		// Token: 0x04005173 RID: 20851
		public Move move;
	}
}

using System;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1
{
	// Token: 0x02000B02 RID: 2818
	[Serializable]
	public struct Move : IEquatable<Move>, IFinalMovement, IMatchResult
	{
		// Token: 0x0600428E RID: 17038 RVA: 0x00155508 File Offset: 0x00153908
		public Move(IntVector2 from, IntVector2 to, bool isSwap = false, bool isFinal = false, bool needsAnimation = true)
		{
			this.from = from;
			this.to = to;
			this.isSwap = isSwap;
			this.needsAnimation = needsAnimation;
			this.destroyGem = false;
			this.IsFinal = isFinal;
		}

		// Token: 0x17000998 RID: 2456
		// (get) Token: 0x0600428F RID: 17039 RVA: 0x00155536 File Offset: 0x00153936
		public IntVector2 Position
		{
			get
			{
				return this.to;
			}
		}

		// Token: 0x17000999 RID: 2457
		// (get) Token: 0x06004290 RID: 17040 RVA: 0x0015553E File Offset: 0x0015393E
		// (set) Token: 0x06004291 RID: 17041 RVA: 0x00155546 File Offset: 0x00153946
		public bool IsFinal { get; set; }

		// Token: 0x06004292 RID: 17042 RVA: 0x00155550 File Offset: 0x00153950
		public static Move FromPortal(IntVector2 from, IntVector2 to)
		{
			return new Move(from, to, false, false, true)
			{
				destroyGem = true
			};
		}

		// Token: 0x06004293 RID: 17043 RVA: 0x00155572 File Offset: 0x00153972
		public override string ToString()
		{
			return string.Format("[Move]{0}{1}", this.from, this.to);
		}

		// Token: 0x06004294 RID: 17044 RVA: 0x00155594 File Offset: 0x00153994
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj.GetType() != typeof(Move))
			{
				return false;
			}
			Move move = (Move)obj;
			return this.isSwap == move.isSwap && this.from == move.from && this.to == move.to;
		}

		// Token: 0x06004295 RID: 17045 RVA: 0x00155608 File Offset: 0x00153A08
		public bool Equals(Move other)
		{
			return this.from.Equals(other.from) && this.to.Equals(other.to) && this.isSwap == other.isSwap;
		}

		// Token: 0x06004296 RID: 17046 RVA: 0x00155658 File Offset: 0x00153A58
		public bool HasSamePositions(Move other)
		{
			return (this.from.Equals(other.from) || this.from.Equals(other.to)) && (this.to.Equals(other.from) || this.to.Equals(other.to));
		}

		// Token: 0x06004297 RID: 17047 RVA: 0x001556C2 File Offset: 0x00153AC2
		public override int GetHashCode()
		{
			return this.isSwap.GetHashCode() ^ this.from.GetHashCode() ^ this.to.GetHashCode();
		}

		// Token: 0x06004298 RID: 17048 RVA: 0x001556F9 File Offset: 0x00153AF9
		public static bool operator ==(Move a, Move b)
		{
			return a.Equals(b);
		}

		// Token: 0x06004299 RID: 17049 RVA: 0x00155703 File Offset: 0x00153B03
		public static bool operator !=(Move a, Move b)
		{
			return !a.Equals(b);
		}

		// Token: 0x04006B70 RID: 27504
		public bool isSwap;

		// Token: 0x04006B71 RID: 27505
		public bool destroyGem;

		// Token: 0x04006B72 RID: 27506
		public IntVector2 from;

		// Token: 0x04006B73 RID: 27507
		public IntVector2 to;

		// Token: 0x04006B75 RID: 27509
		public bool needsAnimation;
	}
}

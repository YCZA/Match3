using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1
{
	// Token: 0x020005D8 RID: 1496
	public struct HammerMatch : IMatchGroup, IMatchWithAffectedFields, IWaterSpreadingResult, IMatchResult
	{
		// Token: 0x060026CA RID: 9930 RVA: 0x000AD7EC File Offset: 0x000ABBEC
		public HammerMatch(IMatchGroup match, IntVector2 position, bool spreadWater)
		{
			this.match = match;
			this.position = position;
			if (match is IMatchWithAffectedFields)
			{
				this.fields = ((IMatchWithAffectedFields)match).Fields;
			}
			else
			{
				this.fields = new List<IntVector2>();
			}
			this.SpreadWater = spreadWater;
		}

		// Token: 0x17000603 RID: 1539
		// (get) Token: 0x060026CB RID: 9931 RVA: 0x000AD83A File Offset: 0x000ABC3A
		public Group Group
		{
			get
			{
				return this.match.Group;
			}
		}

		// Token: 0x17000604 RID: 1540
		// (get) Token: 0x060026CC RID: 9932 RVA: 0x000AD847 File Offset: 0x000ABC47
		public bool ShouldHitAdjacentFields
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000605 RID: 1541
		// (get) Token: 0x060026CD RID: 9933 RVA: 0x000AD84A File Offset: 0x000ABC4A
		public List<IntVector2> Fields
		{
			get
			{
				return this.fields;
			}
		}

		// Token: 0x17000606 RID: 1542
		// (get) Token: 0x060026CE RID: 9934 RVA: 0x000AD852 File Offset: 0x000ABC52
		// (set) Token: 0x060026CF RID: 9935 RVA: 0x000AD85A File Offset: 0x000ABC5A
		public bool SpreadWater { get; private set; }

		// Token: 0x0400518F RID: 20879
		public IntVector2 position;

		// Token: 0x04005190 RID: 20880
		public IMatchGroup match;

		// Token: 0x04005191 RID: 20881
		private List<IntVector2> fields;
	}
}

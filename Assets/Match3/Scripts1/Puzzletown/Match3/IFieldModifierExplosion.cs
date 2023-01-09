using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020005E5 RID: 1509
	public interface IFieldModifierExplosion : IMatchResult
	{
		// Token: 0x1700061E RID: 1566
		// (get) Token: 0x060026EC RID: 9964
		string Type { get; }

		// Token: 0x1700061F RID: 1567
		// (get) Token: 0x060026ED RID: 9965
		bool CountForObjective { get; }
	}
}

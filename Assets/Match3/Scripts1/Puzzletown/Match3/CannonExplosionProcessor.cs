using System.Collections.Generic;
using Match3.Scripts1.Shared.M3Engine;
using Shared.Pooling;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020005FF RID: 1535
	public class CannonExplosionProcessor : IMatchProcessor
	{
		// Token: 0x06002759 RID: 10073 RVA: 0x000AEA18 File Offset: 0x000ACE18
		public CannonExplosionProcessor(GemFactory gemFactory)
		{
			this.gemFactory = gemFactory;
		}

		// Token: 0x0600275A RID: 10074 RVA: 0x000AEA34 File Offset: 0x000ACE34
		public IEnumerable<IMatchResult> Process(Fields fields, List<IMatchResult> allResults)
		{
			this.results.Clear();
			for (int i = 0; i < allResults.Count; i++)
			{
				IMatchResult matchResult = allResults[i];
				if (this.CannonExplosionTriggerNeedsProcessing(matchResult))
				{
					allResults[i] = this.Explode((CannonExplosionTrigger)matchResult, fields);
				}
			}
			return this.results;
		}

		// Token: 0x0600275B RID: 10075 RVA: 0x000AEA94 File Offset: 0x000ACE94
		private bool CannonExplosionTriggerNeedsProcessing(IMatchResult result)
		{
			return result is CannonExplosionTrigger && !((CannonExplosionTrigger)result).IsProcessed;
		}

		// Token: 0x0600275C RID: 10076 RVA: 0x000AEAC0 File Offset: 0x000ACEC0
		private IMatchResult Explode(CannonExplosionTrigger explosionTrigger, Fields fields)
		{
			explosionTrigger.IsProcessed = true;
			IntVector2 cannonPosition = explosionTrigger.cannonPosition;
			List<IntVector2> list = this.CollectExplodingFields(fields, cannonPosition);
			Group group = GroupPool.Get(fields[cannonPosition].gem);
			List<IntVector2> list2 = ListPool<IntVector2>.Create(10);
			this.results.Add(new CannonExplosion(explosionTrigger.cannonPosition, group, list2, list));
			ProcessorHelper.RemoveTiles(fields[explosionTrigger.cannonPosition], this.results);
			fields[explosionTrigger.cannonPosition].HasGem = false;
			foreach (IntVector2 vec in list)
			{
				Field field = fields[vec];
				ProcessorHelper.RemoveCoverAndBlocker(field, this.results);
				ProcessorHelper.RemoveGemCover(field, this.results);
				ProcessorHelper.RemoveTiles(field, this.results);
				ProcessorHelper.RemoveChameleonGem(field, this.results, this.gemFactory);
				if (field.gem.IsMatchable || field.gem.BlocksLineGem())
				{
					group.Add(field.gem);
					fields[vec].HasGem = false;
				}
				else
				{
					list2.Add(field.gridPosition);
				}
			}
			return explosionTrigger;
		}

		// Token: 0x0600275D RID: 10077 RVA: 0x000AEC28 File Offset: 0x000AD028
		private List<IntVector2> CollectExplodingFields(Fields fields, IntVector2 origin)
		{
			List<IntVector2> list = ListPool<IntVector2>.Create(10);
			this.CollectExplodingFieldsForDirection(origin, IntVector2.Left, fields, list);
			this.CollectExplodingFieldsForDirection(origin, IntVector2.Right, fields, list);
			this.CollectExplodingFieldsForDirection(origin, IntVector2.Up, fields, list);
			this.CollectExplodingFieldsForDirection(origin, IntVector2.Down, fields, list);
			return list;
		}

		// Token: 0x0600275E RID: 10078 RVA: 0x000AEC76 File Offset: 0x000AD076
		private void CollectExplodingFieldsForDirection(IntVector2 pos, IntVector2 direction, Fields fields, List<IntVector2> explodingFields)
		{
			pos += direction;
			while (fields.IsValid(pos))
			{
				this.CollectExplodingField(fields[pos], explodingFields);
				pos += direction;
			}
		}

		// Token: 0x0600275F RID: 10079 RVA: 0x000AECAA File Offset: 0x000AD0AA
		private void CollectExplodingField(Field field, List<IntVector2> explodingFields)
		{
			if (field.isOn && !field.isWindow)
			{
				explodingFields.Add(field.gridPosition);
			}
		}

		// Token: 0x040051E1 RID: 20961
		private List<IMatchResult> results = new List<IMatchResult>();

		// Token: 0x040051E2 RID: 20962
		private readonly GemFactory gemFactory;
	}
}

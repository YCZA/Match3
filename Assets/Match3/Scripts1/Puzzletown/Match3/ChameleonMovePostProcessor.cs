using System.Collections.Generic;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000603 RID: 1539
	public class ChameleonMovePostProcessor : IMatchProcessor
	{
		// Token: 0x06002770 RID: 10096 RVA: 0x000AEFAC File Offset: 0x000AD3AC
		public ChameleonMovePostProcessor(FieldMappings fieldMappings)
		{
			this.fieldMappings = fieldMappings;
		}

		// Token: 0x06002771 RID: 10097 RVA: 0x000AEFD4 File Offset: 0x000AD3D4
		public IEnumerable<IMatchResult> Process(Fields fields, List<IMatchResult> allResults)
		{
			this.results.Clear();
			bool flag = allResults[0] is HammerMatch || allResults[0] is HammerRainbowExplosion || allResults[0] is HammerStarExplosion || allResults[0] is BoostSpawnResult;
			this.GetChameleonGems(fields);
			if (!flag && !this.chameleonsToMove.IsNullOrEmptyCollection())
			{
				List<IMatchResult> collection = this.MoveChameleons(fields);
				if (!collection.IsNullOrEmptyCollection())
				{
					this.results.AddRange(collection);
				}
			}
			return this.results;
		}

		// Token: 0x06002772 RID: 10098 RVA: 0x000AF074 File Offset: 0x000AD474
		private List<IMatchResult> MoveChameleons(Fields fields)
		{
			List<IMatchResult> list = new List<IMatchResult>();
			List<ChameleonSwap> list2 = new List<ChameleonSwap>();
			List<IntVector2> list3 = new List<IntVector2>();
			foreach (Gem gem in this.chameleonsToMove)
			{
				IntVector2 position = gem.position;
				IntVector2 intVector = Gem.GemDirectionToVector(gem.direction);
				IntVector2 intVector2;
				if (intVector == IntVector2.Up)
				{
					intVector2 = this.fieldMappings.Above(position);
				}
				else if (intVector == IntVector2.Down)
				{
					intVector2 = this.fieldMappings.Below(position);
				}
				else
				{
					intVector2 = position + intVector;
				}
				if (this.CanSwapWithTarget(fields, position, intVector2) && !list3.Contains(intVector2))
				{
					ChameleonSwap item = new ChameleonSwap(position, intVector2, intVector);
					list2.Add(item);
					list3.Add(intVector2);
				}
				else
				{
					IntVector2 newFacingDirection = this.GetNewFacingDirection(fields, intVector, position);
					GemDirection gemDirection = Gem.VectorToGemDirection(newFacingDirection);
					fields[position].gem.direction = gemDirection;
					ChameleonTurn chameleonTurn = new ChameleonTurn(gemDirection, position);
					list.Add(chameleonTurn);
				}
			}
			foreach (ChameleonSwap chameleonSwap in list2)
			{
				list.AddRange(this.SwapChameleon(fields, chameleonSwap));
			}
			return list;
		}

		// Token: 0x06002773 RID: 10099 RVA: 0x000AF234 File Offset: 0x000AD634
		private List<IMatchResult> SwapChameleon(Fields fields, ChameleonSwap chameleonSwap)
		{
			IntVector2 origin = chameleonSwap.origin;
			IntVector2 target = chameleonSwap.target;
			IntVector2 facingDirection = chameleonSwap.facingDirection;
			fields.SwapGems(target, origin);
			bool flag = false;
			if (facingDirection == IntVector2.Up)
			{
				flag = (this.fieldMappings.PortalUsedAbove(origin) != 0);
			}
			else if (facingDirection == IntVector2.Down)
			{
				flag = (this.fieldMappings.PortalUsedBelow(origin) != 0);
			}
			if (fields[origin].portalId > 0 && flag)
			{
				return this.GetMovesThroughPortal(fields, origin, facingDirection);
			}
			Move move = new Move(origin, target, true, false, true);
			ChameleonMove chameleonMove = new ChameleonMove(move);
			return new List<IMatchResult>
			{
				chameleonMove
			};
		}

		// Token: 0x06002774 RID: 10100 RVA: 0x000AF2FC File Offset: 0x000AD6FC
		private IntVector2 GetNewFacingDirection(Fields fields, IntVector2 facing, IntVector2 originPosition)
		{
			IntVector2 intVector = new IntVector2(-facing.y, facing.x);
			IntVector2 intVector2 = new IntVector2(facing.y, -facing.x);
			IntVector2 intVector3 = new IntVector2(-facing.x, -facing.y);
			IntVector2 adjacentField = this.GetAdjacentField(fields, originPosition, intVector);
			IntVector2 adjacentField2 = this.GetAdjacentField(fields, originPosition, intVector2);
			IntVector2 adjacentField3 = this.GetAdjacentField(fields, originPosition, intVector3);
			if (adjacentField == Fields.invalidPos && adjacentField2 == Fields.invalidPos && adjacentField3 == Fields.invalidPos)
			{
				return facing;
			}
			if (adjacentField == Fields.invalidPos && adjacentField2 == Fields.invalidPos && adjacentField3 != Fields.invalidPos)
			{
				return intVector3;
			}
			if (adjacentField != Fields.invalidPos && adjacentField2 != Fields.invalidPos)
			{
				bool flag = RandomHelper.Next(2) == 0;
				return (!flag) ? intVector2 : intVector;
			}
			if (adjacentField != Fields.invalidPos)
			{
				return intVector;
			}
			return intVector2;
		}

		// Token: 0x06002775 RID: 10101 RVA: 0x000AF424 File Offset: 0x000AD824
		private IntVector2 GetAdjacentField(Fields fields, IntVector2 origin, IntVector2 direction)
		{
			IntVector2 intVector = origin + direction;
			if (direction == IntVector2.Up && this.fieldMappings.PortalUsedAbove(origin) != 0)
			{
				intVector = this.fieldMappings.Above(origin);
			}
			else if (direction == IntVector2.Down && this.fieldMappings.PortalUsedBelow(origin) != 0)
			{
				intVector = this.fieldMappings.Below(origin);
			}
			else if (!fields.IsValid(intVector))
			{
				return Fields.invalidPos;
			}
			bool flag = fields[intVector].isWindow || fields[intVector].isGrowingWindow;
			if (flag)
			{
				IntVector2 intVector2 = (!(direction == IntVector2.Up)) ? this.fieldMappings.Below(intVector) : this.fieldMappings.Above(intVector);
				intVector = ((!fields.IsValid(intVector2)) ? intVector : intVector2);
			}
			return (!this.CanSwapWithTarget(fields, origin, intVector)) ? Fields.invalidPos : intVector;
		}

		// Token: 0x06002776 RID: 10102 RVA: 0x000AF534 File Offset: 0x000AD934
		private bool CanSwapWithTarget(Fields fields, IntVector2 originPosition, IntVector2 targetPosition)
		{
			if (!fields.IsValid(targetPosition))
			{
				return false;
			}
			Field field = fields[targetPosition];
			return (!(IntVector2.Direction(originPosition, targetPosition) == IntVector2.Up) || !field.NeedsGem) && (field.NeedsGem || (field.isOn && (field.CanSwap || field.gem.IsIced) && !field.gem.IsChameleon));
		}

		// Token: 0x06002777 RID: 10103 RVA: 0x000AF5C0 File Offset: 0x000AD9C0
		private List<IMatchResult> GetMovesThroughPortal(Fields fields, IntVector2 origin, IntVector2 facingDirection)
		{
			IntVector2 intVector = (!(facingDirection == IntVector2.Up)) ? this.fieldMappings.Below(origin) : this.fieldMappings.Above(origin);
			IntVector2 intVector2 = IntVector2.OppositeVector(facingDirection);
			Move move = Move.FromPortal(origin, origin + facingDirection);
			List<IMatchResult> list = new List<IMatchResult>
			{
				move
			};
			if (fields[origin].HasGem)
			{
				Move move2 = Move.FromPortal(intVector, intVector + intVector2);
				list.Add(move2);
				SpawnResult spawnResult = new SpawnResult(origin, fields[origin].gem)
				{
					direction = intVector2
				};
				list.Add(spawnResult);
			}
			ChameleonSpawn item = new ChameleonSpawn(intVector, facingDirection, fields[intVector].gem);
			list.Add(item);
			return list;
		}

		// Token: 0x06002778 RID: 10104 RVA: 0x000AF6A0 File Offset: 0x000ADAA0
		private void GetChameleonGems(Fields fields)
		{
			this.chameleonsToMove.Clear();
			for (int i = 0; i < fields.size; i++)
			{
				for (int j = 0; j < fields.size; j++)
				{
					Field field = fields[i, j];
					if (field.CanExplode && field.CanSwap && field.HasGem && field.gem.IsChameleon && !field.gem.processed)
					{
						this.chameleonsToMove.Add(field.gem);
						field.gem.processed = true;
					}
				}
			}
		}

		// Token: 0x040051EB RID: 20971
		private readonly List<IMatchResult> results = new List<IMatchResult>();

		// Token: 0x040051EC RID: 20972
		private readonly Group chameleonsToMove = new Group();

		// Token: 0x040051ED RID: 20973
		private readonly FieldMappings fieldMappings;
	}
}

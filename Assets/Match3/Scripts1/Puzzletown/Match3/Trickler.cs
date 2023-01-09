using System;
using System.Collections.Generic;
using Match3.Scripts1.Shared.DataStructures;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000632 RID: 1586
	public class Trickler : ITrickler
	{
		// Token: 0x06002848 RID: 10312 RVA: 0x000B3ED8 File Offset: 0x000B22D8
		public Trickler(IGemFactory gemFactory, FieldMappings fieldMappings)
		{
			this.gemFactory = (gemFactory as GemFactory);
			this.mappings = fieldMappings;
			this.visitedPositions = new Map<bool>(9);
		}

		// Token: 0x06002849 RID: 10313 RVA: 0x000B3F0C File Offset: 0x000B230C
		public List<IMatchResult> Trickle(Fields map)
		{
			this.result = new TrickleResult
			{
				Steps = new List<List<IMatchResult>>()
			};
			this.fields = map;
			this.fields.ResetPortals();
			List<IMatchResult> list = null;
			while (list == null || list.Count > 0)
			{
				list = new List<IMatchResult>();
				this.teleported.Clear();
				for (int i = 0; i < this.fields.size; i++)
				{
					for (int j = this.fields.size - 1; j >= 0; j--)
					{
						IntVector2 intVector = new IntVector2(j, i);
						Field field = this.fields[intVector];
						if (this.IsFieldAvailable(field))
						{
							if (field.CanSpawnInTrickling && field.NeedsGem)
							{
								list.Add(this.Spawn(field));
								this.teleported.Add(intVector);
							}
							else
							{
								IntVector2 intVector2 = Fields.invalidPos;
								IntVector2 intVector3 = this.FindTricklingFieldAbove(intVector);
								if (this.fields.IsValid(intVector3))
								{
									IntVector2 intVector4 = this.mappings.Above(intVector);
									if (intVector4 == intVector3 && this.IsReadyToTrickle(intVector4))
									{
										intVector2 = intVector4;
									}
								}
								else
								{
									intVector2 = this.FindTricklingPositionFromSides(intVector);
								}
								if (intVector2 != Fields.invalidPos && this.fields[intVector2].HasGem)
								{
									this.teleported.Add(intVector);
									int num = this.mappings.PortalUsedAbove(field.gridPosition);
									int id = this.mappings.PortalUsedBelow(intVector2);
									if (Portal.IsExit(num) && Portal.IsEntrance(id))
									{
										if (this.fields[intVector2].gem.lastPortalUsed == field.portalId)
										{
											list.Add(this.Spawn(field));
										}
										else
										{
											Move move = Move.FromPortal(intVector2, intVector2 + IntVector2.Down);
											this.fields[intVector2].gem.lastPortalUsed = num;
											this.AddMove(move, list);
											this.fields.SwapGems(intVector2, intVector);
											list.Add(new SpawnResult(intVector, field.gem));
											j = this.fields.size;
											i = 0;
										}
									}
									else
									{
										Move move2 = new Move(intVector2, intVector, false, true, true);
										this.AddMove(move2, list);
										this.fields.SwapGems(intVector2, intVector);
									}
								}
							}
						}
					}
				}
				this.result.Steps.Add(list);
				if (this.result.Steps.Count > 100)
				{
					WoogaDebug.LogError(new object[]
					{
						"catch endless loop",
						this.result.Steps
					});
					throw new InvalidProgramException("caught endless loop");
				}
			}
			return new List<IMatchResult>
			{
				this.result
			};
		}

		// Token: 0x0600284A RID: 10314 RVA: 0x000B4220 File Offset: 0x000B2620
		private SpawnResult Spawn(Field field)
		{
			this.gemFactory.CreateGem(field, this.fields, default(Gem), true);
			return new SpawnResult(field.gridPosition, field.gem);
		}

		// Token: 0x0600284B RID: 10315 RVA: 0x000B425B File Offset: 0x000B265B
		private bool IsFieldAvailable(Field field)
		{
			return field.isOn && !field.isWindow && !field.isGrowingWindow && !field.HasGem && field.CanMove;
		}

		// Token: 0x0600284C RID: 10316 RVA: 0x000B4294 File Offset: 0x000B2694
		private IntVector2 FindTricklingPositionFromSides(IntVector2 currentPos)
		{
			IntVector2 intVector = currentPos + IntVector2.Left;
			IntVector2 intVector2 = currentPos + IntVector2.Right;
			IntVector2 pos = intVector2 + IntVector2.Up;
			IntVector2 pos2 = intVector + IntVector2.Up;
			bool flag = this.fields.IsValid(intVector) && this.fields[intVector].HasGem;
			bool flag2 = this.fields.IsValid(intVector2) && this.fields[intVector2].HasGem;
			bool flag3 = flag || !this.CouldTrickle(intVector);
			bool flag4 = flag2 || !this.CouldTrickle(intVector2);
			if (this.IsReadyToTrickle(pos2) && flag3)
			{
				return pos2;
			}
			if (this.IsReadyToTrickle(pos) && flag4)
			{
				return pos;
			}
			return Fields.invalidPos;
		}

		// Token: 0x0600284D RID: 10317 RVA: 0x000B437C File Offset: 0x000B277C
		private void AddMove(Move move, List<IMatchResult> step)
		{
			if (this.result.Steps.Count > 0)
			{
				List<IMatchResult> list = this.result.Steps[this.result.Steps.Count - 1];
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i] is IFinalMovement)
					{
						IFinalMovement finalMovement = (IFinalMovement)list[i];
						if (finalMovement.Position == move.from)
						{
							finalMovement.IsFinal = false;
							list[i] = finalMovement;
							break;
						}
					}
				}
			}
			step.Add(move);
		}

		// Token: 0x0600284E RID: 10318 RVA: 0x000B442E File Offset: 0x000B282E
		private bool IsReadyToTrickle(IntVector2 pos)
		{
			return this.CanTrickle(pos) && !this.teleported.Contains(pos);
		}

		// Token: 0x0600284F RID: 10319 RVA: 0x000B4450 File Offset: 0x000B2850
		private bool CanTrickle(IntVector2 pos)
		{
			if (!this.CouldTrickle(pos))
			{
				return false;
			}
			Field field = this.fields[pos];
			return field.HasGem || field.CanSpawnInTrickling;
		}

		// Token: 0x06002850 RID: 10320 RVA: 0x000B448C File Offset: 0x000B288C
		private bool CouldTrickle(IntVector2 pos)
		{
			if (!this.fields.IsValid(pos))
			{
				return false;
			}
			Field field = this.fields[pos];
			return field.CanTrickle && field.CanMove;
		}

		// Token: 0x06002851 RID: 10321 RVA: 0x000B44CC File Offset: 0x000B28CC
		private IntVector2 FindTricklingFieldAbove(IntVector2 pos)
		{
			this.visitedPositions.Clear();
			return this.FindTricklingFieldAboveRecursive(pos);
		}

		// Token: 0x06002852 RID: 10322 RVA: 0x000B44E0 File Offset: 0x000B28E0
		private IntVector2 FindTricklingFieldAboveRecursive(IntVector2 pos)
		{
			if (this.visitedPositions[pos])
			{
				return Fields.invalidPos;
			}
			this.visitedPositions[pos] = true;
			if (!this.CouldTrickle(pos))
			{
				return Fields.invalidPos;
			}
			IntVector2 intVector = this.mappings.Above(pos);
			if (!this.CouldTrickle(intVector))
			{
				return Fields.invalidPos;
			}
			if (this.CanTrickle(intVector))
			{
				return intVector;
			}
			IntVector2 intVector2 = this.mappings.Above(intVector);
			if (this.CouldTrickle(intVector2))
			{
				return this.FindTricklingFieldAboveRecursive(intVector);
			}
			if (this.fields.IsValid(intVector2) && (this.fields[intVector2].portalId > 0 || intVector2.y - intVector.y > 1))
			{
				return Fields.invalidPos;
			}
			IntVector2 pos2 = intVector2 + IntVector2.Left;
			IntVector2 a = this.FindTricklingFieldAboveRecursive(pos2);
			if (a != Fields.invalidPos)
			{
				return a;
			}
			IntVector2 pos3 = intVector2 + IntVector2.Right;
			IntVector2 a2 = this.FindTricklingFieldAboveRecursive(pos3);
			if (a2 != Fields.invalidPos)
			{
				return a2;
			}
			return Fields.invalidPos;
		}

		// Token: 0x0400524F RID: 21071
		private TrickleResult result;

		// Token: 0x04005250 RID: 21072
		private GemFactory gemFactory;

		// Token: 0x04005251 RID: 21073
		private Fields fields;

		// Token: 0x04005252 RID: 21074
		private FieldMappings mappings;

		// Token: 0x04005253 RID: 21075
		private List<IntVector2> teleported = new List<IntVector2>();

		// Token: 0x04005254 RID: 21076
		private Map<bool> visitedPositions;
	}
}

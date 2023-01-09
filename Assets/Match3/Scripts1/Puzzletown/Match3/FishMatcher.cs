using System;
using System.Collections.Generic;
using System.Linq;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Puzzletown.Match3.Scoring;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Shared.M3Engine;
using Match3.Scripts1.Wooga.Signals;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020005C0 RID: 1472
	public class FishMatcher : APatternMatcher, ISuperGemRemover, ITournamentMatcher
	{
		// Token: 0x0600264D RID: 9805 RVA: 0x000AB684 File Offset: 0x000A9A84
		public FishMatcher(Fields fields, IScoringController controller, FieldMappings fieldMappings, bool doubleFish = false, string hiddenItemName = "") : base(fields)
		{
			this.scoringController = controller;
			this.mappings = fieldMappings;
			this.hiddenItemName = hiddenItemName;
			this.useDoubleFish = doubleFish;
			this.countTournamentMatches = (controller != null && controller.CurrentOngoingTournament == TournamentType.Butterfly);
		}

		// Token: 0x170005D6 RID: 1494
		// (get) Token: 0x0600264E RID: 9806 RVA: 0x000AB8CE File Offset: 0x000A9CCE
		// (set) Token: 0x0600264F RID: 9807 RVA: 0x000AB8D6 File Offset: 0x000A9CD6
		public bool CountScore
		{
			get
			{
				return this.countTournamentMatches;
			}
			set
			{
				this.countTournamentMatches = value;
			}
		}

		// Token: 0x06002650 RID: 9808 RVA: 0x000AB8DF File Offset: 0x000A9CDF
		protected override void InitPatterns()
		{
			this.patterns = new Pattern[]
			{
				new Pattern(new int[,]
				{
					{
						1,
						1
					},
					{
						1,
						1
					}
				})
			};
		}

		// Token: 0x06002651 RID: 9809 RVA: 0x000AB908 File Offset: 0x000A9D08
		public List<IMatchResult> RemoveSuperGems(Fields fields, List<IMatchResult> results)
		{
			this.takenTargets.Clear();
			this.fields = fields;
			List<IMatchResult> list = new List<IMatchResult>();
			for (int i = 0; i < results.Count; i++)
			{
				IMatchResult matchResult = results[i];
				if (matchResult is FishMatch)
				{
					FishMatch fishMatch = (FishMatch)matchResult;
					if (!fishMatch.exploded)
					{
						this.takenTargets.AddRange(fishMatch.Group.Positions);
						this.ActivateTakenTargets(false);
						fishMatch.exploded = true;
						results[i] = fishMatch;
						if (this.ExplodeFish(fishMatch, list) && this.scoringController is ITrackableScoring)
						{
							((ITrackableScoring)this.scoringController).HandleFishExplosionCreated();
						}
						if (this.useDoubleFish)
						{
							this.ExplodeFish(fishMatch, list);
						}
					}
				}
			}
			this.ActivateTakenTargets(true);
			return list;
		}

		// Token: 0x06002652 RID: 9810 RVA: 0x000AB9EC File Offset: 0x000A9DEC
		protected override void CreateResultsFromGroups(List<Group> groups, List<IMatchResult> matchResults, Move move)
		{
			int i = 0;
			int count = groups.Count;
			while (i < count)
			{
				Group group = groups[i];
				if (group.IsCompletelyCovered())
				{
					matchResults.Add(new Match(group, true));
				}
				else
				{
					this.onFishGemCreated.Dispatch();
					FishMatch fishMatch = new FishMatch(group);
					matchResults.Add(fishMatch);
					if (this.countTournamentMatches)
					{
						Vector3 vector = Vector3.zero;
						for (int j = 0; j < fishMatch.Group.Count; j++)
						{
							vector += (Vector3)fishMatch.Group[j].position;
						}
						vector /= (float)fishMatch.Group.Count;
						matchResults.Add(new TournamentScoreMatch(TournamentType.Butterfly, fishMatch.Group.Positions[0], vector));
					}
				}
				i++;
			}
		}

		// Token: 0x06002653 RID: 9811 RVA: 0x000ABAF0 File Offset: 0x000A9EF0
		private void ActivateTakenTargets(bool isOn)
		{
			for (int i = 0; i < this.takenTargets.Count; i++)
			{
				this.fields[this.takenTargets[i]].isOn = isOn;
			}
		}

		// Token: 0x06002654 RID: 9812 RVA: 0x000ABB38 File Offset: 0x000A9F38
		private bool ExplodeFish(FishMatch match, List<IMatchResult> matchResults)
		{
			IntVector2 fishTarget = this.GetFishTarget(this.scoringController.ObjectivesLeft);
			if (fishTarget == Fields.invalidPos)
			{
				WoogaDebug.LogWarning(new object[]
				{
					"No suitable FishTarget found on board."
				});
				return false;
			}
			matchResults.Add(new JumpResult(match.fishOrigin, fishTarget, match.Group.Color));
			bool spreadWater = WaterProcessor.DoesMatchContainWater(match, this.fields);
			FishExplosion fishExplosion = new FishExplosion(this.fields, fishTarget, spreadWater);
			foreach (Gem gem in fishExplosion.Group)
			{
				Field field = this.fields[gem.position];
				if (field.CanExplode)
				{
					base.HitGem(gem);
				}
			}
			this.takenTargets.Add(fishTarget);
			this.fields[fishTarget].isOn = false;
			matchResults.Add(fishExplosion);
			return true;
		}

		// Token: 0x06002655 RID: 9813 RVA: 0x000ABC60 File Offset: 0x000AA060
		private IntVector2 GetFishTarget(Materials objectives)
		{
			IntVector2 intVector = Fields.invalidPos;
			List<string> list = (from ma in objectives
			where ma.amount > 0
			select ma.type).ToList<string>();
			intVector = this.GetObstacleTargetPosition();
			if (intVector == Fields.invalidPos && list.Any<string>())
			{
				intVector = this.GetObjectiveTargetPosition(list);
			}
			if (intVector == Fields.invalidPos)
			{
				intVector = this.GetRandomGemTargetPosition();
			}
			return intVector;
		}

		// Token: 0x06002656 RID: 9814 RVA: 0x000ABD00 File Offset: 0x000AA100
		private IntVector2 GetObjectiveTargetPosition(List<string> objectivesLeft)
		{
			return this.GetTargetPositionFor(this.FindTargetType(objectivesLeft));
		}

		// Token: 0x06002657 RID: 9815 RVA: 0x000ABD10 File Offset: 0x000AA110
		private IntVector2 GetTargetPositionFor(FishMatcher.TargetWrapper targetType)
		{
			switch (targetType.targetType)
			{
			case FishMatcher.TargetWrapper.TargetType.Color:
				return this.GetRandomColorTargetFor(targetType.targetColor);
			case FishMatcher.TargetWrapper.TargetType.ClimberGem:
				return this.fields.GetRandomModifierPosition(this.IsClimberGem);
			case FishMatcher.TargetWrapper.TargetType.Dirt:
				return this.fields.GetRandomModifierPosition(this.IsCoveredByDirt);
			case FishMatcher.TargetWrapper.TargetType.Tile:
				return this.fields.GetRandomModifierPosition(this.IsTile);
			case FishMatcher.TargetWrapper.TargetType.Water:
				return this.fields.GetRandomModifierPosition(this.NeedsWatering);
			case FishMatcher.TargetWrapper.TargetType.WaterAndClimberGem:
				return this.fields.GetRandomModifierPosition(this.IsUnwateredClimberGem);
			case FishMatcher.TargetWrapper.TargetType.ResistantBlocker:
				return this.fields.GetRandomModifierPosition(this.IsResistantBlocker);
			case FishMatcher.TargetWrapper.TargetType.Chameleon:
				return this.fields.GetRandomModifierPosition(this.IsChameleon);
			default:
				return Fields.invalidPos;
			}
		}

		// Token: 0x06002658 RID: 9816 RVA: 0x000ABDE4 File Offset: 0x000AA1E4
		private IntVector2 GetRandomColorTargetFor(GemColor targetColor)
		{
			IntVector2 intVector = this.fields.GetRandomGemWithColor(targetColor).position;
			if (targetColor == GemColor.Droppable)
			{
				intVector = this.mappings.Below(intVector);
				while (!this.fields.IsValid(intVector) || (!this.fields[intVector].gem.IsMatchable && (!this.fields[intVector].IsBlocked || this.fields[intVector].IsColorWheel)))
				{
					intVector = this.fields.GetRandomGemWithColor(GemColor.Droppable).position;
					intVector = this.mappings.Below(intVector);
				}
			}
			return intVector;
		}

		// Token: 0x06002659 RID: 9817 RVA: 0x000ABE9C File Offset: 0x000AA29C
		private FishMatcher.TargetWrapper FindTargetType(List<string> objectivesLeft)
		{
			if (this.fields.CheckForModifier(this.IsResistantBlocker))
			{
				return new FishMatcher.TargetWrapper
				{
					targetType = FishMatcher.TargetWrapper.TargetType.ResistantBlocker
				};
			}
			if (this.fields.CheckForModifier(this.IsCoveredByDirt))
			{
				return new FishMatcher.TargetWrapper
				{
					targetType = FishMatcher.TargetWrapper.TargetType.Dirt
				};
			}
			if (objectivesLeft.Contains("droppable") && this.CheckForDroppableTarget())
			{
				return new FishMatcher.TargetWrapper
				{
					targetType = FishMatcher.TargetWrapper.TargetType.Color,
					targetColor = GemColor.Droppable
				};
			}
			if ((objectivesLeft.Contains("tiles") || objectivesLeft.Contains(this.hiddenItemName)) && this.fields.CheckForModifier(this.IsTile))
			{
				return new FishMatcher.TargetWrapper
				{
					targetType = FishMatcher.TargetWrapper.TargetType.Tile
				};
			}
			if (objectivesLeft.Contains("climber") && objectivesLeft.Contains("water") && this.fields.CheckForModifier(this.IsUnwateredClimberGem))
			{
				return new FishMatcher.TargetWrapper
				{
					targetType = FishMatcher.TargetWrapper.TargetType.WaterAndClimberGem
				};
			}
			if (objectivesLeft.Contains("climber") && this.fields.CheckForModifier(this.IsClimberGem))
			{
				return new FishMatcher.TargetWrapper
				{
					targetType = FishMatcher.TargetWrapper.TargetType.ClimberGem
				};
			}
			if (objectivesLeft.Contains("water") && this.fields.CheckForModifier(this.NeedsWatering))
			{
				return new FishMatcher.TargetWrapper
				{
					targetType = FishMatcher.TargetWrapper.TargetType.Water
				};
			}
			if (objectivesLeft.Contains("chameleon"))
			{
				return new FishMatcher.TargetWrapper
				{
					targetType = FishMatcher.TargetWrapper.TargetType.Chameleon
				};
			}
			return this.TryPickRandomObjectiveColor(objectivesLeft);
		}

		// Token: 0x0600265A RID: 9818 RVA: 0x000AC040 File Offset: 0x000AA440
		private FishMatcher.TargetWrapper TryPickRandomObjectiveColor(List<string> objectivesLeft)
		{
			List<GemColor> list = this.FindColorsForRandomTargetPick(objectivesLeft);
			if (list.Count > 0)
			{
				int index = RandomHelper.Next(0, list.Count);
				GemColor targetColor = list[index];
				return new FishMatcher.TargetWrapper
				{
					targetType = FishMatcher.TargetWrapper.TargetType.Color,
					targetColor = targetColor
				};
			}
			return new FishMatcher.TargetWrapper
			{
				targetType = FishMatcher.TargetWrapper.TargetType.None
			};
		}

		// Token: 0x0600265B RID: 9819 RVA: 0x000AC09C File Offset: 0x000AA49C
		private List<GemColor> FindColorsForRandomTargetPick(List<string> objectivesLeft)
		{
			List<GemColor> list = new List<GemColor>();
			foreach (string value in objectivesLeft)
			{
				GemColor gemColor;
				try
				{
					gemColor = (GemColor)Enum.Parse(typeof(GemColor), value, true);
				}
				catch (ArgumentException)
				{
					continue;
				}
				if (this.IsColorSuitableAsTarget(gemColor) && this.fields.CheckForColor(gemColor))
				{
					list.Add(gemColor);
				}
			}
			return list;
		}

		// Token: 0x0600265C RID: 9820 RVA: 0x000AC148 File Offset: 0x000AA548
		private void AddIfAvailable(Predicate<Field> predicate, List<Predicate<Field>> availableTargets, Fields fields)
		{
			if (fields.CheckForModifier(predicate))
			{
				availableTargets.Add(predicate);
			}
		}

		// Token: 0x0600265D RID: 9821 RVA: 0x000AC160 File Offset: 0x000AA560
		private IntVector2 GetObstacleTargetPosition()
		{
			IntVector2 result = Fields.invalidPos;
			List<Predicate<Field>> list = new List<Predicate<Field>>();
			this.AddIfAvailable(this.IsStone, list, this.fields);
			this.AddIfAvailable(this.IsChain, list, this.fields);
			this.AddIfAvailable(this.IsCrate, list, this.fields);
			this.AddIfAvailable(this.IsCannonball, list, this.fields);
			this.AddIfAvailable(this.IsIcedGem, list, this.fields);
			this.AddIfAvailable(this.IsGrowingWindow, list, this.fields);
			if (list.Count > 0)
			{
				result = this.fields.GetRandomModifierPosition(list.RandomElement(false));
			}
			return result;
		}

		// Token: 0x0600265E RID: 9822 RVA: 0x000AC20B File Offset: 0x000AA60B
		private IntVector2 GetRandomGemTargetPosition()
		{
			return this.fields.GetRandomModifierPosition(this.IsMatchable);
		}

		// Token: 0x0600265F RID: 9823 RVA: 0x000AC21E File Offset: 0x000AA61E
		private bool IsColorSuitableAsTarget(GemColor color)
		{
			return color != GemColor.Undefined && color != GemColor.Droppable && color != GemColor.Climber && color != GemColor.Rainbow && color != GemColor.Random;
		}

		// Token: 0x06002660 RID: 9824 RVA: 0x000AC248 File Offset: 0x000AA648
		private bool CheckForDroppableTarget()
		{
			for (int i = 0; i < this.fields.size; i++)
			{
				for (int j = 0; j < this.fields.size; j++)
				{
					Field field = this.fields[i, j];
					if (field.isOn && field.gem.color == GemColor.Droppable)
					{
						IntVector2 vec = this.mappings.Below(field.gridPosition);
						if (this.fields.IsValid(vec))
						{
							Field field2 = this.fields[vec];
							if (field2.isOn && field2.CanExplode && (field2.gem.IsMatchable || (field2.IsBlocked && !field2.IsColorWheel)))
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x04005122 RID: 20770
		public readonly Signal onFishGemCreated = new Signal();

		// Token: 0x04005123 RID: 20771
		private readonly IScoringController scoringController;

		// Token: 0x04005124 RID: 20772
		private readonly string hiddenItemName;

		// Token: 0x04005125 RID: 20773
		private readonly List<IntVector2> takenTargets = new List<IntVector2>();

		// Token: 0x04005126 RID: 20774
		private readonly bool useDoubleFish;

		// Token: 0x04005127 RID: 20775
		private readonly FieldMappings mappings;

		// Token: 0x04005128 RID: 20776
		private bool countTournamentMatches;

		// Token: 0x04005129 RID: 20777
		private readonly Predicate<Field> IsStone = (Field f) => f.CanExplode && !f.removedModifier && f.IsStone;

		// Token: 0x0400512A RID: 20778
		private readonly Predicate<Field> IsTile = (Field f) => f.CanExplode && f.numTiles > 0 && f.numTiles <= 2 && !f.IsColorWheel;

		// Token: 0x0400512B RID: 20779
		private readonly Predicate<Field> IsCrate = (Field f) => f.CanExplode && f.HasCrates;

		// Token: 0x0400512C RID: 20780
		private readonly Predicate<Field> IsChain = (Field f) => f.CanExplode && f.numChains > 0;

		// Token: 0x0400512D RID: 20781
		private readonly Predicate<Field> IsCannonball = (Field f) => f.isOn && f.CanExplode && f.gem.color == GemColor.Cannonball;

		// Token: 0x0400512E RID: 20782
		private readonly Predicate<Field> IsResistantBlocker = (Field f) => f.isOn && f.CanExplode && f.IsResistantBlocker;

		// Token: 0x0400512F RID: 20783
		private readonly Predicate<Field> IsMatchable = (Field f) => f.isOn && f.CanExplode && f.gem.IsMatchable;

		// Token: 0x04005130 RID: 20784
		private readonly Predicate<Field> IsUnwateredClimberGem = (Field f) => f.isOn && f.CanExplode && f.gem.type == GemType.ClimberGem && !f.IsWatered;

		// Token: 0x04005131 RID: 20785
		private readonly Predicate<Field> IsClimberGem = (Field f) => f.isOn && f.CanExplode && f.gem.type == GemType.ClimberGem;

		// Token: 0x04005132 RID: 20786
		private readonly Predicate<Field> IsIcedGem = (Field f) => f.isOn && f.CanExplode && f.gem.IsIced;

		// Token: 0x04005133 RID: 20787
		private readonly Predicate<Field> IsCoveredByDirt = (Field f) => f.isOn && f.CanExplode && f.gem.IsCoveredByDirt;

		// Token: 0x04005134 RID: 20788
		private readonly Predicate<Field> NeedsWatering = (Field f) => f.isOn && !f.isWindow && f.CanExplode && !f.IsWatered && !f.IsDefinedGemSpawner && !f.IsColorWheel;

		// Token: 0x04005135 RID: 20789
		private readonly Predicate<Field> IsGrowingWindow = (Field f) => f.isOn && f.isGrowingWindow;

		// Token: 0x04005136 RID: 20790
		private readonly Predicate<Field> IsChameleon = (Field f) => f.isOn && f.HasGem && f.gem.IsChameleon;

		// Token: 0x020005C1 RID: 1473
		public class TargetWrapper
		{
			// Token: 0x04005147 RID: 20807
			public FishMatcher.TargetWrapper.TargetType targetType;

			// Token: 0x04005148 RID: 20808
			public GemColor targetColor;

			// Token: 0x020005C2 RID: 1474
			public enum TargetType
			{
				// Token: 0x0400514A RID: 20810
				None,
				// Token: 0x0400514B RID: 20811
				Color,
				// Token: 0x0400514C RID: 20812
				ClimberGem,
				// Token: 0x0400514D RID: 20813
				Dirt,
				// Token: 0x0400514E RID: 20814
				Tile,
				// Token: 0x0400514F RID: 20815
				Water,
				// Token: 0x04005150 RID: 20816
				WaterAndClimberGem,
				// Token: 0x04005151 RID: 20817
				ResistantBlocker,
				// Token: 0x04005152 RID: 20818
				Chameleon
			}
		}
	}
}

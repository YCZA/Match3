using System.Collections;
using System.Collections.Generic;
using Match3.Scripts1.Shared.M3Engine;
using Shared.Pooling;
using Match3.Scripts1.Wooga.Signals;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020005A3 RID: 1443
	public class LastHurray : MonoBehaviour
	{
		// Token: 0x060025C2 RID: 9666 RVA: 0x000A83DA File Offset: 0x000A67DA
		public Coroutine ApplyLastHurray(int movesLeft, bool speedUp)
		{
			if (speedUp)
			{
				this.SpeedUp();
			}
			if (this.boostFactory == null)
			{
				this.boostFactory = this.loader.BoostFactory;
			}
			return base.StartCoroutine(this.ApplyLastHurrayRoutine(movesLeft));
		}

		// Token: 0x060025C3 RID: 9667 RVA: 0x000A8414 File Offset: 0x000A6814
		private IEnumerator ApplyLastHurrayRoutine(int movesLeft)
		{
			this.isRunning = true;
			this.onStarted.Dispatch();
			BackButtonManager.Instance.SetEnabled(false);
			while (movesLeft > 0)
			{
				yield return new WaitUntil(() => !this.loader.MatchEngine.IsResolvingMoves);
				this.CreateSupergem(this.loader.MatchEngine.Fields);
				movesLeft--;
			}
			yield return new WaitForSeconds(this.LastHurrayBeforeExplosionDelay);
			Gem rainbow = default(Gem);
			Group supergems = Pool<Group>.Get();
			do
			{
				rainbow = this.GetSwappableRainbow(this.loader.MatchEngine.Fields);
				while (!rainbow.Equals(default(Gem)))
				{
					this.ExplodeRainbow(rainbow);
					yield return new WaitUntil(() => !this.loader.MatchEngine.IsResolvingMoves);
					rainbow = this.GetSwappableRainbow(this.loader.MatchEngine.Fields);
				}
				supergems = this.GetAllSwappableSuperGems(this.loader.MatchEngine.Fields);
				if (!supergems.IsNullOrEmptyCollection())
				{
					this.ExplodeSupergems(supergems);
					yield return new WaitUntil(() => !this.loader.MatchEngine.IsResolvingMoves);
				}
			}
			while (!this.EmptyRainbowAndSupergems(rainbow, supergems));
			Pool<Group>.ReturnToPool(supergems);
			yield return new WaitForSeconds(this.LastHurrayEndDelay);
			BackButtonManager.Instance.SetEnabled(true);
			this.isRunning = false;
			yield break;
		}

		// Token: 0x060025C4 RID: 9668 RVA: 0x000A8438 File Offset: 0x000A6838
		private bool EmptyRainbowAndSupergems(Gem rainbow, Group supergems)
		{
			return rainbow.Equals(default(Gem)) && supergems.IsNullOrEmptyCollection();
		}

		// Token: 0x060025C5 RID: 9669 RVA: 0x000A8464 File Offset: 0x000A6864
		private void CreateSupergem(Fields fields)
		{
			IntVector2 replacablePosition = this.GetReplacablePosition(fields);
			if (replacablePosition.Equals(Fields.invalidPos))
			{
				WoogaDebug.Log(new object[]
				{
					"Could not create a supergem no suitable gems to replace left"
				});
				return;
			}
			Boosts type = this.availableCreationBoostTypes[RandomHelper.Next(this.availableCreationBoostTypes.Count)];
			IBoost boost = this.boostFactory.GetBoost(type, replacablePosition);
			this.loader.MatchEngine.ApplyBoost(boost);
		}

		// Token: 0x060025C6 RID: 9670 RVA: 0x000A84DC File Offset: 0x000A68DC
		private void ExplodeRainbow(Gem gem)
		{
			IBoost boost = this.boostFactory.GetBoost(Boosts.boost_lh_exploding_rainbow, gem.position);
			this.loader.MatchEngine.ApplyBoost(boost);
		}

		// Token: 0x060025C7 RID: 9671 RVA: 0x000A8510 File Offset: 0x000A6910
		private void ExplodeSupergems(Group supergems)
		{
			List<IBoost> list = new List<IBoost>();
			foreach (Gem gem in supergems)
			{
				IBoost boost = this.boostFactory.GetBoost(Boosts.boost_lh_exploding_supergem, gem.position);
				list.Add(boost);
			}
			this.loader.MatchEngine.ApplyBoosts(list);
		}

		// Token: 0x060025C8 RID: 9672 RVA: 0x000A8594 File Offset: 0x000A6994
		private Gem GetSwappableRainbow(Fields fields)
		{
			Gem result = default(Gem);
			Group allSwappableRainbows = this.GetAllSwappableRainbows(fields);
			int count = allSwappableRainbows.Count;
			if (count > 0)
			{
				int index = RandomHelper.Next(0, count);
				result = allSwappableRainbows[index];
			}
			return result;
		}

		// Token: 0x060025C9 RID: 9673 RVA: 0x000A85D0 File Offset: 0x000A69D0
		private Group GetAllSwappableSuperGems(Fields fields)
		{
			Group group = new Group();
			int i = 0;
			int size = fields.size;
			while (i < size)
			{
				for (int j = 0; j < size; j++)
				{
					Field field = fields[new IntVector2(i, j)];
					if (field != null && field.isOn && field.CanSwap && field.gem.IsSuperGem)
					{
						group.Add(field.gem);
					}
				}
				i++;
			}
			return group;
		}

		// Token: 0x060025CA RID: 9674 RVA: 0x000A865C File Offset: 0x000A6A5C
		private Group GetAllSwappableRainbows(Fields fields)
		{
			Group group = new Group();
			int i = 0;
			int size = fields.size;
			while (i < size)
			{
				for (int j = 0; j < size; j++)
				{
					Field field = fields[new IntVector2(i, j)];
					if (field != null && field.isOn && field.CanSwap && field.gem.color == GemColor.Rainbow)
					{
						group.Add(field.gem);
					}
				}
				i++;
			}
			return group;
		}

		// Token: 0x060025CB RID: 9675 RVA: 0x000A86EC File Offset: 0x000A6AEC
		private IntVector2 GetReplacablePosition(Fields fields)
		{
			IntVector2 result = Fields.invalidPos;
			List<IntVector2> allReplaceablePositions = this.GetAllReplaceablePositions(fields);
			if (allReplaceablePositions.Count > 0)
			{
				result = allReplaceablePositions[RandomHelper.Next(allReplaceablePositions.Count)];
			}
			return result;
		}

		// Token: 0x060025CC RID: 9676 RVA: 0x000A8728 File Offset: 0x000A6B28
		private List<IntVector2> GetAllReplaceablePositions(Fields fields)
		{
			List<IntVector2> list = new List<IntVector2>();
			int i = 0;
			int size = fields.size;
			while (i < size)
			{
				for (int j = 0; j < size; j++)
				{
					Field field = fields[new IntVector2(i, j)];
					if (field != null && field.isOn && field.CanSwap && field.gem.IsMatchable && field.gem.type == GemType.Undefined)
					{
						list.Add(field.gem.position);
					}
				}
				i++;
			}
			return list;
		}

		// Token: 0x060025CD RID: 9677 RVA: 0x000A87CC File Offset: 0x000A6BCC
		private void SpeedUp()
		{
			this.animationController.speed *= this.speedupFactor;
			this.LastHurrayBeforeExplosionDelay /= this.speedupFactor;
			this.LastHurrayEndDelay /= this.speedupFactor;
			this.isSpedUp = true;
		}

		// Token: 0x060025CE RID: 9678 RVA: 0x000A881E File Offset: 0x000A6C1E
		private void Update()
		{
			if (this.isRunning && !this.isSpedUp && Input.GetButtonDown("Fire1"))
			{
				this.SpeedUp();
			}
		}

		// Token: 0x040050F4 RID: 20724
		public int minLevelWithLastHurray = 10;

		// Token: 0x040050F5 RID: 20725
		[SerializeField]
		private float LastHurrayBeforeExplosionDelay = 0.5f;

		// Token: 0x040050F6 RID: 20726
		[SerializeField]
		private float LastHurrayEndDelay = 1f;

		// Token: 0x040050F7 RID: 20727
		[SerializeField]
		private LevelLoader loader;

		// Token: 0x040050F8 RID: 20728
		[SerializeField]
		private float speedupFactor = 10f;

		// Token: 0x040050F9 RID: 20729
		[SerializeField]
		[AutoSet]
		private BoardAnimationController animationController;

		// Token: 0x040050FA RID: 20730
		private BoostFactory boostFactory;

		// Token: 0x040050FB RID: 20731
		private bool isRunning;

		// Token: 0x040050FC RID: 20732
		private bool isSpedUp;

		// Token: 0x040050FD RID: 20733
		public readonly Signal onStarted = new Signal();

		// Token: 0x040050FE RID: 20734
		private List<Boosts> availableCreationBoostTypes = new List<Boosts>
		{
			Boosts.boost_lh_creating_line_horizontal,
			Boosts.boost_lh_creating_line_vertical
		};
	}
}

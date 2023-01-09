using System.Collections;
using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Match3.Scoring;
using Match3.Scripts1.Shared.Flows;
using Wooga.Coroutines;
using Wooga.UnityFramework;

namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000577 RID: 1399
	public class M3_LevelEditorPlayModeFlow : AFlow<PlayModeFlowParameters>
	{
		// Token: 0x060024CB RID: 9419 RVA: 0x000A46A0 File Offset: 0x000A2AA0
		protected override IEnumerator FlowRoutine(PlayModeFlowParameters input)
		{
			input.config.IsEditMode = true;
			input.config.UpdateObjectives();
			this.EnsureGems(input.config);
			LevelRootInput levelRootInput = new LevelRootInput(input.config, LevelPlayMode.Regular);
			Wooroutine<M3_LevelRoot> scene = SceneManager.Instance.LoadSceneWithParams<M3_LevelRoot, LevelRootInput>(levelRootInput, null);
			yield return scene;
			yield return scene.ReturnValue.onCompleted;
			Match3Score score = scene.ReturnValue.onCompleted.Dispatched;
			int coins = score.amounts["coins"];
			int movesNeeded = score.Config.data.moves - score.movesLeft;
			int cascades = score.levelCascades;
			int reshuffles = score.reshuffles;
			WoogaDebug.LogWarningFormatted("Level {0} Finished. Moves Needed: {1}, Coins: {2}", new object[]
			{
				input.levelName,
				movesNeeded,
				coins
			});
			WoogaDebug.LogWarningFormatted("Level {0} Finished. Cascades/Moves: {1}, Reshuffles/Moves: {2}", new object[]
			{
				input.levelName,
				(float)cascades / (float)movesNeeded,
				(float)reshuffles / (float)movesNeeded
			});
			Wooroutine<M3_LevelEditorRoot> editorScene = SceneManager.Instance.LoadSceneWithParams<M3_LevelEditorRoot, string>(input.currentPath, null);
			yield return editorScene;
			yield break;
		}

		// Token: 0x060024CC RID: 9420 RVA: 0x000A46C4 File Offset: 0x000A2AC4
		private void EnsureGems(LevelConfig config)
		{
			if (config.data.gems == null)
			{
				List<GemColor> list = new List<GemColor>();
				foreach (GemColor gemColor in config.layout.gemColors)
				{
					if (gemColor != GemColor.Undefined)
					{
						list.AddIfNotAlreadyPresent(gemColor, false);
					}
				}
				config.data.gems = new MaterialAmount[list.Count];
				for (int j = 0; j < list.Count; j++)
				{
					config.data.gems[j] = new MaterialAmount(list[j].ToString(), 1, MaterialAmountUsage.Undefined, 0);
				}
			}
		}

		// Token: 0x04005060 RID: 20576
		private const string AUTO_RUN_STATS_LEVEL_MOVES_COINS = "Level {0} Finished. Moves Needed: {1}, Coins: {2}";

		// Token: 0x04005061 RID: 20577
		private const string AUTO_RUN_STATS_LEVEL_CASCADE_PER_MOVES_RESHUFFLES_PER_MOVE = "Level {0} Finished. Cascades/Moves: {1}, Reshuffles/Moves: {2}";
	}
}

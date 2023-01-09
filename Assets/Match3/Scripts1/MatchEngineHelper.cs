using System;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Puzzletown.Match3.Scoring;

// Token: 0x020006C6 RID: 1734
namespace Match3.Scripts1
{
	public static class MatchEngineHelper
	{
		// Token: 0x06002B3F RID: 11071 RVA: 0x000C58E4 File Offset: 0x000C3CE4
		public static void AddProcessors(PTMatchEngine matchEngine, APTBoardFactory boardFactory, LevelConfig config, IScoringController scoringController)
		{
			matchEngine.AddTrickler(new Trickler(boardFactory.GemFactory, matchEngine.FieldMappings));
			bool hasGrowingWindows = config.layout.HasGrowingWindows;
			bool flag = config.data.ChameleonCount > 0;
			matchEngine.AddSwapProcessor(new ClimberSwapProcessor(matchEngine.FieldMappings));
			matchEngine.AddProcessor(new ChainProcessor());
			matchEngine.AddProcessor(new IceProcessor());
			matchEngine.AddProcessor(new DirtProcessor());
			matchEngine.AddProcessor(new CannonballProcessor());
			matchEngine.AddProcessor(new CrateProcessor());
			matchEngine.AddProcessor(new StoneProcessor());
			matchEngine.AddProcessor(new ResistantBlockerProcessor());
			if (hasGrowingWindows)
			{
				matchEngine.AddProcessor(new GrowingWindowProcessor(matchEngine.FieldMappings, matchEngine.SpawnerMappings));
			}
			matchEngine.AddProcessor(new TileProcessor());
			matchEngine.AddProcessor(new ClimberGemProcessor());
			matchEngine.AddProcessor(new DefinedSpawnExplosionProcessor());
			matchEngine.AddProcessor(new HiddenItemProcessor(config.layout));
			matchEngine.AddProcessor(new CannonChargeProcessor());
			matchEngine.AddPostProcessor(new ColorWheelProcessor());
			matchEngine.AddPostProcessor(new StackedGemProcessor());
			matchEngine.AddPostProcessor(new WaterProcessor());
			matchEngine.AddPostProcessor(new ChameleonProcessor(boardFactory.GemFactory));
			matchEngine.AddPostTricklingProcessor(new ClimberProcessor(matchEngine.FieldMappings, boardFactory.GemFactory));
			matchEngine.AddPostTricklingProcessor(new CannonExplosionProcessor(boardFactory.GemFactory));
			matchEngine.AddPostTricklingProcessor(new DefinedSpawnProcessor(boardFactory));
			if (flag)
			{
				matchEngine.AddPostTricklingProcessor(new ChameleonMovePostProcessor(matchEngine.FieldMappings));
			}
			if (hasGrowingWindows)
			{
				matchEngine.AddTrigger(new GrowingWindowSpawnTrigger(matchEngine.FieldMappings, matchEngine.SpawnerMappings));
			}
			matchEngine.AddMatcher(new DroppableMatcher(boardFactory.Fields));
			matchEngine.AddMatcher(new ClimberMatcher(boardFactory.Fields, matchEngine.FieldMappings));
			matchEngine.AddMatcher(new BombInstantMatcher(boardFactory.Fields));
			matchEngine.AddMatcher(new ColorWheelMatcher(boardFactory.Fields));
			RainbowMatcher rainbowMatcher = new RainbowMatcher(boardFactory.Fields);
			matchEngine.AddMatcher(rainbowMatcher);
			bool countTournamentMatches = scoringController.CurrentOngoingTournament == TournamentType.Bomb;
			BombMatcher bombMatcher = new BombMatcher(boardFactory.Fields, countTournamentMatches);
			matchEngine.AddMatcher(bombMatcher);
			countTournamentMatches = (scoringController.CurrentOngoingTournament == TournamentType.Line);
			LineGemMatcher lineGemMatcher = new LineGemMatcher(boardFactory.Fields, countTournamentMatches);
			matchEngine.AddMatcher(lineGemMatcher);
			FishMatcher fishMatcher = new FishMatcher(boardFactory.Fields, scoringController, matchEngine.FieldMappings, config.preBoostConfig.UseDoubleFish, config.data.hiddenItemName);
			matchEngine.AddMatcher(fishMatcher);
			matchEngine.AddMatcher(new Count3Matcher(boardFactory.Fields));
			matchEngine.AddMatcher(new LineGemsCombinedMatcher(boardFactory.Fields));
			matchEngine.AddMatcher(new LineGemBombMatcher(boardFactory.Fields));
			matchEngine.AddMatcher(new LineGemRainbowMatcher(boardFactory.Fields));
			matchEngine.AddMatcher(new RainbowBombMatcher(boardFactory.Fields));
			matchEngine.AddMatcher(new BombSwapMatcher(boardFactory.Fields));
			RainbowExplosionMatcher rainbowExplosionMatcher = new RainbowExplosionMatcher(boardFactory.Fields);
			matchEngine.AddMatcher(rainbowExplosionMatcher);
			if (scoringController is ITrackableScoring)
			{
				ITrackableScoring @object = (ITrackableScoring)scoringController;
				rainbowExplosionMatcher.onRainbowExplosionCreated.AddListener(new Action(@object.HandleRainbowExplosionCreated));
				lineGemMatcher.onLineGemExplosionCreated.AddListener(new Action(@object.HandleLineGemExplosionCreated));
				lineGemMatcher.onLineGemCreated.AddListener(new Action(@object.HandleLineGemCreated));
				bombMatcher.onBombGemCreated.AddListener(new Action(@object.HandleBombCreated));
				fishMatcher.onFishGemCreated.AddListener(new Action(@object.HandleFishCreated));
				rainbowMatcher.onRainbowGemCreated.AddListener(new Action(@object.HandleRainbowCreated));
			}
		}
	}
}

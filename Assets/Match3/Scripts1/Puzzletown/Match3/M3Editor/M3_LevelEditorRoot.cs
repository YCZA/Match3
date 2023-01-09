using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Wooga.UnityFramework;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000574 RID: 1396
	[LoadOptions(false, false, false)]
	public class M3_LevelEditorRoot : APtSceneRoot<string>
	{
		// Token: 0x17000583 RID: 1411
		// (get) Token: 0x0600249D RID: 9373 RVA: 0x000A3386 File Offset: 0x000A1786
		public LevelConfig CurrentConfig
		{
			get
			{
				this._currentConfig.layout = new LevelLayout(this.loader.Fields);
				return this._currentConfig;
			}
		}

		// Token: 0x17000584 RID: 1412
		// (get) Token: 0x0600249E RID: 9374 RVA: 0x000A33A9 File Offset: 0x000A17A9
		protected override bool IsSetup
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600249F RID: 9375 RVA: 0x000A33AC File Offset: 0x000A17AC
		private void OnApplicationQuit()
		{
			this.levelsToCheck = null;
		}

		// Token: 0x060024A0 RID: 9376 RVA: 0x000A33B8 File Offset: 0x000A17B8
		public bool IsLevelConfigReady()
		{
			return this.loader != null && this.loader.Fields != null && this._currentConfig != null && this._currentConfig.layout != null;
		}

		// Token: 0x060024A1 RID: 9377 RVA: 0x000A3408 File Offset: 0x000A1808
		protected override void Go()
		{
			Time.timeScale = 1f;
			this.loader = base.GetComponentInChildren<LevelLoaderEditor>();
			this.brushes = base.GetComponent<Brushes>();
			if (!string.IsNullOrEmpty(this.parameters))
			{
				this.filePath = this.parameters;
			}
			if (string.IsNullOrEmpty(this.filePath))
			{
				this.filePath = FieldSerializer.DEFAULT_FIELDS_PATH;
				WoogaDebug.LogWarning(new object[]
				{
					"File Path is empty using default fields file."
				});
			}
			this.LoadFromPath(this.filePath);
			this.StoreFirstUndo();
			if (this.filePath.EndsWith("(TMP)"))
			{
				File.Delete(this.filePath);
				File.Delete(this.filePath + ".meta");
				this.filePath = this.filePath.TrimEnd("(TMP)".ToCharArray());
			}
			this.buttonPlay.onClick.AddListener(new UnityAction(this.HandleButtonPlay));
			this.buttonAutoPlay.onClick.AddListener(new UnityAction(this.HandleButtonAutoPlay));
			this.buttonOpenTestSaving.onClick.AddListener(new UnityAction(this.HandleOpenTestSaving));
			this.buttonOpenCascadeBalancer.onClick.AddListener(new UnityAction(this.HandleOpenCascadeBalancer));
			this.buttonBalanceCascades.onClick.AddListener(new UnityAction(this.HandleBalanceCascades));
			this.buttonSaveTest.onClick.AddListener(new UnityAction(this.HandleButtonSaveTest));
			this.buttonConfirmSaveState.onClick.AddListener(new UnityAction(this.OnConfirmSaveState));
			this.buttonCancelSaveState.onClick.AddListener(new UnityAction(this.OnCancelSaveState));
			this.buttonSaveState.onClick.AddListener(new UnityAction(this.AskSaveState));
			this.buttonSaveLevel.onClick.AddListener(new UnityAction(this.SaveLevelToPath));
			this.buttonDiscard.onClick.AddListener(new UnityAction(this.DiscardChanges));
			this.levelGenerator.levelEditor = this.levelEditor;
			this.levelGenerator.levelConfig = this.CurrentConfig;
			this.layoutModifier.levelEditor = this.levelEditor;
			this.layoutModifier.levelLoader = this.loader;
			// this.ShowConfirmSaveState(false);
		}

		// Token: 0x060024A2 RID: 9378 RVA: 0x000A3667 File Offset: 0x000A1A67
		private void OnConfirmSaveState()
		{
			this.ShowConfirmSaveState(false);
			this.SaveToPath();
		}

		// Token: 0x060024A3 RID: 9379 RVA: 0x000A3676 File Offset: 0x000A1A76
		private void OnCancelSaveState()
		{
			this.ShowConfirmSaveState(false);
		}

		// Token: 0x060024A4 RID: 9380 RVA: 0x000A367F File Offset: 0x000A1A7F
		private void AskSaveState()
		{
			this.ShowConfirmSaveState(true);
		}

		// Token: 0x060024A5 RID: 9381 RVA: 0x000A3688 File Offset: 0x000A1A88
		private void ShowConfirmSaveState(bool isOn)
		{
			this.buttonSaveState.gameObject.SetActive(!isOn);
			this.buttonConfirmSaveState.gameObject.SetActive(isOn);
			this.buttonCancelSaveState.gameObject.SetActive(isOn);
		}

		// 保存关卡槽位
		public void SaveToPath()
		{
			FieldSerializer.SaveToDisk(this.filePath, this.loader.Fields);
		}

		// 保存关卡配置
		private void SaveLevelToPath()
		{
			FieldSerializer.SaveToDisk(this.filePath, this.CurrentConfig);
		}

		// Token: 0x060024A8 RID: 9384 RVA: 0x000A36EB File Offset: 0x000A1AEB
		private void StoreFirstUndo()
		{
			this.levelSaves[0] = FieldSerializer.ConfigToJson(this.CurrentConfig);
		}

		// Token: 0x060024A9 RID: 9385 RVA: 0x000A3704 File Offset: 0x000A1B04
		private void StoreUndo()
		{
			this.editId++;
			this.highestEditId = this.editId;
			this.levelSaves[this.editId] = FieldSerializer.ConfigToJson(this.CurrentConfig);
		}

		// Token: 0x060024AA RID: 9386 RVA: 0x000A373C File Offset: 0x000A1B3C
		private void ReloadRedo()
		{
			if (this.editId >= this.highestEditId)
			{
				return;
			}
			this.editId++;
			this.LoadFromEditId(this.editId);
		}

		// Token: 0x060024AB RID: 9387 RVA: 0x000A376A File Offset: 0x000A1B6A
		private void ReloadUndo()
		{
			if (this.editId == 0)
			{
				return;
			}
			this.editId--;
			this.LoadFromEditId(this.editId);
		}

		// Token: 0x060024AC RID: 9388 RVA: 0x000A3794 File Offset: 0x000A1B94
		private void LoadFromEditId(int editId)
		{
			if (this.levelSaves.ContainsKey(editId))
			{
				string json = this.levelSaves[editId];
				this.LoadFromJson(json);
			}
			else
			{
				global::UnityEngine.Debug.LogWarning("Cannot find save for editId: " + editId);
			}
		}

		// Token: 0x060024AD RID: 9389 RVA: 0x000A37E0 File Offset: 0x000A1BE0
		private void ResetUndo()
		{
			this.editId = 0;
			this.lastSaveEditId = 0;
			this.highestEditId = 0;
		}

		// Token: 0x060024AE RID: 9390 RVA: 0x000A37F7 File Offset: 0x000A1BF7
		private void DiscardChanges()
		{
			this.LoadFromPath(this.filePath);
			this.editId = this.lastSaveEditId;
		}

		// Token: 0x060024AF RID: 9391 RVA: 0x000A3814 File Offset: 0x000A1C14
		private void LoadFromPath(string fileName)
		{
			string json = File.ReadAllText(fileName);
			this.LoadFromJson(json);
		}

		// Token: 0x060024B0 RID: 9392 RVA: 0x000A3830 File Offset: 0x000A1C30
		private void LoadFromJson(string json)
		{
			LevelConfig levelConfig = FieldSerializer.LoadLevelFromJson(json);
			Fields fields = (levelConfig.layout.fields != null) ? null : FieldSerializer.LoadFieldsFromJson(json);
			this.LoadFromConfig(levelConfig, fields);
		}

		// Token: 0x060024B1 RID: 9393 RVA: 0x000A386C File Offset: 0x000A1C6C
		private void LoadFromConfig(LevelConfig config, Fields fields)
		{
			this._currentConfig = config;
			// PTEditorBoardFactory pteditorBoardFactory;
			PTBoardFactory pteditorBoardFactory;
			if (this._currentConfig.layout.fields != null)
			{
				// pteditorBoardFactory = new PTEditorBoardFactory(this._currentConfig);
				pteditorBoardFactory = new PTBoardFactory(this._currentConfig);
			}
			else
			{
				// pteditorBoardFactory = new PTEditorBoardFactory();
				pteditorBoardFactory = new PTBoardFactory();
				pteditorBoardFactory.LoadBoard(fields);
			}
			this.loader.LoadBoard(pteditorBoardFactory.Fields);
			if (this.levelEditor != null)
			{
				this.levelEditor.fields = this.loader.Fields;
			}
			else
			{
				this.levelEditor = new LevelEditor(this.loader.Fields, this.brushes, this.boardview, this.loader.viewExtension);
				this.levelEditor.OnChanged.AddListener(new Action(this.LevelChanged));
				this.StoreUndo();
			}
		}

		// Token: 0x060024B2 RID: 9394 RVA: 0x000A3939 File Offset: 0x000A1D39
		private void LevelChanged()
		{
			this.StoreUndo();
		}

		// Token: 0x060024B3 RID: 9395 RVA: 0x000A3941 File Offset: 0x000A1D41
		public void TryRunBatchRoutine()
		{
		}

		// Token: 0x060024B4 RID: 9396 RVA: 0x000A3944 File Offset: 0x000A1D44
		private IEnumerator RunBatchRoutineWithProgressBar(List<string> filePaths, string progressBarTitle, M3_LevelEditorRoot.BatchRoutine batchRoutine)
		{
			float sumTime = 0f;
			for (int i = 0; i < filePaths.Count; i++)
			{
				float startTime = Time.time;
				this.UpdateProgressBar(i, filePaths.Count, sumTime, progressBarTitle);
				yield return batchRoutine(filePaths[i]);
				sumTime += Time.time - startTime;
				this.UpdateProgressBar(i, filePaths.Count, sumTime, progressBarTitle);
			}
			this.ClearProgressBar();
			yield break;
		}

		// Token: 0x060024B5 RID: 9397 RVA: 0x000A3974 File Offset: 0x000A1D74
		private IEnumerator CheckBatchCascadeBalancerRoutine(List<string> filePaths)
		{
			this.WriteResultsHeaderToFile("Assets/Editor Default Resources/levels_to_batch_check_cascade_balance_results.csv", "level,cascades/Move,reshuffles/Move,cascades/Move\n");
			yield return this.RunBatchRoutineWithProgressBar(filePaths, "Batch-run cascade balancer", new M3_LevelEditorRoot.BatchRoutine(this.CheckCascadeBalancerRoutine));
			WoogaDebug.Log(new object[]
			{
				"SUCCESSFULLY FINISHED RUNNING BATCH-RUN CASCADE BALANCER"
			});
			yield break;
		}

		// Token: 0x060024B6 RID: 9398 RVA: 0x000A3998 File Offset: 0x000A1D98
		private IEnumerator CheckCascadeBalancerRoutine(string path)
		{
			this.ResetUndo();
			this.filePath = path;
			this.LoadFromPath(this.filePath);
			string alternateLevelString = (!this.filePath.Contains(ALTERNATE_LEVEL_VERSION)) ? string.Empty : ALTERNATE_LEVEL_VERSION;
			string levelName = Path.GetFileNameWithoutExtension(this.filePath);
			yield return this.cascadeBalancer.RunLevelRoutine(levelName, this._currentConfig, 70, 100);
			string result = string.Format("{0}{1},{2},{3},{4}\n", new object[]
			{
				levelName,
				alternateLevelString,
				this.cascadeBalancer.MeanCascadesPerMove,
				this.cascadeBalancer.MeanReshufflesPerMove,
				this.cascadeBalancer.MeanMoves
			});
			this.WriteResultsToFile("Assets/Editor Default Resources/levels_to_batch_check_cascade_balance_results.csv", result);
			yield return null;
			yield break;
		}

		// Token: 0x060024B7 RID: 9399 RVA: 0x000A39BC File Offset: 0x000A1DBC
		private void WriteResultsToFile(string filePath, string result)
		{
			using (StreamWriter streamWriter = new StreamWriter(filePath, true))
			{
				streamWriter.Write(result);
			}
		}

		// Token: 0x060024B8 RID: 9400 RVA: 0x000A39FC File Offset: 0x000A1DFC
		private void WriteResultsHeaderToFile(string filePath, string header)
		{
			using (StreamWriter streamWriter = new StreamWriter(filePath, false))
			{
				streamWriter.Write(header);
			}
		}

		// Token: 0x060024B9 RID: 9401 RVA: 0x000A3A3C File Offset: 0x000A1E3C
		private IEnumerator CheckMirroredLevelsRoutine(List<string> filePaths)
		{
			this.mirroredLevelsResults = "Levels that shouldn’t be mirrored:";
			this.WriteResultsHeaderToFile("Assets/Editor Default Resources/mirrored_levels_results.csv", "level,can be mirrored\n");
			yield return this.RunBatchRoutineWithProgressBar(filePaths, "Check if levels can be mirrored", new M3_LevelEditorRoot.BatchRoutine(this.CheckMirroredLevelRoutine));
			WoogaDebug.LogWarning(new object[]
			{
				this.mirroredLevelsResults
			});
			yield break;
		}

		// Token: 0x060024BA RID: 9402 RVA: 0x000A3A60 File Offset: 0x000A1E60
		private IEnumerator CheckMirroredLevelRoutine(string path)
		{
			this.ResetUndo();
			this.filePath = path;
			this.LoadFromPath(this.filePath);
			string alternateLevelString = (!this.filePath.Contains(ALTERNATE_LEVEL_VERSION)) ? string.Empty : ALTERNATE_LEVEL_VERSION;
			
			string levelName = Path.GetFileNameWithoutExtension(this.filePath);
			yield return this.cascadeBalancer.RunLevelRoutine(levelName, this._currentConfig, 20, 100);
			float meanMovesOld = this.cascadeBalancer.MeanMoves;
			string mirroredPath = string.Concat(new object[]
			{
				Path.GetDirectoryName(this.filePath),
				Path.DirectorySeparatorChar,
				Path.GetFileNameWithoutExtension(this.filePath),
				"-mirrored.json"
			});
			LayoutModifierView layoutModifierView = this.layoutModifier;
			if (M3_LevelEditorRoot._003C_003Ef__mg_0024cache0 == null)
			{
				M3_LevelEditorRoot._003C_003Ef__mg_0024cache0 = new ReflectedPosition(LayoutGenerator.HorizontallyReflectedPos);
			}
			layoutModifierView.ApplyModification(M3_LevelEditorRoot._003C_003Ef__mg_0024cache0);
			this.filePath = mirroredPath;
			this.SaveLevelToPath();
			this.LoadFromPath(this.filePath);
			string mirroredLevelName = Path.GetFileNameWithoutExtension(this.filePath);
			yield return this.cascadeBalancer.RunLevelRoutine(mirroredLevelName, this._currentConfig, 20, 100);
			float meanMovesNew = this.cascadeBalancer.MeanMoves;
			float difference = meanMovesOld - meanMovesNew;
			bool shouldNotMirror = Mathf.Abs(difference) > 15f;
			string formattedMessage = (!shouldNotMirror) ? "Can mirror level {0}" : "Can not mirror level {0}";
			levelName += alternateLevelString;
			if (shouldNotMirror)
			{
				string str = string.Format("\n {0} ", levelName);
				this.mirroredLevelsResults += str;
			}
			WoogaDebug.LogFormatted(formattedMessage, new object[]
			{
				levelName
			});
			this.WriteResultsToFile("Assets/Editor Default Resources/mirrored_levels_results.csv", string.Format("{0},{1}\n", levelName, (!shouldNotMirror) ? "YES" : "NO"));
			File.Delete(this.filePath);
			yield return null;
			yield break;
		}

		// Token: 0x060024BB RID: 9403 RVA: 0x000A3A82 File Offset: 0x000A1E82
		private void UpdateProgressBar(int currentRunIndex, int totalRuns, float sumTime, string title)
		{
		}

		// Token: 0x060024BC RID: 9404 RVA: 0x000A3A84 File Offset: 0x000A1E84
		private void ClearProgressBar()
		{
		}

		// Token: 0x060024BD RID: 9405 RVA: 0x000A3A88 File Offset: 0x000A1E88
		private void HandleButtonPlay()
		{
			this.parameters = this.filePath + "(TMP)";
			FieldSerializer.SaveToDisk(this.filePath + "(TMP)", this.CurrentConfig);
			Regex regex = new Regex("\\d{4}");
			System.Text.RegularExpressions.Match match = regex.Match(this.filePath);
			int num = (!int.TryParse(match.Value, out num)) ? 214 : num;
			AreaConfig levelCollectionConfig = new AreaConfig(num);
			this.CurrentConfig.LevelCollectionConfig = levelCollectionConfig;
			new M3_LevelEditorPlayModeFlow().Start(new PlayModeFlowParameters(this.parameters, Path.GetFileNameWithoutExtension(this.filePath), this.CurrentConfig));
		}

		// eli key point: autoPlay
		// Token: 0x060024BE RID: 9406 RVA: 0x000A3B36 File Offset: 0x000A1F36
		private void HandleButtonAutoPlay()
		{
			this.CurrentConfig.IsAutoPlayMode = true;
			Time.timeScale = AUTO_PLAY_TIME_SCALE;
			this.HandleButtonPlay();
		}

		// Token: 0x060024BF RID: 9407 RVA: 0x000A3B54 File Offset: 0x000A1F54
		private void HandleOpenTestSaving()
		{
			this.panelTestSaving.SetActive(!this.panelTestSaving.activeSelf);
		}

		// Token: 0x060024C0 RID: 9408 RVA: 0x000A3B6F File Offset: 0x000A1F6F
		private void HandleOpenCascadeBalancer()
		{
			this.panelCascadeBalancer.SetActive(!this.panelCascadeBalancer.activeSelf);
		}

		// Token: 0x060024C1 RID: 9409 RVA: 0x000A3B8C File Offset: 0x000A1F8C
		private void HandleBalanceCascades()
		{
			int times = 0;
			if (!int.TryParse(this.inputTimes.text, out times))
			{
				WoogaDebug.LogFormatted("Times was set to \"{0}\" using fallback of {1} times", new object[]
				{
					this.inputTimes.text,
					70
				});
				times = 70;
			}
			int movesLimit = 0;
			if (!int.TryParse(this.inputMoves.text, out movesLimit))
			{
				WoogaDebug.LogFormatted("Moves Limit was set to \"{0}\" using fallback of {1} moves", new object[]
				{
					this.inputMoves.text,
					100
				});
				movesLimit = 100;
			}
			if (this.toggleAutoColorAdjustment.isOn)
			{
				int stepSize = 0;
				if (!int.TryParse(this.inputStepSize.text, out stepSize))
				{
					WoogaDebug.LogFormatted("Step size was set to \"{0}\" using fallback of {1}", new object[]
					{
						this.inputStepSize.text,
						2
					});
					stepSize = 2;
				}
				this.cascadeBalancer.BalanceLevel(this.CurrentConfig, this.filePath, times, movesLimit, stepSize);
			}
			else
			{
				this.cascadeBalancer.RunLevel(Path.GetFileNameWithoutExtension(this.filePath), this.CurrentConfig, times, movesLimit);
			}
		}

		// Token: 0x060024C2 RID: 9410 RVA: 0x000A3CB0 File Offset: 0x000A20B0
		private void HandleButtonSaveTest()
		{
			if (string.IsNullOrEmpty(this.testName.text))
			{
				WoogaDebug.LogError(new object[]
				{
					"Please provide a test name"
				});
				return;
			}
			if (!this.CheckMoveInput(this.InputFromX) || !this.CheckMoveInput(this.InputFromY) || !this.CheckMoveInput(this.InputToX) || !this.CheckMoveInput(this.InputToX))
			{
				WoogaDebug.LogError(new object[]
				{
					"Please provide a all data for a move"
				});
				return;
			}
			string testInputPath = this.GetTestInputPath(this.testName.text, Constants.FILE_NAME_CONFIG);
			string testInputPath2 = this.GetTestInputPath(this.testName.text, Constants.FILE_NAME_EXPECTED_FIELDS);
			FieldSerializer.SaveToDisk(testInputPath, this.CurrentConfig);
			FieldSerializer.SaveToDisk(testInputPath2, this.loader.Fields);
			this.filePath = testInputPath2;
		}

		// Token: 0x060024C3 RID: 9411 RVA: 0x000A3D98 File Offset: 0x000A2198
		private void Update()
		{
			if (global::UnityEngine.Input.GetKeyDown(KeyCode.Space))
			{
				this.HandleButtonPlay();
			}
			if (global::UnityEngine.Input.GetKey(KeyCode.LeftShift) && global::UnityEngine.Input.GetKeyDown(KeyCode.Z))
			{
				this.ReloadUndo();
			}
			if (global::UnityEngine.Input.GetKey(KeyCode.LeftShift) && global::UnityEngine.Input.GetKeyDown(KeyCode.X))
			{
				this.ReloadRedo();
			}
		}

		// Token: 0x060024C4 RID: 9412 RVA: 0x000A3DF9 File Offset: 0x000A21F9
		private bool CheckMoveInput(InputField input)
		{
			if (string.IsNullOrEmpty(input.text))
			{
				WoogaDebug.LogError(new object[]
				{
					"Input is empty for",
					input.name
				});
				return false;
			}
			return true;
		}

		// Token: 0x060024C5 RID: 9413 RVA: 0x000A3E2A File Offset: 0x000A222A
		private string GetTestInputPath(string testName, string fileName)
		{
			return string.Concat(new string[]
			{
				Application.dataPath,
				Constants.PATH_TEST_INPUT,
				testName,
				"/",
				fileName
			});
		}

		// Token: 0x0400501F RID: 20511
		public List<string> levelsToCheck;

		// Token: 0x04005020 RID: 20512
		public const string BATCH_RUN_MODE_FLAG = "BATCH_RUN_MODE_FLAG";

		// Token: 0x04005021 RID: 20513
		public const string PATH_JSON_FIELDS = "PATH_JSON_FIELDS";

		// Token: 0x04005022 RID: 20514
		private const string TEMP_PATH_NAME_EXTENSION = "(TMP)";

		// Token: 0x04005023 RID: 20515
		private const string MIRRORED_PATH_NAME_EXTENSION = "-mirrored.json";

		// Token: 0x04005024 RID: 20516
		private const string PATH_TO_BATCH_CHECK_CASCADES_RESULTS_FILE = "Assets/Editor Default Resources/levels_to_batch_check_cascade_balance_results.csv";

		// Token: 0x04005025 RID: 20517
		private const string PATH_TO_MIRRORED_LEVELS_RESULTS_FILE = "Assets/Editor Default Resources/mirrored_levels_results.csv";

		// Token: 0x04005026 RID: 20518
		private const string MIRRORED_LEVELS_STATUS = "Levels that shouldn’t be mirrored:";

		// Token: 0x04005027 RID: 20519
		private const string CAN_MIRROR_LEVEL_MESSAGE = "Can mirror level {0}";

		// Token: 0x04005028 RID: 20520
		private const string CAN_NOT_MIRROR_LEVEL_MESSAGE = "Can not mirror level {0}";

		// Token: 0x04005029 RID: 20521
		private const string BATCH_CASCADE_RESULTS_HEADER = "level,cascades/Move,reshuffles/Move,cascades/Move\n";

		// Token: 0x0400502A RID: 20522
		private const string MIRROR_LEVELS_RESULTS_HEADER = "level,can be mirrored\n";

		// Token: 0x0400502B RID: 20523
		private const string ALTERNATE_LEVEL_VERSION = "Alternate";

		// Token: 0x0400502C RID: 20524
		private const float AUTO_PLAY_TIME_SCALE = 10f;

		// Token: 0x0400502D RID: 20525
		private const float NORMAL_TIME_SCALE = 1f;

		// Token: 0x0400502E RID: 20526
		private const int DEFAULT_LEVEL_NUMBER = 214;

		// Token: 0x0400502F RID: 20527
		private const int CASCADE_BALANCER_DEFAULT_TIMES = 70;

		// Token: 0x04005030 RID: 20528
		private const int CASCADE_BALANCER_DEFAULT_MOVES_LIMIT = 100;

		// Token: 0x04005031 RID: 20529
		private const int CASCADE_BALANCER_DEFAULT_STEPSIZE = 2;

		// Token: 0x04005032 RID: 20530
		private const int MAX_DIFFERENCE_TO_MIRROR = 15;

		// Token: 0x04005033 RID: 20531
		private const int RUN_TIMES_FOR_MIRROR_CHECK = 20;

		// Token: 0x04005034 RID: 20532
		private const int MAX_MOVES_FOR_MIRROR_CHECK = 100;

		// Token: 0x04005035 RID: 20533
		private string mirroredLevelsResults;

		// Token: 0x04005036 RID: 20534
		[WaitForRoot(false, false)]
		private EventSystemRoot eventSystem;

		// Token: 0x04005037 RID: 20535
		[SerializeField]
		private BoardView boardview;

		// Token: 0x04005038 RID: 20536
		[SerializeField]
		private Button buttonPlay;

		// Token: 0x04005039 RID: 20537
		[SerializeField]
		private Button buttonAutoPlay;

		// Token: 0x0400503A RID: 20538
		[Header("Level & State Saving")]
		[SerializeField]
		private Button buttonSaveState;

		// Token: 0x0400503B RID: 20539
		[SerializeField]
		private Button buttonConfirmSaveState;

		// Token: 0x0400503C RID: 20540
		[SerializeField]
		private Button buttonCancelSaveState;

		// Token: 0x0400503D RID: 20541
		[SerializeField]
		private Button buttonSaveLevel;

		// Token: 0x0400503E RID: 20542
		[SerializeField]
		private Button buttonDiscard;

		// Token: 0x0400503F RID: 20543
		[SerializeField]
		private LevelGeneratorView levelGenerator;

		// Token: 0x04005040 RID: 20544
		[SerializeField]
		private LayoutModifierView layoutModifier;

		// Token: 0x04005041 RID: 20545
		[Header("Test Saving")]
		[SerializeField]
		private Button buttonOpenTestSaving;

		// Token: 0x04005042 RID: 20546
		[SerializeField]
		private GameObject panelTestSaving;

		// Token: 0x04005043 RID: 20547
		[SerializeField]
		private InputField testName;

		// Token: 0x04005044 RID: 20548
		[SerializeField]
		private InputField InputFromX;

		// Token: 0x04005045 RID: 20549
		[SerializeField]
		private InputField InputFromY;

		// Token: 0x04005046 RID: 20550
		[SerializeField]
		private InputField InputToX;

		// Token: 0x04005047 RID: 20551
		[SerializeField]
		private InputField InputToY;

		// Token: 0x04005048 RID: 20552
		[SerializeField]
		private Button buttonSaveTest;

		// Token: 0x04005049 RID: 20553
		[Header("Cascade Balancer")]
		[SerializeField]
		private CascadeBalancer cascadeBalancer;

		// Token: 0x0400504A RID: 20554
		[SerializeField]
		private Button buttonOpenCascadeBalancer;

		// Token: 0x0400504B RID: 20555
		[SerializeField]
		private GameObject panelCascadeBalancer;

		// Token: 0x0400504C RID: 20556
		[SerializeField]
		private Button buttonBalanceCascades;

		// Token: 0x0400504D RID: 20557
		[SerializeField]
		private InputField inputTimes;

		// Token: 0x0400504E RID: 20558
		[SerializeField]
		private InputField inputMoves;

		// Token: 0x0400504F RID: 20559
		[SerializeField]
		private InputField inputStepSize;

		// Token: 0x04005050 RID: 20560
		[SerializeField]
		private Toggle toggleAutoColorAdjustment;

		// Token: 0x04005051 RID: 20561
		[SerializeField]
		private Toggle toggleShowAutoPlayInEditor;

		// Token: 0x04005052 RID: 20562
		private Brushes brushes;

		// Token: 0x04005053 RID: 20563
		private LevelLoaderEditor loader;

		// Token: 0x04005054 RID: 20564
		private LevelEditor levelEditor;

		// Token: 0x04005055 RID: 20565
		private string filePath;

		// Token: 0x04005056 RID: 20566
		private LevelConfig _currentConfig;

		// Token: 0x04005057 RID: 20567
		private int editId;

		// Token: 0x04005058 RID: 20568
		private int lastSaveEditId;

		// Token: 0x04005059 RID: 20569
		private int highestEditId;

		// Token: 0x0400505A RID: 20570
		private readonly Dictionary<int, string> levelSaves = new Dictionary<int, string>();

		// Token: 0x0400505B RID: 20571
		[CompilerGenerated]
		private static ReflectedPosition _003C_003Ef__mg_0024cache0;

		// Token: 0x02000575 RID: 1397
		public enum BatchRunMode
		{
			// Token: 0x0400505D RID: 20573
			None,
			// Token: 0x0400505E RID: 20574
			MirroredLevels,
			// Token: 0x0400505F RID: 20575
			CascadeBalancer
		}

		// Token: 0x02000576 RID: 1398
		// (Invoke) Token: 0x060024C7 RID: 9415
		private delegate IEnumerator BatchRoutine(string s);
	}
}

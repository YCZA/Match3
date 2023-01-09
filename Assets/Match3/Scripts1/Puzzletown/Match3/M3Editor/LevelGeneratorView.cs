using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x0200056A RID: 1386
	public class LevelGeneratorView : MonoBehaviour
	{
		// Token: 0x06002463 RID: 9315 RVA: 0x000A1EE8 File Offset: 0x000A02E8
		private void Start()
		{
			this.DisablePanels();
			this.dropdownModTypeMap = new Dictionary<Dropdown, FieldModification>
			{
				{
					this.stonesSetup,
					ModifierGenerator.GenerateStones
				},
				{
					this.chainsSetup,
					ModifierGenerator.GenerateChains
				},
				{
					this.cannonballsSetup,
					ModifierGenerator.GenerateCannonballs
				}
			};
			this.panelLevelSetUp.gameObject.SetActive(false);
			this.panelLevelLayout.gameObject.SetActive(false);
			this.panelInitialObjectPlacement.gameObject.SetActive(false);
			this.btnLevelSetUp.onClick.AddListener(delegate()
			{
				this.ActivatePanel(this.panelLevelSetUp.gameObject);
			});
			this.btnLevelLayout.onClick.AddListener(delegate()
			{
				this.ActivatePanel(this.panelLevelLayout.gameObject);
			});
			this.btnInitialObjectPlacement.onClick.AddListener(delegate()
			{
				this.ActivatePanel(this.panelInitialObjectPlacement.gameObject);
			});
			this.btnLayoutModify.onClick.AddListener(delegate()
			{
				this.ActivatePanel(this.panelLayoutModifiy.gameObject);
			});
			this.btnGenerate.onClick.AddListener(new UnityAction(this.Generate));
			this.objectiveSelection.AddOptions(Enum.GetNames(typeof(Objective)).ToList<string>());
			this.currentObjective = Objective.None;
			this.objectiveSelection.value = (int)this.currentObjective;
			List<string> options = Enum.GetNames(typeof(ModifierUsage)).ToList<string>();
			this.stackedGemsSetup.AddOptions(options);
			this.stackedGemsSetup.value = 2;
			this.stonesSetup.AddOptions(options);
			this.stonesSetup.value = 2;
			this.chainsSetup.AddOptions(options);
			this.chainsSetup.value = 2;
			this.cannonballsSetup.AddOptions(options);
			this.cannonballsSetup.value = 2;
			this.inputMinCut.text = 1.ToString();
			this.inputMaxCut.text = 5.ToString();
			this.inputMinLength.text = 1.ToString();
			this.inputMaxLength.text = 7.ToString();
			this.symmetrySelection.AddOptions(Enum.GetNames(typeof(Symmetry)).ToList<string>());
			this.currentSymmetry = Symmetry.Random;
			this.symmetrySelection.value = (int)this.currentSymmetry;
			this.GetMirrorFunctions(this.currentSymmetry);
			this.inputMinGroups.text = 1.ToString();
			this.inputMaxGroups.text = 3.ToString();
			this.inputMinModLength.text = 1.ToString();
			this.inputMaxModLength.text = 5.ToString();
			this.btNewInitialObjectPlacement.onClick.AddListener(delegate()
			{
				this.PreplaceInitialObjectsAndShow();
			});
		}

		// Token: 0x06002464 RID: 9316 RVA: 0x000A21DC File Offset: 0x000A05DC
		public void ActivatePanel(GameObject go)
		{
			bool activeSelf = go.activeSelf;
			this.DisablePanels();
			go.SetActive(!activeSelf);
		}

		// Token: 0x06002465 RID: 9317 RVA: 0x000A2200 File Offset: 0x000A0600
		private void DisablePanels()
		{
			this.panelLevelSetUp.gameObject.SetActive(false);
			this.panelLevelLayout.gameObject.SetActive(false);
			this.panelInitialObjectPlacement.gameObject.SetActive(false);
			this.panelLayoutModifiy.gameObject.SetActive(false);
		}

		// Token: 0x06002466 RID: 9318 RVA: 0x000A2254 File Offset: 0x000A0654
		private List<Reflection> GetMirrorFunctions(Symmetry symmetry)
		{
			if (symmetry == Symmetry.Random || symmetry == Symmetry.RandomInclNone)
			{
				int num = LevelGeneratorView.typeSymmetryMap.Count;
				if (symmetry == Symmetry.Random)
				{
					num--;
				}
				symmetry = LevelGeneratorView.typeSymmetryMap.Keys.ElementAt(RandomHelper.Next(num));
			}
			this.currentSymmetry = symmetry;
			return LevelGeneratorView.typeSymmetryMap[this.currentSymmetry];
		}

		// Token: 0x06002467 RID: 9319 RVA: 0x000A22B4 File Offset: 0x000A06B4
		private void Generate()
		{
			this.levelEditor.ResetAllFields();
			this.ClearObjectivesAndRatios();
			LevelGenerator.EnsureEnoughColors(this.levelConfig.data);
			this.GenerateLayout();
			this.GenerateAndPreplaceObjectives();
			this.layoutFields = this.levelEditor.fields.DeepCopyFields();
			this.PreplaceInitialObjectsAndShow();
		}

		// Token: 0x06002468 RID: 9320 RVA: 0x000A230C File Offset: 0x000A070C
		private void ClearObjectivesAndRatios()
		{
			this.levelConfig.data.objectives = new MaterialAmount[0];
			this.levelConfig.data.spawnRatioDroppable = new SpawnRatio();
			this.levelConfig.data.spawnRatios = new SpawnRatio[0];
		}

		// Token: 0x06002469 RID: 9321 RVA: 0x000A235C File Offset: 0x000A075C
		private void GenerateLayout()
		{
			int includedMinValue = int.Parse(this.inputMinCut.text);
			int excludedMaxValue = int.Parse(this.inputMaxCut.text);
			int minLength = int.Parse(this.inputMinLength.text);
			int maxLength = int.Parse(this.inputMaxLength.text);
			int count = RandomHelper.Next(includedMinValue, excludedMaxValue);
			LevelGenerator.CutGroups(this.levelEditor.fields, count, minLength, maxLength);
			Symmetry value = (Symmetry)this.symmetrySelection.value;
			List<Reflection> mirrorFunctions = this.GetMirrorFunctions(value);
			List<Reflection> list = new List<Reflection>(mirrorFunctions);
			list.Reverse();
			LevelGenerator.Reflect(this.levelEditor.fields, mirrorFunctions);
			LevelGenerator.PlaceSpawner(this.levelEditor.fields);
			LevelGenerator.RefineLevel(this.levelEditor.fields, this.levelConfig, this.levelEditor.fields.size, list);
			LevelGenerator.PlaceSpawner(this.levelEditor.fields);
		}

		// Token: 0x0600246A RID: 9322 RVA: 0x000A244C File Offset: 0x000A084C
		private void GenerateAndPreplaceObjectives()
		{
			this.currentObjective = (Objective)this.objectiveSelection.value;
			bool horizontalReflection = this.currentSymmetry == Symmetry.Horizontally;
			if (this.currentObjective == Objective.Random)
			{
				int num = RandomHelper.Next(1, Enum.GetValues(typeof(Objective)).Length);
				this.currentObjective = (Objective)num;
			}
			LevelGenerator.GenerateAndPreplaceObjective(this.levelEditor.fields, this.currentObjective, this.levelConfig.data, this.settings, horizontalReflection);
		}

		// Token: 0x0600246B RID: 9323 RVA: 0x000A24CC File Offset: 0x000A08CC
		private void PreplaceInitialObjectsAndShow()
		{
			if (this.layoutFields != null)
			{
				IEnumerator enumerator = this.layoutFields.DeepCopyFields().GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						Field field = (Field)obj;
						this.levelEditor.fields[field.gridPosition] = field;
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = (enumerator as IDisposable)) != null)
					{
						disposable.Dispose();
					}
				}
			}
			this.PreplaceLayoutModifier();
			LevelGenerator.FillBoardWithGems(this.levelEditor.fields);
			this.PreplaceRemainingModifier();
			this.levelEditor.ShowFields();
		}

		// Token: 0x0600246C RID: 9324 RVA: 0x000A2578 File Offset: 0x000A0978
		private void PreplaceLayoutModifier()
		{
			int includedMinValue = int.Parse(this.inputMinGroups.text);
			int num = int.Parse(this.inputMaxGroups.text);
			int includedMinValue2 = int.Parse(this.inputMinModLength.text);
			int num2 = int.Parse(this.inputMaxModLength.text);
			int num3 = RandomHelper.Next(includedMinValue, num + 1);
			List<FieldModification> list = new List<FieldModification>();
			foreach (Dropdown dropdown in this.dropdownModTypeMap.Keys)
			{
				if (this.ModifierCanBePlaced(dropdown))
				{
					list.Add(this.dropdownModTypeMap[dropdown]);
				}
			}
			if (list.Count > 0)
			{
				for (int i = 1; i <= num3; i++)
				{
					FieldModification mod = list[RandomHelper.Next(list.Count)];
					int length = RandomHelper.Next(includedMinValue2, num2 + 1);
					LevelGenerator.AddReflectedModifierGroup(this.levelEditor.fields, length, mod, LevelGeneratorView.typeSymmetryMap[this.currentSymmetry]);
				}
			}
		}

		// Token: 0x0600246D RID: 9325 RVA: 0x000A26B4 File Offset: 0x000A0AB4
		private void PreplaceRemainingModifier()
		{
			if (this.ModifierCanBePlaced(this.stackedGemsSetup))
			{
				int num = RandomHelper.Next(this.settings.StackedGemInitAmountMin, this.settings.StackedGemInitAmountMax + 1);
				bool flag = this.currentObjective == Objective.Recipe;
				bool useRecipeColor = flag;
				for (int i = 0; i < num; i++)
				{
					LevelGenerator.AddReflectedStackedGems(this.levelEditor.fields, ModifierGenerator.GenerateStackedGems, this.levelConfig.data, useRecipeColor, this.settings, LevelGeneratorView.typeSymmetryMap[this.currentSymmetry]);
					useRecipeColor = false;
				}
			}
		}

		// Token: 0x0600246E RID: 9326 RVA: 0x000A2748 File Offset: 0x000A0B48
		private bool ModifierCanBePlaced(Dropdown dropdown)
		{
			ModifierUsage value = (ModifierUsage)dropdown.value;
			return value == ModifierUsage.Use || value == ModifierUsage.Available;
		}

		// Token: 0x04004FCF RID: 20431
		public LevelEditor levelEditor;

		// Token: 0x04004FD0 RID: 20432
		public LevelConfig levelConfig;

		// Token: 0x04004FD1 RID: 20433
		[SerializeField]
		private LevelGenerationGlobals settings;

		// Token: 0x04004FD2 RID: 20434
		[SerializeField]
		private Button btnLevelSetUp;

		// Token: 0x04004FD3 RID: 20435
		[SerializeField]
		private Button btnLevelLayout;

		// Token: 0x04004FD4 RID: 20436
		[SerializeField]
		private Button btnInitialObjectPlacement;

		// Token: 0x04004FD5 RID: 20437
		[SerializeField]
		private Button btnLayoutModify;

		// Token: 0x04004FD6 RID: 20438
		[SerializeField]
		private RectTransform panelLevelSetUp;

		// Token: 0x04004FD7 RID: 20439
		[SerializeField]
		private RectTransform panelLevelLayout;

		// Token: 0x04004FD8 RID: 20440
		[SerializeField]
		private RectTransform panelInitialObjectPlacement;

		// Token: 0x04004FD9 RID: 20441
		[SerializeField]
		private RectTransform panelLayoutModifiy;

		// Token: 0x04004FDA RID: 20442
		[SerializeField]
		private Button btnGenerate;

		// Token: 0x04004FDB RID: 20443
		[Header("Level Setup")]
		[SerializeField]
		private Dropdown objectiveSelection;

		// Token: 0x04004FDC RID: 20444
		[SerializeField]
		private Dropdown stackedGemsSetup;

		// Token: 0x04004FDD RID: 20445
		[SerializeField]
		private Dropdown stonesSetup;

		// Token: 0x04004FDE RID: 20446
		[SerializeField]
		private Dropdown chainsSetup;

		// Token: 0x04004FDF RID: 20447
		[SerializeField]
		private Dropdown portalsSetup;

		// Token: 0x04004FE0 RID: 20448
		[SerializeField]
		private Dropdown cannonballsSetup;

		// Token: 0x04004FE1 RID: 20449
		[Header("Layout Generator")]
		[SerializeField]
		private InputField inputMinCut;

		// Token: 0x04004FE2 RID: 20450
		[SerializeField]
		private InputField inputMaxCut;

		// Token: 0x04004FE3 RID: 20451
		[SerializeField]
		private InputField inputMinLength;

		// Token: 0x04004FE4 RID: 20452
		[SerializeField]
		private InputField inputMaxLength;

		// Token: 0x04004FE5 RID: 20453
		// [SerializeField]
		// private Button buttonRefineManually;

		// Token: 0x04004FE6 RID: 20454
		[SerializeField]
		private Dropdown symmetrySelection;

		// Token: 0x04004FE7 RID: 20455
		[Header("Initial UnityEngine.Object Placement")]
		[SerializeField]
		private InputField inputMinGroups;

		// Token: 0x04004FE8 RID: 20456
		[SerializeField]
		private InputField inputMaxGroups;

		// Token: 0x04004FE9 RID: 20457
		[SerializeField]
		private InputField inputMinModLength;

		// Token: 0x04004FEA RID: 20458
		[SerializeField]
		private InputField inputMaxModLength;

		// Token: 0x04004FEB RID: 20459
		[SerializeField]
		private Button btNewInitialObjectPlacement;

		// Token: 0x04004FEC RID: 20460
		private Objective currentObjective;

		// Token: 0x04004FED RID: 20461
		private Symmetry currentSymmetry;

		// Token: 0x04004FEE RID: 20462
		private Dictionary<Dropdown, FieldModification> dropdownModTypeMap;

		// Token: 0x04004FEF RID: 20463
		private Fields layoutFields;

		// Token: 0x04004FF0 RID: 20464
		private static Dictionary<Symmetry, List<Reflection>> typeSymmetryMap = new Dictionary<Symmetry, List<Reflection>>
		{
			{
				Symmetry.Horizontally,
				new List<Reflection>
				{
					LayoutGenerator.ReflectHorizontally
				}
			},
			{
				Symmetry.Vertically,
				new List<Reflection>
				{
					LayoutGenerator.ReflectVertically
				}
			},
			{
				Symmetry.Center,
				new List<Reflection>
				{
					LayoutGenerator.ReflectHorizontally,
					LayoutGenerator.ReflectVertically
				}
			},
			{
				Symmetry.Point,
				new List<Reflection>
				{
					LayoutGenerator.ReflectPoint
				}
			},
			{
				Symmetry.DiagonallyNW,
				new List<Reflection>
				{
					LayoutGenerator.ReflectDiagonallyNW
				}
			},
			{
				Symmetry.DiagonallyNE,
				new List<Reflection>
				{
					LayoutGenerator.ReflectDiagonallyNE
				}
			},
			{
				Symmetry.None,
				new List<Reflection>()
			}
		};
	}
}

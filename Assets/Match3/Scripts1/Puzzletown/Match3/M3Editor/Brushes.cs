using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Match3.Scripts1.Wooga.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	public class Brushes : MonoBehaviour, ITableViewDataSource
	{
		private sealed class _GetCellForIndex_c__AnonStorey0
		{
			internal BrushGroupButton cell;

			internal Brushes _this;

			internal void __m__0()
			{
				this._this.HandleBrushGroupButtonClicked(this.cell);
			}
		}

		public List<BrushGroup> allBrushGroups = new List<BrushGroup>();

		[SerializeField]
		private TableView tableView;

		[SerializeField]
		private BrushGroupButton defaultButton;

		[SerializeField]
		private GemColorSpriteManager gemColorSpriteManager;

		[SerializeField]
		private GemTypeSpriteManager gemTypeSpriteManager;

		[SerializeField]
		private TileSpriteManager tileSpriteManager;

		[SerializeField]
		private StoneSpriteManager stoneSpriteManager;

		[SerializeField]
		private SpriteManager colorWheelSpriteManager;

		[SerializeField]
		private ChainSpriteManager chainSpriteManager;

		[SerializeField]
		private SpriteManager hiddenItemSpriteManager;

		[SerializeField]
		private SpriteManager portalSpriteManager;

		[SerializeField]
		private CrateSpriteManager cratesSpriteManager;

		[SerializeField]
		private CrateSpriteManager colorCratesSpriteManager;

		[SerializeField]
		private SpriteManager climberSpriteManager;

		[SerializeField]
		private SpriteManager iceSpriteManager;

		[SerializeField]
		private SpriteManager dirtSpriteManager;

		[SerializeField]
		private SpriteManager cannonSpriteManager;

		[SerializeField]
		private SpriteManager resistantBlockerSpriteManager;

		[SerializeField]
		private SpriteManager chameleonSpriteManager;

		[SerializeField]
		private Image currentBrushImage;

		[SerializeField]
		private Button buttonPreviousBrush;

		[SerializeField]
		private Button buttonNextBrush;

		[SerializeField]
		private Sprite colorBrushGroupImage;

		[SerializeField]
		private Sprite typeBrushGroupImage;

		[SerializeField]
		private Sprite spawn;

		[SerializeField]
		private Sprite dropItemSpawn;

		[SerializeField]
		private Sprite dropItemExit;

		[SerializeField]
		private Sprite climberSprite;

		[SerializeField]
		private Sprite climberSpawn;

		[SerializeField]
		private Sprite chameleonSpawn;

		[SerializeField]
		private Sprite climberExit;

		[SerializeField]
		private Sprite waterFields;

		[SerializeField]
		private Sprite definedGemSpawnerSprite;

		[SerializeField]
		private Sprite growingWindowSprite;

		[SerializeField]
		private Sprite resetField;

		[SerializeField]
		private Sprite removeField;

		[SerializeField]
		private Sprite removeCover;

		[SerializeField]
		private Sprite removeItem;

		[SerializeField]
		private Sprite removeType;

		[SerializeField]
		private Sprite removeBackground;

		[SerializeField]
		private Sprite removeHiddenItem;

		[SerializeField]
		private Sprite removeColorWheel;

		[SerializeField]
		private Sprite windowBrush;

		[SerializeField]
		private Sprite treasureBrush;

		[SerializeField]
		private Sprite treasureRemoveBrush;

		[SerializeField]
		private Sprite removeSpawn;

		[SerializeField]
		private Sprite removeExit;

		[SerializeField]
		private Sprite removePortal;

		[SerializeField]
		private Sprite removeGrowingWindow;

		[SerializeField]
		private Sprite removeChameleon;

		private const int ROTATION_STEP = 90;

		private readonly GemColor[] activeColors = new GemColor[]
		{
			GemColor.Random,
			GemColor.Blue,
			GemColor.Coins,
			GemColor.Green,
			GemColor.Orange,
			GemColor.Purple,
			GemColor.Red,
			GemColor.Yellow
		};

		private readonly GemType[] excludedTypes = new GemType[]
		{
			GemType.Undefined,
			GemType.ActivatedBomb,
			GemType.ActivatedSuperBomb,
			GemType.Cannon,
			GemType.Chameleon,
			GemType.ChameleonReduced
		};

		private readonly GemColor[] climberColors = new GemColor[]
		{
			GemColor.Random,
			GemColor.Blue,
			GemColor.Coins,
			GemColor.Green,
			GemColor.Orange,
			GemColor.Purple,
			GemColor.Red,
			GemColor.Yellow
		};

		private readonly GemColor[] crateColors = new GemColor[]
		{
			GemColor.Blue,
			GemColor.Coins,
			GemColor.Green,
			GemColor.Orange,
			GemColor.Purple,
			GemColor.Red,
			GemColor.Yellow
		};

		private readonly GemModifier[] iceModifier = new GemModifier[]
		{
			GemModifier.IceHp1,
			GemModifier.IceHp2,
			GemModifier.IceHp3
		};

		private readonly GemModifier[] dirtModifier = new GemModifier[]
		{
			GemModifier.DirtHp1,
			GemModifier.DirtHp2,
			GemModifier.DirtHp3
		};

		private readonly GemModifier[] cannonModifiers = new GemModifier[]
		{
			GemModifier.PreChargedLittle,
			GemModifier.PreChargedMedium,
			GemModifier.PreChargedMuch
		};

		private BrushGroup currentBrushGroup;

		private BrushGroup layout;

		private BrushGroup removal;

		private BrushGroup gems;

		private BrushGroup supergems;

		private BrushGroup tiles;

		private BrushGroup dropItems;

		private BrushGroup stones;

		private BrushGroup colorWheels;

		private BrushGroup chains;

		private BrushGroup hiddenItems;

		private BrushGroup cannonballs;

		private BrushGroup portals;

		private BrushGroup crates;

		private BrushGroup colorCrates;

		private BrushGroup climber;

		private BrushGroup ice;

		private BrushGroup dirt;

		private BrushGroup cannon;

		private BrushGroup water;

		private BrushGroup definedGemSpawner;

		private BrushGroup resistantBlocker;

		private BrushGroup growingWindow;

		private BrushGroup chameleon;

		private Dictionary<KeyCode, BrushGroup> keysBrushesGroups;

		private readonly KeyCode[] keysIndices = new KeyCode[]
		{
			KeyCode.Alpha1,
			KeyCode.Alpha2,
			KeyCode.Alpha3,
			KeyCode.Alpha4,
			KeyCode.Alpha5,
			KeyCode.Alpha6,
			KeyCode.Alpha7,
			KeyCode.Alpha8,
			KeyCode.Alpha9
		};

		private ABrush removeFieldBrush;

		public ABrush resetFieldBrush;

		private ABrush removeItemBrush;

		private ABrush removeTypeBrush;

		private ABrush removeTilesBrush;

		private ABrush removeCoverBrush;

		private ABrush removeHiddenItemBrush;

		private ABrush removeColorWheelBrush;

		private ABrush removeTreasureBrush;

		private ABrush removeSpawnBrush;

		private ABrush removeExitsBrush;

		private ABrush removePortalsBrush;

		private ABrush resetColorOfCrateBrush;

		private ABrush removeGrowingWindowBrush;

		private ABrush removeChameleonBrush;

		public ABrush CurrentBrush
		{
			get
			{
				return this.currentBrushGroup.CurrentBrush;
			}
		}

		private void Start()
		{
			this.removeFieldBrush = new RemoveFieldBrush(this.removeField);
			this.resetFieldBrush = new ResetFieldBrush(this.resetField);
			this.removeItemBrush = new RemoveItemBrush(this.removeItem);
			this.removeTypeBrush = new TypeBrush(GemType.Undefined, this.removeType, null);
			this.removeTilesBrush = new RemoveModifierTileBrush(this.removeBackground);
			this.removeCoverBrush = new RemoveCoverBrush(this.removeCover);
			this.removeHiddenItemBrush = new RemoveHiddenItemBrush(this.removeHiddenItem);
			this.removeColorWheelBrush = new RemoveColorWheelBrush(this.removeColorWheel);
			this.removeTreasureBrush = new DirtKeepingColorBrush(GemColor.Dirt, this.treasureRemoveBrush, this.removeItemBrush);
			this.removeSpawnBrush = new RemoveSpawnBrush(this.removeSpawn);
			this.removeExitsBrush = new RemoveExitBrush(this.removeExit);
			this.removePortalsBrush = new RemoveModifierPortalBrush(this.removePortal);
			this.removeGrowingWindowBrush = new RemoveGrowingWindowBrush(this.removeGrowingWindow);
			this.removeChameleonBrush = new RemoveChameleonBrush(this.removeChameleon);
			List<ABrush> list = new List<ABrush>();
			List<ABrush> list2 = new List<ABrush>();
			List<ABrush> list3 = new List<ABrush>();
			List<ABrush> list4 = new List<ABrush>();
			List<ABrush> list5 = new List<ABrush>();
			List<ABrush> list6 = new List<ABrush>();
			for (int i = 1; i <= 5; i++)
			{
				list.Add(new BlockerBrush(this.stoneSpriteManager.GetSprite(i), i, this.removeItemBrush));
				if (i <= 2)
				{
					list2.Add(new TileBrush(this.tileSpriteManager.GetSprite(i), i, this.removeTilesBrush));
				}
				if (i <= 3)
				{
					list3.Add(new ChainBrush(this.chainSpriteManager.GetSprite(i), i, this.removeCoverBrush));
				}
				if (i <= 3)
				{
					list5.Add(new CrateBrush(this.cratesSpriteManager.GetSprite(i), i, this.removeCoverBrush));
				}
			}
			for (int j = 2; j <= 4; j++)
			{
				Sprite similar = this.hiddenItemSpriteManager.GetSimilar(j.ToString());
				list4.Add(new HiddenItemBrush(similar, j, false, this.removeHiddenItemBrush));
				Sprite similar2 = this.hiddenItemSpriteManager.GetSimilar(j + "_random");
				list4.Add(new HiddenItemBrush(similar2, j, true, this.removeHiddenItemBrush));
			}
			list6.Add(new ColorWheelBrush(this.colorWheelSpriteManager.GetSimilar("frame_4"), this.removeColorWheelBrush, true));
			list6.Add(new ColorWheelVariantBrush(this.colorWheelSpriteManager.GetSimilar("frame_3"), this.removeColorWheelBrush));
			this.layout = new BrushGroup(this.removeField, this.GetLayoutBrushes());
			this.removal = new BrushGroup(this.removeBackground, this.GetRemovalBrushes());
			this.gems = new BrushGroup(this.colorBrushGroupImage, this.GetColorBrushes());
			this.supergems = new BrushGroup(this.typeBrushGroupImage, this.GetTypeBrushes());
			this.stones = new BrushGroup(this.stoneSpriteManager.GetSprite(1), list);
			this.colorWheels = new BrushGroup(this.colorWheelSpriteManager.GetSimilar("frame_4"), list6);
			this.tiles = new BrushGroup(this.tileSpriteManager.GetSprite(1), list2);
			this.chains = new BrushGroup(this.chainSpriteManager.GetSprite(1), list3);
			this.dropItems = new BrushGroup(this.dropItemSpawn, this.GetDropItemBrushes());
			this.hiddenItems = new BrushGroup(this.hiddenItemSpriteManager.GetSimilar("2"), list4);
			Sprite similar3 = this.gemColorSpriteManager.GetSimilar("cannonball");
			this.cannonballs = new BrushGroup(similar3, new List<ABrush>
			{
				new ColorBrush(GemColor.Cannonball, similar3, this.removeItemBrush)
			});
			this.portals = new BrushGroup(this.portalSpriteManager.GetSimilar("exit"), this.GetPortalBrushes());
			this.crates = new BrushGroup(this.cratesSpriteManager.GetSprite(1), list5);
			this.colorCrates = new BrushGroup(this.colorCratesSpriteManager.GetSimilar(GemColor.Blue.ToString()), this.GetColorCrateBrushes());
			this.climber = new BrushGroup(this.climberSpriteManager.GetSimilar(GemColor.Random.ToString()), this.GetClimberBrushes());
			this.ice = new BrushGroup(this.iceSpriteManager.GetSimilar(GemModifier.IceHp1.ToString()), this.GetIceBrushes());
			this.dirt = new BrushGroup(this.dirtSpriteManager.GetSimilar(GemModifier.DirtHp1.ToString()), this.GetDirtBrushes());
			this.cannon = new BrushGroup(this.cannonSpriteManager.GetSimilar("little"), this.GetCannonBrushes());
			this.water = new BrushGroup(this.waterFields, this.GetWaterAllFieldBrushes());
			this.definedGemSpawner = new BrushGroup(this.definedGemSpawnerSprite, this.GetDefinedGemSpawnFieldBrushes());
			int hp = ResistantBlocker.GetHp(6);
			Sprite similar4 = this.resistantBlockerSpriteManager.GetSimilar(hp.ToString());
			this.resistantBlocker = new BrushGroup(similar4, this.GetResistantBlockerBrushes());
			this.growingWindow = new BrushGroup(this.growingWindowSprite, this.GetGrowingWindowBrushes());
			this.chameleon = new BrushGroup(this.chameleonSpriteManager.GetSimilar("all"), this.GetChameleonBrushes());
			this.keysBrushesGroups = new Dictionary<KeyCode, BrushGroup>
			{
				{
					KeyCode.W,
					this.layout
				},
				{
					KeyCode.R,
					this.removal
				},
				{
					KeyCode.A,
					this.gems
				},
				{
					KeyCode.S,
					this.supergems
				},
				{
					KeyCode.F,
					this.stones
				},
				{
					KeyCode.K,
					this.colorWheels
				},
				{
					KeyCode.T,
					this.tiles
				},
				{
					KeyCode.C,
					this.chains
				},
				{
					KeyCode.D,
					this.dropItems
				},
				{
					KeyCode.H,
					this.hiddenItems
				},
				{
					KeyCode.E,
					this.cannonballs
				},
				{
					KeyCode.P,
					this.portals
				},
				{
					KeyCode.X,
					this.crates
				},
				{
					KeyCode.Y,
					this.colorCrates
				},
				{
					KeyCode.G,
					this.climber
				},
				{
					KeyCode.I,
					this.ice
				},
				{
					KeyCode.Q,
					this.dirt
				},
				{
					KeyCode.N,
					this.cannon
				},
				{
					KeyCode.J,
					this.water
				},
				{
					KeyCode.B,
					this.definedGemSpawner
				},
				{
					KeyCode.M,
					this.resistantBlocker
				},
				{
					KeyCode.O,
					this.growingWindow
				},
				{
					KeyCode.Z,
					this.chameleon
				}
			};
			foreach (KeyValuePair<KeyCode, BrushGroup> current in this.keysBrushesGroups)
			{
				this.allBrushGroups.Add(current.Value);
			}
			this.ShowCurrentBrush(this.allBrushGroups[0]);
			this.tableView.DataSource = this;
			this.tableView.Reload();
			this.buttonPreviousBrush.onClick.AddListener(new UnityAction(this.HandlePreviousBrushClicked));
			this.buttonNextBrush.onClick.AddListener(new UnityAction(this.HandleNextBrushClicked));
		}

		public int GetNumberOfCellsForTableView()
		{
			return this.allBrushGroups.Count;
		}

		public ATableViewReusableCell GetCellForIndex(int index, Transform parent)
		{
			BrushGroupButton cell = this.tableView.GetReusableCell(1001) as BrushGroupButton;
			if (cell == null)
			{
				cell = global::UnityEngine.Object.Instantiate<BrushGroupButton>(this.defaultButton);
				cell.gameObject.SetActive(true);
			}
			cell.transform.SetParent(parent, false);
			cell.name = string.Format("Brush Button {0}", this.allBrushGroups[index].GroupImage);
			cell.brushGroup = this.allBrushGroups[index];
			cell.image.sprite = this.allBrushGroups[index].GroupImage;
			cell.button.onClick.AddListener(delegate
			{
				this.HandleBrushGroupButtonClicked(cell);
			});
			return cell;
		}

		public void HandleBrushGroupButtonClicked(BrushGroupButton sender)
		{
			this.ShowCurrentBrush(sender.brushGroup);
		}

		public void HandleNextBrushClicked()
		{
			this.currentBrushGroup.GoToNextBrush();
			this.ShowCurrentBrush();
		}

		public void HandlePreviousBrushClicked()
		{
			this.currentBrushGroup.GoToPreviousBrush();
			this.ShowCurrentBrush();
		}

		public LayoutElement GetLayoutElementForIndex(int index)
		{
			return this.defaultButton.GetComponent<LayoutElement>();
		}

		private void ShowCurrentBrush(BrushGroup group)
		{
			this.currentBrushGroup = group;
			this.ShowCurrentBrush();
		}

		private void ShowCurrentBrush()
		{
			this.ResetBrushImage();
			this.currentBrushImage.transform.rotation = Quaternion.AngleAxis((float)this.CurrentBrush.RotationAngle, Vector3.back);
			this.currentBrushImage.sprite = this.currentBrushGroup.CurrentBrush.Sprite;
		}

		private void ResetBrushImage()
		{
			this.currentBrushImage.transform.localScale = Vector3.one;
			this.currentBrushImage.color = Color.white;
		}

		private void HandleBrushIndexClicked(int index)
		{
			this.currentBrushGroup.GoToBrushAtIndex(index);
			this.ShowCurrentBrush();
		}

		private List<ABrush> GetPortalBrushes()
		{
			List<ABrush> list = new List<ABrush>();
			for (int i = 1; i <= this.portalSpriteManager.Count; i++)
			{
				string text = Portal.IsExit(i) ? "portal_exit" : "portal_entrance" ;
				text = text + "_" + Portal.GetStringId(i).ToLower();
				Sprite similar = this.portalSpriteManager.GetSimilar(text);
				list.Add(new PortalBrush(similar, i, this.removePortalsBrush));
			}
			list.Add(this.removePortalsBrush);
			return list;
		}

		private List<ABrush> GetColorCrateBrushes()
		{
			List<ABrush> list = new List<ABrush>();
			Sprite sprite = this.cratesSpriteManager.GetSprite(1);
			this.resetColorOfCrateBrush = new ColorCrateBrush(sprite, GemColor.Undefined, this.removeCoverBrush);
			GemColor[] array = this.crateColors;
			for (int i = 0; i < array.Length; i++)
			{
				GemColor color = array[i];
				sprite = this.colorCratesSpriteManager.GetSimilar(color.ToString());
				list.Add(new ColorCrateBrush(sprite, color, this.resetColorOfCrateBrush));
			}
			list.Add(this.resetColorOfCrateBrush);
			return list;
		}

		private List<ABrush> GetClimberBrushes()
		{
			List<ABrush> list = new List<ABrush>();
			list.Add(new ClimberBrush(this.climberSprite, this.removeItemBrush));
			list.Add(new ClimberSpawnBrush(this.climberSpawn, this.removeSpawnBrush));
			list.Add(new ClimberExitBrush(this.climberExit, this.removeExitsBrush));
			for (int i = 0; i < this.climberColors.Length; i++)
			{
				GemColor color = this.climberColors[i];
				Sprite similar = this.climberSpriteManager.GetSimilar(color.ToString());
				list.Add(new ClimberGemBrush(color, GemType.ClimberGem, similar, this.removeItemBrush));
			}
			return list;
		}

		private List<ABrush> GetIceBrushes()
		{
			List<ABrush> list = new List<ABrush>();
			for (int i = 0; i < this.iceModifier.Length; i++)
			{
				GemModifier modifier = this.iceModifier[i];
				list.Add(new IceBrush(modifier, this.iceSpriteManager.GetSimilar(modifier.ToString()), this.removeCoverBrush));
			}
			return list;
		}

		private List<ABrush> GetCannonBrushes()
		{
			List<ABrush> list = new List<ABrush>();
			for (int i = 0; i < this.cannonModifiers.Length; i++)
			{
				GemModifier modifier = this.cannonModifiers[i];
				Sprite similar = this.cannonSpriteManager.GetSimilar(modifier.ToString());
				list.Add(new CannonBrush(modifier, similar, this.removeCoverBrush));
			}
			return list;
		}

		private List<ABrush> GetDirtBrushes()
		{
			List<ABrush> list = new List<ABrush>();
			for (int i = 0; i < this.dirtModifier.Length; i++)
			{
				GemModifier modifier = this.dirtModifier[i];
				list.Add(new DirtBrush(modifier, this.dirtSpriteManager.GetSimilar(modifier.ToString()), this.removeCoverBrush));
			}
			list.Add(new DirtKeepingColorBrush(GemColor.Treasure, this.treasureBrush, this.removeTreasureBrush));
			return list;
		}

		private List<ABrush> GetLayoutBrushes()
		{
			return new List<ABrush>
			{
				this.removeFieldBrush,
				new SpawnBrush(this.spawn, this.removeSpawnBrush),
				new WindowBrush(this.windowBrush, this.resetFieldBrush)
			};
		}

		private List<ABrush> GetRemovalBrushes()
		{
			return new List<ABrush>
			{
				this.removeTilesBrush,
				this.removeItemBrush,
				this.removeTypeBrush,
				this.removeCoverBrush,
				this.resetFieldBrush,
				this.removeHiddenItemBrush,
				this.removeColorWheelBrush,
				this.removeTreasureBrush,
				this.removeExitsBrush,
				this.removeSpawnBrush,
				this.removePortalsBrush,
				this.removeGrowingWindowBrush,
				this.removeChameleonBrush
			};
		}

		private List<ABrush> GetColorBrushes()
		{
			List<ABrush> list = new List<ABrush>();
			GemColor[] array = this.activeColors;
			for (int i = 0; i < array.Length; i++)
			{
				GemColor gemColor = array[i];
				list.Add(new ColorBrush(gemColor, this.gemColorSpriteManager.GetSprite(gemColor), this.removeItemBrush));
			}
			return list;
		}

		private List<ABrush> GetTypeBrushes()
		{
			List<ABrush> list = new List<ABrush>();
			IEnumerator enumerator = Enum.GetValues(typeof(GemType)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					GemType gemType = (GemType)enumerator.Current;
					if (!this.excludedTypes.Contains(gemType))
					{
						list.Add(new TypeBrush(gemType, this.gemTypeSpriteManager.GetSimilar(gemType.ToString()), this.removeTypeBrush));
					}
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
			list.Add(new ColorBrush(GemColor.Rainbow, this.gemColorSpriteManager.GetSprite(GemColor.Rainbow), this.removeItemBrush));
			return list;
		}

		private List<ABrush> GetDropItemBrushes()
		{
			return new List<ABrush>
			{
				new ColorBrush(GemColor.Droppable, this.gemColorSpriteManager.GetSprite(GemColor.Droppable), this.removeItemBrush),
				new DropItemSpawnBrush(this.dropItemSpawn, this.removeSpawnBrush),
				new DropItemExitBrush(this.dropItemExit, this.removeExitsBrush)
			};
		}

		private List<ABrush> GetWaterAllFieldBrushes()
		{
			return new List<ABrush>
			{
				new TileBrush(this.waterFields, 3, this.removeTilesBrush)
			};
		}

		private List<ABrush> GetDefinedGemSpawnFieldBrushes()
		{
			return new List<ABrush>
			{
				new DefinedGemSpawnBrush(this.definedGemSpawnerSprite, this.removeSpawnBrush)
			};
		}

		private List<ABrush> GetResistantBlockerBrushes()
		{
			List<ABrush> list = new List<ABrush>();
			for (int i = 8; i >= 6; i--)
			{
				int hp = ResistantBlocker.GetHp(i);
				Sprite similar = this.resistantBlockerSpriteManager.GetSimilar(hp.ToString());
				list.Add(new BlockerBrush(similar, i, this.removeItemBrush));
			}
			return list;
		}

		private List<ABrush> GetGrowingWindowBrushes()
		{
			return new List<ABrush>
			{
				new GrowingWindowBrush(this.growingWindowSprite, this.removeGrowingWindowBrush)
			};
		}

		private List<ABrush> GetChameleonBrushes()
		{
			List<ABrush> list = new List<ABrush>();
			list.AddRange(this.GetChameleonVariantBrushes(ChameleonVariant.All, "all"));
			list.AddRange(this.GetChameleonVariantBrushes(ChameleonVariant.Reduced, "reduced"));
			list.Add(new ChameleonSpawnBrush(this.chameleonSpawn, this.removeSpawnBrush));
			return list;
		}

		private List<ABrush> GetChameleonVariantBrushes(ChameleonVariant chameleonVariant, string spriteSubstring)
		{
			List<ABrush> list = new List<ABrush>();
			for (int i = 0; i < 4; i++)
			{
				list.Add(new ChameleonBrush(chameleonVariant, i + GemDirection.Left, this.chameleonSpriteManager.GetSimilar(spriteSubstring), this.removeChameleonBrush, i * 90));
			}
			return list;
		}

		private void GoToBrushGroup(BrushGroup group)
		{
			if (this.currentBrushGroup.Equals(group))
			{
				this.HandleNextBrushClicked();
			}
			else
			{
				this.ShowCurrentBrush(group);
				this.HandleBrushIndexClicked(0);
			}
		}
		private void Update()
		{
			foreach (KeyCode current in this.keysBrushesGroups.Keys)
			{
				if (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftAlt) && global::UnityEngine.Input.GetKeyDown(current))
				{
					this.GoToBrushGroup(this.keysBrushesGroups[current]);
					break;
				}
			}
			for (int i = 0; i < this.keysIndices.Length; i++)
			{
				if (!Input.GetKey(KeyCode.LeftControl) && global::UnityEngine.Input.GetKeyDown(this.keysIndices[i]))
				{
					this.HandleBrushIndexClicked(i);
				}
			}
			if (global::UnityEngine.Input.GetKeyDown(KeyCode.LeftArrow))
			{
				this.HandlePreviousBrushClicked();
			}
			else if (global::UnityEngine.Input.GetKeyDown(KeyCode.RightArrow))
			{
				this.HandleNextBrushClicked();
			}
			else if (global::UnityEngine.Input.GetKeyDown(KeyCode.Escape) && this.CurrentBrush.HasRemovalBrush)
			{
				ABrush removalBrush = this.CurrentBrush.RemovalBrush;
				if (removalBrush.Equals(this.removeFieldBrush))
				{
					this.currentBrushGroup = this.layout;
				}
				else if (removalBrush.Equals(this.resetColorOfCrateBrush))
				{
					this.currentBrushGroup = this.colorCrates;
				}
				else
				{
					this.currentBrushGroup = this.removal;
				}
				this.currentBrushGroup.GoToBrush(removalBrush);
				this.ShowCurrentBrush();
			}
		}
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Match3.Scripts1;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Puzzletown.Datasources;
using Match3.Scripts1.Puzzletown.Flows;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.Town;
using Match3.Scripts1.Puzzletown.Tutorials;
using Match3.Scripts1.Shared.ResourceManager;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Signals;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000A41 RID: 2625
namespace Match3.Scripts2.Building.Shop
{
	public class TownShopRoot : APtSceneRoot, IHandler<BuildingShopView.BuildingBuildRequest>, IHandler<ShopTag>, IHandler<PopupOperation>, IHandler<TownOptionsCommand>, IPersistentDialog
	{
		// Token: 0x17000938 RID: 2360
		// (get) Token: 0x06003ED7 RID: 16087 RVA: 0x0013FCB8 File Offset: 0x0013E0B8
		public bool HasSeasonalTabSetup
		{
			get
			{
				if (this.shopTabs != null && this.shopTabs.Length > 0)
				{
					foreach (BuildingType buildingType in this.shopTabs)
					{
						if (buildingType == BuildingType.Building)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x06003ED8 RID: 16088 RVA: 0x0013FD07 File Offset: 0x0013E107
		public void Open(ShopTag tab)
		{
			this.selectedType = tab;
			this.Open();
		}

		// Token: 0x06003ED9 RID: 16089 RVA: 0x0013FD16 File Offset: 0x0013E116
		public void Open()
		{
			this.TrySeasonalsV3Tutorial();
			this.seasonalTimerContainer.SetActive(false);
			this.seasonalTimerContainerGrandPrize.SetActive(false);
			this.friendDecoPanel.SetActive(false);
			WooroutineRunner.StartCoroutine(this.OpenRoutine(), null);
		}

		// Token: 0x06003EDA RID: 16090 RVA: 0x0013FD50 File Offset: 0x0013E150
		private IEnumerator OpenRoutine()
		{
			yield return this.resourceService.onSpritesLoaded;
			BuildingLocation.Selected = null;
			this.dialog.Show();
			base.Enable();
			yield return null;
			this.Refresh();
			BackButtonManager.Instance.AddAction(new Action(this.CloseViaBackButton));
			this.audioService.PlaySFX(AudioId.MenuSlideOpen, false, false, false);
			this.buildingRequestHandled = false;
			this.onShopReady.Dispatch();
			yield break;
		}

		// Token: 0x06003EDB RID: 16091 RVA: 0x0013FD6B File Offset: 0x0013E16B
		public void CloseViaBackButton()
		{
			this.Close();
			this.TryZoomOut();
		}

		// Token: 0x06003EDC RID: 16092 RVA: 0x0013FD7C File Offset: 0x0013E17C
		public void Close()
		{
			if (this.audioService != null)
			{
				this.audioService.PlaySFX(AudioId.MenuSlideClose, false, false, false);
			}
			this.dialog.Hide();
			BackButtonManager.Instance.RemoveAction(new Action(this.CloseViaBackButton));
			this.onClose.Dispatch();
		}

		// 根据StoreTag来筛选建筑
		public IEnumerable<BuildingConfig> GetBuildingConfigsByStoreTag(ShopTag selected)
		{
			IEnumerable<BuildingConfig> source;
			int currentIsland = this.stateService.Buildings.CurrentIsland;
			source = this.config.buildingConfigList.GetAllOnIsland(currentIsland);	// 获取当前岛上的所有建筑
			return from b in source
				where b.shop_tag == (int)selected
				select b;
		}
		// 根据BuildingType来筛选建筑
		public IEnumerable<BuildingConfig> GetBuildingConfigs(ShopTag selected)
		{
			// bool isBuilding = selected == BuildingType.Building;
			// bool isAlphabet = selected == BuildingType.Alphabet;
			// bool isFriend = selected == BuildingType.Friend;
			IEnumerable<BuildingConfig> source;
			// eli key point 活动相关，先不管
			// if (isBuilding)
			// {
			// 	SeasonConfig activeSeason = this.seasonService.GetActiveSeason();
			// 	if (activeSeason == null)
			// 	{
			// 		return Enumerable.Empty<BuildingConfig>();
			// 	}
			// 	string[] setNames = activeSeason.SetNames;
			// 	source = from b in this.config.buildingConfigList.GetAllForSeasons(setNames)
			// 	where !b.IsSeasonGrandPrice()
			// 	select b;
			// }
			// eli key point 字母建筑，先不管
			// else if (isAlphabet)
			// {
			// source = this.config.buildingConfigList.buildings;
			// }
			// eli key point 好友，先不管
			// else if (isFriend)
			// {
			// List<BuildingConfig> list = new List<BuildingConfig>();
			// IEnumerable<BuildingConfig> enumerable = from b in this.config.buildingConfigList.buildings
			// where b.IsFriendDeco()
			// select b;
			// bool flag4 = false;
			// foreach (BuildingConfig buildingConfig in enumerable)
			// {
			// 	if (buildingConfig.UnlockResource.type == "friends" && buildingConfig.UnlockResource.amount > this.stateService.Facebook.friendCountHighWater && !flag4)
			// 	{
			// 		list.Add(new BuildingConfig
			// 		{
			// 			name = "fbButton",
			// 			type = 1024,
			// 			unlock_level = 1
			// 		});
			// 		flag4 = true;
			// 	}
			// 	list.Add(buildingConfig);
			// }
			// source = list;
			// }
			// else
			{
				int currentIsland = this.stateService.Buildings.CurrentIsland;
				source = this.config.buildingConfigList.GetAllOnIsland(currentIsland);	// 获取当前岛上的所有建筑
			}
			return from b in source
				where (b.type & (int)selected) != 0
				select b;
		}

		// Token: 0x06003EDE RID: 16094 RVA: 0x0013FFE8 File Offset: 0x0013E3E8
		public void OnBuildingPurchaseCancelled()
		{
			if (this.seasonService.GrandPrizeReady)
			{
				this.TryZoomOut();
				BuildingConfig grandPrizeBuildingConfig = this.seasonService.GetGrandPrizeBuildingConfig();
				BlockerManager.global.Append(new GrandPricePlaceBlocker(grandPrizeBuildingConfig));
			}
			else
			{
				this.Open();
			}
		}

		// Token: 0x06003EDF RID: 16095 RVA: 0x00140034 File Offset: 0x0013E434
		public void Handle(BuildingShopView.BuildingBuildRequest request)
		{
			if (request.Config.name == "fbButton")
			{
				this.facebookService.AddFriends(MultiFriendsSelectorRoot.FriendSelectorType.friends_deco);
				return;
			}
			if (!this.buildingRequestHandled)
			{
				this.buildingRequestHandled = true;
				this.Close();
				this.AddOnPurchaseCancelledListener();
				request.shouldPanCamera = !this.townMain.IsTutorialRunning;
				this.onBuildingClick.Dispatch(request);
			}
		}

		// Token: 0x06003EE0 RID: 16096 RVA: 0x001400A8 File Offset: 0x0013E4A8
		public void Handle(ShopTag type)
		{
			this.selectedType = type;
			this.Refresh();
		}

		// Token: 0x06003EE1 RID: 16097 RVA: 0x001400B7 File Offset: 0x0013E4B7
		public void Handle(PopupOperation op)
		{
			if (op != PopupOperation.Close)
			{
				if (op == PopupOperation.Details)
				{
					SceneManager.Instance.LoadScene<TownShopSeasonalInfoRoot>(null);
				}
			}
			else
			{
				this.Close();
				this.TryZoomOut();
			}
		}

		// Token: 0x06003EE2 RID: 16098 RVA: 0x001400EE File Offset: 0x0013E4EE
		public void Handle(TownOptionsCommand evt)
		{
			if (evt == TownOptionsCommand.LogIn)
			{
				base.StartCoroutine(this.FacebookLoginRoutine());
			}
		}

		// Token: 0x06003EE3 RID: 16099 RVA: 0x00140108 File Offset: 0x0013E508
		private IEnumerator FacebookLoginRoutine()
		{
			Coroutine flow = new FacebookLoginFlow(FacebookLoginContext.friends_deco).Start();
			yield return flow;
			this.Refresh();
			yield break;
		}

		// Token: 0x06003EE4 RID: 16100 RVA: 0x00140123 File Offset: 0x0013E523
		private void TryZoomOut()
		{
			if (BuildingLocation.Selected == null)
			{
				BlockerManager.global.Append(PanCameraFlow.CreateZoomOutFlow());
			}
		}

		// Token: 0x06003EE5 RID: 16101 RVA: 0x0014013E File Offset: 0x0013E53E
		public bool IsBuildingNewAndAvailable(BuildingConfig building)
		{
			return !this.stateService.Buildings.IsBuildingReviewed(building.name) && this.GetShopDataState(building) == BuildingShopDataState.Available;
		}

		// Token: 0x06003EE6 RID: 16102 RVA: 0x00140170 File Offset: 0x0013E570
		protected override IEnumerator GoRoutine()
		{
// 			List<BuildingType> tabs = new List<BuildingType>
// 			{
// 				BuildingType.Flower,
// #if !REVIEW_VERSION
// 			BuildingType.Monument,
// #endif
// 				BuildingType.Road,
// 				BuildingType.Friend,
// 				BuildingType.Alphabet,
// 				BuildingType.Storage
// 			};
			List<ShopTag> tabs = new List<ShopTag>
			{
				ShopTag.RarityLevel1,
				ShopTag.RarityLevel2,
				ShopTag.RarityLevel3,
				ShopTag.Storage,
			};

			// if (!this.sbsService.SbsConfig.feature_switches.alphabet_decos_enabled)
			// {
			// 	tabs.Remove(BuildingType.Alphabet);
			// }
			// else
			// {
			// 	Wooroutine<bool> isAlphabetAvailable = this.abs.IsBundleAvailable(this.config.SbsConfig.promo_popup.AlphabetAssetBundleName);
			// 	yield return isAlphabetAvailable;
			// 	if (!isAlphabetAvailable.ReturnValue)
			// 	{
			// 		tabs.Remove(BuildingType.Alphabet);
			// 	}
			// }
			// if (!this.sbsService.SbsConfig.feature_switches.friends_decos_enabled)
			// {
			// 	tabs.Remove(BuildingType.Friend);
			// }
			// else
			// {
			// 	Wooroutine<bool> isFriendBundleAvailable = this.abs.IsBundleAvailable("buildings_friendship_2018");
			// 	yield return isFriendBundleAvailable;
			// 	if (!isFriendBundleAvailable.ReturnValue)
			// 	{
			// 		tabs.Remove(BuildingType.Friend);
			// 	}
			// 	else
			// 	{
			// 		yield return this.SetupFriendDecoTab();
			// 	}
			// }
			this.shopTabs = tabs.ToArray();
			this.buildingsService = new BuildingServices();
			this.buildingsService.Init(this.config, this.resourceService, this.stateService);
			if (this.seasonService.IsActive)
			{
				yield return this.SetupSeasonalTab();
			}
			base.Disable();
			yield break;
		}

		// Token: 0x06003EE7 RID: 16103 RVA: 0x0014018C File Offset: 0x0013E58C
		private IEnumerator SetupSeasonalTab()
		{
			SeasonConfig promotedSeason = this.seasonService.GetActiveSeason();
			Wooroutine<bool> bundleExistsRoutine = this.seasonService.AreAllActiveSeasonBundlesAvailable();
			yield return bundleExistsRoutine;
			bool bundleExists = false;
			try
			{
				bundleExists = bundleExistsRoutine.ReturnValue;
			}
			catch (Exception ex)
			{
				WoogaDebug.Log(new object[]
				{
					ex.Message
				});
			}
			if (bundleExists)
			{
				Wooroutine<SpriteManager> spriteManagerRoutine = new SeasonSpriteManagerFlow().Start<SpriteManager>();
				yield return spriteManagerRoutine;
				this.seasonSpriteManager = spriteManagerRoutine.ReturnValue;
				Wooroutine<Sprite> sprite = this.abs.LoadAsset<Sprite>(promotedSeason.PrimaryBundleName, promotedSeason.CurrentIconName);
				yield return sprite;
				try
				{
					this.seasonalIcon = sprite.ReturnValue;
				}
				catch (Exception ex2)
				{
					WoogaDebug.LogWarning(new object[]
					{
						ex2.Message
					});
				}
				// if (this.seasonalIcon != null)
				// {
				// 	List<ShopTag> list = new List<ShopTag>(this.shopTabs);
				// 	list.Insert(0, BuildingType.Building);
				// 	this.shopTabs = list.ToArray();
				// 	this.seasonalTabInitialized = true;
				// }
			}
			yield break;
		}

		// Token: 0x06003EE8 RID: 16104 RVA: 0x001401A8 File Offset: 0x0013E5A8
		private void TrySeasonalsV3Tutorial()
		{
			if (this.seasonalTabInitialized && this.seasonService.IsSeasonalsV3 && !this.progression.IsTutorialCompleted("seasonalsV3ShopTutorial"))
			{
				this.selectedType = ShopTag.RarityLevel1;
				this.seasonalsV3TutorialMarker.enabled = true;
			}
		}

		// Token: 0x06003EE9 RID: 16105 RVA: 0x001401F8 File Offset: 0x0013E5F8
		private IEnumerator SetupFriendDecoTab()
		{
			Wooroutine<bool> isFriendBundleAvailable = this.abs.IsBundleAvailable("buildings_friendship_2018");
			yield return isFriendBundleAvailable;
			if (isFriendBundleAvailable.ReturnValue)
			{
				Wooroutine<SpriteManager> spriteManagerFlow = new BundledSpriteManagerLoaderFlow().Start(new BundledSpriteManagerLoaderFlow.Input
				{
					bundleName = "buildings_friendship_2018",
					path = "Assets/Puzzletown/Town/Art/Buildings/friendship_decos_2018/FriendDecoSpriteManager.prefab"
				});
				yield return spriteManagerFlow;
				SpriteManager spriteManager = spriteManagerFlow.ReturnValue;
				if (spriteManager != null)
				{
					this.friendDecoPanelImage.sprite = spriteManager.GetSimilar("illustration");
					this.friendDecoIcon = spriteManager.GetSimilar("icon");
					this.friendCellBackgroundImage.sprite = spriteManager.GetSimilar("shopcell");
					this.friendCellBackgroundImage.sprite = spriteManager.GetSimilar("facebook");
				}
			}
			yield break;
		}

		// Token: 0x06003EEA RID: 16106 RVA: 0x00140213 File Offset: 0x0013E613
		protected override void OnDisable()
		{
			this.onShopReady.Clear();
			base.OnDisable();
		}

		// Token: 0x06003EEB RID: 16107 RVA: 0x00140228 File Offset: 0x0013E628
		private void Refresh()
		{
			this.categories.Show(this.GetShopTabs());
			this.audioService.PlaySFX(AudioId.TabClick, false, false, false);
			// eli key point 显示建筑商店中的商品
			// 审核版, 商店合并标签(合并flower和monument)1
			// #if REVIEW_VERSION
			// {
			// 	if ((selectedType & BuildingType.Monument) != 0 || (selectedType & BuildingType.Flower) != 0)
			// 	{
			// 		selectedType |= BuildingType.Monument;
			// 		selectedType |= BuildingType.Flower;
			// 	}
			// }
			// #endif
			var shopData = this.GetShopData(this.selectedType).ToList();
			// 排序
// #if REVIEW_VERSION
// 			{
// 				shopData.Sort((b1, b2)=>
// 				{
// 					bool b1Unlock = quests.IsLevelUnlocked(b1.data.unlock_level) && b1.data.chapter_id <= quests.Chapter;
// 					bool b2Unlock = quests.IsLevelUnlocked(b2.data.unlock_level) && b2.data.chapter_id <= quests.Chapter;
// 					// 已解锁的在前面
// 					if (b1Unlock != b2Unlock)
// 					{
// 						return b1Unlock ? -1 : 1;
// 					}
//
// 					// 按解锁章节和解锁等级排序
// 					if (b1.data.chapter_id == b2.data.chapter_id)
// 					{
// 						if (b1.data.unlock_level == b2.data.unlock_level)
// 						{
// 							return 0;
// 						}
// 						return b1.data.unlock_level > b2.data.unlock_level ? 1 : -1;
// 					}
// 				
// 					return b1.data.chapter_id > b2.data.chapter_id ? 1 : -1;
// 				});
// 			}
// #endif
			this.buildings.Show(shopData);
			this.SnapToLatestUnlocked();
		}

		// Token: 0x06003EEC RID: 16108 RVA: 0x00140278 File Offset: 0x0013E678
		private void AddOnPurchaseCancelledListener()
		{
			this.townMain.townLoader.buildingServices.BuildingsController.onPurchaseCancelled.RemoveAllListeners();
			this.townMain.townLoader.buildingServices.BuildingsController.onPurchaseCancelled.AddListener(new Action(this.OnBuildingPurchaseCancelled));
		}

		// Token: 0x06003EED RID: 16109 RVA: 0x001402D0 File Offset: 0x0013E6D0
		private void SnapToLatestUnlocked()
		{
			List<BuildingConfig> self = new List<BuildingConfig>(this.GetBuildingConfigsByStoreTag(this.selectedType));
			int num = self.CountIf((BuildingConfig c) => this.GetShopDataState(c).Equals(BuildingShopDataState.Available));
			float preferredHeight = this.buildings.prototypeCells[0].GetComponent<LayoutElement>().preferredHeight;
			float num2 = this.buildings.prototypeCells[0].GetComponent<LayoutElement>().preferredWidth;
			num2 += this.scrollGridLayout.spacing.x;
			int num3 = (int)(this.scrollRect.viewport.rect.width / num2);
			bool flag = this.scrollRect.viewport.rect.width > this.scrollRect.viewport.rect.height;
			int num4 = (!flag) ? 1 : 0;
			int num5 = Mathf.Max((num - 1) / num3 - num4, 0);
			Canvas.ForceUpdateCanvases();
			if (this.selectedType == ShopTag.RarityLevel1)
			{
				this.scrollRect.content.localPosition = new Vector3(0f, 0f, 0f);
			}
			else
			{
				this.scrollRect.content.localPosition = new Vector3(0f, (float)num5 * preferredHeight);
			}
		}

		// Token: 0x06003EEE RID: 16110 RVA: 0x00140418 File Offset: 0x0013E818
		private IEnumerable<BuildingShopData> GetShopData(ShopTag selected)
		{
			// 活动相关
			// this.SetupSeasonalContainers(selected);
			this.seasonalTimerContainerGrandPrize.SetActive(false); // 暂时设置为false
			// 和friend相关
			// this.SetupFriendContainer(selected);
		
			// 收起的建筑
			if (selected == ShopTag.Storage)
			{
				Dictionary<string, int> storedBuildings = this.stateService.Buildings.GetAllStoredBuildings();
				foreach (KeyValuePair<string, int> kv in storedBuildings)
				{
					BuildingConfig buildingConfig = this.config.buildingConfigList.GetConfig(kv.Key);
					if (buildingConfig == null)
					{
						WoogaDebug.LogWarning(new object[]
						{
							"Tried to show a stored building which is not in the config",
							kv.Key
						});
						break;
					}
					yield return new BuildingShopData
					{
						data = buildingConfig,
						buildingImage = this.resourceService.GetWrappedSpriteOrPlaceholder(buildingConfig).asset,
						buildingName = this.localeService.GetText(buildingConfig.Name_LocaleKey, new LocaParam[0]),
						message = kv.Value.ToString(),
						state = BuildingShopDataState.Stored,
						isReviewed = true,
						normalSpriteManager = this.normalSpriteManager
					};
				}
				bool noStoredItems = storedBuildings.Count == 0;
				this.storeEmpty.SetActive(noStoredItems);
			}
			else
			{
				this.storeEmpty.SetActive(false);	// 在“存储”页签没有东西时才显示storeEmpty
				// foreach (BuildingConfig building in this.GetBuildingConfigs(selected))
				foreach (BuildingConfig building in this.GetBuildingConfigsByStoreTag(selected))
				{
					int existingCount = this.stateService.Buildings.CountOfBuilding(building.name);
					BuildingShopDataState state = this.GetShopDataState(building, existingCount);
					bool isReviewed = this.stateService.Buildings.IsBuildingReviewed(building.name);
					string message = this.GetBuildingMessage(building);
					if (state == BuildingShopDataState.Stored)
					{
						message = this.stateService.Buildings.GetStoredBuildingsCount(building).ToString();
					}
					yield return new BuildingShopData
					{
						data = building,
						buildingImage = this.resourceService.GetWrappedSpriteOrPlaceholder(building).asset,
						buildingName = this.localeService.GetText(building.Name_LocaleKey, new LocaParam[0]),
						message = message,
						state = state,
						isReviewed = isReviewed,
						seasonSpriteManager = this.seasonSpriteManager,
						normalSpriteManager = this.normalSpriteManager
					};
					if (state == BuildingShopDataState.Available)
					{
						this.stateService.Buildings.SetBuildingReviewed(building.name);
					}
				}
			}
		}

		// Token: 0x06003EEF RID: 16111 RVA: 0x00140444 File Offset: 0x0013E844
		private void SetupSeasonalContainers(BuildingType buildingType)
		{
			bool isBuilding = buildingType == BuildingType.Building;
			bool flag2 = this.seasonService.GetGrandPrizeBuildingConfig() != null;
			bool active = isBuilding && !flag2;
			bool active2 = isBuilding && flag2;
			this.seasonalTimerContainer.SetActive(active);
			this.seasonalTimerContainerGrandPrize.SetActive(active2);
			if (isBuilding)
			{
				// eli key point 活动，头奖相关
				this.SetupSeasonalTimer();
				base.StartCoroutine(this.SetupGrandPrizeView());
			}
			else
			{
				this.grandPrizeView.Hide();
			}
		}

		// Token: 0x06003EF0 RID: 16112 RVA: 0x001404C4 File Offset: 0x0013E8C4
		private void SetupFriendContainer(BuildingType buildingType)
		{
			if (buildingType == BuildingType.Friend)
			{
				this.friendDecoPanel.SetActive(!this.facebookService.LoggedIn());
				this.contentPanel.SetActive(this.facebookService.LoggedIn());
				this.inviteFriendText.SetActive(this.facebookService.LoggedIn());
			}
			else
			{
				this.friendDecoPanel.SetActive(false);
				this.inviteFriendText.SetActive(false);
				this.contentPanel.SetActive(true);
			}
		}

		// Token: 0x06003EF1 RID: 16113 RVA: 0x0014054C File Offset: 0x0013E94C
		private IEnumerator SetupGrandPrizeView()
		{
			BuildingConfig buildingConfig = this.seasonService.GetGrandPrizeBuildingConfig();
			if (buildingConfig == null)
			{
				yield break;
			}
			SeasonConfig seasonConfig = this.seasonService.GetActiveSeason();
			BuildingShopData shopData = new BuildingShopData
			{
				data = buildingConfig,
				buildingImage = this.resourceService.GetWrappedSpriteOrPlaceholder(buildingConfig).asset,
				buildingName = this.localeService.GetText(buildingConfig.Name_LocaleKey, new LocaParam[0]),
				state = BuildingShopDataState.Available,
				isReviewed = true
			};
			Wooroutine<Texture> texture = this.abs.LoadAsset<Texture>(seasonConfig.PrimaryBundleName, seasonConfig.FxTexturePath);
			yield return texture;
			GrandPrizeView.Config config = new GrandPrizeView.Config
			{
				shopData = shopData,
				seasonSpriteManager = this.seasonSpriteManager,
				fxTexture = texture.ReturnValue,
				required = buildingConfig.costs[0].amount,
				collected = this.seasonService.GrandPrizeProgress,
				showInfoButton = this.seasonService.IsSeasonalsV3
			};
			this.grandPrizeView.Setup(config);
			yield break;
		}

		// Token: 0x06003EF2 RID: 16114 RVA: 0x00140568 File Offset: 0x0013E968
		private void SetupSeasonalTimer()
		{
			SeasonConfig activeSeason = this.seasonService.GetActiveSeason();
			if (activeSeason != null)
			{
				string title_loca_key = activeSeason.title_loca_key;
				string text = this.localeService.GetText(title_loca_key, new LocaParam[0]);
				this.seasonalTitle.text = text;
				this.seasonalTitleGrandPrize.text = text;
				DateTime endDate = activeSeason.EndDate;
				this.seasonalTimer.SetTargetTime(endDate, true, null);
				this.seasonalTimerGrandPrize.SetTargetTime(endDate, true, null);
			}
		}

		// Token: 0x06003EF3 RID: 16115 RVA: 0x001405DC File Offset: 0x0013E9DC
		private BuildingShopDataState GetShopDataState(BuildingConfig building)
		{
			return this.GetShopDataState(building, this.stateService.Buildings.CountOfBuilding(building.name));
		}

		// Token: 0x06003EF4 RID: 16116 RVA: 0x001405FC File Offset: 0x0013E9FC
		private BuildingShopDataState GetShopDataState(BuildingConfig building, int existingCount)
		{
			if (this.stateService.Buildings.IsBuildingStored(building.name))
			{
				return BuildingShopDataState.Stored;
			}
			if (building.max_number > 0 && building.max_number <= existingCount)
			{
				return BuildingShopDataState.LimitReached;
			}
			if (!this.quests.IsLevelUnlocked(building.unlock_level))
			{
				return BuildingShopDataState.Locked;
			}
			if (building.chapter_id > this.quests.Chapter)
			{
				return BuildingShopDataState.Locked;
			}
			if (!building.IsFriendDeco())
			{
				return BuildingShopDataState.Available;
			}
			if (!this.facebookService.LoggedIn())
			{
				return BuildingShopDataState.LockedByFb;
			}
			if (building.name == "fbButton")
			{
				return BuildingShopDataState.InviteFriend;
			}
			if (building.UnlockResource.type == "friends" && building.UnlockResource.amount <= this.stateService.Facebook.friendCountHighWater)
			{
				return BuildingShopDataState.Available;
			}
			return BuildingShopDataState.LockedByFb;
		}

		// Token: 0x06003EF5 RID: 16117 RVA: 0x001406EC File Offset: 0x0013EAEC
		private string GetBuildingMessage(BuildingConfig building)
		{
			if (building.chapter_id > this.quests.Chapter)
			{
				return string.Format(this.localeService.GetText("shop.buildings.chapter_locked", new LocaParam[0]), building.chapter_id + 1);
			}
			if (!this.quests.IsLevelUnlocked(building.unlock_level))
			{
				return string.Format(this.localeService.GetText("shop.buildings.level_locked", new LocaParam[0]), building.unlock_level);
			}
			if (building.IsFriendDeco() && this.stateService.Facebook.friendCountHighWater < building.UnlockResource.amount)
			{
				return string.Format(this.localeService.GetText("ui.shop.friendsdecos.button.text.2", new LocaParam[0]), building.UnlockResource.amount);
			}
			return "Unknown lock reason";
		}

		// Token: 0x06003EF6 RID: 16118 RVA: 0x001407D7 File Offset: 0x0013EBD7
		private IEnumerable<BuildingShopTabData> GetShopTabs()
		{
			return Array.ConvertAll<ShopTag, BuildingShopTabData>(this.shopTabs, (ShopTag tab) => this.GetShopTabData(tab));
		}

		// 设置页签的图标
		private BuildingShopTabData GetShopTabData(ShopTag category)
		{
			BuildingShopTabData buildingShopTabData = new BuildingShopTabData
			{
				data = category,
				badgeCount = ((category != ShopTag.Storage) ? this.CountNewItems(category) : this.stateService.Buildings.GetCountOfAllStoredBuildings()),
				state = ((category != this.selectedType) ? UiTabState.Inactive : UiTabState.Active)
			};
			// if (category == BuildingType.Building)
			// {
			// 	buildingShopTabData.sprite = this.seasonalIcon;
			// }
			// else if (category == BuildingType.Friend)
			// {
			// 	buildingShopTabData.sprite = this.friendDecoIcon;
			// }
			return buildingShopTabData;
		}

		// Token: 0x06003EF8 RID: 16120 RVA: 0x00140882 File Offset: 0x0013EC82
		private int CountNewItems(ShopTag category)
		{
			return this.GetBuildingConfigsByStoreTag(category).CountIf(new Func<BuildingConfig, bool>(this.IsBuildingNewAndAvailable));
		}

		// Token: 0x0400681C RID: 26652
		public const string FRIEND_UNLOCK_RESOURCE = "friends";

		// Token: 0x0400681D RID: 26653
		public const string FRIEND_DECO_BUNDLE_NAME = "buildings_friendship_2018";

		// Token: 0x0400681E RID: 26654
		public const string FRIEND_DECO_SPRITE_MANAGER_PATH = "Assets/Puzzletown/Town/Art/Buildings/friendship_decos_2018/FriendDecoSpriteManager.prefab";

		// Token: 0x0400681F RID: 26655
		[WaitForService(true, true)]
		private GameStateService stateService;

		// Token: 0x04006820 RID: 26656
		[WaitForService(true, true)]
		private ConfigService config;

		// Token: 0x04006821 RID: 26657
		[WaitForService(true, true)]
		private QuestService quests;

		// Token: 0x04006822 RID: 26658
		[WaitForService(true, true)]
		private ILocalizationService localeService;

		// Token: 0x04006823 RID: 26659
		[WaitForService(true, true)]
		private AudioService audioService;

		// Token: 0x04006824 RID: 26660
		[WaitForService(true, true)]
		private SBSService sbsService;

		// Token: 0x04006825 RID: 26661
		[WaitForService(true, true)]
		private AssetBundleService abs;

		// Token: 0x04006826 RID: 26662
		[WaitForService(true, true)]
		private SeasonService seasonService;

		// Token: 0x04006827 RID: 26663
		[WaitForService(true, true)]
		private FacebookService facebookService;

		// Token: 0x04006828 RID: 26664
		[WaitForService(true, true)]
		private ProgressionDataService.Service progression;

		// Token: 0x04006829 RID: 26665
		[WaitForRoot(false, false)]
		private BuildingResourceServiceRoot resourceService;

		// Token: 0x0400682A RID: 26666
		[WaitForRoot(false, false)]
		private TownMainRoot townMain;

		// Token: 0x0400682B RID: 26667
		public AwaitSignal onShopReady = new AwaitSignal();

		// Token: 0x0400682C RID: 26668
		public readonly Signal<BuildingShopView.BuildingBuildRequest> onBuildingClick = new Signal<BuildingShopView.BuildingBuildRequest>();

		// Token: 0x0400682D RID: 26669
		public readonly Signal onClose = new Signal();

		// Token: 0x0400682E RID: 26670
		public BuildingShopDataSource buildings;

		// Token: 0x0400682F RID: 26671
		public BuildingShopTabsDataSource categories;

		// Token: 0x04006830 RID: 26672
		public AnimatedUi dialog;

		// Token: 0x04006831 RID: 26673
		public ScrollRect scrollRect;

		// Token: 0x04006832 RID: 26674
		public GameObject storeEmpty;

		// Token: 0x04006833 RID: 26675
		public ShopTag[] shopTabs;

		// Token: 0x04006834 RID: 26676
		[SerializeField]
		private GridLayoutGroup scrollGridLayout;

		// Token: 0x04006835 RID: 26677
		[SerializeField]
		private TextMeshProUGUI seasonalTitle;

		// Token: 0x04006836 RID: 26678
		[SerializeField]
		private GameObject seasonalTimerContainer;

		// Token: 0x04006837 RID: 26679
		[SerializeField]
		private CountdownTimer seasonalTimer;

		// Token: 0x04006838 RID: 26680
		[SerializeField]
		private TextMeshProUGUI seasonalTitleGrandPrize;

		// Token: 0x04006839 RID: 26681
		[SerializeField]
		private GameObject seasonalTimerContainerGrandPrize;

		// Token: 0x0400683A RID: 26682
		[SerializeField]
		private CountdownTimer seasonalTimerGrandPrize;

		// Token: 0x0400683B RID: 26683
		[SerializeField]
		private GrandPrizeView grandPrizeView;

		// Token: 0x0400683C RID: 26684
		[SerializeField]
		private GameObject friendDecoPanel;

		// Token: 0x0400683D RID: 26685
		[SerializeField]
		private Image friendDecoPanelImage;

		// Token: 0x0400683E RID: 26686
		[SerializeField]
		private Image friendCellBackgroundImage;

		// Token: 0x0400683F RID: 26687
		[SerializeField]
		private GameObject inviteFriendText;

		// Token: 0x04006840 RID: 26688
		[SerializeField]
		private GameObject contentPanel;

		// Token: 0x04006841 RID: 26689
		[SerializeField]
		private SpriteManager normalSpriteManager;

		// Token: 0x04006842 RID: 26690
		[Header("Tutorials")]
		[SerializeField]
		private TutorialMarker seasonalsV3TutorialMarker;

		// Token: 0x04006843 RID: 26691
		private BuildingServices buildingsService;

		// Token: 0x04006844 RID: 26692
		private ShopTag selectedType = ShopTag.RarityLevel1;

		// Token: 0x04006845 RID: 26693
		private QuestTaskData[] activeQuests;

		// Token: 0x04006846 RID: 26694
		private Sprite seasonalIcon;

		// Token: 0x04006847 RID: 26695
		private Sprite friendDecoIcon;

		// Token: 0x04006848 RID: 26696
		private SpriteManager seasonSpriteManager;

		// Token: 0x04006849 RID: 26697
		private bool seasonalTabInitialized;

		// Token: 0x0400684A RID: 26698
		private bool buildingRequestHandled;
	}
}

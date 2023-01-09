using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Puzzletown.Match3;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x02000761 RID: 1889
	public class ConfigService : AService
	{
		// Token: 0x06002EDA RID: 11994 RVA: 0x000DAE7F File Offset: 0x000D927F
		public ConfigService()
		{
			if (Application.isPlaying)
			{
				WooroutineRunner.StartCoroutine(this.InitRoutine(), null);
			}
		}

		// Token: 0x1700074C RID: 1868
		// (get) Token: 0x06002EDB RID: 11995 RVA: 0x000DAE9E File Offset: 0x000D929E
		// (set) Token: 0x06002EDC RID: 11996 RVA: 0x000DAEA6 File Offset: 0x000D92A6
		public bool IsValid { get; protected set; }

		// Token: 0x1700074D RID: 1869
		// (get) Token: 0x06002EDD RID: 11997 RVA: 0x000DAEAF File Offset: 0x000D92AF
		public PTConfig SbsConfig
		{
			get
			{
				return this.sbs.SbsConfig;
			}
		}

		// Token: 0x1700074E RID: 1870
		// (get) Token: 0x06002EDE RID: 11998 RVA: 0x000DAEBC File Offset: 0x000D92BC
		public FeatureSwitchesConfig FeatureSwitchesConfig
		{
			get
			{
				return this.SbsConfig.feature_switches;
			}
		}

		// Token: 0x06002EDF RID: 11999 RVA: 0x000DAECC File Offset: 0x000D92CC
		private IEnumerator InitRoutine()
		{
			yield return ServiceLocator.Instance.Inject(this);
			FieldInfo[] fieldInfos = this.GetFieldTypes();
			List<KeyValuePair<string, string>> assetDescriptors = this.GetAssetDescriptors(fieldInfos);
			Wooroutine<List<TextAsset>> configs = this.bundleService.LoadAssets<TextAsset>(assetDescriptors);
			yield return configs;
			try
			{
				for (int i = 0; i < fieldInfos.Length; i++)
				{
					object obj = Activator.CreateInstance(fieldInfos[i].FieldType);
					JsonUtility.FromJsonOverwrite(configs.ReturnValue[i].text, obj);
					this.Set(fieldInfos[i].Name, obj);
					IInitializable initializable = obj as IInitializable;
					if (initializable != null)
					{
						initializable.Init();
					}
				}
				this.buildingConfigList.SetupSeasonalsV3ABTest(this.SbsConfig.feature_switches.seasonals_v3);
				this.IsValid = true;
				
				// 审核版只开放前10个章节, 并去掉一些建筑
				// #if REVIEW_VERSION
				// {
				// 	ChapterData[] temp_data = new ChapterData[10];
				// 	Array.Copy(chapter.chapters, temp_data, 10);
				// 	chapter.chapters = temp_data;
				//
				// 	general.tier_unlocked.last_area = 5;
				// }
				// #endif
			}
			catch (Exception ex)
			{
				Debug.LogError(ex.StackTrace);
				WoogaDebug.LogError(new object[]
				{
					"[ConfigService] Exception",
					ex
				});
			}
			finally
			{
				base.OnInitialized.Dispatch();
			}
		}

		// Token: 0x06002EE0 RID: 12000 RVA: 0x000DAEE8 File Offset: 0x000D92E8
		protected List<KeyValuePair<string, string>> GetAssetDescriptors(FieldInfo[] fieldInfos)
		{
			List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
			foreach (FieldInfo type in fieldInfos)
			{
				string configFileName = ConfigService.GetConfigFileName(type);
				string key = "config";
				list.Add(new KeyValuePair<string, string>(key, configFileName));
			}
			return list;
		}

		// eli key point: GetConfigFileName() 获取配置文件路径 通过反射来赋值
		public static string GetConfigFileName(FieldInfo type)
		{
			string str = (!type.FieldType.Name.EndsWith("Config")) ? string.Empty : "config";
			string str2 = (type.Name + str).ToLower();
			return "Assets/Puzzletown/Config/Json/" + str2 + ".json.txt";
		}

		// Token: 0x06002EE2 RID: 12002 RVA: 0x000DAF8D File Offset: 0x000D938D
		public FieldInfo[] GetFieldTypes()
		{
			return base.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
		}

		// Token: 0x06002EE3 RID: 12003 RVA: 0x000DAF9C File Offset: 0x000D939C
		public void Set(string fieldName, object value)
		{
			FieldInfo[] fields = base.GetType().GetFields();
			foreach (FieldInfo fieldInfo in fields)
			{
				if (fieldInfo.Name == fieldName)
				{
					fieldInfo.SetValue(this, value);
					return;
				}
			}
			throw new InvalidOperationException("Cannot find type in ConfigContainer class: " + value.GetType());
		}

		// Token: 0x06002EE4 RID: 12004 RVA: 0x000DB000 File Offset: 0x000D9400
		public T Get<T>()
		{
			FieldInfo[] fields = base.GetType().GetFields();
			foreach (FieldInfo fieldInfo in fields)
			{
				if (fieldInfo.FieldType == typeof(T))
				{
					return (T)((object)fieldInfo.GetValue(this));
				}
			}
			throw new InvalidOperationException("Cannot find type in ConfigContainer class: " + typeof(T));
		}

		// Token: 0x0400580A RID: 22538
		public const string JSON_PATH = "Assets/Puzzletown/Config/Json/";

		// Token: 0x0400580B RID: 22539
		public const string SBS_JSON_PATH = "Assets/Resources/SbsConfigService/";

		// Token: 0x0400580C RID: 22540
		[WaitForService(true, true)]
		protected AssetBundleService bundleService;

		// Token: 0x0400580D RID: 22541
		[WaitForService(true, true)]
		protected SBSService sbs;

		// Token: 0x0400580F RID: 22543
		public ForceUpdateConfig forceUpdate;

		// Token: 0x04005810 RID: 22544
		public BuildingConfigList buildingConfigList;

		// Token: 0x04005811 RID: 22545
		public StartBuildingConfig startBuilding;

		// Token: 0x04005812 RID: 22546
		public AreasConfig areas;

		// Token: 0x04005813 RID: 22547
		public GeneralConfig general;

		// Token: 0x04005814 RID: 22548
		public IAPConfigDataList iapConfigDataList;

		// Token: 0x04005815 RID: 22549
		public ChapterConfig chapter;

		// Token: 0x04005816 RID: 22550
		public VillagerConfig villager;

		// Token: 0x04005817 RID: 22551
		public VillagerBehaviorConfig villagerBehavior;

		// Token: 0x04005818 RID: 22552
		public StoryDialogueConfig storyDialogue;
	}
}

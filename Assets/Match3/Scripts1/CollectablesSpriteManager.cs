using System;
using System.Collections;
using Match3.Scripts1.Puzzletown.Services;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using UnityEngine;

// Token: 0x02000830 RID: 2096
namespace Match3.Scripts1
{
	public class CollectablesSpriteManager : SpriteManager
	{
		// Token: 0x06003427 RID: 13351 RVA: 0x000F8735 File Offset: 0x000F6B35
		public override void Initialize()
		{
			base.Initialize();
			WooroutineRunner.StartCoroutine(this.SetupRoutine(), null);
		}

		// Token: 0x06003428 RID: 13352 RVA: 0x000F874C File Offset: 0x000F6B4C
		private IEnumerator SetupRoutine()
		{
			yield return ServiceLocator.Instance.Inject(this);
			this.LoadQuestItems(this.questService.questManager.CurrentQuestProgress);
			this.questService.questManager.OnQuestChanged.AddListener(new Action<QuestProgress>(this.LoadQuestItems));
			yield break;
		}

		// Token: 0x06003429 RID: 13353 RVA: 0x000F8767 File Offset: 0x000F6B67
		private void OnDestroy()
		{
			this.questService.questManager.OnQuestChanged.RemoveListener(new Action<QuestProgress>(this.LoadQuestItems));
		}

		// Token: 0x0600342A RID: 13354 RVA: 0x000F878A File Offset: 0x000F6B8A
		private void LoadQuestItems(QuestProgress progress)
		{
			if (progress != null && progress.configData != null)
			{
				WooroutineRunner.StartCoroutine(this.LoadQuestItems(progress.configData), null);
			}
		}

		// Token: 0x0600342B RID: 13355 RVA: 0x000F87B0 File Offset: 0x000F6BB0
		private IEnumerator LoadQuestItems(QuestData questData)
		{
			foreach (string taskname in questData.task_item)
			{
				if (!this.m_lookup.ContainsKey(taskname))
				{
					// eli key point: 载入任务图片
					string spritePath = string.Format("Assets/Puzzletown/Shared/Art/Collectables/Bundle/ui_item_{0}.png", taskname);
					global::UnityEngine.Debug.Log("[CollectableSpriteManager] loading: " + taskname + " from " + spritePath);
					Wooroutine<Sprite> spriteLoader = this.assetBundleService.LoadAsset<Sprite>("collectables", spritePath);
					yield return spriteLoader;
					if (spriteLoader.ReturnValue != null)
					{
						this.m_lookup[taskname] = spriteLoader.ReturnValue;
					}
				}
			}
			yield break;
		}

		// Token: 0x04005C00 RID: 23552
		public const string SPRITE_PATH = "Assets/Puzzletown/Shared/Art/Collectables/Bundle/ui_item_{0}.png";

		// Token: 0x04005C01 RID: 23553
		public const string BUNDLE_NAME = "collectables";

		// Token: 0x04005C02 RID: 23554
		[WaitForService(true, true)]
		private QuestService questService;

		// Token: 0x04005C03 RID: 23555
		[WaitForService(true, true)]
		private AssetBundleService assetBundleService;
	}
}

using System.Collections;
using Match3.Scripts1.Puzzletown.Services;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3.UI.DataViews
{
	// Token: 0x020006D4 RID: 1748
	public class BuildingMaterialAmountView : MaterialAmountView
	{
		// Token: 0x06002B87 RID: 11143 RVA: 0x000C8218 File Offset: 0x000C6618
		public override void Refresh(MaterialAmount mat)
		{
			base.Refresh(mat);
			if (this.image && this.manager && !this.data.type.IsNullOrEmpty())
			{
				if (this.data.type.StartsWith("iso_"))
				{
					if (base.gameObject.activeInHierarchy)
					{
						this.image.color = Color.clear;
						WooroutineRunner.StartCoroutine(this.ShowBuildingRoutine(), null);
					}
				}
				else
				{
					this.image.sprite = this.manager.GetSimilar(this.data.type);
				}
			}
		}

		// Token: 0x06002B88 RID: 11144 RVA: 0x000C82D0 File Offset: 0x000C66D0
		private IEnumerator ShowBuildingRoutine()
		{
			Wooroutine<ConfigService> configServiceLoader = ServiceLocator.Instance.Await<ConfigService>(true);
			yield return configServiceLoader;
			ConfigService configService = configServiceLoader.ReturnValue;
			Wooroutine<BuildingResourceServiceRoot> resourceServiceLoader = SceneManager.Instance.Await<BuildingResourceServiceRoot>(true);
			yield return resourceServiceLoader;
			BuildingResourceServiceRoot resourceService = resourceServiceLoader.ReturnValue;
			BuildingConfig buildingConfig = configService.buildingConfigList.GetConfig(this.data.type);
			this.image.sprite = resourceService.GetWrappedSpriteOrPlaceholder(buildingConfig).asset;
			this.image.color = Color.white;
			yield return null;
			yield break;
		}
	}
}

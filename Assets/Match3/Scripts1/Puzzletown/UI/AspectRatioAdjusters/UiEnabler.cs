using System;
using System.Collections;
using Wooga.Coroutines;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.UI.AspectRatioAdjusters
{
	// Token: 0x02000896 RID: 2198
	public class UiEnabler : AUiAdjuster
	{
		// Token: 0x060035CD RID: 13773 RVA: 0x00102BC4 File Offset: 0x00100FC4
		protected override void AdjustValues()
		{
			foreach (UiEnabler.Setting setting in this.enabledOrientations)
			{
				ScreenOrientation screenOrientation;
				ScreenOrientation screenOrientation2;
				if (this.useSimilarOrientations)
				{
					screenOrientation = AUiAdjuster.SimilarOrientation;
					screenOrientation2 = AUiAdjuster.GetSimilarOrientation(setting.orientation);
				}
				else
				{
					screenOrientation = AUiAdjuster.Orientation;
					screenOrientation2 = setting.orientation;
				}
				if (screenOrientation2 == screenOrientation)
				{
					base.gameObject.SetActive(true);
					return;
				}
			}
			base.gameObject.SetActive(false);
		}

		// Token: 0x060035CE RID: 13774 RVA: 0x00102C48 File Offset: 0x00101048
		private IEnumerator DisabledUpdate()
		{
			for (;;)
			{
				yield return null;
				base.Update();
			}
			yield break;
		}

		// Token: 0x060035CF RID: 13775 RVA: 0x00102C63 File Offset: 0x00101063
		private void OnEnable()
		{
			if (this.updateRoutine != null)
			{
				WooroutineRunner.Stop(this.updateRoutine);
			}
		}

		// Token: 0x060035D0 RID: 13776 RVA: 0x00102C7B File Offset: 0x0010107B
		private void OnDisable()
		{
			if (this.updateRoutine != null)
			{
				base.StopCoroutine(this.updateRoutine);
			}
			this.updateRoutine = WooroutineRunner.StartCoroutine(this.DisabledUpdate(), null);
		}

		// Token: 0x060035D1 RID: 13777 RVA: 0x00102CA6 File Offset: 0x001010A6
		private void OnDestroy()
		{
			if (this.updateRoutine != null)
			{
				WooroutineRunner.Stop(this.updateRoutine);
			}
		}

		// Token: 0x04005DC7 RID: 24007
		[Tooltip("For example: 'LandscapeLeft' and 'Landscape' will both work for 'Landscape'")]
		[SerializeField]
		private bool useSimilarOrientations = true;

		// Token: 0x04005DC8 RID: 24008
		[SerializeField]
		private UiEnabler.Setting[] enabledOrientations;

		// Token: 0x04005DC9 RID: 24009
		private Coroutine updateRoutine;

		// Token: 0x02000897 RID: 2199
		[Serializable]
		public class Setting : AUiAdjuster.UiAdjusterSetting
		{
		}
	}
}

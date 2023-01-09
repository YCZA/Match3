using System.Collections;
using Match3.Scripts1.Puzzletown.Services;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x020009CC RID: 2508
	public class IslandSwitchButton : MonoBehaviour
	{
		// Token: 0x17000914 RID: 2324
		// (get) Token: 0x06003CBA RID: 15546 RVA: 0x001308CA File Offset: 0x0012ECCA
		// (set) Token: 0x06003CBB RID: 15547 RVA: 0x001308D2 File Offset: 0x0012ECD2
		public bool AllBundlesAreAvailable { get; private set; }

		// Token: 0x06003CBC RID: 15548 RVA: 0x001308DC File Offset: 0x0012ECDC
		private IEnumerator Start()
		{
			yield return ServiceLocator.Instance.Inject(this);
			foreach (string bundleName in this.requiredBundles)
			{
				this.abs.PreLoadBundle(bundleName, null);
			}
			if (this.refreshRoutine == null)
			{
				this.refreshRoutine = WooroutineRunner.StartCoroutine(this.CheckSpinnerVisibilityRoutine(), null);
			}
			yield break;
		}

		// Token: 0x06003CBD RID: 15549 RVA: 0x001308F7 File Offset: 0x0012ECF7
		private void OnDestroy()
		{
			if (this.refreshRoutine != null)
			{
				WooroutineRunner.Stop(this.refreshRoutine);
				this.refreshRoutine = null;
			}
		}

		// Token: 0x06003CBE RID: 15550 RVA: 0x00130918 File Offset: 0x0012ED18
		private IEnumerator CheckSpinnerVisibilityRoutine()
		{
			WaitForSeconds wait = new WaitForSeconds(this.updateIntervalSecs);
			while (!this.AllBundlesAreAvailable)
			{
				yield return this.CheckBundleAvailabilityRoutine();
				yield return wait;
			}
			if (this.AllBundlesAreAvailable)
			{
				this.refreshRoutine = null;
			}
			yield break;
		}

		// Token: 0x06003CBF RID: 15551 RVA: 0x00130934 File Offset: 0x0012ED34
		private IEnumerator CheckBundleAvailabilityRoutine()
		{
			if (this.requiredBundles != null && this.requiredBundles.Length > 0)
			{
				Wooroutine<bool> routine = this.abs.AreAllBundlesAvailable(this.requiredBundles);
				yield return routine;
				this.AllBundlesAreAvailable = routine.ReturnValue;
			}
			else
			{
				this.AllBundlesAreAvailable = true;
			}
			this.RefreshSpinner();
			yield break;
		}

		// Token: 0x06003CC0 RID: 15552 RVA: 0x00130950 File Offset: 0x0012ED50
		private void RefreshSpinner()
		{
			if (this.spinner != null)
			{
				this.spinner.SetActive(!this.AllBundlesAreAvailable);
			}
			if (this.shipIcon != null)
			{
				this.shipIcon.gameObject.SetActive(this.AllBundlesAreAvailable);
			}
		}

		// Token: 0x04006578 RID: 25976
		public GameObject spinner;

		// Token: 0x04006579 RID: 25977
		public GameObject shipIcon;

		// Token: 0x0400657A RID: 25978
		public string[] requiredBundles;

		// Token: 0x0400657B RID: 25979
		[Range(0.1f, 2f)]
		public float updateIntervalSecs;

		// Token: 0x0400657D RID: 25981
		private Coroutine refreshRoutine;

		// Token: 0x0400657E RID: 25982
		[WaitForService(true, true)]
		private AssetBundleService abs;
	}
}

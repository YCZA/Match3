using System.Collections;
using Wooga.Coroutines;
using UnityEngine;

// Token: 0x02000B32 RID: 2866
namespace Match3.Scripts1
{
	public class IphoneXOverlayRoot : APtSceneRoot
	{
		// Token: 0x0600433E RID: 17214 RVA: 0x00157D00 File Offset: 0x00156100
		private void Start()
		{
			global::UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			WooroutineRunner.StartCoroutine(this.UpdateRoutine(), null);
		}

		// Token: 0x0600433F RID: 17215 RVA: 0x00157D1C File Offset: 0x0015611C
		private IEnumerator UpdateRoutine()
		{
			bool isIphoneXResolution = false;
			for (;;)
			{
				yield return new WaitForSeconds(1f);
				isIphoneXResolution = (Screen.width == 1125 && Screen.height == 2436);
				isIphoneXResolution |= (Screen.width == 2436 && Screen.height == 1125);
				base.gameObject.SetActive(isIphoneXResolution);
			}
			yield break;
		}
	}
}

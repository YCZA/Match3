using System.Collections;
using DG.Tweening;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Services;
using Wooga.UnityFramework;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000842 RID: 2114
namespace Match3.Scripts1
{
	public class BannerNewCollectableRoot : APtSceneRoot<string>, IDisposableDialog
	{
		// Token: 0x06003470 RID: 13424 RVA: 0x000FAE18 File Offset: 0x000F9218
		protected override IEnumerator GoRoutine()
		{
			if (base.registeredFirst)
			{
				this.parameters = "coins";
			}
			this.collectable.sprite = this.collectablesManager.GetSimilar(this.parameters, false);
			if (!this.collectable.sprite)
			{
				WoogaDebug.LogWarning(new object[]
				{
					"Could not find collectable",
					this.parameters
				});
				base.Destroy();
				yield break;
			}
			Vector3 a = this.collectable.transform.position;
			Vector3 b = this.target.position;
			Vector3 c = Vector3.Cross(b - a, Vector3.forward) * this.curve + Vector3.Lerp(a, b, this.curvePosition);
			Vector3[] points = new Vector3[]
			{
				a,
				c,
				b
			};
			base.GetComponentInChildren<Canvas>().enabled = true;
			this.audioService.PlaySFX(AudioId.QuestItemAppear, false, false, false);
			yield return new WaitForSeconds(this.idleTime);
			this.doober.DOPath(points, this.flyDuration, PathType.CatmullRom, PathMode.Full3D, 10, null);
			yield return new WaitForSeconds(this.flyDuration);
			this.audioService.PlaySFX(AudioId.QuestItemAddedToDiary, false, false, false);
			yield return new WaitForSeconds(this.cooldownTime);
			base.Destroy();
			yield break;
		}

		// Token: 0x06003471 RID: 13425 RVA: 0x000FAE33 File Offset: 0x000F9233
		private void Update()
		{
			if (global::UnityEngine.Input.touchCount > 0 || Input.GetMouseButton(0))
			{
				Time.timeScale = 2f;
			}
		}

		// Token: 0x06003472 RID: 13426 RVA: 0x000FAE55 File Offset: 0x000F9255
		protected override void OnDisable()
		{
			base.OnDisable();
			Time.timeScale = 1f;
		}

		// Token: 0x04005C5D RID: 23645
		[WaitForService(true, true)]
		private AudioService audioService;

		// Token: 0x04005C5E RID: 23646
		public SpriteManager collectablesManager;

		// Token: 0x04005C5F RID: 23647
		public Image collectable;

		// Token: 0x04005C60 RID: 23648
		public Transform doober;

		// Token: 0x04005C61 RID: 23649
		public Transform target;

		// Token: 0x04005C62 RID: 23650
		public float idleTime = 0.5f;

		// Token: 0x04005C63 RID: 23651
		public float flyDuration = 1f;

		// Token: 0x04005C64 RID: 23652
		public float curve = 0.5f;

		// Token: 0x04005C65 RID: 23653
		public float curvePosition = 0.5f;

		// Token: 0x04005C66 RID: 23654
		public float cooldownTime = 2f;
	}
}

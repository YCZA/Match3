using System.Collections;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Services;
using Wooga.UnityFramework;
using UnityEngine;

// Token: 0x0200002D RID: 45
namespace Match3.Scripts1
{
	public class SoundTriggerSelect : MonoBehaviour
	{
		// Token: 0x06000171 RID: 369 RVA: 0x00008A04 File Offset: 0x00006E04
		private IEnumerator Start()
		{
			yield return ServiceLocator.Instance.Inject(this);
			yield break;
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00008A1F File Offset: 0x00006E1F
		public void PlaySound()
		{
			if (this.audioService != null)
			{
				this.audioService.PlaySFX(this.soundToPlay, false, false, false);
			}
		}

		// Token: 0x06000173 RID: 371 RVA: 0x00008A41 File Offset: 0x00006E41
		private void OnEnable()
		{
			if (this.audioService != null && this.playOnEnable)
			{
				this.audioService.PlaySFX(this.soundToPlay, false, false, false);
			}
		}

		// Token: 0x0400013B RID: 315
		[WaitForService(true, true)]
		private AudioService audioService;

		// Token: 0x0400013C RID: 316
		public AudioId soundToPlay;

		// Token: 0x0400013D RID: 317
		public bool playOnEnable;
	}
}

using System.Collections;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Services;
using Wooga.UnityFramework;
using UnityEngine;

// Token: 0x0200002B RID: 43
namespace Match3.Scripts1
{
	public class PlaySoundFromAnimationEvent : MonoBehaviour
	{
		// Token: 0x0600016A RID: 362 RVA: 0x00008840 File Offset: 0x00006C40
		private IEnumerator Start()
		{
			yield return ServiceLocator.Instance.Inject(this);
			yield break;
		}

		// Token: 0x0600016B RID: 363 RVA: 0x0000885B File Offset: 0x00006C5B
		public void Play(AudioId id)
		{
			if (this.audioService != null)
			{
				this.audioService.PlaySFX(id, false, false, false);
			}
		}

		// Token: 0x04000138 RID: 312
		[WaitForService(true, true)]
		private AudioService audioService;
	}
}

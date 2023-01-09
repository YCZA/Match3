using System.Collections;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Services;
using Wooga.UnityFramework;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x0200002C RID: 44
namespace Match3.Scripts1
{
	public class SoundTrigger : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x0600016D RID: 365 RVA: 0x0000891C File Offset: 0x00006D1C
		public void OnPointerClick(PointerEventData eventData)
		{
			this.audioService.PlaySFX(this.audioId, false, false, false);
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00008934 File Offset: 0x00006D34
		private IEnumerator Start()
		{
			yield return ServiceLocator.Instance.Inject(this);
			yield break;
		}

		// Token: 0x0600016F RID: 367 RVA: 0x0000894F File Offset: 0x00006D4F
		public void OnPointerDownDelegate(PointerEventData data)
		{
			this.audioService.PlaySFX(this.audioId, false, false, false);
		}

		// Token: 0x04000139 RID: 313
		[WaitForService(true, true)]
		private AudioService audioService;

		// Token: 0x0400013A RID: 314
		public AudioId audioId = AudioId.NormalClick;
	}
}

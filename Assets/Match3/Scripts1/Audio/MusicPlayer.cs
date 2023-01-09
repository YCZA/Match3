using System.Collections;
using Match3.Scripts1.Puzzletown.Services;
using UnityEngine;

namespace Match3.Scripts1.Audio
{
	// Token: 0x02000029 RID: 41
	public class MusicPlayer : MonoBehaviour
	{
		// Token: 0x06000163 RID: 355 RVA: 0x00008544 File Offset: 0x00006944
		public void SetMusicTracks(MusicTrack[] tracks)
		{
			this.musicTracks = tracks;
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00008550 File Offset: 0x00006950
		public void Init(AudioService audioService)
		{
			this.audioService = audioService;
			if (this.soundscape != null)
			{
				audioService.PlayMusic(this.soundscape.track, this.soundscape.volume, false, true, false);
			}
			if (!this.musicTracks.IsNullOrEmptyCollection())
			{
				base.StartCoroutine(this.MusicRepeaterRoutine());
			}
		}

		// Token: 0x06000165 RID: 357 RVA: 0x000085AC File Offset: 0x000069AC
		private IEnumerator MusicRepeaterRoutine()
		{
			for (int i = 0; i < this.musicTracks.Length; i++)
			{
				AudioClip clip2 = this.GetClip(this.musicTracks[i]);
				if (clip2 != null && this.audioService.IsMusicClipPlaying(clip2))
				{
					this.trackIndex = i;
					break;
				}
			}
			for (;;)
			{
				MusicTrack track = this.musicTracks[this.trackIndex];
				AudioClip clip = this.GetClip(track);
				if (clip != null)
				{
					this.musicIntervalWait = new WaitForSeconds(this.musicInterval + clip.length);
					this.audioService.PlayMusic(clip, track.volume, false, false, true);
					yield return this.musicIntervalWait;
				}
				this.trackIndex++;
				this.trackIndex %= this.musicTracks.Length;
			}
			yield break;
		}

		// Token: 0x06000166 RID: 358 RVA: 0x000085C8 File Offset: 0x000069C8
		private void OnDestroy()
		{
			for (int i = 0; i < this.musicTracks.Length; i++)
			{
				AudioClip clip = this.GetClip(this.musicTracks[i]);
				if (clip != null)
				{
					clip.UnloadAudioData();
				}
			}
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00008610 File Offset: 0x00006A10
		private AudioClip GetClip(MusicTrack track)
		{
			if (track == null || this.audioService == null)
			{
				return null;
			}
			return (!(track.track != null)) ? this.audioService.GetAudioClip(track.audioId) : track.track;
		}

		// Token: 0x0400012F RID: 303
		[SerializeField]
		private MusicTrack soundscape;

		// Token: 0x04000130 RID: 304
		[SerializeField]
		private float musicInterval;

		// Token: 0x04000131 RID: 305
		[SerializeField]
		private MusicTrack[] musicTracks;

		// Token: 0x04000132 RID: 306
		private AudioService audioService;

		// Token: 0x04000133 RID: 307
		private int trackIndex;

		// Token: 0x04000134 RID: 308
		private WaitForSeconds musicIntervalWait;
	}
}

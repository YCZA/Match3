using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Match3.Scripts1.Audio;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using UnityEngine;
using UnityEngine.Audio;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x0200074D RID: 1869
	public class AudioService : AService
	{
		// Token: 0x06002E3A RID: 11834 RVA: 0x000D79B4 File Offset: 0x000D5DB4
		public AudioService()
		{
			WooroutineRunner.StartCoroutine(this.InitRoutine(), null);
		}

		// Token: 0x17000722 RID: 1826
		// (get) Token: 0x06002E3B RID: 11835 RVA: 0x000D7A07 File Offset: 0x000D5E07
		public AudioMixer Mixer
		{
			get
			{
				return this.mixer;
			}
		}

		// Token: 0x06002E3C RID: 11836 RVA: 0x000D7A10 File Offset: 0x000D5E10
		private IEnumerator InitRoutine()
		{
			this.mixer = (Resources.Load("Audio/Mixer/GeneralMixer") as AudioMixer);
			this.musicGroup = this.mixer.FindMatchingGroups("Music")[0];
			this.sfxGroup = this.mixer.FindMatchingGroups("SFX")[0];
			this.voiceGroup = this.mixer.FindMatchingGroups("Voice")[0];
			this.mixer.ClearFloat(AudioGroup.Master.ToString());
			this.CreateAudioGroupStates();
			yield return ServiceLocator.Instance.Inject(this);
			this.CreateAudioResourcesManager();
			base.OnInitialized.Dispatch();
			yield break;
		}

		// Token: 0x06002E3D RID: 11837 RVA: 0x000D7A2C File Offset: 0x000D5E2C
		public void Enable()
		{
			this.mixer.ClearFloat(AudioGroup.Master.ToString());
		}

		// Token: 0x06002E3E RID: 11838 RVA: 0x000D7A54 File Offset: 0x000D5E54
		public void Disable()
		{
			this.audioResourcesManager.Reset();
		}

		// Token: 0x06002E3F RID: 11839 RVA: 0x000D7A61 File Offset: 0x000D5E61
		public AudioClip GetAudioClip(AudioId audioId)
		{
			if (this.audioResourcesManager.AudioRegistry.audioItemDictionary.ContainsKey(audioId))
			{
				return this.audioResourcesManager.AudioRegistry.audioItemDictionary[audioId].audioClip;
			}
			return null;
		}

		// Token: 0x06002E40 RID: 11840 RVA: 0x000D7A9C File Offset: 0x000D5E9C
		public AudioId GetSimilar(string substring)
		{
			if (this.getSimilarCache.ContainsKey(substring))
			{
				return this.getSimilarCache[substring];
			}
			AudioId audioId = AudioId.Default;
			IEnumerator enumerator = Enum.GetValues(typeof(AudioId)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					AudioId audioId2 = (AudioId)obj;
					if (audioId2.ToString().Contains(substring, StringComparison.OrdinalIgnoreCase))
					{
						audioId = audioId2;
						break;
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			this.getSimilarCache.Add(substring, audioId);
			return audioId;
		}

		// Token: 0x06002E41 RID: 11841 RVA: 0x000D7B54 File Offset: 0x000D5F54
		public void LoadSettings()
		{
			bool toggle = this.settingsService.GetToggle(ToggleSetting.Music);
			if (this.IsOn(AudioGroup.Music) != toggle)
			{
				this.ToggleMusicVolume(0f);
			}
			bool toggle2 = this.settingsService.GetToggle(ToggleSetting.Sound);
			if (this.IsOn(AudioGroup.SFX) != toggle2)
			{
				this.ToggleSfxVolume(0f);
			}
		}

		// Token: 0x06002E42 RID: 11842 RVA: 0x000D7BAB File Offset: 0x000D5FAB
		public void ChangeSetting(ToggleSetting setting, bool value)
		{
			if (setting != ToggleSetting.Music)
			{
				if (setting == ToggleSetting.Sound)
				{
					this.ToggleSfxVolume(0f);
				}
			}
			else
			{
				this.ToggleMusicVolume(0f);
			}
		}

		// Token: 0x06002E43 RID: 11843 RVA: 0x000D7BE0 File Offset: 0x000D5FE0
		private void CreateAudioGroupStates()
		{
			AudioMixerGroup[] array = this.mixer.FindMatchingGroups("/");
			foreach (AudioMixerGroup audioMixerGroup in array)
			{
				this.CreateAndAddAudioGroupState(audioMixerGroup.name);
			}
		}

		// Token: 0x06002E44 RID: 11844 RVA: 0x000D7C28 File Offset: 0x000D6028
		public AudioSource PlaySFX(AudioId audioId, bool duck = false, bool loop = false, bool muteMusic = false)
		{
			if (this.audioResourceManagerObject != null && this.audioResourcesManager.AudioRegistry.audioItemDictionary.ContainsKey(audioId))
			{
				AudioItem audioItem = this.audioResourcesManager.AudioRegistry.audioItemDictionary[audioId];
				AudioSource audioSource = null;
				if (audioItem.audioClip)
				{
					if (!this.lastPlayedTracker.ContainsKey(audioId))
					{
						this.lastPlayedTracker.Add(audioId, -10f);
					}
					if (Time.time >= this.lastPlayedTracker[audioId] + 0.08f)
					{
						this.lastPlayedTracker[audioId] = Time.time;
						AudioMixerGroup mixerGroup = (!duck) ? this.sfxGroup : this.voiceGroup;
						audioSource = this.audioResourcesManager.GetAvailableSFXAudioSource();
						if (muteMusic)
						{
							WooroutineRunner.StartCoroutine(this.ToogleMusicRoutine(0f), null);
							WooroutineRunner.StartCoroutine(this.ToogleMusicRoutine(audioItem.audioClip.length), null);
						}
						this.SetupAudioSourceAndPlay(mixerGroup, audioSource, audioItem.audioClip, false, 1f, audioItem.volume);
					}
				}
				else
				{
					WoogaDebug.LogWarning(new object[]
					{
						"No audioClip found for " + audioId.ToString() + ". Fix in Audio Registry!"
					});
				}
				return audioSource;
			}
			return null;
		}

		// Token: 0x06002E45 RID: 11845 RVA: 0x000D7D7C File Offset: 0x000D617C
		public AudioClip PlayMusic(AudioId audioId, bool forcePlay = false, bool loop = true, bool additive = false)
		{
			AudioItem audioItem = this.audioResourcesManager.AudioRegistry.audioItemDictionary[audioId];
			if (audioItem.audioClip)
			{
				this.PlayMusic(audioItem.audioClip, audioItem.volume, forcePlay, loop, additive);
			}
			else
			{
				WoogaDebug.LogWarning(new object[]
				{
					"No audioClip found for " + audioId.ToString() + ". Fix in Audio Registry!"
				});
			}
			return audioItem.audioClip;
		}

		// Token: 0x06002E46 RID: 11846 RVA: 0x000D7DFC File Offset: 0x000D61FC
		public AudioClip PlayMusic(AudioClip audioClip, float volume = 1f, bool forcePlay = false, bool loop = true, bool additive = false)
		{
			if (this.audioResourceManagerObject == null)
			{
				return null;
			}
			HashSet<AudioSource> hashSet = new HashSet<AudioSource>();
			AudioSource audioSource = this.audioResourcesManager.GetMusicAudioSource(hashSet);
			if (!forcePlay && this.IsClipPlaying(hashSet, audioClip))
			{
				return audioClip;
			}
			if (audioSource == null)
			{
				audioSource = this.audioResourcesManager.CreateAndAddMusicAudioSource();
				WoogaDebug.LogWarning(new object[]
				{
					"Creating an extra Audio Source for Music. You might be trying to switch music clips too fast. Try to reduce the cross fade duration."
				});
			}
			WooroutineRunner.StartCoroutine(this.CrossFadeMusicRoutine(audioClip, hashSet, audioSource, loop, volume, 1f, additive), null);
			return audioClip;
		}

		// Token: 0x06002E47 RID: 11847 RVA: 0x000D7E8C File Offset: 0x000D628C
		public bool IsMusicClipPlaying(AudioClip clip)
		{
			HashSet<AudioSource> hashSet = new HashSet<AudioSource>();
			this.audioResourcesManager.GetMusicAudioSource(hashSet);
			return hashSet.Any((AudioSource s) => s.clip == clip);
		}

		// Token: 0x06002E48 RID: 11848 RVA: 0x000D7ECC File Offset: 0x000D62CC
		private IEnumerator CrossFadeMusicRoutine(AudioClip audioClip, HashSet<AudioSource> playingAsSet, AudioSource availableAS, bool loop, float volume = 1f, float pitch = 1f, bool additive = false)
		{
			if (availableAS != null)
			{
				this.SetupAudioSourceAndPlay(this.musicGroup, availableAS, audioClip, loop, pitch, 0f);
				WooroutineRunner.StartCoroutine(this.FadeAudioSourceVolumeRoutine(availableAS, 0f, volume, this.crossFadeDuration), null);
			}
			if (!additive)
			{
				foreach (AudioSource audioSource in playingAsSet)
				{
					WooroutineRunner.StartCoroutine(this.FadeAudioSourceVolumeRoutine(audioSource, audioSource.volume, 0f, this.crossFadeDuration), null);
				}
			}
			yield return new WaitForSeconds(this.crossFadeDuration);
			if (!additive)
			{
				foreach (AudioSource audioSource2 in playingAsSet)
				{
					this.audioResourcesManager.ResetAudioSourceAndStop(audioSource2);
				}
			}
			yield break;
		}

		// Token: 0x06002E49 RID: 11849 RVA: 0x000D7F1C File Offset: 0x000D631C
		public void ToggleMasterVolume(float fadeDuration = 0f)
		{
			this.ToggleGroupVolume(AudioGroup.Master, fadeDuration);
		}

		// Token: 0x06002E4A RID: 11850 RVA: 0x000D7F26 File Offset: 0x000D6326
		public void ToggleSfxVolume(float fadeDuration = 0f)
		{
			this.ToggleGroupVolume(AudioGroup.SFX, fadeDuration);
			this.ToggleGroupVolume(AudioGroup.Voice, fadeDuration);
		}

		// Token: 0x06002E4B RID: 11851 RVA: 0x000D7F38 File Offset: 0x000D6338
		public void ToggleMusicVolume(float fadeDuration = 0f)
		{
			this.ToggleGroupVolume(AudioGroup.Music, fadeDuration);
		}

		// Token: 0x06002E4C RID: 11852 RVA: 0x000D7F44 File Offset: 0x000D6344
		public bool IsOn(AudioGroup group)
		{
			AudioService.AudioGroupState audioGroupState = this.GetAudioGroupState(group.ToString());
			return audioGroupState.volumeOn;
		}

		// Token: 0x06002E4D RID: 11853 RVA: 0x000D7F6C File Offset: 0x000D636C
		public void ToggleGroupVolume(AudioGroup audioGroup, float fadeDuration = 0f)
		{
			string text = audioGroup.ToString();
			float start;
			this.mixer.GetFloat(text, out start);
			AudioService.AudioGroupState audioGroupState = this.GetAudioGroupState(text);
			this.KillGroupVolumeFadeRoutine(text);
			if (audioGroupState.volumeOn)
			{
				audioGroupState.volumeOn = false;
				audioGroupState.volumeFadeRoutine = WooroutineRunner.StartCoroutine(this.FadeAudioGroupVolumeRoutine(text, start, -80f, fadeDuration), null);
			}
			else
			{
				audioGroupState.volumeOn = true;
				audioGroupState.volumeFadeRoutine = WooroutineRunner.StartCoroutine(this.FadeAudioGroupVolumeRoutine(text, start, 0f, fadeDuration), null);
			}
		}

		// Token: 0x06002E4E RID: 11854 RVA: 0x000D7FF8 File Offset: 0x000D63F8
		private void CreateAudioResourcesManager()
		{
			this.audioResourceManagerObject = new GameObject("AudioResourcesManager");
			this.audioResourcesManager = this.audioResourceManagerObject.AddComponent<AudioResourcesManager>();
			this.audioResourcesManager.Init(this, this.assetBundleService, 5, this.initialSfxPoolSize, -80f);
		}

		// Token: 0x06002E4F RID: 11855 RVA: 0x000D8044 File Offset: 0x000D6444
		public override void DeInit()
		{
			base.DeInit();
			if (this.audioResourceManagerObject != null)
			{
				this.audioResourcesManager.DeInit();
				global::UnityEngine.Object.DestroyImmediate(this.audioResourceManagerObject);
				this.audioResourceManagerObject = null;
			}
		}

		// Token: 0x06002E50 RID: 11856 RVA: 0x000D807C File Offset: 0x000D647C
		private AudioService.AudioGroupState CreateAndAddAudioGroupState(string groupName)
		{
			AudioService.AudioGroupState audioGroupState = new AudioService.AudioGroupState();
			audioGroupState.name = groupName;
			float num;
			this.mixer.GetFloat(groupName, out num);
			audioGroupState.volumeOn = (num > -80f);
			this.audioGroupStates[groupName] = audioGroupState;
			return audioGroupState;
		}

		// Token: 0x06002E51 RID: 11857 RVA: 0x000D80C4 File Offset: 0x000D64C4
		private void SetupAudioSourceAndPlay(AudioMixerGroup mixerGroup, AudioSource audioSource, AudioClip audioClip, bool loop = false, float pitch = 1f, float volume = 1f)
		{
			if (audioSource)
			{
				audioSource.outputAudioMixerGroup = mixerGroup;
				audioSource.loop = loop;
				audioSource.volume = volume;
				audioSource.pitch = pitch;
				audioSource.clip = audioClip;
				audioSource.gameObject.SetActive(true);
				audioSource.Play();
			}
		}

		// Token: 0x06002E52 RID: 11858 RVA: 0x000D8114 File Offset: 0x000D6514
		private AudioService.AudioGroupState GetAudioGroupState(string groupName)
		{
			if (this.audioGroupStates.ContainsKey(groupName))
			{
				return this.audioGroupStates[groupName];
			}
			WoogaDebug.LogError(new object[]
			{
				"Audio Group State not found! Make sure all audio groups have equivalent names in the AudioGroup enum."
			});
			return null;
		}

		// Token: 0x06002E53 RID: 11859 RVA: 0x000D8148 File Offset: 0x000D6548
		private void KillGroupVolumeFadeRoutine(string groupName)
		{
			if (this.audioGroupStates.ContainsKey(groupName) && this.audioGroupStates[groupName].volumeFadeRoutine != null)
			{
				WooroutineRunner.Stop(this.audioGroupStates[groupName].volumeFadeRoutine);
				this.audioGroupStates[groupName].volumeFadeRoutine = null;
			}
		}

		// Token: 0x06002E54 RID: 11860 RVA: 0x000D81A4 File Offset: 0x000D65A4
		private IEnumerator FadeAudioSourceVolumeRoutine(AudioSource audioSource, float start, float end, float duration)
		{
			float t = 0f;
			while (t < 1f && audioSource != null)
			{
				audioSource.volume = Mathf.Lerp(start, end, t);
				t += Time.deltaTime / duration;
				yield return null;
			}
			if (audioSource != null)
			{
				audioSource.volume = end;
			}
			yield break;
		}

		// Token: 0x06002E55 RID: 11861 RVA: 0x000D81D8 File Offset: 0x000D65D8
		private IEnumerator FadeAudioGroupVolumeRoutine(string groupName, float start, float end, float duration)
		{
			float t = 0f;
			while (t < 1f)
			{
				this.mixer.SetFloat(groupName, Mathf.Lerp(start, end, t));
				t += Time.deltaTime / duration;
				yield return null;
			}
			this.mixer.SetFloat(groupName, end);
			yield break;
		}

		// Token: 0x06002E56 RID: 11862 RVA: 0x000D8210 File Offset: 0x000D6610
		private bool IsClipPlaying(HashSet<AudioSource> playingAudioSources, string clipName)
		{
			return playingAudioSources.Any((AudioSource aSource) => aSource.clip.name == clipName);
		}

		// Token: 0x06002E57 RID: 11863 RVA: 0x000D823C File Offset: 0x000D663C
		private bool IsClipPlaying(HashSet<AudioSource> playingAudioSources, AudioClip clip)
		{
			return playingAudioSources.Any((AudioSource aSource) => aSource.clip == clip);
		}

		// Token: 0x06002E58 RID: 11864 RVA: 0x000D8268 File Offset: 0x000D6668
		private IEnumerator ToogleMusicRoutine(float delay)
		{
			if (this.settingsService.GetToggle(ToggleSetting.Music))
			{
				if (delay > 0f)
				{
					yield return new WaitForSeconds(delay);
				}
				this.ToggleMusicVolume(0.5f);
			}
			yield break;
		}

		// Token: 0x0400579A RID: 22426
		[WaitForService(true, true)]
		private GameSettingsService settingsService;

		// Token: 0x0400579B RID: 22427
		[WaitForService(true, true)]
		private AssetBundleService assetBundleService;

		// Token: 0x0400579C RID: 22428
		public const string COLLECTED = "Collected";

		// Token: 0x0400579D RID: 22429
		public const string ACQUIRED = "Acquired";

		// Token: 0x0400579E RID: 22430
		public const string SPENT = "Spent";

		// Token: 0x0400579F RID: 22431
		public const string PURCHASED = "Purchased";

		// Token: 0x040057A0 RID: 22432
		private const int MUSIC_POOL_SIZE = 5;

		// Token: 0x040057A1 RID: 22433
		private const float NORMAL_DB_OUTPUT = 0f;

		// Token: 0x040057A2 RID: 22434
		private const float MUTE_DB_OUTPUT = -80f;

		// Token: 0x040057A3 RID: 22435
		private const float SFX_COOLDOWN_TIME = 0.08f;

		// Token: 0x040057A4 RID: 22436
		private const float MUTE_FADE_DURATION = 0.5f;

		// Token: 0x040057A5 RID: 22437
		private AudioMixer mixer;

		// Token: 0x040057A6 RID: 22438
		private AudioMixerGroup musicGroup;

		// Token: 0x040057A7 RID: 22439
		private AudioMixerGroup sfxGroup;

		// Token: 0x040057A8 RID: 22440
		private AudioMixerGroup voiceGroup;

		// Token: 0x040057A9 RID: 22441
		private AudioResourcesManager audioResourcesManager;

		// Token: 0x040057AA RID: 22442
		public int initialSfxPoolSize = 3;

		// Token: 0x040057AB RID: 22443
		public float crossFadeDuration = 1f;

		// Token: 0x040057AC RID: 22444
		private Dictionary<string, AudioService.AudioGroupState> audioGroupStates = new Dictionary<string, AudioService.AudioGroupState>();

		// Token: 0x040057AD RID: 22445
		private Dictionary<AudioId, float> lastPlayedTracker = new Dictionary<AudioId, float>();

		// Token: 0x040057AE RID: 22446
		private Dictionary<string, AudioId> getSimilarCache = new Dictionary<string, AudioId>();

		// Token: 0x040057AF RID: 22447
		private AudioListener _audioListener;

		// Token: 0x040057B0 RID: 22448
		private GameObject audioResourceManagerObject;

		// Token: 0x0200074E RID: 1870
		private class AudioGroupState
		{
			// Token: 0x040057B1 RID: 22449
			public string name;

			// Token: 0x040057B2 RID: 22450
			public bool volumeOn;

			// Token: 0x040057B3 RID: 22451
			public Coroutine volumeFadeRoutine;
		}

		// Token: 0x0200074F RID: 1871
		private struct ClipEntry
		{
			// Token: 0x040057B4 RID: 22452
			public int managerUid;

			// Token: 0x040057B5 RID: 22453
			public AudioClip clip;
		}
	}
}

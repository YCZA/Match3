using System.Collections;
using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Services;
using Wooga.Coroutines;
using UnityEngine;

namespace Match3.Scripts1.Audio
{
	// Token: 0x02000026 RID: 38
	public class AudioResourcesManager : MonoBehaviour
	{
		// Token: 0x1700003A RID: 58
		// (get) Token: 0x0600014F RID: 335 RVA: 0x00007E82 File Offset: 0x00006282
		// (set) Token: 0x06000150 RID: 336 RVA: 0x00007E8A File Offset: 0x0000628A
		public AudioRegistry AudioRegistry { get; private set; }

		// Token: 0x06000151 RID: 337 RVA: 0x00007E93 File Offset: 0x00006293
		private void Start()
		{
			global::UnityEngine.Object.DontDestroyOnLoad(this);
			base.gameObject.AddComponent<AudioListener>();
			WooroutineRunner.StartCoroutine(this.AudioSourceCleaningRoutine(), null);
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00007EB4 File Offset: 0x000062B4
		public void Init(AudioService audioService, AssetBundleService assetBundleService, int musicPoolSize, int initialSfxPoolSize, float muteDbOutput)
		{
			this.audioService = audioService;
			this.assetBundleService = assetBundleService;
			this.muteDbOutput = muteDbOutput;
			this.CreateInitialMusicAudioSources(musicPoolSize);
			this.CreateInitialSfxAudioSources(initialSfxPoolSize);
			this.AudioRegistry = (Resources.Load("Audio/AudioRegistry", typeof(AudioRegistry)) as AudioRegistry);
			WooroutineRunner.StartCoroutine(this.LoadAudioRegistryCDNRoutine(), null);
		}

		// Token: 0x06000153 RID: 339 RVA: 0x00007F14 File Offset: 0x00006314
		private IEnumerator LoadAudioRegistryCDNRoutine()
		{
			Wooroutine<AudioRegistry> cdnLoad = this.assetBundleService.LoadAsset<AudioRegistry>("audio", "Assets/Audio/AudioRegistryCDN.asset");
			yield return cdnLoad;
			AudioRegistry audioRegistryCDN = cdnLoad.ReturnValue;
			if (audioRegistryCDN != null)
			{
				this.AudioRegistry.Merge(audioRegistryCDN);
			}
			yield break;
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00007F30 File Offset: 0x00006330
		public void Reset()
		{
			this.audioService.Mixer.SetFloat(AudioGroup.Master.ToString(), this.muteDbOutput);
			foreach (AudioSource audioSource in this.sfxAudioSources)
			{
				this.ResetAudioSourceAndStop(audioSource);
			}
			foreach (AudioSource audioSource2 in this.musicAudioSources)
			{
				this.ResetAudioSourceAndStop(audioSource2);
			}
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00008000 File Offset: 0x00006400
		public AudioSource GetAvailableSFXAudioSource()
		{
			foreach (AudioSource audioSource in this.sfxAudioSources)
			{
				if (!audioSource.isActiveAndEnabled)
				{
					return audioSource;
				}
			}
			return this.CreateAndAddSfxAudioSource();
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00008070 File Offset: 0x00006470
		public AudioSource CreateAndAddMusicAudioSource()
		{
			return this.CreateAndAddAudioSource("MUSIC AS", this.musicAudioSources);
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00008084 File Offset: 0x00006484
		public AudioSource GetMusicAudioSource(HashSet<AudioSource> playingAsSet)
		{
			AudioSource audioSource = null;
			foreach (AudioSource audioSource2 in this.musicAudioSources)
			{
				if (!audioSource2.isActiveAndEnabled && audioSource == null)
				{
					audioSource = audioSource2;
				}
				else if (audioSource2.isPlaying)
				{
					playingAsSet.Add(audioSource2);
				}
				else if (audioSource2.clip != null)
				{
					this.ResetAudioSourceAndStop(audioSource2);
				}
			}
			return audioSource;
		}

		// Token: 0x06000158 RID: 344 RVA: 0x0000812C File Offset: 0x0000652C
		public void ResetAudioSourceAndStop(AudioSource audioSource)
		{
			if (audioSource)
			{
				audioSource.Stop();
				audioSource.clip = null;
				audioSource.pitch = 1f;
				audioSource.volume = 1f;
				audioSource.outputAudioMixerGroup = null;
				audioSource.gameObject.SetActive(false);
			}
		}

		// Token: 0x06000159 RID: 345 RVA: 0x0000817A File Offset: 0x0000657A
		public void DeInit()
		{
			if (this.sfxAudioSources != null)
			{
				this.sfxAudioSources.Clear();
			}
			if (this.musicAudioSources != null)
			{
				this.musicAudioSources.Clear();
			}
		}

		// Token: 0x0600015A RID: 346 RVA: 0x000081A8 File Offset: 0x000065A8
		private IEnumerator AudioSourceCleaningRoutine()
		{
			for (;;)
			{
				foreach (AudioSource audioSource in this.sfxAudioSources)
				{
					if (audioSource.isActiveAndEnabled && !audioSource.isPlaying)
					{
						this.ResetAudioSourceAndStop(audioSource);
					}
				}
				foreach (AudioSource audioSource2 in this.musicAudioSources)
				{
					if (audioSource2.isActiveAndEnabled && !audioSource2.isPlaying)
					{
						this.ResetAudioSourceAndStop(audioSource2);
					}
				}
				yield return this.sfxCleanerWait;
			}
			yield break;
		}

		// Token: 0x0600015B RID: 347 RVA: 0x000081C4 File Offset: 0x000065C4
		private void CreateInitialMusicAudioSources(int amount)
		{
			for (int i = 0; i < amount; i++)
			{
				this.CreateAndAddMusicAudioSource();
			}
		}

		// Token: 0x0600015C RID: 348 RVA: 0x000081EC File Offset: 0x000065EC
		private void CreateInitialSfxAudioSources(int amount)
		{
			for (int i = 0; i < amount; i++)
			{
				this.CreateAndAddSfxAudioSource();
			}
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00008212 File Offset: 0x00006612
		private AudioSource CreateAndAddSfxAudioSource()
		{
			return this.CreateAndAddAudioSource("SFX AS", this.sfxAudioSources);
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00008228 File Offset: 0x00006628
		private AudioSource CreateAndAddAudioSource(string baseName, HashSet<AudioSource> audioSourceSet)
		{
			GameObject gameObject = new GameObject(baseName);
			gameObject.transform.parent = base.transform;
			AudioSource audioSource = gameObject.AddComponent<AudioSource>();
			audioSourceSet.Add(audioSource);
			audioSource.playOnAwake = false;
			gameObject.SetActive(false);
			return audioSource;
		}

		// Token: 0x0400011F RID: 287
		private const string SFX_AS_BASE_NAME = "SFX AS";

		// Token: 0x04000120 RID: 288
		private const string MUSIC_AS_BASE_NAME = "MUSIC AS";

		// Token: 0x04000121 RID: 289
		private const float SFX_AUDIO_CLEAN_RATE = 1.5f;

		// Token: 0x04000122 RID: 290
		public const string AUDIO_REGISTRY_PATH = "Audio/AudioRegistry";

		// Token: 0x04000123 RID: 291
		public const string AUDIO_REGISTRY_CDN_PATH = "Assets/Audio/AudioRegistryCDN.asset";

		// Token: 0x04000124 RID: 292
		public const string AUDIO_BUNDLE = "audio";

		// Token: 0x04000125 RID: 293
		private readonly WaitForSeconds sfxCleanerWait = new WaitForSeconds(1.5f);

		// Token: 0x04000126 RID: 294
		private float muteDbOutput;

		// Token: 0x04000127 RID: 295
		private AudioService audioService;

		// Token: 0x04000128 RID: 296
		private AssetBundleService assetBundleService;

		// Token: 0x04000129 RID: 297
		private HashSet<AudioSource> musicAudioSources = new HashSet<AudioSource>();

		// Token: 0x0400012A RID: 298
		private HashSet<AudioSource> sfxAudioSources = new HashSet<AudioSource>();
	}
}

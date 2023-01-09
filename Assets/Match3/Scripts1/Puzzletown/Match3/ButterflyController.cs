using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Services;
using Shared.Pooling;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020004D6 RID: 1238
	public class ButterflyController : AReleasable
	{
		// Token: 0x17000546 RID: 1350
		// (get) Token: 0x0600227F RID: 8831 RVA: 0x00098935 File Offset: 0x00096D35
		private AudioService AudioService
		{
			get
			{
				if (this.audioService == null)
				{
					this.audioService = base.GetComponentInParent<M3_LevelRoot>().audioService;
				}
				return this.audioService;
			}
		}

		// Token: 0x06002280 RID: 8832 RVA: 0x0009895C File Offset: 0x00096D5C
		public void Initialize(IntVector2 goal, float tweenDuration, GemColor gemColor, ObjectPool objectPool)
		{
			this.flyDuration = tweenDuration;
			this.pool = objectPool;
			this.effectColor = this.recolorizer.GetColors(gemColor).colorA;
			this.recolorizer.SetColor(gemColor);
			this.goalDirection = (Vector3)goal - base.transform.position;
			this.goalDirection.Normalize();
			float x;
			float y;
			if (Mathf.Abs(this.goalDirection.x) > Mathf.Abs(this.goalDirection.y))
			{
				x = Mathf.Sign(this.goalDirection.x);
				y = 0f;
			}
			else
			{
				x = 0f;
				y = Mathf.Sign(this.goalDirection.y);
			}
			this.startDirection = new Vector3(x, y, 0f);
			base.transform.up = this.startDirection;
			base.transform.rotation = Quaternion.Euler(0f, 0f, base.transform.rotation.eulerAngles.z);
			RuntimeAnimatorController runtimeAnimatorController = this.animator.runtimeAnimatorController;
			foreach (AnimationClip animationClip in runtimeAnimatorController.animationClips)
			{
				if (this.createDuration == 0f && animationClip.name == "Butterfly_lift")
				{
					this.createDuration = animationClip.length;
				}
				else if (this.endDuration == 0f && animationClip.name == "Butterfly_end")
				{
					this.endDuration = animationClip.length;
				}
			}
			this.totalDuration = this.createDuration + this.flyDuration + this.endDuration;
			this.SetupParticleSystems();
		}

		// Token: 0x06002281 RID: 8833 RVA: 0x00098B33 File Offset: 0x00096F33
		public void StartButterfly()
		{
			this.timer = 0f;
			this.TransitionToCreate();
		}

		// Token: 0x06002282 RID: 8834 RVA: 0x00098B46 File Offset: 0x00096F46
		public float GetCreateEffectDuration()
		{
			return this.createDuration;
		}

		// Token: 0x06002283 RID: 8835 RVA: 0x00098B50 File Offset: 0x00096F50
		private void SetupParticleSystems()
		{
			this.trailParticleSystem = this.pool.Get(this.trailVfxPrefab).GetComponent<ParticleSystem>();
			this.SetUpParticleSystemInHierarchy(this.trailParticleSystem, true);
			this.createParticleSystem = this.pool.Get(this.createVfxPrefab).GetComponent<ParticleSystem>();
			this.SetUpParticleSystemInHierarchy(this.createParticleSystem, true);
			this.finalButterfliesParticleSystem = this.pool.Get(this.finalButterfliesVFfxPrefab).GetComponent<ParticleSystem>();
			this.SetUpParticleSystemInHierarchy(this.finalButterfliesParticleSystem, true);
			this.finalImpactParticleSystem = this.pool.Get(this.finalImpactVfxPrefab).GetComponent<ParticleSystem>();
			this.SetUpParticleSystemInHierarchy(this.finalImpactParticleSystem, false);
		}

		// Token: 0x06002284 RID: 8836 RVA: 0x00098C01 File Offset: 0x00097001
		private void TransitionToCreate()
		{
			this.AudioService.PlaySFX(AudioId.FishJump, false, false, false);
			this.createParticleSystem.Play();
			this.butterflyState = ButterflyState.Lift;
		}

		// Token: 0x06002285 RID: 8837 RVA: 0x00098C28 File Offset: 0x00097028
		private void TransitionToFly()
		{
			this.butterflyFlyingAudio = this.AudioService.PlaySFX(AudioId.ButterflyFlying, false, true, false);
			this.butterflyState = ButterflyState.Fly;
			this.animator.SetTrigger(ButterflyController.FLY_TRIGGER);
			this.trailParticleSystem.Stop();
			this.trailParticleSystem.Play();
		}

		// Token: 0x06002286 RID: 8838 RVA: 0x00098C7B File Offset: 0x0009707B
		private void TransitionToEnd()
		{
			this.butterflyState = ButterflyState.End;
			this.animator.SetTrigger(ButterflyController.END_TRIGGER);
			this.trailParticleSystem.Stop();
			this.finalImpactParticleSystem.Play();
		}

		// Token: 0x06002287 RID: 8839 RVA: 0x00098CAC File Offset: 0x000970AC
		private void FinalizeView()
		{
			this.audioService.PlaySFX(AudioId.FishExplode, false, false, false);
			this.butterflyState = ButterflyState.Idle;
			if (this.butterflyFlyingAudio)
			{
				this.butterflyFlyingAudio.Stop();
			}
			this.butterflyFlyingAudio = null;
			this.CleanUpParticleSystem(this.trailParticleSystem, 0f);
			this.CleanUpParticleSystem(this.createParticleSystem, 0f);
			ParticleSystem.MainModule main = this.finalButterfliesParticleSystem.main;
			this.CleanUpParticleSystem(this.finalImpactParticleSystem, main.duration);
			main = this.finalButterfliesParticleSystem.main;
			this.CleanUpParticleSystem(this.finalButterfliesParticleSystem, 0.5f + main.duration);
			this.finalButterfliesParticleSystem.Play();
			this.Release(0f);
		}

		// Token: 0x06002288 RID: 8840 RVA: 0x00098D70 File Offset: 0x00097170
		private void SetUpParticleSystemInHierarchy(ParticleSystem particleSystemVfx, bool adjustColor = false)
		{
			particleSystemVfx.transform.position = base.transform.position;
			particleSystemVfx.transform.SetParent(base.transform);
			if (adjustColor)
			{
				var go1 = particleSystemVfx.main;
				go1.startColor = this.effectColor;
			}
		}

		// Token: 0x06002289 RID: 8841 RVA: 0x00098DC3 File Offset: 0x000971C3
		private void CleanUpParticleSystem(ParticleSystem particleSystemVfx, float delay = 0f)
		{
			if (particleSystemVfx == null)
			{
				return;
			}
			particleSystemVfx.Stop();
			particleSystemVfx.transform.SetParent(base.transform.parent);
			particleSystemVfx.gameObject.Release(delay);
		}

		// Token: 0x0600228A RID: 8842 RVA: 0x00098DFC File Offset: 0x000971FC
		private void Update()
		{
			this.timer += Time.deltaTime;
			ButterflyState butterflyState = this.butterflyState;
			if (butterflyState != ButterflyState.Lift)
			{
				if (butterflyState != ButterflyState.Fly)
				{
					if (butterflyState == ButterflyState.End)
					{
						if (this.timer >= this.totalDuration)
						{
							this.FinalizeView();
						}
					}
				}
				else
				{
					float t = (this.timer - this.createDuration) / this.flyDuration;
					base.transform.up = Vector3.Lerp(this.startDirection, this.goalDirection, t);
					if (this.timer >= this.createDuration + this.flyDuration)
					{
						this.TransitionToEnd();
					}
				}
			}
			else if (this.timer >= this.createDuration)
			{
				this.TransitionToFly();
			}
		}

		// Token: 0x04004E01 RID: 19969
		public GameObject createVfxPrefab;

		// Token: 0x04004E02 RID: 19970
		public GameObject trailVfxPrefab;

		// Token: 0x04004E03 RID: 19971
		public GameObject finalButterfliesVFfxPrefab;

		// Token: 0x04004E04 RID: 19972
		public GameObject finalImpactVfxPrefab;

		// Token: 0x04004E05 RID: 19973
		public Animator animator;

		// Token: 0x04004E06 RID: 19974
		public Recolorizer recolorizer;

		// Token: 0x04004E07 RID: 19975
		private const string LIFT_ANIMATION_NAME = "Butterfly_lift";

		// Token: 0x04004E08 RID: 19976
		private const string END_ANIMATION_NAME = "Butterfly_end";

		// Token: 0x04004E09 RID: 19977
		private static readonly int FLY_TRIGGER = Animator.StringToHash("fly");

		// Token: 0x04004E0A RID: 19978
		private static readonly int END_TRIGGER = Animator.StringToHash("end");

		// Token: 0x04004E0B RID: 19979
		private ButterflyState butterflyState;

		// Token: 0x04004E0C RID: 19980
		private ParticleSystem trailParticleSystem;

		// Token: 0x04004E0D RID: 19981
		private ParticleSystem createParticleSystem;

		// Token: 0x04004E0E RID: 19982
		private ParticleSystem finalButterfliesParticleSystem;

		// Token: 0x04004E0F RID: 19983
		private ParticleSystem finalImpactParticleSystem;

		// Token: 0x04004E10 RID: 19984
		private float totalDuration;

		// Token: 0x04004E11 RID: 19985
		private float createDuration;

		// Token: 0x04004E12 RID: 19986
		private float flyDuration;

		// Token: 0x04004E13 RID: 19987
		private float endDuration;

		// Token: 0x04004E14 RID: 19988
		private Color effectColor;

		// Token: 0x04004E15 RID: 19989
		private float timer;

		// Token: 0x04004E16 RID: 19990
		private ObjectPool pool;

		// Token: 0x04004E17 RID: 19991
		private AudioSource butterflyFlyingAudio;

		// Token: 0x04004E18 RID: 19992
		private AudioService audioService;

		// Token: 0x04004E19 RID: 19993
		private Vector3 goalDirection;

		// Token: 0x04004E1A RID: 19994
		private Vector3 startDirection;
	}
}

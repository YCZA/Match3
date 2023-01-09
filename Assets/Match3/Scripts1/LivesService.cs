using System;
using System.Collections;
using Match3.Scripts1.Puzzletown.Services;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Signals;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using UnityEngine;

// Token: 0x020007D8 RID: 2008
namespace Match3.Scripts1
{
	public class LivesService : AService
	{
		// Token: 0x0600317C RID: 12668 RVA: 0x000E8B1B File Offset: 0x000E6F1B
		public LivesService()
		{
			WooroutineRunner.StartCoroutine(this.LoadRoutine(), null);
		}

		// Token: 0x170007C3 RID: 1987
		// (get) Token: 0x0600317D RID: 12669 RVA: 0x000E8B4D File Offset: 0x000E6F4D
		public bool IsCurrentlyUnlimitedLives
		{
			get
			{
				return this.stateService.UnlimitedLivesEnd > DateTime.Now;
			}
		}

		// Token: 0x0600317E RID: 12670 RVA: 0x000E8B64 File Offset: 0x000E6F64
		public void StartUnlimitedLives(int numberOfMinutesToAdd)
		{
			if (this.IsCurrentlyUnlimitedLives)
			{
				this.stateService.UnlimitedLivesEnd = this.stateService.UnlimitedLivesEnd.AddMinutes((double)numberOfMinutesToAdd);
			}
			else
			{
				this.stateService.UnlimitedLivesEnd = DateTime.Now.AddMinutes((double)numberOfMinutesToAdd);
			}
		}

		// Token: 0x170007C4 RID: 1988
		// (get) Token: 0x0600317F RID: 12671 RVA: 0x000E8BBB File Offset: 0x000E6FBB
		public string ResourceKey
		{
			get
			{
				return this.configService.general.lives.resource_key;
			}
		}

		// Token: 0x170007C5 RID: 1989
		// (get) Token: 0x06003180 RID: 12672 RVA: 0x000E8BD2 File Offset: 0x000E6FD2
		public int RechargeRate
		{
			get
			{
				return this.configService.general.lives.recharge_rate;
			}
		}

		// Token: 0x170007C6 RID: 1990
		// (get) Token: 0x06003181 RID: 12673 RVA: 0x000E8BE9 File Offset: 0x000E6FE9
		public int MaxLives
		{
			get
			{
				return this.configService.general.lives.max_lives;
			}
		}

		// Token: 0x170007C7 RID: 1991
		// (get) Token: 0x06003182 RID: 12674 RVA: 0x000E8C00 File Offset: 0x000E7000
		private TimeSpan RechargeTimeSpan
		{
			get
			{
				return TimeSpan.FromSeconds((double)this.RechargeRate);
			}
		}

		// Token: 0x170007C8 RID: 1992
		// (get) Token: 0x06003183 RID: 12675 RVA: 0x000E8C0E File Offset: 0x000E700E
		public int CurrentLives
		{
			get
			{
				return this.stateService.Resources.GetAmount(this.ResourceKey);
			}
		}

		// Token: 0x170007C9 RID: 1993
		// (get) Token: 0x06003185 RID: 12677 RVA: 0x000E8C46 File Offset: 0x000E7046
		// (set) Token: 0x06003184 RID: 12676 RVA: 0x000E8C26 File Offset: 0x000E7026
		public int SecondsRemaining
		{
			get
			{
				return this.secondsRemaining;
			}
			set
			{
				if (value != this.secondsRemaining)
				{
					this.secondsRemaining = value;
					this.OnLifeTimerChanged.Dispatch();
				}
			}
		}

		// Token: 0x170007CA RID: 1994
		// (get) Token: 0x06003186 RID: 12678 RVA: 0x000E8C4E File Offset: 0x000E704E
		private bool LifeFull
		{
			get
			{
				return this.CurrentLives == this.MaxLives;
			}
		}

		// Token: 0x06003187 RID: 12679 RVA: 0x000E8C60 File Offset: 0x000E7060
		private IEnumerator LoadRoutine()
		{
			yield return ServiceLocator.Instance.Inject(this);
			this.stateService.Resources.onChanged.AddListener(new Action<MaterialChange>(this.OnMaterialChanged));
			WooroutineRunner.StartCoroutine(this.Tick(), null);
			base.OnInitialized.Dispatch();
			yield break;
		}

		// Token: 0x06003188 RID: 12680 RVA: 0x000E8C7C File Offset: 0x000E707C
		private IEnumerator Tick()
		{
			WaitForEndOfFrame wait = new WaitForEndOfFrame();
			WaitForSeconds waitForOneSecond = new WaitForSeconds(1f);
			for (;;)
			{
				yield return wait;
				if (this.IsCurrentlyUnlimitedLives)
				{
					yield return waitForOneSecond;
					this.OnLifeTimerChanged.Dispatch();
				}
				else if (this.CurrentLives >= this.MaxLives)
				{
					this.SecondsRemaining = -1;
				}
				else
				{
					DateTime currentTime = DateTime.UtcNow;
					if ((currentTime - this.stateService.Lives.LastLifeGiven).Ticks < 0L)
					{
					}
					int secondsElapsed = (int)(currentTime - this.stateService.Lives.LastLifeGiven).TotalSeconds;
					this.SecondsRemaining = this.RechargeRate - secondsElapsed;
					if (this.SecondsRemaining <= 0)
					{
						int lives = secondsElapsed / this.RechargeRate;
						int num = secondsElapsed % this.RechargeRate;
						this.AddLives(lives, "时间恢复");
						this.stateService.Lives.LastLifeGiven = currentTime - TimeSpan.FromSeconds((double)num);
						this.stateService.Save(false);
						this.SecondsRemaining = this.RechargeRate - (int)(currentTime - this.stateService.Lives.LastLifeGiven).TotalSeconds;
					}
				}
			}
			yield break;
		}

		// Token: 0x06003189 RID: 12681 RVA: 0x000E8C98 File Offset: 0x000E7098
		public bool UseLife(string way = "unknown")
		{
			if (this.CurrentLives <= 0)
			{
				return false;
			}
			if (this.IsCurrentlyUnlimitedLives)
			{
				return false;
			}
			if (this.CurrentLives == this.MaxLives)
			{
				this.stateService.Lives.LastLifeGiven = DateTime.UtcNow;
				this.stateService.Save(false);
			}
			this.stateService.Resources.AddMaterial(this.ResourceKey, -1, this.MaxLives, true, way);
			return true;
		}

		// Token: 0x0600318A RID: 12682 RVA: 0x000E8D11 File Offset: 0x000E7111
		public void AddLives(int lives, string way = "unkonwn")
		{
			this.stateService.Resources.AddMaterial(this.ResourceKey, lives, this.MaxLives, true, way);
		}

		// Token: 0x0600318B RID: 12683 RVA: 0x000E8D31 File Offset: 0x000E7131
		public void Refill()
		{
			this.AddLives(this.MaxLives, "钻石购买");
		}

		// Token: 0x0600318C RID: 12684 RVA: 0x000E8D3F File Offset: 0x000E713F
		private void OnMaterialChanged(MaterialChange change)
		{
			if (change.name == "lives" && change.Delta != 0)
			{
				this.OnLivesChanged.Dispatch();
			}
		}

		// Token: 0x04005A0F RID: 23055
		public readonly Signal OnLivesChanged = new Signal();

		// Token: 0x04005A10 RID: 23056
		public readonly Signal OnLifeTimerChanged = new Signal();

		// Token: 0x04005A11 RID: 23057
		[WaitForService(true, true)]
		private ConfigService configService;

		// Token: 0x04005A12 RID: 23058
		[WaitForService(true, true)]
		private GameStateService stateService;

		// Token: 0x04005A13 RID: 23059
		public const string UNLIMITED_LIVES = "lives_unlimited";

		// Token: 0x04005A14 RID: 23060
		private int secondsRemaining = -1;
	}
}

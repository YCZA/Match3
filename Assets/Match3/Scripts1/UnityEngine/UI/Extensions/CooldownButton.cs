using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000BA3 RID: 2979
	[AddComponentMenu("UI/Extensions/Cooldown Button")]
	public class CooldownButton : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
	{
		// Token: 0x17000A0C RID: 2572
		// (get) Token: 0x060045C1 RID: 17857 RVA: 0x00161E96 File Offset: 0x00160296
		// (set) Token: 0x060045C2 RID: 17858 RVA: 0x00161E9E File Offset: 0x0016029E
		public float CooldownTimeout
		{
			get
			{
				return this.cooldownTimeout;
			}
			set
			{
				this.cooldownTimeout = value;
			}
		}

		// Token: 0x17000A0D RID: 2573
		// (get) Token: 0x060045C3 RID: 17859 RVA: 0x00161EA7 File Offset: 0x001602A7
		// (set) Token: 0x060045C4 RID: 17860 RVA: 0x00161EAF File Offset: 0x001602AF
		public float CooldownSpeed
		{
			get
			{
				return this.cooldownSpeed;
			}
			set
			{
				this.cooldownSpeed = value;
			}
		}

		// Token: 0x17000A0E RID: 2574
		// (get) Token: 0x060045C5 RID: 17861 RVA: 0x00161EB8 File Offset: 0x001602B8
		public bool CooldownInEffect
		{
			get
			{
				return this.cooldownInEffect;
			}
		}

		// Token: 0x17000A0F RID: 2575
		// (get) Token: 0x060045C6 RID: 17862 RVA: 0x00161EC0 File Offset: 0x001602C0
		// (set) Token: 0x060045C7 RID: 17863 RVA: 0x00161EC8 File Offset: 0x001602C8
		public bool CooldownActive
		{
			get
			{
				return this.cooldownActive;
			}
			set
			{
				this.cooldownActive = value;
			}
		}

		// Token: 0x17000A10 RID: 2576
		// (get) Token: 0x060045C8 RID: 17864 RVA: 0x00161ED1 File Offset: 0x001602D1
		// (set) Token: 0x060045C9 RID: 17865 RVA: 0x00161ED9 File Offset: 0x001602D9
		public float CooldownTimeElapsed
		{
			get
			{
				return this.cooldownTimeElapsed;
			}
			set
			{
				this.cooldownTimeElapsed = value;
			}
		}

		// Token: 0x17000A11 RID: 2577
		// (get) Token: 0x060045CA RID: 17866 RVA: 0x00161EE2 File Offset: 0x001602E2
		public float CooldownTimeRemaining
		{
			get
			{
				return this.cooldownTimeRemaining;
			}
		}

		// Token: 0x17000A12 RID: 2578
		// (get) Token: 0x060045CB RID: 17867 RVA: 0x00161EEA File Offset: 0x001602EA
		public int CooldownPercentRemaining
		{
			get
			{
				return this.cooldownPercentRemaining;
			}
		}

		// Token: 0x17000A13 RID: 2579
		// (get) Token: 0x060045CC RID: 17868 RVA: 0x00161EF2 File Offset: 0x001602F2
		public int CooldownPercentComplete
		{
			get
			{
				return this.cooldownPercentComplete;
			}
		}

		// Token: 0x060045CD RID: 17869 RVA: 0x00161EFC File Offset: 0x001602FC
		private void Update()
		{
			if (this.CooldownActive)
			{
				this.cooldownTimeRemaining -= Time.deltaTime * this.cooldownSpeed;
				this.cooldownTimeElapsed = this.CooldownTimeout - this.CooldownTimeRemaining;
				if (this.cooldownTimeRemaining < 0f)
				{
					this.StopCooldown();
				}
				else
				{
					this.cooldownPercentRemaining = (int)(100f * this.cooldownTimeRemaining * this.CooldownTimeout / 100f);
					this.cooldownPercentComplete = (int)((this.CooldownTimeout - this.cooldownTimeRemaining) / this.CooldownTimeout * 100f);
				}
			}
		}

		// Token: 0x060045CE RID: 17870 RVA: 0x00161F9C File Offset: 0x0016039C
		public void PauseCooldown()
		{
			if (this.CooldownInEffect)
			{
				this.CooldownActive = false;
			}
		}

		// Token: 0x060045CF RID: 17871 RVA: 0x00161FB0 File Offset: 0x001603B0
		public void RestartCooldown()
		{
			if (this.CooldownInEffect)
			{
				this.CooldownActive = true;
			}
		}

		// Token: 0x060045D0 RID: 17872 RVA: 0x00161FC4 File Offset: 0x001603C4
		public void StopCooldown()
		{
			this.cooldownTimeElapsed = this.CooldownTimeout;
			this.cooldownTimeRemaining = 0f;
			this.cooldownPercentRemaining = 0;
			this.cooldownPercentComplete = 100;
			this.cooldownActive = (this.cooldownInEffect = false);
			if (this.OnCoolDownFinish != null)
			{
				this.OnCoolDownFinish.Invoke(this.buttonSource.button);
			}
		}

		// Token: 0x060045D1 RID: 17873 RVA: 0x00162028 File Offset: 0x00160428
		public void CancelCooldown()
		{
			this.cooldownActive = (this.cooldownInEffect = false);
		}

		// Token: 0x060045D2 RID: 17874 RVA: 0x00162048 File Offset: 0x00160448
		void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
		{
			this.buttonSource = eventData;
			if (this.CooldownInEffect && this.OnButtonClickDuringCooldown != null)
			{
				this.OnButtonClickDuringCooldown.Invoke(eventData.button);
			}
			if (!this.CooldownInEffect)
			{
				if (this.OnCooldownStart != null)
				{
					this.OnCooldownStart.Invoke(eventData.button);
				}
				this.cooldownTimeRemaining = this.cooldownTimeout;
				this.cooldownActive = (this.cooldownInEffect = true);
			}
		}

		// Token: 0x04006D50 RID: 27984
		[SerializeField]
		private float cooldownTimeout;

		// Token: 0x04006D51 RID: 27985
		[SerializeField]
		private float cooldownSpeed = 1f;

		// Token: 0x04006D52 RID: 27986
		[SerializeField]
		[ReadOnly]
		private bool cooldownActive;

		// Token: 0x04006D53 RID: 27987
		[SerializeField]
		[ReadOnly]
		private bool cooldownInEffect;

		// Token: 0x04006D54 RID: 27988
		[SerializeField]
		[ReadOnly]
		private float cooldownTimeElapsed;

		// Token: 0x04006D55 RID: 27989
		[SerializeField]
		[ReadOnly]
		private float cooldownTimeRemaining;

		// Token: 0x04006D56 RID: 27990
		[SerializeField]
		[ReadOnly]
		private int cooldownPercentRemaining;

		// Token: 0x04006D57 RID: 27991
		[SerializeField]
		[ReadOnly]
		private int cooldownPercentComplete;

		// Token: 0x04006D58 RID: 27992
		private PointerEventData buttonSource;

		// Token: 0x04006D59 RID: 27993
		[Tooltip("Event that fires when a button is initially pressed down")]
		public CooldownButton.CooldownButtonEvent OnCooldownStart;

		// Token: 0x04006D5A RID: 27994
		[Tooltip("Event that fires when a button is released")]
		public CooldownButton.CooldownButtonEvent OnButtonClickDuringCooldown;

		// Token: 0x04006D5B RID: 27995
		[Tooltip("Event that continually fires while a button is held down")]
		public CooldownButton.CooldownButtonEvent OnCoolDownFinish;

		// Token: 0x02000BA4 RID: 2980
		[Serializable]
		public class CooldownButtonEvent : UnityEvent<PointerEventData.InputButton>
		{
		}
	}
}

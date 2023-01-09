using Match3.Scripts1.UnityEngine;
using UnityEngine;

// Token: 0x02000B2E RID: 2862
namespace Match3.Scripts1
{
	public class AnimatedUi : MonoBehaviour, IVisible
	{
		// Token: 0x06004328 RID: 17192 RVA: 0x00128C89 File Offset: 0x00127089
		public AnimatedUi()
		{
			this.IsVisible = false;
		}

		// Token: 0x170009AE RID: 2478
		// (get) Token: 0x06004329 RID: 17193 RVA: 0x00128C98 File Offset: 0x00127098
		private Animation Animation
		{
			get
			{
				if (this._animation)
				{
					return this._animation;
				}
				this._animation = base.gameObject.GetComponent<Animation>();
				if (!this._animation)
				{
					this._animation = base.gameObject.AddComponent<Animation>();
				}
				if (this.open != null)
				{
					this._animation.AddClip(this.open, this.open.name);
				}
				if (this.close != null)
				{
					this._animation.AddClip(this.close, this.close.name);
				}
				return this._animation;
			}
		}

		// Token: 0x170009AF RID: 2479
		// (get) Token: 0x0600432A RID: 17194 RVA: 0x00128D4E File Offset: 0x0012714E
		// (set) Token: 0x0600432B RID: 17195 RVA: 0x00128D56 File Offset: 0x00127156
		public bool IsVisible { get; private set; }

		// Token: 0x170009B0 RID: 2480
		// (get) Token: 0x0600432C RID: 17196 RVA: 0x00128D5F File Offset: 0x0012715F
		// (set) Token: 0x0600432D RID: 17197 RVA: 0x00128D67 File Offset: 0x00127167
		public bool IsClosing { get; protected set; }

		// Token: 0x170009B1 RID: 2481
		// (get) Token: 0x0600432E RID: 17198 RVA: 0x00128D70 File Offset: 0x00127170
		public bool IsOpening
		{
			get
			{
				return this.open != null && this.Animation.IsPlaying(this.open.name);
			}
		}

		// Token: 0x0600432F RID: 17199 RVA: 0x00128D9C File Offset: 0x0012719C
		protected void OnEnable()
		{
			this.IsClosing = false;
		}

		// Token: 0x06004330 RID: 17200 RVA: 0x00128DA5 File Offset: 0x001271A5
		protected void OnDisable()
		{
			this.IsClosing = false;
		}

		// Token: 0x06004331 RID: 17201 RVA: 0x00128DB0 File Offset: 0x001271B0
		public void Show()
		{
			if (this.IsVisible || base.gameObject == null)
			{
				return;
			}
			base.gameObject.SetActive(true);
			if (this.open != null)
			{
				this.Animation.Play(this.open.name, PlayMode.StopSameLayer);
			}
			this.IsVisible = true;
		}

		// Token: 0x06004332 RID: 17202 RVA: 0x00128E18 File Offset: 0x00127218
		public void HideAndDisable(bool disableOnComplete)
		{
			if (!this.IsVisible)
			{
				base.gameObject.SetActive(false);
				return;
			}
			this.IsClosing = true;
			this.Animation.Stop();
			if (this.close != null)
			{
				this.Animation.Play(this.close.name, PlayMode.StopSameLayer);
				if (disableOnComplete)
				{
					base.Invoke("DisableDialogHolder", this.close.length);
				}
			}
			this.IsVisible = false;
		}

		// Token: 0x06004333 RID: 17203 RVA: 0x00128E9B File Offset: 0x0012729B
		public void Hide()
		{
			this.HideAndDisable(true);
		}

		// Token: 0x06004334 RID: 17204 RVA: 0x00128EA4 File Offset: 0x001272A4
		public void DisableDialogHolder()
		{
			this.ExecuteOnParent(delegate(IPersistentDialog d)
			{
				d.Disable();
			});
			this.ExecuteOnParent(delegate(IDisposableDialog d)
			{
				d.Destroy();
			});
		}

		// Token: 0x06004335 RID: 17205 RVA: 0x00128EF7 File Offset: 0x001272F7
		public void SetVisibility(bool visible)
		{
			if (visible)
			{
				this.Show();
			}
			else
			{
				this.Hide();
			}
		}

		// Token: 0x04006BD2 RID: 27602
		public AnimationClip open;

		// Token: 0x04006BD3 RID: 27603
		public AnimationClip close;

		// Token: 0x04006BD4 RID: 27604
		private Animation _animation;
	}
}

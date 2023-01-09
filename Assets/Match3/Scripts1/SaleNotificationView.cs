using System;
using System.Collections;
using Match3.Scripts1.Puzzletown.Services;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000A1B RID: 2587
namespace Match3.Scripts1
{
	public class SaleNotificationView : MonoBehaviour
	{
		// Token: 0x06003E31 RID: 15921 RVA: 0x0013B668 File Offset: 0x00139A68
		public void Show(Sale sale, int lowTime, string discountStr, Action refresh)
		{
			this.refresh = refresh;
			this.sale = sale;
			this.lowTime = lowTime;
			base.gameObject.SetActive(true);
			DateTime lowDateTime = sale.GetLowDateTime(lowTime);
			this.countdownTimer.SetTargetTime(sale.endDateLocal, lowDateTime, false, new Action(this.Hide));
			this.saleIcon.sprite = this.saleIconSprites.GetSimilar(sale.iconName);
			this.discountText.text = discountStr;
			bool flag = lowDateTime < DateTime.Now;
			if (this.showLimitedTime != null && flag && !this.showLimitedTime.isPlaying)
			{
				this.showLimitedTime.Play();
			}
		}

		// Token: 0x06003E32 RID: 15922 RVA: 0x0013B728 File Offset: 0x00139B28
		private void OnEnable()
		{
			if (this.sale != null)
			{
				DateTime lowDateTime = this.sale.GetLowDateTime(this.lowTime);
				bool flag = lowDateTime < DateTime.Now;
				if (this.showLimitedTime != null && flag && !this.showLimitedTime.isPlaying)
				{
					this.showLimitedTime.Play();
				}
			}
		}

		// Token: 0x06003E33 RID: 15923 RVA: 0x0013B791 File Offset: 0x00139B91
		public void Hide()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x06003E34 RID: 15924 RVA: 0x0013B79F File Offset: 0x00139B9F
		public void OnShowSalePanel()
		{
			base.StartCoroutine(this.ShowSalePanelRoutine());
		}

		// Token: 0x06003E35 RID: 15925 RVA: 0x0013B7B0 File Offset: 0x00139BB0
		private IEnumerator ShowSalePanelRoutine()
		{
			Wooroutine<SalePopupRoot> scene = SceneManager.Instance.LoadSceneWithParams<SalePopupRoot, string>("level_start_banner", null);
			yield return scene;
			yield return scene.ReturnValue.onDestroyed;
			if (this.refresh != null)
			{
				this.refresh();
			}
			yield break;
		}

		// Token: 0x0400671F RID: 26399
		[SerializeField]
		private CountdownTimer countdownTimer;

		// Token: 0x04006720 RID: 26400
		[SerializeField]
		private Animation showLimitedTime;

		// Token: 0x04006721 RID: 26401
		[SerializeField]
		private SpriteManager saleIconSprites;

		// Token: 0x04006722 RID: 26402
		[SerializeField]
		private Image saleIcon;

		// Token: 0x04006723 RID: 26403
		[SerializeField]
		private TMP_Text discountText;

		// Token: 0x04006724 RID: 26404
		private Action refresh;

		// Token: 0x04006725 RID: 26405
		private int lowTime;

		// Token: 0x04006726 RID: 26406
		private Sale sale;
	}
}

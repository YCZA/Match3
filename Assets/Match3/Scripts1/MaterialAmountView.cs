using System;
using System.Collections;
using Match3.Scripts1.Puzzletown.Town;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.UI;
using Wooga.UnityFramework;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

// Token: 0x020006DA RID: 1754
namespace Match3.Scripts1
{
	public class MaterialAmountView : ATableViewReusableCell, IDataView<MaterialAmount>, IEditorDescription
	{
		// Token: 0x170006E6 RID: 1766
		// (get) Token: 0x06002B9E RID: 11166 RVA: 0x000C7BF7 File Offset: 0x000C5FF7
		public MaterialAmount Data
		{
			get
			{
				return this.data;
			}
		}

		// Token: 0x170006E7 RID: 1767
		// (get) Token: 0x06002B9F RID: 11167 RVA: 0x000C7BFF File Offset: 0x000C5FFF
		public CanvasGroup canvasGroup
		{
			get
			{
				return base.GetComponent<CanvasGroup>();
			}
		}

		// Token: 0x06002BA0 RID: 11168 RVA: 0x000C7C07 File Offset: 0x000C6007
		private void Start()
		{
			WooroutineRunner.StartCoroutine(this.StartRoutine(), null);
		}

		// Token: 0x06002BA1 RID: 11169 RVA: 0x000C7C18 File Offset: 0x000C6018
		private IEnumerator StartRoutine()
		{
			yield return ServiceLocator.Instance.Inject(this);
			this.Refresh(this.data);
			yield break;
		}

		// Token: 0x06002BA2 RID: 11170 RVA: 0x000C7C33 File Offset: 0x000C6033
		public void HideAmounts(bool hide)
		{
			if (this.label)
			{
				this.label.gameObject.SetActive(!hide);
			}
		}

		// Token: 0x06002BA3 RID: 11171 RVA: 0x000C7C5C File Offset: 0x000C605C
		public void Show(MaterialAmount mat)
		{
			MaterialAmountUsage materialAmountUsage = mat.Usage;
			if (materialAmountUsage != MaterialAmountUsage.Undefined)
			{
				if (mat.Usage == this.usage)
				{
					this.data = mat;
					this.Refresh(this.data);
				}
			}
			else
			{
				this.data = mat;
				this.Refresh(this.data);
			}
			this.Show();
		}

		// Token: 0x06002BA4 RID: 11172 RVA: 0x000C7CC4 File Offset: 0x000C60C4
		public void Show()
		{
			if (!base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(true);
			}
			if (this.canvasGroup)
			{
				this.canvasGroup.alpha = 1f;
			}
		}

		// Token: 0x06002BA5 RID: 11173 RVA: 0x000C7D02 File Offset: 0x000C6102
		public void Hide()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x06002BA6 RID: 11174 RVA: 0x000C7D10 File Offset: 0x000C6110
		public virtual void Refresh(MaterialAmount mat)
		{
			if (this.loc == null || string.IsNullOrEmpty(this.data.type))
			{
				return;
			}
			if (this.label)
			{
				MaterialAmountUsage materialAmountUsage = this.usage;
				if (materialAmountUsage != MaterialAmountUsage.Income)
				{
					this.label.text = mat.ToString(this.loc);
				}
				else
				{
					this.label.text = ((mat.amount >= 0) ? ("+" + mat.ToString(this.loc)) : mat.ToString(this.loc));
				}
			}
			if (this.specialOfferLabel)
			{
				this.specialOfferLabel.text = mat.SpecialOffer.ToString();
			}
			if (this.image && this.manager)
			{
				this.image.sprite = this.manager.GetSimilar(this.data.type);
			}
		}

		// Token: 0x06002BA7 RID: 11175 RVA: 0x000C7E38 File Offset: 0x000C6238
		public virtual string GetEditorDescription()
		{
			MaterialAmountUsage materialAmountUsage = this.usage;
			if (materialAmountUsage != MaterialAmountUsage.Undefined)
			{
				return this.usage.ToString();
			}
			return (!this.manager) ? "No Manager" : this.manager.name;
		}

		// Token: 0x170006E8 RID: 1768
		// (get) Token: 0x06002BA8 RID: 11176 RVA: 0x000C7E90 File Offset: 0x000C6290
		public override int reusableId
		{
			get
			{
				MaterialAmountUsage materialAmountUsage = this.usage;
				if (materialAmountUsage != MaterialAmountUsage.Undefined)
				{
					return (int)this.usage;
				}
				return (!this.manager) ? 0 : this.manager.name.GetHashCode();
			}
		}

		// Token: 0x06002BA9 RID: 11177 RVA: 0x000C7EDC File Offset: 0x000C62DC
		public static void CollectMaterials(TownResourcePanelRoot ui, GameObject root)
		{
			MaterialAmountView[] ie = Array.FindAll<MaterialAmountView>(root.GetComponentsInChildren<MaterialAmountView>(), (MaterialAmountView v) => v.usage == MaterialAmountUsage.Reward);
			ie.ForEach(delegate(MaterialAmountView view)
			{
				ui.CollectMaterials(view.Data, view.image.transform, true);
			});
		}

		// Token: 0x040054BB RID: 21691
		[WaitForService(true, true)]
		protected ILocalizationService loc;

		// Token: 0x040054BC RID: 21692
		protected MaterialAmount data;

		// Token: 0x040054BD RID: 21693
		public TMP_Text label;

		// Token: 0x040054BE RID: 21694
		public TMP_Text specialOfferLabel;

		// Token: 0x040054BF RID: 21695
		public Image image;

		// Token: 0x040054C0 RID: 21696
		public SpriteManager manager;

		// Token: 0x040054C1 RID: 21697
		[FormerlySerializedAs("showSign")]
		public MaterialAmountUsage usage;
	}
}

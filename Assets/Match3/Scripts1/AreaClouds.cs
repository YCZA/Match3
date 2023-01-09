using System.Collections.Generic;
using UnityEngine;

// Token: 0x020008DE RID: 2270
namespace Match3.Scripts1
{
	public class AreaClouds : AVisibleGameObject, IHandler<BuildingOperation>
	{
		// Token: 0x0600373C RID: 14140 RVA: 0x0010DB69 File Offset: 0x0010BF69
		public void Toggle()
		{
			this.isSelected = !this.isSelected;
		}

		// Token: 0x17000876 RID: 2166
		// (get) Token: 0x0600373D RID: 14141 RVA: 0x0010DB7A File Offset: 0x0010BF7A
		// (set) Token: 0x0600373E RID: 14142 RVA: 0x0010DB84 File Offset: 0x0010BF84
		public bool isSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				if (this.indicators != null)
				{
					this.indicators.ForEach(delegate(ChapterIndicator ind)
					{
						// 审核模式不显示chapter indicator
// #if !REVIEW_VERSION
						ind.SetVisibility(value);
// #endif
					});
				}
				if (this.selectedMaterial && this.defaultMaterial && this.cloudsRenderer)
				{
					this.cloudsRenderer.material = ((!value) ? this.defaultMaterial : this.selectedMaterial);
				}
				this._isSelected = value;
			}
		}

		// Token: 0x0600373F RID: 14143 RVA: 0x0010DC23 File Offset: 0x0010C023
		public void OnDestroy()
		{
			if (this.areaLabel != null)
			{
				this.areaLabel.RemoveLocaListener();
			}
		}

		// Token: 0x06003740 RID: 14144 RVA: 0x0010DC41 File Offset: 0x0010C041
		public void Handle(BuildingOperation op)
		{
			base.GetComponentInParent<Clouds>().OnAreaClick(this.area);
		}

		// Token: 0x04005F68 RID: 24424
		private bool _isSelected;

		// Token: 0x04005F69 RID: 24425
		public int area;

		// Token: 0x04005F6A RID: 24426
		public Renderer cloudsRenderer;

		// Token: 0x04005F6B RID: 24427
		public TownAreaLabel areaLabel;

		// Token: 0x04005F6C RID: 24428
		public Material defaultMaterial;

		// Token: 0x04005F6D RID: 24429
		public Material selectedMaterial;

		// Token: 0x04005F6E RID: 24430
		public IEnumerable<ChapterIndicator> indicators;
	}
}

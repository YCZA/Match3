using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000C25 RID: 3109
	[AddComponentMenu("UI/Extensions/Pagination Manager")]
	public class PaginationManager : ToggleGroup
	{
		// Token: 0x0600495F RID: 18783 RVA: 0x001771E8 File Offset: 0x001755E8
		protected PaginationManager()
		{
		}

		// Token: 0x17000AB5 RID: 2741
		// (get) Token: 0x06004960 RID: 18784 RVA: 0x001771F0 File Offset: 0x001755F0
		public int CurrentPage
		{
			get
			{
				return this.scrollSnap.CurrentPage;
			}
		}

		// Token: 0x06004961 RID: 18785 RVA: 0x00177200 File Offset: 0x00175600
		protected override void Start()
		{
			base.Start();
			if (this.scrollSnap == null)
			{
				global::UnityEngine.Debug.LogError("A ScrollSnap script must be attached");
				return;
			}
			if (this.scrollSnap.Pagination)
			{
				this.scrollSnap.Pagination = null;
			}
			this.scrollSnap.OnSelectionPageChangedEvent.AddListener(new UnityAction<int>(this.SetToggleGraphics));
			this.scrollSnap.OnSelectionChangeEndEvent.AddListener(new UnityAction<int>(this.OnPageChangeEnd));
			this.m_PaginationChildren = base.GetComponentsInChildren<Toggle>().ToList<Toggle>();
			for (int i = 0; i < this.m_PaginationChildren.Count; i++)
			{
				this.m_PaginationChildren[i].onValueChanged.AddListener(new UnityAction<bool>(this.ToggleClick));
				this.m_PaginationChildren[i].group = this;
				this.m_PaginationChildren[i].isOn = false;
			}
			this.SetToggleGraphics(this.CurrentPage);
			if (this.m_PaginationChildren.Count != this.scrollSnap._scroll_rect.content.childCount)
			{
				global::UnityEngine.Debug.LogWarning("Uneven pagination icon to page count");
			}
		}

		// Token: 0x06004962 RID: 18786 RVA: 0x00177336 File Offset: 0x00175736
		public void GoToScreen(int pageNo)
		{
			this.scrollSnap.GoToScreen(pageNo);
		}

		// Token: 0x06004963 RID: 18787 RVA: 0x00177344 File Offset: 0x00175744
		private void ToggleClick(Toggle target)
		{
			if (!target.isOn)
			{
				this.isAClick = true;
				this.GoToScreen(this.m_PaginationChildren.IndexOf(target));
			}
		}

		// Token: 0x06004964 RID: 18788 RVA: 0x0017736C File Offset: 0x0017576C
		private void ToggleClick(bool toggle)
		{
			if (toggle)
			{
				for (int i = 0; i < this.m_PaginationChildren.Count; i++)
				{
					if (this.m_PaginationChildren[i].isOn)
					{
						this.GoToScreen(i);
						break;
					}
				}
			}
		}

		// Token: 0x06004965 RID: 18789 RVA: 0x001773BD File Offset: 0x001757BD
		private void ToggleClick(int target)
		{
			this.isAClick = true;
			this.GoToScreen(target);
		}

		// Token: 0x06004966 RID: 18790 RVA: 0x001773CD File Offset: 0x001757CD
		private void SetToggleGraphics(int pageNo)
		{
			if (!this.isAClick)
			{
				this.m_PaginationChildren[pageNo].isOn = true;
			}
		}

		// Token: 0x06004967 RID: 18791 RVA: 0x001773EC File Offset: 0x001757EC
		private void OnPageChangeEnd(int pageNo)
		{
			this.isAClick = false;
		}

		// Token: 0x04006FC7 RID: 28615
		private List<Toggle> m_PaginationChildren;

		// Token: 0x04006FC8 RID: 28616
		[SerializeField]
		private ScrollSnapBase scrollSnap;

		// Token: 0x04006FC9 RID: 28617
		private bool isAClick;
	}
}

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000C2A RID: 3114
	[AddComponentMenu("UI/Extensions/ScrollRectEx")]
	public class ScrollRectEx : ScrollRect
	{
		// Token: 0x06004976 RID: 18806 RVA: 0x0017768C File Offset: 0x00175A8C
		private void DoForParents<T>(Action<T> action) where T : IEventSystemHandler
		{
			Transform parent = base.transform.parent;
			while (parent != null)
			{
				foreach (Component component in parent.GetComponents<Component>())
				{
					if (component is T)
					{
						action((T)((object)((IEventSystemHandler)component)));
					}
				}
				parent = parent.parent;
			}
		}

		// Token: 0x06004977 RID: 18807 RVA: 0x001776F8 File Offset: 0x00175AF8
		public override void OnInitializePotentialDrag(PointerEventData eventData)
		{
			this.DoForParents<IInitializePotentialDragHandler>(delegate(IInitializePotentialDragHandler parent)
			{
				parent.OnInitializePotentialDrag(eventData);
			});
			base.OnInitializePotentialDrag(eventData);
		}

		// Token: 0x06004978 RID: 18808 RVA: 0x00177730 File Offset: 0x00175B30
		public override void OnDrag(PointerEventData eventData)
		{
			if (this.routeToParent)
			{
				this.DoForParents<IDragHandler>(delegate(IDragHandler parent)
				{
					parent.OnDrag(eventData);
				});
			}
			else
			{
				base.OnDrag(eventData);
			}
		}

		// Token: 0x06004979 RID: 18809 RVA: 0x00177778 File Offset: 0x00175B78
		public override void OnBeginDrag(PointerEventData eventData)
		{
			if (!base.horizontal && Math.Abs(eventData.delta.x) > Math.Abs(eventData.delta.y))
			{
				this.routeToParent = true;
			}
			else if (!base.vertical && Math.Abs(eventData.delta.x) < Math.Abs(eventData.delta.y))
			{
				this.routeToParent = true;
			}
			else
			{
				this.routeToParent = false;
			}
			if (this.routeToParent)
			{
				this.DoForParents<IBeginDragHandler>(delegate(IBeginDragHandler parent)
				{
					parent.OnBeginDrag(eventData);
				});
			}
			else
			{
				base.OnBeginDrag(eventData);
			}
		}

		// Token: 0x0600497A RID: 18810 RVA: 0x00177860 File Offset: 0x00175C60
		public override void OnEndDrag(PointerEventData eventData)
		{
			if (this.routeToParent)
			{
				this.DoForParents<IEndDragHandler>(delegate(IEndDragHandler parent)
				{
					parent.OnEndDrag(eventData);
				});
			}
			else
			{
				base.OnEndDrag(eventData);
			}
			this.routeToParent = false;
		}

		// Token: 0x0600497B RID: 18811 RVA: 0x001778B0 File Offset: 0x00175CB0
		public override void OnScroll(PointerEventData eventData)
		{
			if (!base.horizontal && Math.Abs(eventData.scrollDelta.x) > Math.Abs(eventData.scrollDelta.y))
			{
				this.routeToParent = true;
			}
			else if (!base.vertical && Math.Abs(eventData.scrollDelta.x) < Math.Abs(eventData.scrollDelta.y))
			{
				this.routeToParent = true;
			}
			else
			{
				this.routeToParent = false;
			}
			if (this.routeToParent)
			{
				this.DoForParents<IScrollHandler>(delegate(IScrollHandler parent)
				{
					parent.OnScroll(eventData);
				});
			}
			else
			{
				base.OnScroll(eventData);
			}
		}

		// Token: 0x04006FD3 RID: 28627
		private bool routeToParent;
	}
}

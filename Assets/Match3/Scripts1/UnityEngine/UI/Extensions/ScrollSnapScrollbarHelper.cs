using UnityEngine;
using UnityEngine.EventSystems;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000BFA RID: 3066
	public class ScrollSnapScrollbarHelper : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IEventSystemHandler
	{
		// Token: 0x060047FB RID: 18427 RVA: 0x0016F9C7 File Offset: 0x0016DDC7
		public void OnBeginDrag(PointerEventData eventData)
		{
			this.OnScrollBarDown();
		}

		// Token: 0x060047FC RID: 18428 RVA: 0x0016F9CF File Offset: 0x0016DDCF
		public void OnDrag(PointerEventData eventData)
		{
			this.ss.CurrentPage();
		}

		// Token: 0x060047FD RID: 18429 RVA: 0x0016F9DD File Offset: 0x0016DDDD
		public void OnEndDrag(PointerEventData eventData)
		{
			this.OnScrollBarUp();
		}

		// Token: 0x060047FE RID: 18430 RVA: 0x0016F9E5 File Offset: 0x0016DDE5
		public void OnPointerDown(PointerEventData eventData)
		{
			this.OnScrollBarDown();
		}

		// Token: 0x060047FF RID: 18431 RVA: 0x0016F9ED File Offset: 0x0016DDED
		public void OnPointerUp(PointerEventData eventData)
		{
			this.OnScrollBarUp();
		}

		// Token: 0x06004800 RID: 18432 RVA: 0x0016F9F5 File Offset: 0x0016DDF5
		private void OnScrollBarDown()
		{
			if (this.ss != null)
			{
				this.ss.SetLerp(false);
				this.ss.StartScreenChange();
			}
		}

		// Token: 0x06004801 RID: 18433 RVA: 0x0016FA19 File Offset: 0x0016DE19
		private void OnScrollBarUp()
		{
			this.ss.SetLerp(true);
			this.ss.ChangePage(this.ss.CurrentPage());
		}

		// Token: 0x04006EF5 RID: 28405
		internal IScrollSnap ss;
	}
}

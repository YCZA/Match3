using System.Collections.Generic;
using Match3.Scripts1.Wooga.UI;
using UnityEngine;

// Token: 0x02000976 RID: 2422
namespace Match3.Scripts1
{
	public class HideWhenBlurred : AVisibleGameObject, IEditorDescription
	{
		// Token: 0x170008FC RID: 2300
		// (get) Token: 0x06003B10 RID: 15120 RVA: 0x001246B4 File Offset: 0x00122AB4
		private Canvas canvas
		{
			get
			{
				return base.GetComponent<Canvas>();
			}
		}

		// Token: 0x170008FD RID: 2301
		// (get) Token: 0x06003B11 RID: 15121 RVA: 0x001246BC File Offset: 0x00122ABC
		public override bool IsVisible
		{
			get
			{
				return (!this.canvas) ? base.IsVisible : this.canvas.enabled;
			}
		}

		// Token: 0x06003B12 RID: 15122 RVA: 0x001246E4 File Offset: 0x00122AE4
		public override void Show()
		{
			if (this.canvas)
			{
				this.canvas.enabled = true;
				if (this.relatedCameras != null)
				{
					this.relatedCameras.ForEach(delegate(Camera camera)
					{
						if (camera)
						{
							camera.enabled = true;
						}
					});
				}
			}
			else
			{
				base.Show();
			}
		}

		// Token: 0x06003B13 RID: 15123 RVA: 0x0012474C File Offset: 0x00122B4C
		public override void Hide()
		{
			if (this.canvas)
			{
				this.canvas.enabled = false;
				this.relatedCameras = base.transform.parent.GetComponentsInChildren<Camera>();
				if (this.relatedCameras != null)
				{
					this.relatedCameras.ForEach(delegate(Camera camera)
					{
						if (camera)
						{
							camera.enabled = false;
						}
					});
				}
			}
			else
			{
				base.Hide();
			}
		}

		// Token: 0x06003B14 RID: 15124 RVA: 0x001247C9 File Offset: 0x00122BC9
		public string GetEditorDescription()
		{
			return "HideWhenBlurred";
		}

		// Token: 0x06003B15 RID: 15125 RVA: 0x001247D0 File Offset: 0x00122BD0
		public static HideWhenBlurred[] FindAllActive()
		{
			List<HideWhenBlurred> list = new List<HideWhenBlurred>(global::UnityEngine.Object.FindObjectsOfType<HideWhenBlurred>());
			list.RemoveAll((HideWhenBlurred obj) => !obj.IsVisible);
			return list.ToArray();
		}

		// Token: 0x040062EF RID: 25327
		private Camera[] relatedCameras;
	}
}

using System.Collections.Generic;
using Match3.Scripts1.Wooga.UI;
using UnityEngine;

// Token: 0x02000972 RID: 2418
namespace Match3.Scripts1
{
	public class BlurCamera : MonoBehaviour, IEditorDescription
	{
		// Token: 0x06003AFA RID: 15098 RVA: 0x00123FB0 File Offset: 0x001223B0
		public void SetCanvas(Canvas[] canvas)
		{
			this.m_ownerCanvas = canvas;
		}

		// Token: 0x06003AFB RID: 15099 RVA: 0x00123FBC File Offset: 0x001223BC
		public void Activate()
		{
			if (base.gameObject != null)
			{
				// eli key point 打开窗口，显示模糊背景时调用这里
				base.gameObject.SetActive(false);
				if (this.m_ownerCanvas != null)
				{
					this.m_relatedCameras = new List<Camera>();
					foreach (Canvas canvas in this.m_ownerCanvas)
					{
						canvas.enabled = false;
						this.m_relatedCameras.AddRangeElementsIfNotAlreadyPresent(canvas.transform.parent.GetComponentsInChildren<Camera>(), false);
					}
				}
				if (this.m_relatedCameras != null)
				{
					this.m_relatedCameras.ForEach(delegate(Camera c)
					{
						if (c)
						{
							c.enabled = false;
						}
					});
				}
				return;
			}
		}

		// Token: 0x06003AFC RID: 15100 RVA: 0x00124078 File Offset: 0x00122478
		public void Deactivate()
		{
			if (base.gameObject != null)
			{
				base.gameObject.SetActive(true);
				if (this.m_ownerCanvas != null)
				{
					foreach (Canvas canvas in this.m_ownerCanvas)
					{
						if (canvas != null)
						{
							canvas.enabled = true;
						}
					}
					if (this.m_relatedCameras != null)
					{
						this.m_relatedCameras.ForEach(delegate(Camera c)
						{
							if (c != null)
							{
								c.enabled = true;
							}
						});
						this.m_relatedCameras = null;
					}
				}
				return;
			}
		}

		// Token: 0x06003AFD RID: 15101 RVA: 0x00124120 File Offset: 0x00122520
		private void OnDestroy()
		{
			if (this.m_ownerCanvas != null)
			{
				foreach (Canvas canvas in this.m_ownerCanvas)
				{
					if (canvas != null)
					{
						canvas.enabled = true;
					}
				}
				this.m_ownerCanvas = null;
				if (this.m_relatedCameras != null)
				{
					this.m_relatedCameras.ForEach(delegate(Camera camera)
					{
						if (camera)
						{
							camera.enabled = true;
						}
					});
					this.m_relatedCameras = null;
				}
			}
		}

		// Token: 0x06003AFE RID: 15102 RVA: 0x001241AB File Offset: 0x001225AB
		public string GetEditorDescription()
		{
			return "BlurCamera";
		}

		// Token: 0x040062DB RID: 25307
		private Canvas[] m_ownerCanvas;

		// Token: 0x040062DC RID: 25308
		private List<Camera> m_relatedCameras;
	}
}

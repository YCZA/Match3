using System;
using System.Collections.Generic;
using Match3.Scripts1;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts2.UI.General
{
	public class MaskBackground : MonoBehaviour
	{
		// Token: 0x06003AE8 RID: 15080 RVA: 0x00123AF0 File Offset: 0x00121EF0
		private void OnEnable()
		{
			this.AddInputBlocker();
			base.GetComponentsInChildren<DisableWhenBlurred>().ForEach(delegate(DisableWhenBlurred c)
			{
				c.SetActive(false);
			});
			this.m_counter = MaskBackground.s_counter++;
			this.m_hiddenObjects = HideWhenBlurred.FindAllActive();
			this.m_hiddenObjects.ForEach(delegate(HideWhenBlurred obj)
			{
				obj.Hide();
			});
			this.m_blurCameras = FindObjectsOfType<BlurCamera>();
			this.m_blurCameras.ForEach(delegate(BlurCamera cam)
			{
				// 不必隐藏主相机
				// cam.Activate();
			});
			// this.m_outputCamera = this.CreaterBlur();
			// 由模糊背景，改为使用暗色背景
			this.CreateMask();
		}

		// Token: 0x06003AE9 RID: 15081 RVA: 0x00123BB0 File Offset: 0x00121FB0
		private void AddInputBlocker()
		{
			if (this.inputBlocker == null)
			{
				Canvas canvas = base.GetComponentInParent<Canvas>();
				if (canvas == null)
				{
					canvas = base.GetComponentInChildren<Canvas>();
				}
				if (canvas != null)
				{
					this.inputBlocker = new GameObject("Input Blocker");
					this.inputBlocker.transform.parent = canvas.transform;
					this.inputBlocker.transform.SetAsFirstSibling();
					this.inputBlocker.AddComponent<DummyGraphic>();
					RectTransform component = this.inputBlocker.GetComponent<RectTransform>();
					component.anchorMin = new Vector2(0f, 0f);
					component.anchorMax = new Vector2(1f, 1f);
					component.anchoredPosition = new Vector2(0.5f, 0.5f);
					component.localScale = Vector3.one;
				}
			}
		}

		// Token: 0x06003AEA RID: 15082 RVA: 0x00123C8C File Offset: 0x0012208C
		private void OnDisable()
		{
			MaskBackground maskBackground = this.FindNextBlur();
			if (maskBackground)
			{
				maskBackground.AddDependenciesFrom(this);
				if (this.m_outputCamera && maskBackground.m_outputCamera)
				{
					this.m_outputCamera.MoveTexturesTo(maskBackground.m_outputCamera);
				}
			}
			else
			{
				this.ClearDependencies();
			}
			if (this.m_outputCamera)
			{
				UnityEngine.Object.Destroy(this.m_outputCamera.gameObject);
			}

			DestoryMask();
		}

		// Token: 0x06003AEB RID: 15083 RVA: 0x00123D10 File Offset: 0x00122110
		private void AddDependenciesFrom(MaskBackground other)
		{
			if (other == null)
			{
				return;
			}
			List<BlurCamera> list = new List<BlurCamera>();
			list.AddRange(this.m_blurCameras);
			if (other.m_blurCameras != null)
			{
				list.AddRange(other.m_blurCameras);
			}
			this.m_blurCameras = list.ToArray();
			List<HideWhenBlurred> list2 = new List<HideWhenBlurred>();
			list2.AddRange(this.m_hiddenObjects);
			if (other.m_hiddenObjects != null)
			{
				list2.AddRange(other.m_hiddenObjects);
			}
			this.m_hiddenObjects = list2.ToArray();
		}

		// Token: 0x06003AEC RID: 15084 RVA: 0x00123D94 File Offset: 0x00122194
		private void ClearDependencies()
		{
			if (this.m_blurCameras != null)
			{
				this.m_blurCameras.ForEach(delegate(BlurCamera cam)
				{
					if (cam)
					{
						cam.Deactivate();
					}
				});
			}
			if (this.m_hiddenObjects != null)
			{
				this.m_hiddenObjects.ForEach(delegate(HideWhenBlurred obj)
				{
					if (obj)
					{
						obj.Show();
					}
				});
			}
		}

		// Token: 0x06003AED RID: 15085 RVA: 0x00123E08 File Offset: 0x00122208
		private MaskBackground FindNextBlur()
		{
			MaskBackground[] array = UnityEngine.Object.FindObjectsOfType<MaskBackground>();
			Array.Sort<MaskBackground>(array, (MaskBackground x, MaskBackground y) => x.m_counter.CompareTo(y.m_counter));
			return Array.Find<MaskBackground>(array, (MaskBackground x) => x.m_counter > this.m_counter && x.enabled);
		}

		// Token: 0x06003AEE RID: 15086 RVA: 0x00123E50 File Offset: 0x00122250
		private void Update()
		{
			if (this.m_outputCamera)
			{
				this.m_outputCamera.amount = this.blurAmount;
			}
		}

		private void CreateMask()
		{
			// 要防止出现多层暗色背景
			maskWindowCount++;
			if (maskWindowCount != 1) return;
				
			string arg = base.transform.parent ? base.transform.parent.name : base.name;
			GameObject canvas = new GameObject(string.Format("Mask for {0}", arg));
			canvas.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
			canvas.GetComponent<Canvas>().sortingOrder = -32768;
			GameObject imageGo = new GameObject("mask");
			imageGo.transform.parent = canvas.transform;
		
			var image = imageGo.AddComponent<Image>();
			image.color = new Color(0, 0, 0, 0.68f);
			image.rectTransform.anchorMax = new Vector2(1, 1);
			image.rectTransform.anchorMin = new Vector2(0, 0);
			image.rectTransform.anchoredPosition = new Vector2(0, 0);
			image.rectTransform.sizeDelta = new Vector2(50, 50);

			mask = canvas.GetComponent<Canvas>();
		}

		private void DestoryMask()
		{
			maskWindowCount--;
			if (mask && maskWindowCount == 0)
			{
				Destroy(mask.gameObject);
			}
		}

		private BlurTextureOnScreen CreaterBlur()
		{
			// 由模糊背景，改为使用暗色背景(mask)
			string arg = (!base.transform.parent) ? base.name : base.transform.parent.name;
			GameObject gameObject = new GameObject(string.Format("Blur for {0}", arg));
			Camera camera = gameObject.AddComponent<Camera>();
			// 关闭msaa就可以显示模糊效果了，如果开启msaa好像什么都没渲染，前面的UI移动时会有重影
			// 升级到unity2019后又没有模糊效果了, 不清楚原因, 不如直接干掉这一功能
			camera.allowMSAA = false;
			camera.cullingMask = 0;
			camera.clearFlags = CameraClearFlags.Depth;
			BlurTextureOnScreen blurTextureOnScreen = gameObject.AddComponent<BlurTextureOnScreen>();
			blurTextureOnScreen.amount = this.blurAmount;
			Canvas[] allSiblingCanvases = this.GetAllSiblingCanvases(base.GetComponentInParent<Canvas>());
			gameObject.AddComponent<BlurCamera>().SetCanvas(allSiblingCanvases);
			return blurTextureOnScreen;
		}

		// Token: 0x06003AF0 RID: 15088 RVA: 0x00123F08 File Offset: 0x00122308
		private Canvas[] GetAllSiblingCanvases(Canvas canvas)
		{
			if (canvas != null)
			{
				Transform parent = canvas.transform.parent;
				return parent.GetComponentsInChildren<Canvas>(true);
			}
			return null;
		}

		// Token: 0x040062CE RID: 25294
		private BlurTextureOnScreen m_outputCamera;
	
		private static Canvas mask;
		private static int maskWindowCount = 0;

		// Token: 0x040062CF RID: 25295
		private BlurCamera[] m_blurCameras;

		// Token: 0x040062D0 RID: 25296
		private HideWhenBlurred[] m_hiddenObjects;

		// Token: 0x040062D1 RID: 25297
		private int m_counter;

		// Token: 0x040062D2 RID: 25298
		private static int s_counter;

		// Token: 0x040062D3 RID: 25299
		private GameObject inputBlocker;

		// Token: 0x040062D4 RID: 25300
		[Range(0f, 1f)]
		public float blurAmount;
	}
}

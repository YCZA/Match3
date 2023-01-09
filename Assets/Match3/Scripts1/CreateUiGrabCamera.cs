using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000898 RID: 2200
namespace Match3.Scripts1
{
	[RequireComponent(typeof(RectTransform))]
	public class CreateUiGrabCamera : UIBehaviour
	{
		// Token: 0x17000850 RID: 2128
		// (get) Token: 0x060035D4 RID: 13780 RVA: 0x00102D78 File Offset: 0x00101178
		private static Camera BakingCamera
		{
			get
			{
				if (!CreateUiGrabCamera.m_bakingCamera)
				{
					GameObject gameObject = new GameObject("Global UI Baker");
					CreateUiGrabCamera.m_bakingCamera = gameObject.AddComponent<Camera>();
					CreateUiGrabCamera.m_bakingCamera.enabled = false;
				}
				return CreateUiGrabCamera.m_bakingCamera;
			}
		}

		// Token: 0x060035D5 RID: 13781 RVA: 0x00102DBA File Offset: 0x001011BA
		protected override void OnEnable()
		{
			base.OnEnable();
			this.BakeGraphic();
		}

		// Token: 0x060035D6 RID: 13782 RVA: 0x00102DC8 File Offset: 0x001011C8
		private void BakeGraphic()
		{
			// 缺少shader，也不清楚作用
			return;
			if (this.m_currentRoutine != null)
			{
				base.StopCoroutine(this.m_currentRoutine);
			}
			this.m_currentRoutine = base.StartCoroutine(this.BakeGraphicRoutine());
		}

		// Token: 0x060035D7 RID: 13783 RVA: 0x00102DF4 File Offset: 0x001011F4
		private IEnumerator BakeGraphicRoutine()
		{
			yield return null;
			yield return null;
			RectTransform rectTransform = base.GetComponent<RectTransform>();
			Canvas canvas = base.GetComponentInParent<Canvas>();
			Rect rect = rectTransform.rect;
			float downsample = 1920f / (float)Mathf.Max(Screen.height, Screen.width);
			while (!canvas.enabled || rect.width == 0f || rect.height == 0f || base.GetComponent<Camera>() || base.GetComponent<RawImage>())
			{
				if (!base.enabled)
				{
					yield break;
				}
				rect = rectTransform.rect;
				yield return null;
			}
			canvas.renderMode = RenderMode.WorldSpace;
			this.m_texture = this.CreateRenderTexture(rect, downsample);
			this.PopulateBakableGraphics();
			this.RenderRect(rect, this.m_texture);
			this.AddRawImageComponent(this.m_texture);
			this.DisableBakedGraphics();
			canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			this.m_currentRoutine = null;
			yield break;
		}

		// Token: 0x060035D8 RID: 13784 RVA: 0x00102E0F File Offset: 0x0010120F
		protected override void OnDisable()
		{
			base.OnDisable();
			this.RestoreBakedGraphic();
		}

		// Token: 0x060035D9 RID: 13785 RVA: 0x00102E1D File Offset: 0x0010121D
		public void RefreshBakedImage()
		{
			this.RestoreBakedGraphic();
			this.BakeGraphic();
		}

		// Token: 0x060035DA RID: 13786 RVA: 0x00102E2C File Offset: 0x0010122C
		private void RestoreBakedGraphic()
		{
			if (this.m_texture)
			{
				this.m_texture.Release();
				global::UnityEngine.Object.Destroy(this.m_texture);
				this.m_texture = null;
			}
			foreach (Graphic graphic in this.m_bakedObjects)
			{
				graphic.enabled = true;
			}
			this.m_bakedObjects.Clear();
			RawImage component = base.gameObject.GetComponent<RawImage>();
			if (component)
			{
				global::UnityEngine.Object.Destroy(component);
			}
		}

		// Token: 0x060035DB RID: 13787 RVA: 0x00102EE0 File Offset: 0x001012E0
		private RenderTexture CreateRenderTexture(Rect rect, float downsample)
		{
			return new RenderTexture((int)(rect.width / downsample), (int)(rect.height / downsample), 0)
			{
				name = base.name
			};
		}

		// Token: 0x060035DC RID: 13788 RVA: 0x00102F18 File Offset: 0x00101318
		private void AddRawImageComponent(RenderTexture texture)
		{
			RawImage rawImage = base.gameObject.AddComponent<RawImage>();
			rawImage.texture = texture;
			rawImage.raycastTarget = false;
			rawImage.material = this.displayMaterial;
		}

		// Token: 0x060035DD RID: 13789 RVA: 0x00102F4C File Offset: 0x0010134C
		private void DisableBakedGraphics()
		{
			Vector3 position = base.transform.position;
			foreach (Graphic graphic in this.m_bakedObjects)
			{
				Vector3 position2 = graphic.transform.position;
				graphic.transform.position = new Vector3(position2.x, position2.y, position.z);
				graphic.enabled = false;
			}
			base.GetComponentsInParent<RectMask2D>().ForEach(delegate(RectMask2D mask)
			{
				mask.enabled = false;
			});
		}

		// Token: 0x060035DE RID: 13790 RVA: 0x00103010 File Offset: 0x00101410
		private void PopulateBakableGraphics()
		{
			this.m_bakedObjects.Clear();
			Vector3 position = base.transform.position;
			foreach (Graphic graphic in base.GetComponentsInChildren<Graphic>())
			{
				if (!this.IsNonBakableGraphic(graphic))
				{
					Vector3 position2 = graphic.transform.position;
					graphic.transform.position = new Vector3(position2.x, position2.y, position.z + 5.5f);
					this.m_bakedObjects.Add(graphic);
				}
			}
			base.GetComponentsInParent<RectMask2D>().ForEach(delegate(RectMask2D mask)
			{
				mask.enabled = true;
			});
		}

		// Token: 0x060035DF RID: 13791 RVA: 0x001030D0 File Offset: 0x001014D0
		private bool IsNonBakableGraphic(Graphic graphic)
		{
			bool flag = graphic.transform == base.transform;
			bool flag2 = graphic.tag == this.DYNAMIC_UI_TAG;
			bool raycastTarget = graphic.raycastTarget;
			bool enabled = graphic.enabled;
			RawImage component = graphic.GetComponent<RawImage>();
			if (flag || flag2 || !enabled || component || raycastTarget)
			{
				return true;
			}
			foreach (RectTransform rectTransform in graphic.GetComponentsInParent<RectTransform>())
			{
				if (rectTransform.tag == this.DYNAMIC_UI_TAG)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060035E0 RID: 13792 RVA: 0x00103180 File Offset: 0x00101580
		private Camera SetupBakeCamera(Rect r)
		{
			Vector3 vector = this.CalculateWorldSpaceScale();
			Camera bakingCamera = CreateUiGrabCamera.BakingCamera;
			bakingCamera.cullingMask = 1 << base.gameObject.layer;
			bakingCamera.useOcclusionCulling = false;
			bakingCamera.backgroundColor = new Color(0f, 0f, 0f, 0f);
			bakingCamera.nearClipPlane = 1f;
			bakingCamera.farClipPlane = 10f;
			bakingCamera.projectionMatrix = Matrix4x4.Ortho(r.xMin * vector.x, r.xMax * vector.x, r.yMin * vector.y, r.yMax * vector.y, 1f, 10f);
			return bakingCamera;
		}

		// Token: 0x060035E1 RID: 13793 RVA: 0x00103240 File Offset: 0x00101640
		private void RenderRect(Rect r, RenderTexture texture)
		{
			Camera camera = this.SetupBakeCamera(r);
			camera.targetTexture = texture;
			camera.transform.SetParent(base.transform, false);
			camera.clearFlags = CameraClearFlags.Color;
			camera.RenderWithShader(this.prerenderShader, "RenderType");
			camera.targetTexture = null;
			camera.transform.SetParent(null, false);
		}

		// Token: 0x060035E2 RID: 13794 RVA: 0x0010329C File Offset: 0x0010169C
		private Vector3 CalculateWorldSpaceScale()
		{
			Transform transform = base.transform;
			Vector3 one = Vector3.one;
			while (transform)
			{
				one.Scale(transform.localScale);
				transform = transform.parent;
			}
			return one;
		}

		// Token: 0x060035E3 RID: 13795 RVA: 0x001032DC File Offset: 0x001016DC
		private void OnDrawGizmos()
		{
			Gizmos.color = new Color(0f, 1f, 0f, 0.25f);
			this.DrawGizmoRect(base.GetComponent<RectTransform>(), false);
			Gizmos.color = new Color(1f, 0f, 0f, 1f);
			foreach (RectTransform rectTransform in base.GetComponentsInChildren<RectTransform>())
			{
				if (!(rectTransform.gameObject.tag != this.DYNAMIC_UI_TAG))
				{
					this.DrawGizmoRect(rectTransform, true);
				}
			}
			foreach (Image image in base.GetComponentsInChildren<Image>())
			{
				if (image.raycastTarget)
				{
					this.DrawGizmoRect(image.rectTransform, true);
				}
			}
			foreach (RawImage rawImage in base.GetComponentsInChildren<RawImage>())
			{
				this.DrawGizmoRect(rawImage.rectTransform, true);
			}
		}

		// Token: 0x060035E4 RID: 13796 RVA: 0x001033F0 File Offset: 0x001017F0
		private void DrawGizmoRect(RectTransform rt, bool wire = false)
		{
			Gizmos.matrix = rt.localToWorldMatrix;
			if (wire)
			{
				Gizmos.DrawWireCube(rt.rect.center, rt.rect.size);
			}
			else
			{
				Gizmos.DrawCube(rt.rect.center, rt.rect.size);
			}
		}

		// Token: 0x04005DCA RID: 24010
		private const float ZNEAR = 1f;

		// Token: 0x04005DCB RID: 24011
		private const float ZFAR = 10f;

		// Token: 0x04005DCC RID: 24012
		private const float DOWNSAMPLE = 2f;

		// Token: 0x04005DCD RID: 24013
		private readonly string DYNAMIC_UI_TAG = "Dynamic UI";

		// Token: 0x04005DCE RID: 24014
		private readonly List<Graphic> m_bakedObjects = new List<Graphic>();

		// Token: 0x04005DCF RID: 24015
		private static Camera m_bakingCamera;

		// Token: 0x04005DD0 RID: 24016
		private RenderTexture m_texture;

		// Token: 0x04005DD1 RID: 24017
		private Coroutine m_currentRoutine;

		// Token: 0x04005DD2 RID: 24018
		public Shader prerenderShader;

		// Token: 0x04005DD3 RID: 24019
		public Material displayMaterial;
	}
}

using UnityEngine;

// Token: 0x0200091B RID: 2331
namespace Match3.Scripts1
{
	[ExecuteInEditMode]
	public class FixedFrameLerp : MonoBehaviour
	{
		// Token: 0x170008BA RID: 2234
		// (get) Token: 0x060038CA RID: 14538 RVA: 0x00117C7B File Offset: 0x0011607B
		// (set) Token: 0x060038CB RID: 14539 RVA: 0x00117C82 File Offset: 0x00116082
		public static Matrix4x4 _IsoTransform { get; private set; }

		// Token: 0x170008BB RID: 2235
		// (get) Token: 0x060038CC RID: 14540 RVA: 0x00117C8A File Offset: 0x0011608A
		// (set) Token: 0x060038CD RID: 14541 RVA: 0x00117C91 File Offset: 0x00116091
		public static Matrix4x4 _IsoTransform2 { get; private set; }

		// Token: 0x170008BC RID: 2236
		// (get) Token: 0x060038CE RID: 14542 RVA: 0x00117C99 File Offset: 0x00116099
		// (set) Token: 0x060038CF RID: 14543 RVA: 0x00117CA0 File Offset: 0x001160A0
		public static Matrix4x4 _GroundTransform { get; private set; }

		// Token: 0x170008BD RID: 2237
		// (get) Token: 0x060038D0 RID: 14544 RVA: 0x00117CA8 File Offset: 0x001160A8
		// (set) Token: 0x060038D1 RID: 14545 RVA: 0x00117CAF File Offset: 0x001160AF
		public static Matrix4x4 _GroundTransform2 { get; private set; }

		// Token: 0x170008BE RID: 2238
		// (get) Token: 0x060038D2 RID: 14546 RVA: 0x00117CB8 File Offset: 0x001160B8
		public static float FrameLerp
		{
			get
			{
				float num = FixedFrameLerp.fixedFrameTime / Time.fixedDeltaTime;
				return (FixedFrameLerp.FillFrame != 0) ? num : (1f - num);
			}
		}

		// Token: 0x170008BF RID: 2239
		// (get) Token: 0x060038D3 RID: 14547 RVA: 0x00117CE8 File Offset: 0x001160E8
		public static int FillFrame
		{
			get
			{
				return FixedFrameLerp.frame % 2;
			}
		}

		// Token: 0x060038D4 RID: 14548 RVA: 0x00117CF4 File Offset: 0x001160F4
		public static void SetupMatrices()
		{
			Matrix4x4 lhs = Matrix4x4.TRS(new Vector3(0.5f, 0f, 0.5f), Quaternion.Euler(0f, 45f, 0f), Vector3.one * 1.41421f);
			Matrix4x4 lhs2 = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 45f, 0f), Vector3.one * 1.41421f);
			Matrix4x4 identity = Matrix4x4.identity;
			identity.SetRow(1, new Vector4(0f, 0.7193398f, 0f, 0f));
			identity.SetRow(2, new Vector4(0f, 0.6946584f, 1f, 0f));
			Matrix4x4 identity2 = Matrix4x4.identity;
			identity2.SetRow(1, new Vector4(0f, 0f, 0f, 0f));
			identity2.SetRow(2, new Vector4(0f, 1.43287f, 1f, 0f));
			FixedFrameLerp._IsoTransform = lhs * identity;
			FixedFrameLerp._IsoTransform2 = lhs2 * identity;
			FixedFrameLerp._GroundTransform = lhs * identity2;
			FixedFrameLerp._GroundTransform2 = lhs2 * identity2;
			FixedFrameLerp._IsoMatrices[0] = FixedFrameLerp._IsoTransform;
			FixedFrameLerp._IsoMatrices[1] = FixedFrameLerp._GroundTransform;
			Shader.SetGlobalMatrix("_IsoTransform", FixedFrameLerp._IsoTransform);
			Shader.SetGlobalMatrix("_GroundTransform", FixedFrameLerp._GroundTransform);
			Shader.SetGlobalMatrixArray("_IsoMatrices", FixedFrameLerp._IsoMatrices);
			Shader.SetGlobalFloat("_frameLerp", 0f);
		}

		// Token: 0x060038D5 RID: 14549 RVA: 0x00117E92 File Offset: 0x00116292
		private void Awake()
		{
			FixedFrameLerp.SetupMatrices();
		}

		// Token: 0x060038D6 RID: 14550 RVA: 0x00117E99 File Offset: 0x00116299
		private void Update()
		{
			FixedFrameLerp.fixedFrameTime += Time.deltaTime;
			Shader.SetGlobalFloat("_frameLerp", FixedFrameLerp.FrameLerp);
		}

		// Token: 0x060038D7 RID: 14551 RVA: 0x00117EBA File Offset: 0x001162BA
		private void FixedUpdate()
		{
			FixedFrameLerp.fixedFrameTime = 0f;
			FixedFrameLerp.frame++;
		}

		// Token: 0x0400612D RID: 24877
		public static float fixedFrameTime = 0f;

		// Token: 0x0400612E RID: 24878
		public static int frame = 0;

		// Token: 0x04006133 RID: 24883
		private static Matrix4x4[] _IsoMatrices = new Matrix4x4[2];
	}
}

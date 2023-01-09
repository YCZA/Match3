using UnityEngine;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000B97 RID: 2967
	public class TiltWindow : MonoBehaviour
	{
		// Token: 0x0600457A RID: 17786 RVA: 0x0015FF24 File Offset: 0x0015E324
		private void Start()
		{
			this.mTrans = base.transform;
			this.mStart = this.mTrans.localRotation;
		}

		// Token: 0x0600457B RID: 17787 RVA: 0x0015FF44 File Offset: 0x0015E344
		private void Update()
		{
			Vector3 mousePosition = global::UnityEngine.Input.mousePosition;
			float num = (float)Screen.width * 0.5f;
			float num2 = (float)Screen.height * 0.5f;
			float x = Mathf.Clamp((mousePosition.x - num) / num, -1f, 1f);
			float y = Mathf.Clamp((mousePosition.y - num2) / num2, -1f, 1f);
			this.mRot = Vector2.Lerp(this.mRot, new Vector2(x, y), Time.deltaTime * 5f);
			this.mTrans.localRotation = this.mStart * Quaternion.Euler(-this.mRot.y * this.range.y, this.mRot.x * this.range.x, 0f);
		}

		// Token: 0x04006CEE RID: 27886
		public Vector2 range = new Vector2(5f, 3f);

		// Token: 0x04006CEF RID: 27887
		private Transform mTrans;

		// Token: 0x04006CF0 RID: 27888
		private Quaternion mStart;

		// Token: 0x04006CF1 RID: 27889
		private Vector2 mRot = Vector2.zero;
	}
}

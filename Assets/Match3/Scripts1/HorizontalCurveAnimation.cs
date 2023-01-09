using UnityEngine;

// Token: 0x02000992 RID: 2450
namespace Match3.Scripts1
{
	public class HorizontalCurveAnimation : MonoBehaviour
	{
		// Token: 0x06003B9A RID: 15258 RVA: 0x00127E3C File Offset: 0x0012623C
		private void Update()
		{
			this.time += Time.deltaTime;
			float x = Mathf.Lerp(this.range.x, this.range.y, this.curve.Evaluate(this.time / this.timespan));
			base.transform.localPosition = new Vector3(x, base.transform.localPosition.y, base.transform.transform.localPosition.z);
		}

		// Token: 0x0400638E RID: 25486
		public AnimationCurve curve;

		// Token: 0x0400638F RID: 25487
		public Vector2 range = new Vector2(0f, 100f);

		// Token: 0x04006390 RID: 25488
		public float timespan = 5f;

		// Token: 0x04006391 RID: 25489
		private float time;
	}
}

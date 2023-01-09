using UnityEngine;

// Token: 0x020004D8 RID: 1240
namespace Match3.Scripts1
{
	[ExecuteInEditMode]
	public class AnimateTextureOffset : MonoBehaviour
	{
		// Token: 0x06002296 RID: 8854 RVA: 0x000991E0 File Offset: 0x000975E0
		private void OnEnable()
		{
			if (this.PlayOnAwake && !this.IsPlaying)
			{
				if (this.MaterialToAnimate == null)
				{
					this.MaterialToAnimate = this.meshRenderer.sharedMaterial;
				}
				this.IsPlaying = true;
				this.TextureOffset = this.MaterialToAnimate.GetTextureOffset(this.PropertyName);
				this.AnimationStart = Time.realtimeSinceStartup;
			}
		}

		// Token: 0x06002297 RID: 8855 RVA: 0x0009924E File Offset: 0x0009764E
		private void OnDisable()
		{
			this.IsPlaying = false;
		}

		// Token: 0x06002298 RID: 8856 RVA: 0x00099258 File Offset: 0x00097658
		private void Update()
		{
			if (this.IsPlaying)
			{
				if (Time.realtimeSinceStartup < this.AnimationStart + this.AnimationLength)
				{
					this.Progress = (Time.realtimeSinceStartup - this.AnimationStart) / this.AnimationLength;
					this.TextureOffset.x = this.AnimX.Evaluate(this.Progress);
					this.TextureOffset.y = this.AnimY.Evaluate(this.Progress);
					this.MaterialToAnimate.SetTextureOffset(this.PropertyName, this.TextureOffset);
				}
				else
				{
					this.Progress = 1f;
					this.TextureOffset.x = this.AnimX.Evaluate(this.Progress);
					this.TextureOffset.y = this.AnimY.Evaluate(this.Progress);
					this.MaterialToAnimate.SetTextureOffset(this.PropertyName, this.TextureOffset);
					this.IsPlaying = false;
				}
			}
		}

		// Token: 0x04004E25 RID: 20005
		public bool PlayOnAwake = true;

		// Token: 0x04004E26 RID: 20006
		public string PropertyName = "_MainTex";

		// Token: 0x04004E27 RID: 20007
		public AnimationCurve AnimX;

		// Token: 0x04004E28 RID: 20008
		public AnimationCurve AnimY;

		// Token: 0x04004E29 RID: 20009
		public float AnimationLength = 1f;

		// Token: 0x04004E2A RID: 20010
		public MeshRenderer meshRenderer;

		// Token: 0x04004E2B RID: 20011
		private Material MaterialToAnimate;

		// Token: 0x04004E2C RID: 20012
		private float AnimationStart;

		// Token: 0x04004E2D RID: 20013
		private bool IsPlaying;

		// Token: 0x04004E2E RID: 20014
		private Vector2 TextureOffset;

		// Token: 0x04004E2F RID: 20015
		private float Progress;
	}
}

using Shared.Pooling;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020004E1 RID: 1249
	[RequireComponent(typeof(LineRenderer))]
	public class SinusLineRenderer : AReleasable
	{
		// Token: 0x1700054A RID: 1354
		// (get) Token: 0x060022B4 RID: 8884 RVA: 0x000998D2 File Offset: 0x00097CD2
		// (set) Token: 0x060022B5 RID: 8885 RVA: 0x000998DA File Offset: 0x00097CDA
		public float StayTime
		{
			get
			{
				return this.stayTime;
			}
			set
			{
				this.stayTime = value;
			}
		}

		// Token: 0x060022B6 RID: 8886 RVA: 0x000998E4 File Offset: 0x00097CE4
		private void Awake()
		{
			this.lineRenderer = base.gameObject.GetComponent<LineRenderer>();
			if (this.lineRenderer == null)
			{
				WoogaDebug.LogError(new object[]
				{
					"No Line Renderer attached"
				});
			}
			this.propertyBlock = new MaterialPropertyBlock();
			this.shaderID = Shader.PropertyToID("_Distance");
		}

		// Token: 0x060022B7 RID: 8887 RVA: 0x00099944 File Offset: 0x00097D44
		private void OnDisable()
		{
			this.timer = 0f;
			if (this.moveTransformAlong && this.moveTransform != null)
			{
				this.moveTransform.position = base.transform.position;
			}
			base.transform.localPosition = Vector3.zero;
		}

		// Token: 0x060022B8 RID: 8888 RVA: 0x000999A0 File Offset: 0x00097DA0
		private void Update()
		{
			this.timer += Time.deltaTime / this.stayTime;
			if (this.animateAlpha)
			{
				Color color = new Color(this.startColor.r, this.startColor.g, this.startColor.b, Mathf.Lerp(this.startColor.a, this.alphaStaytimeValue.x, 1f - this.timer));
				Color color2 = new Color(this.endColor.r, this.endColor.g, this.endColor.b, Mathf.Lerp(this.endColor.a, this.alphaStaytimeValue.y, 1f - this.timer));
				this.lineRenderer.startColor = color;
				this.lineRenderer.endColor = color2;
			}
			if (this.moveTransformAlong && this.moveTransform != null)
			{
				this.moveTransform.position = Vector3.Lerp(base.transform.position, this.targetPosition, this.timer);
			}
		}

		// Token: 0x060022B9 RID: 8889 RVA: 0x00099ACC File Offset: 0x00097ECC
		public void SetupLineRenderer()
		{
			this.lineRenderer.startColor = this.startColor;
			this.lineRenderer.endColor = this.endColor;
			this.timer = 0f;
			this.direction = this.targetPosition - base.transform.position;
			this.distance = this.direction.magnitude;
			this.directionForwardCross = Vector3.Cross(this.direction.normalized, Vector3.forward);
			int num = (int)(this.distance + 1f);
			this.vertexPoints = num * this.segmentResolution;
			this.lineRenderer.positionCount = this.vertexPoints;
			Vector3[] array = new Vector3[this.vertexPoints];
			int i = 0;
			float num2 = (!this.randomizeSinusPhase) ? 0f : (global::UnityEngine.Random.value * (float)this.vertexPoints * 2f);
			while (i < this.vertexPoints)
			{
				array[i] = base.transform.position + (float)i * this.direction.normalized / (float)this.segmentResolution;
				float d = this.sinusInfluenceShape.Evaluate(Mathf.Clamp01((float)i / (float)this.segmentResolution / (float)num));
				if (this.sinus1Frequency > 0f)
				{
					array[i] += this.directionForwardCross * Mathf.Sin((float)i * this.sinus1Frequency + num2) * d;
				}
				if (this.sinus2StrengthMultiplier > 0f || this.sinus2Frequency > 0f)
				{
					array[i] += this.directionForwardCross * Mathf.Sin((float)i * this.sinus2Frequency) * d * this.sinus2StrengthMultiplier;
				}
				float num3 = global::UnityEngine.Random.Range(-this.noiseMultiplier.y, this.noiseMultiplier.y);
				float num4 = global::UnityEngine.Random.Range(-this.noiseMultiplier.x, this.noiseMultiplier.x);
				if (this.noiseAlongLine)
				{
					array[i] += this.directionForwardCross * num3 * d;
					array[i] += this.direction * num4 * d;
				}
				else
				{
					array[i] += new Vector3(num4, num3, 0f) * d;
				}
				i++;
			}
			this.lineRenderer.SetPositions(array);
			this.lineRenderer.GetPropertyBlock(this.propertyBlock);
			this.propertyBlock.SetFloat(this.shaderID, this.distance * this.UV2tilingPerUnit);
			this.lineRenderer.SetPropertyBlock(this.propertyBlock);
			if (this.constantSpeed)
			{
				this.stayTime = this.distance / this.speed;
			}
		}

		// Token: 0x04004E4A RID: 20042
		public Vector3 targetPosition;

		// Token: 0x04004E4B RID: 20043
		public Color startColor = Color.white;

		// Token: 0x04004E4C RID: 20044
		public Color endColor = Color.black;

		// Token: 0x04004E4D RID: 20045
		[SerializeField]
		private float speed = 4.42f;

		// Token: 0x04004E4E RID: 20046
		[SerializeField]
		private bool constantSpeed;

		// Token: 0x04004E4F RID: 20047
		public bool animateAlpha = true;

		// Token: 0x04004E50 RID: 20048
		public Vector2 alphaStaytimeValue = new Vector2(1f, 0f);

		// Token: 0x04004E51 RID: 20049
		public bool randomizeSinusPhase;

		// Token: 0x04004E52 RID: 20050
		public float sinus1Frequency = 1f;

		// Token: 0x04004E53 RID: 20051
		public float sinus2Frequency = 0.5f;

		// Token: 0x04004E54 RID: 20052
		public float sinus2StrengthMultiplier = 0.5f;

		// Token: 0x04004E55 RID: 20053
		public int segmentResolution;

		// Token: 0x04004E56 RID: 20054
		public Transform moveTransform;

		// Token: 0x04004E57 RID: 20055
		public bool moveTransformAlong;

		// Token: 0x04004E58 RID: 20056
		public bool noiseAlongLine = true;

		// Token: 0x04004E59 RID: 20057
		public Vector2 noiseMultiplier = new Vector2(0.5f, 0.5f);

		// Token: 0x04004E5A RID: 20058
		public AnimationCurve sinusInfluenceShape = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x04004E5B RID: 20059
		public float UV2tilingPerUnit = 2f;

		// Token: 0x04004E5C RID: 20060
		private float timer;

		// Token: 0x04004E5D RID: 20061
		private float stayTime = 1f;

		// Token: 0x04004E5E RID: 20062
		private LineRenderer lineRenderer;

		// Token: 0x04004E5F RID: 20063
		private int shaderID;

		// Token: 0x04004E60 RID: 20064
		private int vertexPoints = 20;

		// Token: 0x04004E61 RID: 20065
		private MaterialPropertyBlock propertyBlock;

		// Token: 0x04004E62 RID: 20066
		private float distance;

		// Token: 0x04004E63 RID: 20067
		private Vector3 direction;

		// Token: 0x04004E64 RID: 20068
		private Vector3 directionForwardCross;
	}
}

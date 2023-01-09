using System.Collections.Generic;
using UnityEngine;

// Token: 0x020008BA RID: 2234
namespace Match3.Scripts1
{
	public class RangeMovementBehavior : MonoBehaviour
	{
		// Token: 0x0600368B RID: 13963 RVA: 0x00109E3C File Offset: 0x0010823C
		private void Start()
		{
			this.entities = new List<RangeMovementBehavior.entity>();
			for (int i = 0; i < this.grpEntities.transform.childCount; i++)
			{
				RangeMovementBehavior.entity entity = new RangeMovementBehavior.entity();
				entity.origin = this.grpEntities.transform.GetChild(i).transform.position;
				entity.entityObj = this.grpEntities.transform.GetChild(i);
				entity.childObj = null;
				if (entity.entityObj.childCount > 0)
				{
					entity.childObj = entity.entityObj.GetChild(0);
				}
				entity.turningFlag = false;
				this.entities.Add(entity);
				this.entities[i].entityObj.transform.Rotate(0f, (float)global::UnityEngine.Random.Range(0, 360), 0f);
			}
		}

		// Token: 0x0600368C RID: 13964 RVA: 0x00109F21 File Offset: 0x00108321
		private void Update()
		{
			this.UpdateEntity();
		}

		// Token: 0x0600368D RID: 13965 RVA: 0x00109F2C File Offset: 0x0010832C
		private void UpdateEntity()
		{
			for (int i = 0; i < this.entities.Count; i++)
			{
				this.entities[i].entityObj.Translate(0f, 0f, this.movementSpeed);
				if (!this.entities[i].turningFlag)
				{
					if ((this.entities[i].origin - this.entities[i].entityObj.position).magnitude > this.maxDistance)
					{
						float num = Vector3.Dot(this.entities[i].origin - this.entities[i].entityObj.position, this.entities[i].entityObj.forward);
						if (num < 0f)
						{
							this.InitTurn(i, this.minTurnDuration, this.maxTurnDuration, this.turningAngleRandom);
						}
					}
				}
				else if (this.entities[i].turnTime <= this.entities[i].turnDuration)
				{
					this.entities[i].turnTime += Time.deltaTime;
					if (this.shouldBankInCornersFlag && this.entities[i].childObj != null)
					{
						this.entities[i].childObj.localEulerAngles = new Vector3(0f, 0f, this.maxBankingAngle * this.entities[i].turnAngleMultiplier * this.bankingCurve.Evaluate(this.entities[i].turnTime / this.entities[i].turnDuration));
					}
					this.entities[i].entityObj.localEulerAngles = new Vector3(0f, this.entities[i].turnStartAngle + this.entities[i].turnAngle * (this.entities[i].turnTime / this.entities[i].turnDuration), 0f);
				}
				else
				{
					this.entities[i].turningFlag = false;
				}
			}
		}

		// Token: 0x0600368E RID: 13966 RVA: 0x0010A19C File Offset: 0x0010859C
		private void InitTurn(int index, float minDuration, float maxDuration, float angleRandom)
		{
			this.entities[index].turnTime = 0f;
			this.entities[index].turnDuration = global::UnityEngine.Random.Range(minDuration, maxDuration);
			this.entities[index].turnStartAngle = this.entities[index].entityObj.transform.localEulerAngles.y;
			bool flag = this.IsTurningLeft(this.entities[index].entityObj, this.entities[index].origin);
			this.entities[index].turnAngleMultiplier = ((!flag) ? 1f : -1f);
			this.entities[index].turnAngle = 200f + global::UnityEngine.Random.Range(-angleRandom, angleRandom);
			if (flag)
			{
				this.entities[index].turnAngle -= 360f;
			}
			this.entities[index].turningFlag = true;
		}

		// Token: 0x0600368F RID: 13967 RVA: 0x0010A2B0 File Offset: 0x001086B0
		private bool IsTurningLeft(Transform entity, Vector3 origin)
		{
			return Vector3.Cross(origin - entity.position, entity.forward).y > 0f;
		}

		// Token: 0x04005E8B RID: 24203
		private List<RangeMovementBehavior.entity> entities;

		// Token: 0x04005E8C RID: 24204
		public Transform grpEntities;

		// Token: 0x04005E8D RID: 24205
		public float movementSpeed = 0.01f;

		// Token: 0x04005E8E RID: 24206
		public float maxDistance = 5f;

		// Token: 0x04005E8F RID: 24207
		public float minTurnDuration = 1f;

		// Token: 0x04005E90 RID: 24208
		public float maxTurnDuration = 3f;

		// Token: 0x04005E91 RID: 24209
		public float turningAngleRandom = 30f;

		// Token: 0x04005E92 RID: 24210
		public float maxBankingAngle = 45f;

		// Token: 0x04005E93 RID: 24211
		public AnimationCurve bankingCurve;

		// Token: 0x04005E94 RID: 24212
		public bool shouldBankInCornersFlag;

		// Token: 0x04005E95 RID: 24213
		public const float TURN_TOWARDS_ORIGIN_ANGLE = 200f;

		// Token: 0x020008BB RID: 2235
		public class entity
		{
			// Token: 0x04005E96 RID: 24214
			public Vector3 origin;

			// Token: 0x04005E97 RID: 24215
			public Transform entityObj;

			// Token: 0x04005E98 RID: 24216
			public Transform childObj;

			// Token: 0x04005E99 RID: 24217
			public float turnStartAngle;

			// Token: 0x04005E9A RID: 24218
			public float turnAngle;

			// Token: 0x04005E9B RID: 24219
			public float turnTime;

			// Token: 0x04005E9C RID: 24220
			public float turnDuration;

			// Token: 0x04005E9D RID: 24221
			public bool turningFlag;

			// Token: 0x04005E9E RID: 24222
			public float turnAngleMultiplier;
		}
	}
}

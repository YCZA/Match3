using System;
using UnityEngine;

namespace Match3.Scripts1.Spine.Unity
{
	// Token: 0x02000259 RID: 601
	public static class SkeletonExtensions
	{
		// Token: 0x06001284 RID: 4740 RVA: 0x0003A8F2 File Offset: 0x00038CF2
		public static Color GetColor(this Skeleton s)
		{
			return new Color(s.r, s.g, s.b, s.a);
		}

		// Token: 0x06001285 RID: 4741 RVA: 0x0003A911 File Offset: 0x00038D11
		public static Color GetColor(this RegionAttachment a)
		{
			return new Color(a.r, a.g, a.b, a.a);
		}

		// Token: 0x06001286 RID: 4742 RVA: 0x0003A930 File Offset: 0x00038D30
		public static Color GetColor(this MeshAttachment a)
		{
			return new Color(a.r, a.g, a.b, a.a);
		}

		// Token: 0x06001287 RID: 4743 RVA: 0x0003A94F File Offset: 0x00038D4F
		public static Color GetColor(this WeightedMeshAttachment a)
		{
			return new Color(a.r, a.g, a.b, a.a);
		}

		// Token: 0x06001288 RID: 4744 RVA: 0x0003A96E File Offset: 0x00038D6E
		public static void SetColor(this Skeleton skeleton, Color color)
		{
			skeleton.A = color.a;
			skeleton.R = color.r;
			skeleton.G = color.g;
			skeleton.B = color.b;
		}

		// Token: 0x06001289 RID: 4745 RVA: 0x0003A9A4 File Offset: 0x00038DA4
		public static void SetColor(this Skeleton skeleton, Color32 color)
		{
			skeleton.A = (float)color.a * 0.003921569f;
			skeleton.R = (float)color.r * 0.003921569f;
			skeleton.G = (float)color.g * 0.003921569f;
			skeleton.B = (float)color.b * 0.003921569f;
		}

		// Token: 0x0600128A RID: 4746 RVA: 0x0003AA01 File Offset: 0x00038E01
		public static void SetColor(this Slot slot, Color color)
		{
			slot.A = color.a;
			slot.R = color.r;
			slot.G = color.g;
			slot.B = color.b;
		}

		// Token: 0x0600128B RID: 4747 RVA: 0x0003AA38 File Offset: 0x00038E38
		public static void SetColor(this Slot slot, Color32 color)
		{
			slot.A = (float)color.a * 0.003921569f;
			slot.R = (float)color.r * 0.003921569f;
			slot.G = (float)color.g * 0.003921569f;
			slot.B = (float)color.b * 0.003921569f;
		}

		// Token: 0x0600128C RID: 4748 RVA: 0x0003AA95 File Offset: 0x00038E95
		public static void SetColor(this RegionAttachment attachment, Color color)
		{
			attachment.A = color.a;
			attachment.R = color.r;
			attachment.G = color.g;
			attachment.B = color.b;
		}

		// Token: 0x0600128D RID: 4749 RVA: 0x0003AACC File Offset: 0x00038ECC
		public static void SetColor(this RegionAttachment attachment, Color32 color)
		{
			attachment.A = (float)color.a * 0.003921569f;
			attachment.R = (float)color.r * 0.003921569f;
			attachment.G = (float)color.g * 0.003921569f;
			attachment.B = (float)color.b * 0.003921569f;
		}

		// Token: 0x0600128E RID: 4750 RVA: 0x0003AB29 File Offset: 0x00038F29
		public static void SetColor(this MeshAttachment attachment, Color color)
		{
			attachment.A = color.a;
			attachment.R = color.r;
			attachment.G = color.g;
			attachment.B = color.b;
		}

		// Token: 0x0600128F RID: 4751 RVA: 0x0003AB60 File Offset: 0x00038F60
		public static void SetColor(this MeshAttachment attachment, Color32 color)
		{
			attachment.A = (float)color.a * 0.003921569f;
			attachment.R = (float)color.r * 0.003921569f;
			attachment.G = (float)color.g * 0.003921569f;
			attachment.B = (float)color.b * 0.003921569f;
		}

		// Token: 0x06001290 RID: 4752 RVA: 0x0003ABBD File Offset: 0x00038FBD
		public static void SetColor(this WeightedMeshAttachment attachment, Color color)
		{
			attachment.A = color.a;
			attachment.R = color.r;
			attachment.G = color.g;
			attachment.B = color.b;
		}

		// Token: 0x06001291 RID: 4753 RVA: 0x0003ABF4 File Offset: 0x00038FF4
		public static void SetColor(this WeightedMeshAttachment attachment, Color32 color)
		{
			attachment.A = (float)color.a * 0.003921569f;
			attachment.R = (float)color.r * 0.003921569f;
			attachment.G = (float)color.g * 0.003921569f;
			attachment.B = (float)color.b * 0.003921569f;
		}

		// Token: 0x06001292 RID: 4754 RVA: 0x0003AC51 File Offset: 0x00039051
		public static void SetPosition(this Bone bone, Vector2 position)
		{
			bone.X = position.x;
			bone.Y = position.y;
		}

		// Token: 0x06001293 RID: 4755 RVA: 0x0003AC6D File Offset: 0x0003906D
		public static void SetPosition(this Bone bone, Vector3 position)
		{
			bone.X = position.x;
			bone.Y = position.y;
		}

		// Token: 0x06001294 RID: 4756 RVA: 0x0003AC89 File Offset: 0x00039089
		public static Vector2 GetSkeletonSpacePosition(this Bone bone)
		{
			return new Vector2(bone.worldX, bone.worldY);
		}

		// Token: 0x06001295 RID: 4757 RVA: 0x0003AC9C File Offset: 0x0003909C
		public static Vector3 GetWorldPosition(this Bone bone, Transform parentTransform)
		{
			return parentTransform.TransformPoint(new Vector3(bone.worldX, bone.worldY));
		}

		// Token: 0x06001296 RID: 4758 RVA: 0x0003ACB8 File Offset: 0x000390B8
		public static void PoseWithAnimation(this Skeleton skeleton, string animationName, float time, bool loop)
		{
			Animation animation = skeleton.data.FindAnimation(animationName);
			if (animation == null)
			{
				return;
			}
			animation.Apply(skeleton, 0f, time, loop, null);
		}

		// Token: 0x06001297 RID: 4759 RVA: 0x0003ACE8 File Offset: 0x000390E8
		public static void SetDrawOrderToSetupPose(this Skeleton skeleton)
		{
			Slot[] items = skeleton.slots.Items;
			int count = skeleton.slots.Count;
			ExposedList<Slot> drawOrder = skeleton.drawOrder;
			drawOrder.Clear(false);
			drawOrder.GrowIfNeeded(count);
			Array.Copy(items, drawOrder.Items, count);
		}

		// Token: 0x06001298 RID: 4760 RVA: 0x0003AD30 File Offset: 0x00039130
		public static void SetColorToSetupPose(this Slot slot)
		{
			slot.r = slot.data.r;
			slot.g = slot.data.g;
			slot.b = slot.data.b;
			slot.a = slot.data.a;
		}

		// Token: 0x06001299 RID: 4761 RVA: 0x0003AD84 File Offset: 0x00039184
		public static void SetAttachmentToSetupPose(this Slot slot)
		{
			SlotData data = slot.data;
			slot.Attachment = slot.bone.skeleton.GetAttachment(data.name, data.attachmentName);
		}

		// Token: 0x0600129A RID: 4762 RVA: 0x0003ADBC File Offset: 0x000391BC
		public static void SetSlotAttachmentToSetupPose(this Skeleton skeleton, int slotIndex)
		{
			Slot slot = skeleton.slots.Items[slotIndex];
			Attachment attachment = skeleton.GetAttachment(slotIndex, slot.data.attachmentName);
			slot.Attachment = attachment;
		}

		// Token: 0x0600129B RID: 4763 RVA: 0x0003ADF4 File Offset: 0x000391F4
		public static void SetKeyedItemsToSetupPose(this Animation animation, Skeleton skeleton)
		{
			Timeline[] items = animation.timelines.Items;
			int i = 0;
			int num = items.Length;
			while (i < num)
			{
				items[i].SetToSetupPose(skeleton);
				i++;
			}
		}

		// Token: 0x0600129C RID: 4764 RVA: 0x0003AE2C File Offset: 0x0003922C
		public static void SetToSetupPose(this Timeline timeline, Skeleton skeleton)
		{
			if (timeline != null)
			{
				if (timeline is RotateTimeline)
				{
					Bone bone = skeleton.bones.Items[((RotateTimeline)timeline).boneIndex];
					bone.rotation = bone.data.rotation;
				}
				else if (timeline is TranslateTimeline)
				{
					Bone bone2 = skeleton.bones.Items[((TranslateTimeline)timeline).boneIndex];
					bone2.x = bone2.data.x;
					bone2.y = bone2.data.y;
				}
				else if (timeline is ScaleTimeline)
				{
					Bone bone3 = skeleton.bones.Items[((ScaleTimeline)timeline).boneIndex];
					bone3.scaleX = bone3.data.scaleX;
					bone3.scaleY = bone3.data.scaleY;
				}
				else if (timeline is FfdTimeline)
				{
					Slot slot = skeleton.slots.Items[((FfdTimeline)timeline).slotIndex];
					slot.attachmentVerticesCount = 0;
				}
				else if (timeline is AttachmentTimeline)
				{
					skeleton.SetSlotAttachmentToSetupPose(((AttachmentTimeline)timeline).slotIndex);
				}
				else if (timeline is ColorTimeline)
				{
					skeleton.slots.Items[((ColorTimeline)timeline).slotIndex].SetColorToSetupPose();
				}
				else if (timeline is IkConstraintTimeline)
				{
					IkConstraintTimeline ikConstraintTimeline = (IkConstraintTimeline)timeline;
					IkConstraint ikConstraint = skeleton.ikConstraints.Items[ikConstraintTimeline.ikConstraintIndex];
					IkConstraintData data = ikConstraint.data;
					ikConstraint.bendDirection = data.bendDirection;
					ikConstraint.mix = data.mix;
				}
				else if (timeline is DrawOrderTimeline)
				{
					skeleton.SetDrawOrderToSetupPose();
				}
			}
		}

		// Token: 0x040042A2 RID: 17058
		private const float ByteToFloat = 0.003921569f;
	}
}

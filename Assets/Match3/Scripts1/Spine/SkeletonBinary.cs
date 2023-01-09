using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Match3.Scripts1.Spine
{
	// Token: 0x0200021A RID: 538
	public class SkeletonBinary
	{
		// Token: 0x0600109B RID: 4251 RVA: 0x00028FB0 File Offset: 0x000273B0
		public SkeletonBinary(params Atlas[] atlasArray) : this(new AtlasAttachmentLoader(atlasArray))
		{
		}

		// Token: 0x0600109C RID: 4252 RVA: 0x00028FC0 File Offset: 0x000273C0
		public SkeletonBinary(AttachmentLoader attachmentLoader)
		{
			if (attachmentLoader == null)
			{
				throw new ArgumentNullException("attachmentLoader");
			}
			this.attachmentLoader = attachmentLoader;
			this.Scale = 1f;
		}

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x0600109D RID: 4253 RVA: 0x0002900E File Offset: 0x0002740E
		// (set) Token: 0x0600109E RID: 4254 RVA: 0x00029016 File Offset: 0x00027416
		public float Scale { get; set; }

		// Token: 0x0600109F RID: 4255 RVA: 0x00029020 File Offset: 0x00027420
		public SkeletonData ReadSkeletonData(string path)
		{
			SkeletonData result;
			using (BufferedStream bufferedStream = new BufferedStream(new FileStream(path, FileMode.Open)))
			{
				SkeletonData skeletonData = this.ReadSkeletonData(bufferedStream);
				skeletonData.name = Path.GetFileNameWithoutExtension(path);
				result = skeletonData;
			}
			return result;
		}

		// Token: 0x060010A0 RID: 4256 RVA: 0x00029074 File Offset: 0x00027474
		public SkeletonData ReadSkeletonData(Stream input)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			float scale = this.Scale;
			SkeletonData skeletonData = new SkeletonData();
			skeletonData.hash = this.ReadString(input);
			if (skeletonData.hash.Length == 0)
			{
				skeletonData.hash = null;
			}
			skeletonData.version = this.ReadString(input);
			if (skeletonData.version.Length == 0)
			{
				skeletonData.version = null;
			}
			skeletonData.width = this.ReadFloat(input);
			skeletonData.height = this.ReadFloat(input);
			bool flag = SkeletonBinary.ReadBoolean(input);
			if (flag)
			{
				skeletonData.imagesPath = this.ReadString(input);
				if (skeletonData.imagesPath.Length == 0)
				{
					skeletonData.imagesPath = null;
				}
			}
			int i = 0;
			int num = SkeletonBinary.ReadVarint(input, true);
			while (i < num)
			{
				string name = this.ReadString(input);
				BoneData parent = (i != 0) ? skeletonData.bones.Items[SkeletonBinary.ReadVarint(input, true)] : null;
				BoneData boneData = new BoneData(name, parent);
				boneData.rotation = this.ReadFloat(input);
				boneData.x = this.ReadFloat(input) * scale;
				boneData.y = this.ReadFloat(input) * scale;
				boneData.scaleX = this.ReadFloat(input);
				boneData.scaleY = this.ReadFloat(input);
				boneData.shearX = this.ReadFloat(input);
				boneData.shearY = this.ReadFloat(input);
				boneData.length = this.ReadFloat(input) * scale;
				boneData.inheritRotation = SkeletonBinary.ReadBoolean(input);
				boneData.inheritScale = SkeletonBinary.ReadBoolean(input);
				if (flag)
				{
					SkeletonBinary.ReadInt(input);
				}
				skeletonData.bones.Add(boneData);
				i++;
			}
			int j = 0;
			int num2 = SkeletonBinary.ReadVarint(input, true);
			while (j < num2)
			{
				IkConstraintData ikConstraintData = new IkConstraintData(this.ReadString(input));
				int k = 0;
				int num3 = SkeletonBinary.ReadVarint(input, true);
				while (k < num3)
				{
					ikConstraintData.bones.Add(skeletonData.bones.Items[SkeletonBinary.ReadVarint(input, true)]);
					k++;
				}
				ikConstraintData.target = skeletonData.bones.Items[SkeletonBinary.ReadVarint(input, true)];
				ikConstraintData.mix = this.ReadFloat(input);
				ikConstraintData.bendDirection = (int)SkeletonBinary.ReadSByte(input);
				skeletonData.ikConstraints.Add(ikConstraintData);
				j++;
			}
			int l = 0;
			int num4 = SkeletonBinary.ReadVarint(input, true);
			while (l < num4)
			{
				TransformConstraintData transformConstraintData = new TransformConstraintData(this.ReadString(input));
				transformConstraintData.bone = skeletonData.bones.Items[SkeletonBinary.ReadVarint(input, true)];
				transformConstraintData.target = skeletonData.bones.Items[SkeletonBinary.ReadVarint(input, true)];
				transformConstraintData.offsetRotation = this.ReadFloat(input);
				transformConstraintData.offsetX = this.ReadFloat(input) * scale;
				transformConstraintData.offsetY = this.ReadFloat(input) * scale;
				transformConstraintData.offsetScaleX = this.ReadFloat(input);
				transformConstraintData.offsetScaleY = this.ReadFloat(input);
				transformConstraintData.offsetShearY = this.ReadFloat(input);
				transformConstraintData.rotateMix = this.ReadFloat(input);
				transformConstraintData.translateMix = this.ReadFloat(input);
				transformConstraintData.scaleMix = this.ReadFloat(input);
				transformConstraintData.shearMix = this.ReadFloat(input);
				skeletonData.transformConstraints.Add(transformConstraintData);
				l++;
			}
			int m = 0;
			int num5 = SkeletonBinary.ReadVarint(input, true);
			while (m < num5)
			{
				string name2 = this.ReadString(input);
				BoneData boneData2 = skeletonData.bones.Items[SkeletonBinary.ReadVarint(input, true)];
				SlotData slotData = new SlotData(name2, boneData2);
				int num6 = SkeletonBinary.ReadInt(input);
				slotData.r = (float)(((long)num6 & (-16777216)) >> 24) / 255f;
				slotData.g = (float)((num6 & 16711680) >> 16) / 255f;
				slotData.b = (float)((num6 & 65280) >> 8) / 255f;
				slotData.a = (float)(num6 & 255) / 255f;
				slotData.attachmentName = this.ReadString(input);
				slotData.blendMode = (BlendMode)SkeletonBinary.ReadVarint(input, true);
				skeletonData.slots.Add(slotData);
				m++;
			}
			Skin skin = this.ReadSkin(input, "default", flag);
			if (skin != null)
			{
				skeletonData.defaultSkin = skin;
				skeletonData.skins.Add(skin);
			}
			int n = 0;
			int num7 = SkeletonBinary.ReadVarint(input, true);
			while (n < num7)
			{
				skeletonData.skins.Add(this.ReadSkin(input, this.ReadString(input), flag));
				n++;
			}
			int num8 = 0;
			int count = this.linkedMeshes.Count;
			while (num8 < count)
			{
				SkeletonJson.LinkedMesh linkedMesh = this.linkedMeshes[num8];
				Skin skin2 = (linkedMesh.skin != null) ? skeletonData.FindSkin(linkedMesh.skin) : skeletonData.DefaultSkin;
				if (skin2 == null)
				{
					throw new Exception("Skin not found: " + linkedMesh.skin);
				}
				Attachment attachment = skin2.GetAttachment(linkedMesh.slotIndex, linkedMesh.parent);
				if (attachment == null)
				{
					throw new Exception("Parent mesh not found: " + linkedMesh.parent);
				}
				if (linkedMesh.mesh is MeshAttachment)
				{
					MeshAttachment meshAttachment = (MeshAttachment)linkedMesh.mesh;
					meshAttachment.ParentMesh = (MeshAttachment)attachment;
					meshAttachment.UpdateUVs();
				}
				else
				{
					WeightedMeshAttachment weightedMeshAttachment = (WeightedMeshAttachment)linkedMesh.mesh;
					weightedMeshAttachment.ParentMesh = (WeightedMeshAttachment)attachment;
					weightedMeshAttachment.UpdateUVs();
				}
				num8++;
			}
			this.linkedMeshes.Clear();
			int num9 = 0;
			int num10 = SkeletonBinary.ReadVarint(input, true);
			while (num9 < num10)
			{
				EventData eventData = new EventData(this.ReadString(input));
				eventData.Int = SkeletonBinary.ReadVarint(input, false);
				eventData.Float = this.ReadFloat(input);
				eventData.String = this.ReadString(input);
				skeletonData.events.Add(eventData);
				num9++;
			}
			int num11 = 0;
			int num12 = SkeletonBinary.ReadVarint(input, true);
			while (num11 < num12)
			{
				this.ReadAnimation(this.ReadString(input), input, skeletonData);
				num11++;
			}
			skeletonData.bones.TrimExcess();
			skeletonData.slots.TrimExcess();
			skeletonData.skins.TrimExcess();
			skeletonData.events.TrimExcess();
			skeletonData.animations.TrimExcess();
			skeletonData.ikConstraints.TrimExcess();
			return skeletonData;
		}

		// Token: 0x060010A1 RID: 4257 RVA: 0x00029708 File Offset: 0x00027B08
		private Skin ReadSkin(Stream input, string skinName, bool nonessential)
		{
			int num = SkeletonBinary.ReadVarint(input, true);
			if (num == 0)
			{
				return null;
			}
			Skin skin = new Skin(skinName);
			for (int i = 0; i < num; i++)
			{
				int slotIndex = SkeletonBinary.ReadVarint(input, true);
				int j = 0;
				int num2 = SkeletonBinary.ReadVarint(input, true);
				while (j < num2)
				{
					string text = this.ReadString(input);
					skin.AddAttachment(slotIndex, text, this.ReadAttachment(input, skin, slotIndex, text, nonessential));
					j++;
				}
			}
			return skin;
		}

		// Token: 0x060010A2 RID: 4258 RVA: 0x00029788 File Offset: 0x00027B88
		private Attachment ReadAttachment(Stream input, Skin skin, int slotIndex, string attachmentName, bool nonessential)
		{
			float scale = this.Scale;
			string text = this.ReadString(input);
			if (text == null)
			{
				text = attachmentName;
			}
			switch (input.ReadByte())
			{
			case 0:
			{
				string text2 = this.ReadString(input);
				float rotation = this.ReadFloat(input);
				float num = this.ReadFloat(input);
				float num2 = this.ReadFloat(input);
				float scaleX = this.ReadFloat(input);
				float scaleY = this.ReadFloat(input);
				float num3 = this.ReadFloat(input);
				float num4 = this.ReadFloat(input);
				int num5 = SkeletonBinary.ReadInt(input);
				if (text2 == null)
				{
					text2 = text;
				}
				RegionAttachment regionAttachment = this.attachmentLoader.NewRegionAttachment(skin, text, text2);
				if (regionAttachment == null)
				{
					return null;
				}
				regionAttachment.Path = text2;
				regionAttachment.x = num * scale;
				regionAttachment.y = num2 * scale;
				regionAttachment.scaleX = scaleX;
				regionAttachment.scaleY = scaleY;
				regionAttachment.rotation = rotation;
				regionAttachment.width = num3 * scale;
				regionAttachment.height = num4 * scale;
				regionAttachment.r = (float)(((long)num5 & -16777216) >> 24) / 255f;
				regionAttachment.g = (float)((num5 & 16711680) >> 16) / 255f;
				regionAttachment.b = (float)((num5 & 65280) >> 8) / 255f;
				regionAttachment.a = (float)(num5 & 255) / 255f;
				regionAttachment.UpdateOffset();
				return regionAttachment;
			}
			case 1:
			{
				float[] vertices = this.ReadFloatArray(input, SkeletonBinary.ReadVarint(input, true) * 2, scale);
				BoundingBoxAttachment boundingBoxAttachment = this.attachmentLoader.NewBoundingBoxAttachment(skin, text);
				if (boundingBoxAttachment == null)
				{
					return null;
				}
				boundingBoxAttachment.vertices = vertices;
				return boundingBoxAttachment;
			}
			case 2:
			{
				string text3 = this.ReadString(input);
				int num6 = SkeletonBinary.ReadInt(input);
				int n = SkeletonBinary.ReadVarint(input, true) * 2;
				float[] regionUVs = this.ReadFloatArray(input, n, 1f);
				int[] triangles = this.ReadShortArray(input);
				float[] vertices2 = this.ReadFloatArray(input, n, scale);
				int hullLength = SkeletonBinary.ReadVarint(input, true);
				int[] edges = null;
				float num7 = 0f;
				float num8 = 0f;
				if (nonessential)
				{
					edges = this.ReadShortArray(input);
					num7 = this.ReadFloat(input);
					num8 = this.ReadFloat(input);
				}
				if (text3 == null)
				{
					text3 = text;
				}
				MeshAttachment meshAttachment = this.attachmentLoader.NewMeshAttachment(skin, text, text3);
				if (meshAttachment == null)
				{
					return null;
				}
				meshAttachment.Path = text3;
				meshAttachment.r = (float)(((long)num6 & (-16777216)) >> 24) / 255f;
				meshAttachment.g = (float)((num6 & 16711680) >> 16) / 255f;
				meshAttachment.b = (float)((num6 & 65280) >> 8) / 255f;
				meshAttachment.a = (float)(num6 & 255) / 255f;
				meshAttachment.vertices = vertices2;
				meshAttachment.triangles = triangles;
				meshAttachment.regionUVs = regionUVs;
				meshAttachment.UpdateUVs();
				meshAttachment.HullLength = hullLength;
				if (nonessential)
				{
					meshAttachment.Edges = edges;
					meshAttachment.Width = num7 * scale;
					meshAttachment.Height = num8 * scale;
				}
				return meshAttachment;
			}
			case 3:
			{
				string text4 = this.ReadString(input);
				int num9 = SkeletonBinary.ReadInt(input);
				int num10 = SkeletonBinary.ReadVarint(input, true);
				float[] array = this.ReadFloatArray(input, num10 * 2, 1f);
				int[] triangles2 = this.ReadShortArray(input);
				List<float> list = new List<float>(array.Length * 3 * 3);
				List<int> list2 = new List<int>(array.Length * 3);
				for (int i = 0; i < num10; i++)
				{
					int num11 = (int)this.ReadFloat(input);
					list2.Add(num11);
					for (int j = 0; j < num11; j++)
					{
						list2.Add((int)this.ReadFloat(input));
						list.Add(this.ReadFloat(input) * scale);
						list.Add(this.ReadFloat(input) * scale);
						list.Add(this.ReadFloat(input));
					}
				}
				int num12 = SkeletonBinary.ReadVarint(input, true);
				int[] edges2 = null;
				float num13 = 0f;
				float num14 = 0f;
				if (nonessential)
				{
					edges2 = this.ReadShortArray(input);
					num13 = this.ReadFloat(input);
					num14 = this.ReadFloat(input);
				}
				if (text4 == null)
				{
					text4 = text;
				}
				WeightedMeshAttachment weightedMeshAttachment = this.attachmentLoader.NewWeightedMeshAttachment(skin, text, text4);
				if (weightedMeshAttachment == null)
				{
					return null;
				}
				weightedMeshAttachment.Path = text4;
				weightedMeshAttachment.r = (float)(((long)num9 & (-16777216)) >> 24) / 255f;
				weightedMeshAttachment.g = (float)((num9 & 16711680) >> 16) / 255f;
				weightedMeshAttachment.b = (float)((num9 & 65280) >> 8) / 255f;
				weightedMeshAttachment.a = (float)(num9 & 255) / 255f;
				weightedMeshAttachment.bones = list2.ToArray();
				weightedMeshAttachment.weights = list.ToArray();
				weightedMeshAttachment.triangles = triangles2;
				weightedMeshAttachment.regionUVs = array;
				weightedMeshAttachment.UpdateUVs();
				weightedMeshAttachment.HullLength = num12 * 2;
				if (nonessential)
				{
					weightedMeshAttachment.Edges = edges2;
					weightedMeshAttachment.Width = num13 * scale;
					weightedMeshAttachment.Height = num14 * scale;
				}
				return weightedMeshAttachment;
			}
			case 4:
			{
				string text5 = this.ReadString(input);
				int num15 = SkeletonBinary.ReadInt(input);
				string skin2 = this.ReadString(input);
				string parent = this.ReadString(input);
				bool inheritFFD = SkeletonBinary.ReadBoolean(input);
				float num16 = 0f;
				float num17 = 0f;
				if (nonessential)
				{
					num16 = this.ReadFloat(input);
					num17 = this.ReadFloat(input);
				}
				if (text5 == null)
				{
					text5 = text;
				}
				MeshAttachment meshAttachment2 = this.attachmentLoader.NewMeshAttachment(skin, text, text5);
				if (meshAttachment2 == null)
				{
					return null;
				}
				meshAttachment2.Path = text5;
				meshAttachment2.r = (float)(((long)num15 & -16777216) >> 24) / 255f;
				meshAttachment2.g = (float)((num15 & 16711680) >> 16) / 255f;
				meshAttachment2.b = (float)((num15 & 65280) >> 8) / 255f;
				meshAttachment2.a = (float)(num15 & 255) / 255f;
				meshAttachment2.inheritFFD = inheritFFD;
				if (nonessential)
				{
					meshAttachment2.Width = num16 * scale;
					meshAttachment2.Height = num17 * scale;
				}
				this.linkedMeshes.Add(new SkeletonJson.LinkedMesh(meshAttachment2, skin2, slotIndex, parent));
				return meshAttachment2;
			}
			case 5:
			{
				string text6 = this.ReadString(input);
				int num18 = SkeletonBinary.ReadInt(input);
				string skin3 = this.ReadString(input);
				string parent2 = this.ReadString(input);
				bool inheritFFD2 = SkeletonBinary.ReadBoolean(input);
				float num19 = 0f;
				float num20 = 0f;
				if (nonessential)
				{
					num19 = this.ReadFloat(input);
					num20 = this.ReadFloat(input);
				}
				if (text6 == null)
				{
					text6 = text;
				}
				WeightedMeshAttachment weightedMeshAttachment2 = this.attachmentLoader.NewWeightedMeshAttachment(skin, text, text6);
				if (weightedMeshAttachment2 == null)
				{
					return null;
				}
				weightedMeshAttachment2.Path = text6;
				weightedMeshAttachment2.r = (float)(((long)num18 & -16777216) >> 24) / 255f;
				weightedMeshAttachment2.g = (float)((num18 & 16711680) >> 16) / 255f;
				weightedMeshAttachment2.b = (float)((num18 & 65280) >> 8) / 255f;
				weightedMeshAttachment2.a = (float)(num18 & 255) / 255f;
				weightedMeshAttachment2.inheritFFD = inheritFFD2;
				if (nonessential)
				{
					weightedMeshAttachment2.Width = num19 * scale;
					weightedMeshAttachment2.Height = num20 * scale;
				}
				this.linkedMeshes.Add(new SkeletonJson.LinkedMesh(weightedMeshAttachment2, skin3, slotIndex, parent2));
				return weightedMeshAttachment2;
			}
			default:
				return null;
			}
		}

		// Token: 0x060010A3 RID: 4259 RVA: 0x00029EE4 File Offset: 0x000282E4
		private float[] ReadFloatArray(Stream input, int n, float scale)
		{
			float[] array = new float[n];
			if (scale == 1f)
			{
				for (int i = 0; i < n; i++)
				{
					array[i] = this.ReadFloat(input);
				}
			}
			else
			{
				for (int j = 0; j < n; j++)
				{
					array[j] = this.ReadFloat(input) * scale;
				}
			}
			return array;
		}

		// Token: 0x060010A4 RID: 4260 RVA: 0x00029F44 File Offset: 0x00028344
		private int[] ReadShortArray(Stream input)
		{
			int num = SkeletonBinary.ReadVarint(input, true);
			int[] array = new int[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = (input.ReadByte() << 8 | input.ReadByte());
			}
			return array;
		}

		// Token: 0x060010A5 RID: 4261 RVA: 0x00029F88 File Offset: 0x00028388
		private void ReadAnimation(string name, Stream input, SkeletonData skeletonData)
		{
			ExposedList<Timeline> exposedList = new ExposedList<Timeline>();
			float scale = this.Scale;
			float num = 0f;
			int i = 0;
			int num2 = SkeletonBinary.ReadVarint(input, true);
			while (i < num2)
			{
				int slotIndex = SkeletonBinary.ReadVarint(input, true);
				int j = 0;
				int num3 = SkeletonBinary.ReadVarint(input, true);
				while (j < num3)
				{
					int num4 = input.ReadByte();
					int num5 = SkeletonBinary.ReadVarint(input, true);
					if (num4 != 5)
					{
						if (num4 == 4)
						{
							AttachmentTimeline attachmentTimeline = new AttachmentTimeline(num5);
							attachmentTimeline.slotIndex = slotIndex;
							for (int k = 0; k < num5; k++)
							{
								attachmentTimeline.SetFrame(k, this.ReadFloat(input), this.ReadString(input));
							}
							exposedList.Add(attachmentTimeline);
							num = Math.Max(num, attachmentTimeline.frames[num5 - 1]);
						}
					}
					else
					{
						ColorTimeline colorTimeline = new ColorTimeline(num5);
						colorTimeline.slotIndex = slotIndex;
						for (int l = 0; l < num5; l++)
						{
							float time = this.ReadFloat(input);
							int num6 = SkeletonBinary.ReadInt(input);
							float r = (float)(((long)num6 & (long)(-16777216)) >> 24) / 255f;
							float g = (float)((num6 & 16711680) >> 16) / 255f;
							float b = (float)((num6 & 65280) >> 8) / 255f;
							float a = (float)(num6 & 255) / 255f;
							colorTimeline.SetFrame(l, time, r, g, b, a);
							if (l < num5 - 1)
							{
								this.ReadCurve(input, l, colorTimeline);
							}
						}
						exposedList.Add(colorTimeline);
						num = Math.Max(num, colorTimeline.frames[num5 * 5 - 5]);
					}
					j++;
				}
				i++;
			}
			int m = 0;
			int num7 = SkeletonBinary.ReadVarint(input, true);
			while (m < num7)
			{
				int boneIndex = SkeletonBinary.ReadVarint(input, true);
				int n = 0;
				int num8 = SkeletonBinary.ReadVarint(input, true);
				while (n < num8)
				{
					int num9 = input.ReadByte();
					int num10 = SkeletonBinary.ReadVarint(input, true);
					switch (num9)
					{
					case 0:
					{
						RotateTimeline rotateTimeline = new RotateTimeline(num10);
						rotateTimeline.boneIndex = boneIndex;
						for (int num11 = 0; num11 < num10; num11++)
						{
							rotateTimeline.SetFrame(num11, this.ReadFloat(input), this.ReadFloat(input));
							if (num11 < num10 - 1)
							{
								this.ReadCurve(input, num11, rotateTimeline);
							}
						}
						exposedList.Add(rotateTimeline);
						num = Math.Max(num, rotateTimeline.frames[num10 * 2 - 2]);
						break;
					}
					case 1:
					case 2:
					case 3:
					{
						float num12 = 1f;
						TranslateTimeline translateTimeline;
						if (num9 == 2)
						{
							translateTimeline = new ScaleTimeline(num10);
						}
						else if (num9 == 3)
						{
							translateTimeline = new ShearTimeline(num10);
						}
						else
						{
							translateTimeline = new TranslateTimeline(num10);
							num12 = scale;
						}
						translateTimeline.boneIndex = boneIndex;
						for (int num13 = 0; num13 < num10; num13++)
						{
							translateTimeline.SetFrame(num13, this.ReadFloat(input), this.ReadFloat(input) * num12, this.ReadFloat(input) * num12);
							if (num13 < num10 - 1)
							{
								this.ReadCurve(input, num13, translateTimeline);
							}
						}
						exposedList.Add(translateTimeline);
						num = Math.Max(num, translateTimeline.frames[num10 * 3 - 3]);
						break;
					}
					}
					n++;
				}
				m++;
			}
			int num14 = 0;
			int num15 = SkeletonBinary.ReadVarint(input, true);
			while (num14 < num15)
			{
				IkConstraintData item = skeletonData.ikConstraints.Items[SkeletonBinary.ReadVarint(input, true)];
				int num16 = SkeletonBinary.ReadVarint(input, true);
				IkConstraintTimeline ikConstraintTimeline = new IkConstraintTimeline(num16);
				ikConstraintTimeline.ikConstraintIndex = skeletonData.ikConstraints.IndexOf(item);
				for (int num17 = 0; num17 < num16; num17++)
				{
					ikConstraintTimeline.SetFrame(num17, this.ReadFloat(input), this.ReadFloat(input), (int)SkeletonBinary.ReadSByte(input));
					if (num17 < num16 - 1)
					{
						this.ReadCurve(input, num17, ikConstraintTimeline);
					}
				}
				exposedList.Add(ikConstraintTimeline);
				num = Math.Max(num, ikConstraintTimeline.frames[num16 * 3 - 3]);
				num14++;
			}
			int num18 = 0;
			int num19 = SkeletonBinary.ReadVarint(input, true);
			while (num18 < num19)
			{
				TransformConstraintData item2 = skeletonData.transformConstraints.Items[SkeletonBinary.ReadVarint(input, true)];
				int num20 = SkeletonBinary.ReadVarint(input, true);
				TransformConstraintTimeline transformConstraintTimeline = new TransformConstraintTimeline(num20);
				transformConstraintTimeline.transformConstraintIndex = skeletonData.transformConstraints.IndexOf(item2);
				for (int num21 = 0; num21 < num20; num21++)
				{
					transformConstraintTimeline.SetFrame(num21, this.ReadFloat(input), this.ReadFloat(input), this.ReadFloat(input), this.ReadFloat(input), this.ReadFloat(input));
					if (num21 < num20 - 1)
					{
						this.ReadCurve(input, num21, transformConstraintTimeline);
					}
				}
				exposedList.Add(transformConstraintTimeline);
				num = Math.Max(num, transformConstraintTimeline.frames[num20 * 5 - 5]);
				num18++;
			}
			int num22 = 0;
			int num23 = SkeletonBinary.ReadVarint(input, true);
			while (num22 < num23)
			{
				Skin skin = skeletonData.skins.Items[SkeletonBinary.ReadVarint(input, true)];
				int num24 = 0;
				int num25 = SkeletonBinary.ReadVarint(input, true);
				while (num24 < num25)
				{
					int slotIndex2 = SkeletonBinary.ReadVarint(input, true);
					int num26 = 0;
					int num27 = SkeletonBinary.ReadVarint(input, true);
					while (num26 < num27)
					{
						Attachment attachment = skin.GetAttachment(slotIndex2, this.ReadString(input));
						int num28 = SkeletonBinary.ReadVarint(input, true);
						FfdTimeline ffdTimeline = new FfdTimeline(num28);
						ffdTimeline.slotIndex = slotIndex2;
						ffdTimeline.attachment = attachment;
						for (int num29 = 0; num29 < num28; num29++)
						{
							float time2 = this.ReadFloat(input);
							int num30;
							if (attachment is MeshAttachment)
							{
								num30 = ((MeshAttachment)attachment).vertices.Length;
							}
							else
							{
								num30 = ((WeightedMeshAttachment)attachment).weights.Length / 3 * 2;
							}
							int num31 = SkeletonBinary.ReadVarint(input, true);
							float[] array;
							if (num31 == 0)
							{
								if (attachment is MeshAttachment)
								{
									array = ((MeshAttachment)attachment).vertices;
								}
								else
								{
									array = new float[num30];
								}
							}
							else
							{
								array = new float[num30];
								int num32 = SkeletonBinary.ReadVarint(input, true);
								num31 += num32;
								if (scale == 1f)
								{
									for (int num33 = num32; num33 < num31; num33++)
									{
										array[num33] = this.ReadFloat(input);
									}
								}
								else
								{
									for (int num34 = num32; num34 < num31; num34++)
									{
										array[num34] = this.ReadFloat(input) * scale;
									}
								}
								if (attachment is MeshAttachment)
								{
									float[] vertices = ((MeshAttachment)attachment).vertices;
									int num35 = 0;
									int num36 = array.Length;
									while (num35 < num36)
									{
										array[num35] += vertices[num35];
										num35++;
									}
								}
							}
							ffdTimeline.SetFrame(num29, time2, array);
							if (num29 < num28 - 1)
							{
								this.ReadCurve(input, num29, ffdTimeline);
							}
						}
						exposedList.Add(ffdTimeline);
						num = Math.Max(num, ffdTimeline.frames[num28 - 1]);
						num26++;
					}
					num24++;
				}
				num22++;
			}
			int num37 = SkeletonBinary.ReadVarint(input, true);
			if (num37 > 0)
			{
				DrawOrderTimeline drawOrderTimeline = new DrawOrderTimeline(num37);
				int count = skeletonData.slots.Count;
				for (int num38 = 0; num38 < num37; num38++)
				{
					float time3 = this.ReadFloat(input);
					int num39 = SkeletonBinary.ReadVarint(input, true);
					int[] array2 = new int[count];
					for (int num40 = count - 1; num40 >= 0; num40--)
					{
						array2[num40] = -1;
					}
					int[] array3 = new int[count - num39];
					int num41 = 0;
					int num42 = 0;
					for (int num43 = 0; num43 < num39; num43++)
					{
						int num44 = SkeletonBinary.ReadVarint(input, true);
						while (num41 != num44)
						{
							array3[num42++] = num41++;
						}
						array2[num41 + SkeletonBinary.ReadVarint(input, true)] = num41++;
					}
					while (num41 < count)
					{
						array3[num42++] = num41++;
					}
					for (int num45 = count - 1; num45 >= 0; num45--)
					{
						if (array2[num45] == -1)
						{
							array2[num45] = array3[--num42];
						}
					}
					drawOrderTimeline.SetFrame(num38, time3, array2);
				}
				exposedList.Add(drawOrderTimeline);
				num = Math.Max(num, drawOrderTimeline.frames[num37 - 1]);
			}
			int num46 = SkeletonBinary.ReadVarint(input, true);
			if (num46 > 0)
			{
				EventTimeline eventTimeline = new EventTimeline(num46);
				for (int num47 = 0; num47 < num46; num47++)
				{
					float time4 = this.ReadFloat(input);
					EventData eventData = skeletonData.events.Items[SkeletonBinary.ReadVarint(input, true)];
					eventTimeline.SetFrame(num47, new Event(time4, eventData)
					{
						Int = SkeletonBinary.ReadVarint(input, false),
						Float = this.ReadFloat(input),
						String = ((!SkeletonBinary.ReadBoolean(input)) ? eventData.String : this.ReadString(input))
					});
				}
				exposedList.Add(eventTimeline);
				num = Math.Max(num, eventTimeline.frames[num46 - 1]);
			}
			exposedList.TrimExcess();
			skeletonData.animations.Add(new Animation(name, exposedList, num));
		}

		// Token: 0x060010A6 RID: 4262 RVA: 0x0002A8F4 File Offset: 0x00028CF4
		private void ReadCurve(Stream input, int frameIndex, CurveTimeline timeline)
		{
			int num = input.ReadByte();
			if (num != 1)
			{
				if (num == 2)
				{
					timeline.SetCurve(frameIndex, this.ReadFloat(input), this.ReadFloat(input), this.ReadFloat(input), this.ReadFloat(input));
				}
			}
			else
			{
				timeline.SetStepped(frameIndex);
			}
		}

		// Token: 0x060010A7 RID: 4263 RVA: 0x0002A950 File Offset: 0x00028D50
		private static sbyte ReadSByte(Stream input)
		{
			int num = input.ReadByte();
			if (num == -1)
			{
				throw new EndOfStreamException();
			}
			return (sbyte)num;
		}

		// Token: 0x060010A8 RID: 4264 RVA: 0x0002A973 File Offset: 0x00028D73
		private static bool ReadBoolean(Stream input)
		{
			return input.ReadByte() != 0;
		}

		// Token: 0x060010A9 RID: 4265 RVA: 0x0002A984 File Offset: 0x00028D84
		private float ReadFloat(Stream input)
		{
			this.buffer[3] = (byte)input.ReadByte();
			this.buffer[2] = (byte)input.ReadByte();
			this.buffer[1] = (byte)input.ReadByte();
			this.buffer[0] = (byte)input.ReadByte();
			return BitConverter.ToSingle(this.buffer, 0);
		}

		// Token: 0x060010AA RID: 4266 RVA: 0x0002A9D9 File Offset: 0x00028DD9
		private static int ReadInt(Stream input)
		{
			return (input.ReadByte() << 24) + (input.ReadByte() << 16) + (input.ReadByte() << 8) + input.ReadByte();
		}

		// Token: 0x060010AB RID: 4267 RVA: 0x0002AA00 File Offset: 0x00028E00
		private static int ReadVarint(Stream input, bool optimizePositive)
		{
			int num = input.ReadByte();
			int num2 = num & 127;
			if ((num & 128) != 0)
			{
				num = input.ReadByte();
				num2 |= (num & 127) << 7;
				if ((num & 128) != 0)
				{
					num = input.ReadByte();
					num2 |= (num & 127) << 14;
					if ((num & 128) != 0)
					{
						num = input.ReadByte();
						num2 |= (num & 127) << 21;
						if ((num & 128) != 0)
						{
							num2 |= (input.ReadByte() & 127) << 28;
						}
					}
				}
			}
			return (!optimizePositive) ? (num2 >> 1 ^ -(num2 & 1)) : num2;
		}

		// Token: 0x060010AC RID: 4268 RVA: 0x0002AAA0 File Offset: 0x00028EA0
		private string ReadString(Stream input)
		{
			int num = SkeletonBinary.ReadVarint(input, true);
			if (num == 0)
			{
				return null;
			}
			if (num != 1)
			{
				num--;
				byte[] array = this.buffer;
				if (array.Length < num)
				{
					array = new byte[num];
				}
				SkeletonBinary.ReadFully(input, array, 0, num);
				return Encoding.UTF8.GetString(array, 0, num);
			}
			return string.Empty;
		}

		// Token: 0x060010AD RID: 4269 RVA: 0x0002AB00 File Offset: 0x00028F00
		private static void ReadFully(Stream input, byte[] buffer, int offset, int length)
		{
			while (length > 0)
			{
				int num = input.Read(buffer, offset, length);
				if (num <= 0)
				{
					throw new EndOfStreamException();
				}
				offset += num;
				length -= num;
			}
		}

		// Token: 0x0400414F RID: 16719
		public const int TIMELINE_ROTATE = 0;

		// Token: 0x04004150 RID: 16720
		public const int TIMELINE_TRANSLATE = 1;

		// Token: 0x04004151 RID: 16721
		public const int TIMELINE_SCALE = 2;

		// Token: 0x04004152 RID: 16722
		public const int TIMELINE_SHEAR = 3;

		// Token: 0x04004153 RID: 16723
		public const int TIMELINE_ATTACHMENT = 4;

		// Token: 0x04004154 RID: 16724
		public const int TIMELINE_COLOR = 5;

		// Token: 0x04004155 RID: 16725
		public const int CURVE_LINEAR = 0;

		// Token: 0x04004156 RID: 16726
		public const int CURVE_STEPPED = 1;

		// Token: 0x04004157 RID: 16727
		public const int CURVE_BEZIER = 2;

		// Token: 0x04004159 RID: 16729
		private AttachmentLoader attachmentLoader;

		// Token: 0x0400415A RID: 16730
		private byte[] buffer = new byte[32];

		// Token: 0x0400415B RID: 16731
		private List<SkeletonJson.LinkedMesh> linkedMeshes = new List<SkeletonJson.LinkedMesh>();
	}
}

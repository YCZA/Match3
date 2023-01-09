using System;
using System.Collections.Generic;
using System.IO;

namespace Match3.Scripts1.Spine
{
	// Token: 0x0200021E RID: 542
	public class SkeletonJson
	{
		// Token: 0x060010ED RID: 4333 RVA: 0x0002B714 File Offset: 0x00029B14
		public SkeletonJson(params Atlas[] atlasArray) : this(new AtlasAttachmentLoader(atlasArray))
		{
		}

		// Token: 0x060010EE RID: 4334 RVA: 0x0002B722 File Offset: 0x00029B22
		public SkeletonJson(AttachmentLoader attachmentLoader)
		{
			if (attachmentLoader == null)
			{
				throw new ArgumentNullException("attachmentLoader cannot be null.");
			}
			this.attachmentLoader = attachmentLoader;
			this.Scale = 1f;
		}

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x060010EF RID: 4335 RVA: 0x0002B758 File Offset: 0x00029B58
		// (set) Token: 0x060010F0 RID: 4336 RVA: 0x0002B760 File Offset: 0x00029B60
		public float Scale { get; set; }

		// Token: 0x060010F1 RID: 4337 RVA: 0x0002B76C File Offset: 0x00029B6C
		public SkeletonData ReadSkeletonData(string path)
		{
			SkeletonData result;
			using (StreamReader streamReader = new StreamReader(path))
			{
				SkeletonData skeletonData = this.ReadSkeletonData(streamReader);
				skeletonData.name = Path.GetFileNameWithoutExtension(path);
				result = skeletonData;
			}
			return result;
		}

		// Token: 0x060010F2 RID: 4338 RVA: 0x0002B7BC File Offset: 0x00029BBC
		public SkeletonData ReadSkeletonData(TextReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader cannot be null.");
			}
			float scale = this.Scale;
			SkeletonData skeletonData = new SkeletonData();
			Dictionary<string, object> dictionary = Json.Deserialize(reader) as Dictionary<string, object>;
			if (dictionary == null)
			{
				throw new Exception("Invalid JSON.");
			}
			if (dictionary.ContainsKey("skeleton"))
			{
				Dictionary<string, object> dictionary2 = (Dictionary<string, object>)dictionary["skeleton"];
				skeletonData.hash = (string)dictionary2["hash"];
				skeletonData.version = (string)dictionary2["spine"];
				skeletonData.width = this.GetFloat(dictionary2, "width", 0f);
				skeletonData.height = this.GetFloat(dictionary2, "height", 0f);
			}
			foreach (object obj in ((List<object>)dictionary["bones"]))
			{
				Dictionary<string, object> dictionary3 = (Dictionary<string, object>)obj;
				BoneData boneData = null;
				if (dictionary3.ContainsKey("parent"))
				{
					boneData = skeletonData.FindBone((string)dictionary3["parent"]);
					if (boneData == null)
					{
						throw new Exception("Parent bone not found: " + dictionary3["parent"]);
					}
				}
				BoneData boneData2 = new BoneData((string)dictionary3["name"], boneData);
				boneData2.length = this.GetFloat(dictionary3, "length", 0f) * scale;
				boneData2.x = this.GetFloat(dictionary3, "x", 0f) * scale;
				boneData2.y = this.GetFloat(dictionary3, "y", 0f) * scale;
				boneData2.rotation = this.GetFloat(dictionary3, "rotation", 0f);
				boneData2.scaleX = this.GetFloat(dictionary3, "scaleX", 1f);
				boneData2.scaleY = this.GetFloat(dictionary3, "scaleY", 1f);
				boneData2.shearX = this.GetFloat(dictionary3, "shearX", 0f);
				boneData2.shearY = this.GetFloat(dictionary3, "shearY", 0f);
				boneData2.inheritScale = this.GetBoolean(dictionary3, "inheritScale", true);
				boneData2.inheritRotation = this.GetBoolean(dictionary3, "inheritRotation", true);
				skeletonData.bones.Add(boneData2);
			}
			if (dictionary.ContainsKey("ik"))
			{
				foreach (object obj2 in ((List<object>)dictionary["ik"]))
				{
					Dictionary<string, object> dictionary4 = (Dictionary<string, object>)obj2;
					IkConstraintData ikConstraintData = new IkConstraintData((string)dictionary4["name"]);
					foreach (object obj3 in ((List<object>)dictionary4["bones"]))
					{
						string text = (string)obj3;
						BoneData boneData3 = skeletonData.FindBone(text);
						if (boneData3 == null)
						{
							throw new Exception("IK bone not found: " + text);
						}
						ikConstraintData.bones.Add(boneData3);
					}
					string text2 = (string)dictionary4["target"];
					ikConstraintData.target = skeletonData.FindBone(text2);
					if (ikConstraintData.target == null)
					{
						throw new Exception("Target bone not found: " + text2);
					}
					ikConstraintData.bendDirection = ((!this.GetBoolean(dictionary4, "bendPositive", true)) ? -1 : 1);
					ikConstraintData.mix = this.GetFloat(dictionary4, "mix", 1f);
					skeletonData.ikConstraints.Add(ikConstraintData);
				}
			}
			if (dictionary.ContainsKey("transform"))
			{
				foreach (object obj4 in ((List<object>)dictionary["transform"]))
				{
					Dictionary<string, object> dictionary5 = (Dictionary<string, object>)obj4;
					TransformConstraintData transformConstraintData = new TransformConstraintData((string)dictionary5["name"]);
					string text3 = (string)dictionary5["bone"];
					transformConstraintData.bone = skeletonData.FindBone(text3);
					if (transformConstraintData.bone == null)
					{
						throw new Exception("Bone not found: " + text3);
					}
					string text4 = (string)dictionary5["target"];
					transformConstraintData.target = skeletonData.FindBone(text4);
					if (transformConstraintData.target == null)
					{
						throw new Exception("Target bone not found: " + text4);
					}
					transformConstraintData.offsetRotation = this.GetFloat(dictionary5, "rotation", 0f);
					transformConstraintData.offsetX = this.GetFloat(dictionary5, "x", 0f) * scale;
					transformConstraintData.offsetY = this.GetFloat(dictionary5, "y", 0f) * scale;
					transformConstraintData.offsetScaleX = this.GetFloat(dictionary5, "scaleX", 0f);
					transformConstraintData.offsetScaleY = this.GetFloat(dictionary5, "scaleY", 0f);
					transformConstraintData.offsetShearY = this.GetFloat(dictionary5, "shearY", 0f);
					transformConstraintData.rotateMix = this.GetFloat(dictionary5, "rotateMix", 1f);
					transformConstraintData.translateMix = this.GetFloat(dictionary5, "translateMix", 1f);
					transformConstraintData.scaleMix = this.GetFloat(dictionary5, "scaleMix", 1f);
					transformConstraintData.shearMix = this.GetFloat(dictionary5, "shearMix", 1f);
					skeletonData.transformConstraints.Add(transformConstraintData);
				}
			}
			if (dictionary.ContainsKey("slots"))
			{
				foreach (object obj5 in ((List<object>)dictionary["slots"]))
				{
					Dictionary<string, object> dictionary6 = (Dictionary<string, object>)obj5;
					string name = (string)dictionary6["name"];
					string text5 = (string)dictionary6["bone"];
					BoneData boneData4 = skeletonData.FindBone(text5);
					if (boneData4 == null)
					{
						throw new Exception("Slot bone not found: " + text5);
					}
					SlotData slotData = new SlotData(name, boneData4);
					if (dictionary6.ContainsKey("color"))
					{
						string hexString = (string)dictionary6["color"];
						slotData.r = this.ToColor(hexString, 0);
						slotData.g = this.ToColor(hexString, 1);
						slotData.b = this.ToColor(hexString, 2);
						slotData.a = this.ToColor(hexString, 3);
					}
					if (dictionary6.ContainsKey("attachment"))
					{
						slotData.attachmentName = (string)dictionary6["attachment"];
					}
					if (dictionary6.ContainsKey("blend"))
					{
						slotData.blendMode = (BlendMode)Enum.Parse(typeof(BlendMode), (string)dictionary6["blend"], false);
					}
					else
					{
						slotData.blendMode = BlendMode.normal;
					}
					skeletonData.slots.Add(slotData);
				}
			}
			if (dictionary.ContainsKey("skins"))
			{
				foreach (KeyValuePair<string, object> keyValuePair in ((Dictionary<string, object>)dictionary["skins"]))
				{
					Skin skin = new Skin(keyValuePair.Key);
					foreach (KeyValuePair<string, object> keyValuePair2 in ((Dictionary<string, object>)keyValuePair.Value))
					{
						int slotIndex = skeletonData.FindSlotIndex(keyValuePair2.Key);
						foreach (KeyValuePair<string, object> keyValuePair3 in ((Dictionary<string, object>)keyValuePair2.Value))
						{
							Attachment attachment = this.ReadAttachment(skin, slotIndex, keyValuePair3.Key, (Dictionary<string, object>)keyValuePair3.Value);
							if (attachment != null)
							{
								skin.AddAttachment(slotIndex, keyValuePair3.Key, attachment);
							}
						}
					}
					skeletonData.skins.Add(skin);
					if (skin.name == "default")
					{
						skeletonData.defaultSkin = skin;
					}
				}
			}
			int i = 0;
			int count = this.linkedMeshes.Count;
			while (i < count)
			{
				SkeletonJson.LinkedMesh linkedMesh = this.linkedMeshes[i];
				Skin skin2 = (linkedMesh.skin != null) ? skeletonData.FindSkin(linkedMesh.skin) : skeletonData.defaultSkin;
				if (skin2 == null)
				{
					throw new Exception("Slot not found: " + linkedMesh.skin);
				}
				Attachment attachment2 = skin2.GetAttachment(linkedMesh.slotIndex, linkedMesh.parent);
				if (attachment2 == null)
				{
					throw new Exception("Parent mesh not found: " + linkedMesh.parent);
				}
				if (linkedMesh.mesh is MeshAttachment)
				{
					MeshAttachment meshAttachment = (MeshAttachment)linkedMesh.mesh;
					meshAttachment.ParentMesh = (MeshAttachment)attachment2;
					meshAttachment.UpdateUVs();
				}
				else
				{
					WeightedMeshAttachment weightedMeshAttachment = (WeightedMeshAttachment)linkedMesh.mesh;
					weightedMeshAttachment.ParentMesh = (WeightedMeshAttachment)attachment2;
					weightedMeshAttachment.UpdateUVs();
				}
				i++;
			}
			this.linkedMeshes.Clear();
			if (dictionary.ContainsKey("events"))
			{
				foreach (KeyValuePair<string, object> keyValuePair4 in ((Dictionary<string, object>)dictionary["events"]))
				{
					Dictionary<string, object> map = (Dictionary<string, object>)keyValuePair4.Value;
					EventData eventData = new EventData(keyValuePair4.Key);
					eventData.Int = this.GetInt(map, "int", 0);
					eventData.Float = this.GetFloat(map, "float", 0f);
					eventData.String = this.GetString(map, "string", null);
					skeletonData.events.Add(eventData);
				}
			}
			if (dictionary.ContainsKey("animations"))
			{
				foreach (KeyValuePair<string, object> keyValuePair5 in ((Dictionary<string, object>)dictionary["animations"]))
				{
					this.ReadAnimation(keyValuePair5.Key, (Dictionary<string, object>)keyValuePair5.Value, skeletonData);
				}
			}
			skeletonData.bones.TrimExcess();
			skeletonData.slots.TrimExcess();
			skeletonData.skins.TrimExcess();
			skeletonData.events.TrimExcess();
			skeletonData.animations.TrimExcess();
			skeletonData.ikConstraints.TrimExcess();
			return skeletonData;
		}

		// Token: 0x060010F3 RID: 4339 RVA: 0x0002C404 File Offset: 0x0002A804
		private Attachment ReadAttachment(Skin skin, int slotIndex, string name, Dictionary<string, object> map)
		{
			if (map.ContainsKey("name"))
			{
				name = (string)map["name"];
			}
			float scale = this.Scale;
			AttachmentType attachmentType = AttachmentType.region;
			if (map.ContainsKey("type"))
			{
				string text = (string)map["type"];
				if (text == "skinnedmesh")
				{
					text = "weightedmesh";
				}
				attachmentType = (AttachmentType)Enum.Parse(typeof(AttachmentType), text, false);
			}
			string path = name;
			if (map.ContainsKey("path"))
			{
				path = (string)map["path"];
			}
			switch (attachmentType)
			{
			case AttachmentType.region:
			{
				RegionAttachment regionAttachment = this.attachmentLoader.NewRegionAttachment(skin, name, path);
				if (regionAttachment == null)
				{
					return null;
				}
				regionAttachment.Path = path;
				regionAttachment.x = this.GetFloat(map, "x", 0f) * scale;
				regionAttachment.y = this.GetFloat(map, "y", 0f) * scale;
				regionAttachment.scaleX = this.GetFloat(map, "scaleX", 1f);
				regionAttachment.scaleY = this.GetFloat(map, "scaleY", 1f);
				regionAttachment.rotation = this.GetFloat(map, "rotation", 0f);
				regionAttachment.width = this.GetFloat(map, "width", 32f) * scale;
				regionAttachment.height = this.GetFloat(map, "height", 32f) * scale;
				regionAttachment.UpdateOffset();
				if (map.ContainsKey("color"))
				{
					string hexString = (string)map["color"];
					regionAttachment.r = this.ToColor(hexString, 0);
					regionAttachment.g = this.ToColor(hexString, 1);
					regionAttachment.b = this.ToColor(hexString, 2);
					regionAttachment.a = this.ToColor(hexString, 3);
				}
				return regionAttachment;
			}
			case AttachmentType.boundingbox:
			{
				BoundingBoxAttachment boundingBoxAttachment = this.attachmentLoader.NewBoundingBoxAttachment(skin, name);
				if (boundingBoxAttachment == null)
				{
					return null;
				}
				boundingBoxAttachment.vertices = this.GetFloatArray(map, "vertices", scale);
				return boundingBoxAttachment;
			}
			case AttachmentType.mesh:
			case AttachmentType.linkedmesh:
			{
				MeshAttachment meshAttachment = this.attachmentLoader.NewMeshAttachment(skin, name, path);
				if (meshAttachment == null)
				{
					return null;
				}
				meshAttachment.Path = path;
				if (map.ContainsKey("color"))
				{
					string hexString2 = (string)map["color"];
					meshAttachment.r = this.ToColor(hexString2, 0);
					meshAttachment.g = this.ToColor(hexString2, 1);
					meshAttachment.b = this.ToColor(hexString2, 2);
					meshAttachment.a = this.ToColor(hexString2, 3);
				}
				meshAttachment.Width = (float)this.GetInt(map, "width", 0) * scale;
				meshAttachment.Height = (float)this.GetInt(map, "height", 0) * scale;
				string @string = this.GetString(map, "parent", null);
				if (@string == null)
				{
					meshAttachment.vertices = this.GetFloatArray(map, "vertices", scale);
					meshAttachment.triangles = this.GetIntArray(map, "triangles");
					meshAttachment.regionUVs = this.GetFloatArray(map, "uvs", 1f);
					meshAttachment.UpdateUVs();
					meshAttachment.HullLength = this.GetInt(map, "hull", 0) * 2;
					if (map.ContainsKey("edges"))
					{
						meshAttachment.Edges = this.GetIntArray(map, "edges");
					}
				}
				else
				{
					meshAttachment.InheritFFD = this.GetBoolean(map, "ffd", true);
					this.linkedMeshes.Add(new SkeletonJson.LinkedMesh(meshAttachment, this.GetString(map, "skin", null), slotIndex, @string));
				}
				return meshAttachment;
			}
			case AttachmentType.weightedmesh:
			case AttachmentType.weightedlinkedmesh:
			{
				WeightedMeshAttachment weightedMeshAttachment = this.attachmentLoader.NewWeightedMeshAttachment(skin, name, path);
				if (weightedMeshAttachment == null)
				{
					return null;
				}
				weightedMeshAttachment.Path = path;
				if (map.ContainsKey("color"))
				{
					string hexString3 = (string)map["color"];
					weightedMeshAttachment.r = this.ToColor(hexString3, 0);
					weightedMeshAttachment.g = this.ToColor(hexString3, 1);
					weightedMeshAttachment.b = this.ToColor(hexString3, 2);
					weightedMeshAttachment.a = this.ToColor(hexString3, 3);
				}
				weightedMeshAttachment.Width = (float)this.GetInt(map, "width", 0) * scale;
				weightedMeshAttachment.Height = (float)this.GetInt(map, "height", 0) * scale;
				string string2 = this.GetString(map, "parent", null);
				if (string2 == null)
				{
					float[] floatArray = this.GetFloatArray(map, "uvs", 1f);
					float[] floatArray2 = this.GetFloatArray(map, "vertices", 1f);
					List<float> list = new List<float>(floatArray.Length * 3 * 3);
					List<int> list2 = new List<int>(floatArray.Length * 3);
					int i = 0;
					int num = floatArray2.Length;
					while (i < num)
					{
						int num2 = (int)floatArray2[i++];
						list2.Add(num2);
						int num3 = i + num2 * 4;
						while (i < num3)
						{
							list2.Add((int)floatArray2[i]);
							list.Add(floatArray2[i + 1] * scale);
							list.Add(floatArray2[i + 2] * scale);
							list.Add(floatArray2[i + 3]);
							i += 4;
						}
					}
					weightedMeshAttachment.bones = list2.ToArray();
					weightedMeshAttachment.weights = list.ToArray();
					weightedMeshAttachment.triangles = this.GetIntArray(map, "triangles");
					weightedMeshAttachment.regionUVs = floatArray;
					weightedMeshAttachment.UpdateUVs();
					weightedMeshAttachment.HullLength = this.GetInt(map, "hull", 0) * 2;
					if (map.ContainsKey("edges"))
					{
						weightedMeshAttachment.Edges = this.GetIntArray(map, "edges");
					}
				}
				else
				{
					weightedMeshAttachment.InheritFFD = this.GetBoolean(map, "ffd", true);
					this.linkedMeshes.Add(new SkeletonJson.LinkedMesh(weightedMeshAttachment, this.GetString(map, "skin", null), slotIndex, string2));
				}
				return weightedMeshAttachment;
			}
			default:
				return null;
			}
		}

		// Token: 0x060010F4 RID: 4340 RVA: 0x0002CA30 File Offset: 0x0002AE30
		private float[] GetFloatArray(Dictionary<string, object> map, string name, float scale)
		{
			List<object> list = (List<object>)map[name];
			float[] array = new float[list.Count];
			if (scale == 1f)
			{
				int i = 0;
				int count = list.Count;
				while (i < count)
				{
					array[i] = (float)list[i];
					i++;
				}
			}
			else
			{
				int j = 0;
				int count2 = list.Count;
				while (j < count2)
				{
					array[j] = (float)list[j] * scale;
					j++;
				}
			}
			return array;
		}

		// Token: 0x060010F5 RID: 4341 RVA: 0x0002CAC4 File Offset: 0x0002AEC4
		private int[] GetIntArray(Dictionary<string, object> map, string name)
		{
			List<object> list = (List<object>)map[name];
			int[] array = new int[list.Count];
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				array[i] = (int)((float)list[i]);
				i++;
			}
			return array;
		}

		// Token: 0x060010F6 RID: 4342 RVA: 0x0002CB14 File Offset: 0x0002AF14
		private float GetFloat(Dictionary<string, object> map, string name, float defaultValue)
		{
			if (!map.ContainsKey(name))
			{
				return defaultValue;
			}
			return (float)map[name];
		}

		// Token: 0x060010F7 RID: 4343 RVA: 0x0002CB30 File Offset: 0x0002AF30
		private int GetInt(Dictionary<string, object> map, string name, int defaultValue)
		{
			if (!map.ContainsKey(name))
			{
				return defaultValue;
			}
			return (int)((float)map[name]);
		}

		// Token: 0x060010F8 RID: 4344 RVA: 0x0002CB4D File Offset: 0x0002AF4D
		private bool GetBoolean(Dictionary<string, object> map, string name, bool defaultValue)
		{
			if (!map.ContainsKey(name))
			{
				return defaultValue;
			}
			return (bool)map[name];
		}

		// Token: 0x060010F9 RID: 4345 RVA: 0x0002CB69 File Offset: 0x0002AF69
		private string GetString(Dictionary<string, object> map, string name, string defaultValue)
		{
			if (!map.ContainsKey(name))
			{
				return defaultValue;
			}
			return (string)map[name];
		}

		// Token: 0x060010FA RID: 4346 RVA: 0x0002CB85 File Offset: 0x0002AF85
		private float ToColor(string hexString, int colorIndex)
		{
			if (hexString.Length != 8)
			{
				throw new ArgumentException("Color hexidecimal length must be 8, recieved: " + hexString);
			}
			return (float)Convert.ToInt32(hexString.Substring(colorIndex * 2, 2), 16) / 255f;
		}

		// Token: 0x060010FB RID: 4347 RVA: 0x0002CBBC File Offset: 0x0002AFBC
		private void ReadAnimation(string name, Dictionary<string, object> map, SkeletonData skeletonData)
		{
			ExposedList<Timeline> exposedList = new ExposedList<Timeline>();
			float num = 0f;
			float scale = this.Scale;
			if (map.ContainsKey("slots"))
			{
				foreach (KeyValuePair<string, object> keyValuePair in ((Dictionary<string, object>)map["slots"]))
				{
					string key = keyValuePair.Key;
					int slotIndex = skeletonData.FindSlotIndex(key);
					Dictionary<string, object> dictionary = (Dictionary<string, object>)keyValuePair.Value;
					foreach (KeyValuePair<string, object> keyValuePair2 in dictionary)
					{
						List<object> list = (List<object>)keyValuePair2.Value;
						string key2 = keyValuePair2.Key;
						if (key2 == "color")
						{
							ColorTimeline colorTimeline = new ColorTimeline(list.Count);
							colorTimeline.slotIndex = slotIndex;
							int num2 = 0;
							foreach (object obj in list)
							{
								Dictionary<string, object> dictionary2 = (Dictionary<string, object>)obj;
								float time = (float)dictionary2["time"];
								string hexString = (string)dictionary2["color"];
								colorTimeline.SetFrame(num2, time, this.ToColor(hexString, 0), this.ToColor(hexString, 1), this.ToColor(hexString, 2), this.ToColor(hexString, 3));
								this.ReadCurve(colorTimeline, num2, dictionary2);
								num2++;
							}
							exposedList.Add(colorTimeline);
							num = Math.Max(num, colorTimeline.frames[colorTimeline.FrameCount * 5 - 5]);
						}
						else
						{
							if (!(key2 == "attachment"))
							{
								throw new Exception(string.Concat(new string[]
								{
									"Invalid timeline type for a slot: ",
									key2,
									" (",
									key,
									")"
								}));
							}
							AttachmentTimeline attachmentTimeline = new AttachmentTimeline(list.Count);
							attachmentTimeline.slotIndex = slotIndex;
							int num3 = 0;
							foreach (object obj2 in list)
							{
								Dictionary<string, object> dictionary3 = (Dictionary<string, object>)obj2;
								float time2 = (float)dictionary3["time"];
								attachmentTimeline.SetFrame(num3++, time2, (string)dictionary3["name"]);
							}
							exposedList.Add(attachmentTimeline);
							num = Math.Max(num, attachmentTimeline.frames[attachmentTimeline.FrameCount - 1]);
						}
					}
				}
			}
			if (map.ContainsKey("bones"))
			{
				foreach (KeyValuePair<string, object> keyValuePair3 in ((Dictionary<string, object>)map["bones"]))
				{
					string key3 = keyValuePair3.Key;
					int num4 = skeletonData.FindBoneIndex(key3);
					if (num4 == -1)
					{
						throw new Exception("Bone not found: " + key3);
					}
					Dictionary<string, object> dictionary4 = (Dictionary<string, object>)keyValuePair3.Value;
					foreach (KeyValuePair<string, object> keyValuePair4 in dictionary4)
					{
						List<object> list2 = (List<object>)keyValuePair4.Value;
						string key4 = keyValuePair4.Key;
						if (key4 == "rotate")
						{
							RotateTimeline rotateTimeline = new RotateTimeline(list2.Count);
							rotateTimeline.boneIndex = num4;
							int num5 = 0;
							foreach (object obj3 in list2)
							{
								Dictionary<string, object> dictionary5 = (Dictionary<string, object>)obj3;
								float time3 = (float)dictionary5["time"];
								rotateTimeline.SetFrame(num5, time3, (float)dictionary5["angle"]);
								this.ReadCurve(rotateTimeline, num5, dictionary5);
								num5++;
							}
							exposedList.Add(rotateTimeline);
							num = Math.Max(num, rotateTimeline.frames[rotateTimeline.FrameCount * 2 - 2]);
						}
						else
						{
							if (!(key4 == "translate") && !(key4 == "scale") && !(key4 == "shear"))
							{
								throw new Exception(string.Concat(new string[]
								{
									"Invalid timeline type for a bone: ",
									key4,
									" (",
									key3,
									")"
								}));
							}
							float num6 = 1f;
							TranslateTimeline translateTimeline;
							if (key4 == "scale")
							{
								translateTimeline = new ScaleTimeline(list2.Count);
							}
							else if (key4 == "shear")
							{
								translateTimeline = new ShearTimeline(list2.Count);
							}
							else
							{
								translateTimeline = new TranslateTimeline(list2.Count);
								num6 = scale;
							}
							translateTimeline.boneIndex = num4;
							int num7 = 0;
							foreach (object obj4 in list2)
							{
								Dictionary<string, object> dictionary6 = (Dictionary<string, object>)obj4;
								float time4 = (float)dictionary6["time"];
								float @float = this.GetFloat(dictionary6, "x", 0f);
								float float2 = this.GetFloat(dictionary6, "y", 0f);
								translateTimeline.SetFrame(num7, time4, @float * num6, float2 * num6);
								this.ReadCurve(translateTimeline, num7, dictionary6);
								num7++;
							}
							exposedList.Add(translateTimeline);
							num = Math.Max(num, translateTimeline.frames[translateTimeline.FrameCount * 3 - 3]);
						}
					}
				}
			}
			if (map.ContainsKey("ik"))
			{
				foreach (KeyValuePair<string, object> keyValuePair5 in ((Dictionary<string, object>)map["ik"]))
				{
					IkConstraintData item = skeletonData.FindIkConstraint(keyValuePair5.Key);
					List<object> list3 = (List<object>)keyValuePair5.Value;
					IkConstraintTimeline ikConstraintTimeline = new IkConstraintTimeline(list3.Count);
					ikConstraintTimeline.ikConstraintIndex = skeletonData.ikConstraints.IndexOf(item);
					int num8 = 0;
					foreach (object obj5 in list3)
					{
						Dictionary<string, object> dictionary7 = (Dictionary<string, object>)obj5;
						float time5 = (float)dictionary7["time"];
						float float3 = this.GetFloat(dictionary7, "mix", 1f);
						bool boolean = this.GetBoolean(dictionary7, "bendPositive", true);
						ikConstraintTimeline.SetFrame(num8, time5, float3, (!boolean) ? -1 : 1);
						this.ReadCurve(ikConstraintTimeline, num8, dictionary7);
						num8++;
					}
					exposedList.Add(ikConstraintTimeline);
					num = Math.Max(num, ikConstraintTimeline.frames[ikConstraintTimeline.FrameCount * 3 - 3]);
				}
			}
			if (map.ContainsKey("transform"))
			{
				foreach (KeyValuePair<string, object> keyValuePair6 in ((Dictionary<string, object>)map["transform"]))
				{
					TransformConstraintData item2 = skeletonData.FindTransformConstraint(keyValuePair6.Key);
					List<object> list4 = (List<object>)keyValuePair6.Value;
					TransformConstraintTimeline transformConstraintTimeline = new TransformConstraintTimeline(list4.Count);
					transformConstraintTimeline.transformConstraintIndex = skeletonData.transformConstraints.IndexOf(item2);
					int num9 = 0;
					foreach (object obj6 in list4)
					{
						Dictionary<string, object> dictionary8 = (Dictionary<string, object>)obj6;
						float time6 = (float)dictionary8["time"];
						float float4 = this.GetFloat(dictionary8, "rotateMix", 1f);
						float float5 = this.GetFloat(dictionary8, "translateMix", 1f);
						float float6 = this.GetFloat(dictionary8, "scaleMix", 1f);
						float float7 = this.GetFloat(dictionary8, "shearMix", 1f);
						transformConstraintTimeline.SetFrame(num9, time6, float4, float5, float6, float7);
						this.ReadCurve(transformConstraintTimeline, num9, dictionary8);
						num9++;
					}
					exposedList.Add(transformConstraintTimeline);
					num = Math.Max(num, transformConstraintTimeline.frames[transformConstraintTimeline.FrameCount * 5 - 5]);
				}
			}
			if (map.ContainsKey("ffd"))
			{
				foreach (KeyValuePair<string, object> keyValuePair7 in ((Dictionary<string, object>)map["ffd"]))
				{
					Skin skin = skeletonData.FindSkin(keyValuePair7.Key);
					foreach (KeyValuePair<string, object> keyValuePair8 in ((Dictionary<string, object>)keyValuePair7.Value))
					{
						int slotIndex2 = skeletonData.FindSlotIndex(keyValuePair8.Key);
						foreach (KeyValuePair<string, object> keyValuePair9 in ((Dictionary<string, object>)keyValuePair8.Value))
						{
							List<object> list5 = (List<object>)keyValuePair9.Value;
							FfdTimeline ffdTimeline = new FfdTimeline(list5.Count);
							Attachment attachment = skin.GetAttachment(slotIndex2, keyValuePair9.Key);
							if (attachment == null)
							{
								throw new Exception("FFD attachment not found: " + keyValuePair9.Key);
							}
							ffdTimeline.slotIndex = slotIndex2;
							ffdTimeline.attachment = attachment;
							int num10;
							if (attachment is MeshAttachment)
							{
								num10 = ((MeshAttachment)attachment).vertices.Length;
							}
							else
							{
								num10 = ((WeightedMeshAttachment)attachment).Weights.Length / 3 * 2;
							}
							int num11 = 0;
							foreach (object obj7 in list5)
							{
								Dictionary<string, object> dictionary9 = (Dictionary<string, object>)obj7;
								float[] array;
								if (!dictionary9.ContainsKey("vertices"))
								{
									if (attachment is MeshAttachment)
									{
										array = ((MeshAttachment)attachment).vertices;
									}
									else
									{
										array = new float[num10];
									}
								}
								else
								{
									List<object> list6 = (List<object>)dictionary9["vertices"];
									array = new float[num10];
									int @int = this.GetInt(dictionary9, "offset", 0);
									if (scale == 1f)
									{
										int i = 0;
										int count = list6.Count;
										while (i < count)
										{
											array[i + @int] = (float)list6[i];
											i++;
										}
									}
									else
									{
										int j = 0;
										int count2 = list6.Count;
										while (j < count2)
										{
											array[j + @int] = (float)list6[j] * scale;
											j++;
										}
									}
									if (attachment is MeshAttachment)
									{
										float[] vertices = ((MeshAttachment)attachment).vertices;
										for (int k = 0; k < num10; k++)
										{
											array[k] += vertices[k];
										}
									}
								}
								ffdTimeline.SetFrame(num11, (float)dictionary9["time"], array);
								this.ReadCurve(ffdTimeline, num11, dictionary9);
								num11++;
							}
							exposedList.Add(ffdTimeline);
							num = Math.Max(num, ffdTimeline.frames[ffdTimeline.FrameCount - 1]);
						}
					}
				}
			}
			if (map.ContainsKey("drawOrder") || map.ContainsKey("draworder"))
			{
				List<object> list7 = (List<object>)map[(!map.ContainsKey("drawOrder")) ? "draworder" : "drawOrder"];
				DrawOrderTimeline drawOrderTimeline = new DrawOrderTimeline(list7.Count);
				int count3 = skeletonData.slots.Count;
				int num12 = 0;
				foreach (object obj8 in list7)
				{
					Dictionary<string, object> dictionary10 = (Dictionary<string, object>)obj8;
					int[] array2 = null;
					if (dictionary10.ContainsKey("offsets"))
					{
						array2 = new int[count3];
						for (int l = count3 - 1; l >= 0; l--)
						{
							array2[l] = -1;
						}
						List<object> list8 = (List<object>)dictionary10["offsets"];
						int[] array3 = new int[count3 - list8.Count];
						int m = 0;
						int num13 = 0;
						foreach (object obj9 in list8)
						{
							Dictionary<string, object> dictionary11 = (Dictionary<string, object>)obj9;
							int num14 = skeletonData.FindSlotIndex((string)dictionary11["slot"]);
							if (num14 == -1)
							{
								throw new Exception("Slot not found: " + dictionary11["slot"]);
							}
							while (m != num14)
							{
								array3[num13++] = m++;
							}
							int num15 = m + (int)((float)dictionary11["offset"]);
							array2[num15] = m++;
						}
						while (m < count3)
						{
							array3[num13++] = m++;
						}
						for (int n = count3 - 1; n >= 0; n--)
						{
							if (array2[n] == -1)
							{
								array2[n] = array3[--num13];
							}
						}
					}
					drawOrderTimeline.SetFrame(num12++, (float)dictionary10["time"], array2);
				}
				exposedList.Add(drawOrderTimeline);
				num = Math.Max(num, drawOrderTimeline.frames[drawOrderTimeline.FrameCount - 1]);
			}
			if (map.ContainsKey("events"))
			{
				List<object> list9 = (List<object>)map["events"];
				EventTimeline eventTimeline = new EventTimeline(list9.Count);
				int num16 = 0;
				foreach (object obj10 in list9)
				{
					Dictionary<string, object> dictionary12 = (Dictionary<string, object>)obj10;
					EventData eventData = skeletonData.FindEvent((string)dictionary12["name"]);
					if (eventData == null)
					{
						throw new Exception("Event not found: " + dictionary12["name"]);
					}
					Event @event = new Event((float)dictionary12["time"], eventData);
					@event.Int = this.GetInt(dictionary12, "int", eventData.Int);
					@event.Float = this.GetFloat(dictionary12, "float", eventData.Float);
					@event.String = this.GetString(dictionary12, "string", eventData.String);
					eventTimeline.SetFrame(num16++, @event);
				}
				exposedList.Add(eventTimeline);
				num = Math.Max(num, eventTimeline.frames[eventTimeline.FrameCount - 1]);
			}
			exposedList.TrimExcess();
			skeletonData.animations.Add(new Animation(name, exposedList, num));
		}

		// Token: 0x060010FC RID: 4348 RVA: 0x0002DD58 File Offset: 0x0002C158
		private void ReadCurve(CurveTimeline timeline, int frameIndex, Dictionary<string, object> valueMap)
		{
			if (!valueMap.ContainsKey("curve"))
			{
				return;
			}
			object obj = valueMap["curve"];
			if (obj.Equals("stepped"))
			{
				timeline.SetStepped(frameIndex);
			}
			else if (obj is List<object>)
			{
				List<object> list = (List<object>)obj;
				timeline.SetCurve(frameIndex, (float)list[0], (float)list[1], (float)list[2], (float)list[3]);
			}
		}

		// Token: 0x04004174 RID: 16756
		private AttachmentLoader attachmentLoader;

		// Token: 0x04004175 RID: 16757
		private List<SkeletonJson.LinkedMesh> linkedMeshes = new List<SkeletonJson.LinkedMesh>();

		// Token: 0x0200021F RID: 543
		internal class LinkedMesh
		{
			// Token: 0x060010FD RID: 4349 RVA: 0x0002DDE7 File Offset: 0x0002C1E7
			public LinkedMesh(Attachment mesh, string skin, int slotIndex, string parent)
			{
				this.mesh = mesh;
				this.skin = skin;
				this.slotIndex = slotIndex;
				this.parent = parent;
			}

			// Token: 0x04004176 RID: 16758
			internal string parent;

			// Token: 0x04004177 RID: 16759
			internal string skin;

			// Token: 0x04004178 RID: 16760
			internal int slotIndex;

			// Token: 0x04004179 RID: 16761
			internal Attachment mesh;
		}
	}
}

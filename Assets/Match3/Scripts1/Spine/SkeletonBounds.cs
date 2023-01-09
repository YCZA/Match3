using System;

namespace Match3.Scripts1.Spine
{
	// Token: 0x0200021B RID: 539
	public class SkeletonBounds
	{
		// Token: 0x060010AE RID: 4270 RVA: 0x0002AB3A File Offset: 0x00028F3A
		public SkeletonBounds()
		{
			this.BoundingBoxes = new ExposedList<BoundingBoxAttachment>();
			this.Polygons = new ExposedList<Polygon>();
		}

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x060010AF RID: 4271 RVA: 0x0002AB63 File Offset: 0x00028F63
		// (set) Token: 0x060010B0 RID: 4272 RVA: 0x0002AB6B File Offset: 0x00028F6B
		public ExposedList<BoundingBoxAttachment> BoundingBoxes { get; private set; }

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x060010B1 RID: 4273 RVA: 0x0002AB74 File Offset: 0x00028F74
		// (set) Token: 0x060010B2 RID: 4274 RVA: 0x0002AB7C File Offset: 0x00028F7C
		public ExposedList<Polygon> Polygons { get; private set; }

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x060010B3 RID: 4275 RVA: 0x0002AB85 File Offset: 0x00028F85
		// (set) Token: 0x060010B4 RID: 4276 RVA: 0x0002AB8D File Offset: 0x00028F8D
		public float MinX
		{
			get
			{
				return this.minX;
			}
			set
			{
				this.minX = value;
			}
		}

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x060010B5 RID: 4277 RVA: 0x0002AB96 File Offset: 0x00028F96
		// (set) Token: 0x060010B6 RID: 4278 RVA: 0x0002AB9E File Offset: 0x00028F9E
		public float MinY
		{
			get
			{
				return this.minY;
			}
			set
			{
				this.minY = value;
			}
		}

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x060010B7 RID: 4279 RVA: 0x0002ABA7 File Offset: 0x00028FA7
		// (set) Token: 0x060010B8 RID: 4280 RVA: 0x0002ABAF File Offset: 0x00028FAF
		public float MaxX
		{
			get
			{
				return this.maxX;
			}
			set
			{
				this.maxX = value;
			}
		}

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x060010B9 RID: 4281 RVA: 0x0002ABB8 File Offset: 0x00028FB8
		// (set) Token: 0x060010BA RID: 4282 RVA: 0x0002ABC0 File Offset: 0x00028FC0
		public float MaxY
		{
			get
			{
				return this.maxY;
			}
			set
			{
				this.maxY = value;
			}
		}

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x060010BB RID: 4283 RVA: 0x0002ABC9 File Offset: 0x00028FC9
		public float Width
		{
			get
			{
				return this.maxX - this.minX;
			}
		}

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x060010BC RID: 4284 RVA: 0x0002ABD8 File Offset: 0x00028FD8
		public float Height
		{
			get
			{
				return this.maxY - this.minY;
			}
		}

		// Token: 0x060010BD RID: 4285 RVA: 0x0002ABE8 File Offset: 0x00028FE8
		public void Update(Skeleton skeleton, bool updateAabb)
		{
			ExposedList<BoundingBoxAttachment> boundingBoxes = this.BoundingBoxes;
			ExposedList<Polygon> polygons = this.Polygons;
			ExposedList<Slot> slots = skeleton.slots;
			int count = slots.Count;
			boundingBoxes.Clear(true);
			int i = 0;
			int count2 = polygons.Count;
			while (i < count2)
			{
				this.polygonPool.Add(polygons.Items[i]);
				i++;
			}
			polygons.Clear(true);
			for (int j = 0; j < count; j++)
			{
				Slot slot = slots.Items[j];
				BoundingBoxAttachment boundingBoxAttachment = slot.attachment as BoundingBoxAttachment;
				if (boundingBoxAttachment != null)
				{
					boundingBoxes.Add(boundingBoxAttachment);
					int count3 = this.polygonPool.Count;
					Polygon polygon;
					if (count3 > 0)
					{
						polygon = this.polygonPool.Items[count3 - 1];
						this.polygonPool.RemoveAt(count3 - 1);
					}
					else
					{
						polygon = new Polygon();
					}
					polygons.Add(polygon);
					int num = boundingBoxAttachment.Vertices.Length;
					polygon.Count = num;
					if (polygon.Vertices.Length < num)
					{
						polygon.Vertices = new float[num];
					}
					boundingBoxAttachment.ComputeWorldVertices(slot.bone, polygon.Vertices);
				}
			}
			if (updateAabb)
			{
				this.aabbCompute();
			}
		}

		// Token: 0x060010BE RID: 4286 RVA: 0x0002AD38 File Offset: 0x00029138
		private void aabbCompute()
		{
			float val = 2.1474836E+09f;
			float val2 = 2.1474836E+09f;
			float val3 = -2.1474836E+09f;
			float val4 = -2.1474836E+09f;
			ExposedList<Polygon> polygons = this.Polygons;
			int i = 0;
			int count = polygons.Count;
			while (i < count)
			{
				Polygon polygon = polygons.Items[i];
				float[] vertices = polygon.Vertices;
				int j = 0;
				int count2 = polygon.Count;
				while (j < count2)
				{
					float val5 = vertices[j];
					float val6 = vertices[j + 1];
					val = Math.Min(val, val5);
					val2 = Math.Min(val2, val6);
					val3 = Math.Max(val3, val5);
					val4 = Math.Max(val4, val6);
					j += 2;
				}
				i++;
			}
			this.minX = val;
			this.minY = val2;
			this.maxX = val3;
			this.maxY = val4;
		}

		// Token: 0x060010BF RID: 4287 RVA: 0x0002AE0A File Offset: 0x0002920A
		public bool AabbContainsPoint(float x, float y)
		{
			return x >= this.minX && x <= this.maxX && y >= this.minY && y <= this.maxY;
		}

		// Token: 0x060010C0 RID: 4288 RVA: 0x0002AE40 File Offset: 0x00029240
		public bool AabbIntersectsSegment(float x1, float y1, float x2, float y2)
		{
			float num = this.minX;
			float num2 = this.minY;
			float num3 = this.maxX;
			float num4 = this.maxY;
			if ((x1 <= num && x2 <= num) || (y1 <= num2 && y2 <= num2) || (x1 >= num3 && x2 >= num3) || (y1 >= num4 && y2 >= num4))
			{
				return false;
			}
			float num5 = (y2 - y1) / (x2 - x1);
			float num6 = num5 * (num - x1) + y1;
			if (num6 > num2 && num6 < num4)
			{
				return true;
			}
			num6 = num5 * (num3 - x1) + y1;
			if (num6 > num2 && num6 < num4)
			{
				return true;
			}
			float num7 = (num2 - y1) / num5 + x1;
			if (num7 > num && num7 < num3)
			{
				return true;
			}
			num7 = (num4 - y1) / num5 + x1;
			return num7 > num && num7 < num3;
		}

		// Token: 0x060010C1 RID: 4289 RVA: 0x0002AF20 File Offset: 0x00029320
		public bool AabbIntersectsSkeleton(SkeletonBounds bounds)
		{
			return this.minX < bounds.maxX && this.maxX > bounds.minX && this.minY < bounds.maxY && this.maxY > bounds.minY;
		}

		// Token: 0x060010C2 RID: 4290 RVA: 0x0002AF74 File Offset: 0x00029374
		public bool ContainsPoint(Polygon polygon, float x, float y)
		{
			float[] vertices = polygon.Vertices;
			int count = polygon.Count;
			int num = count - 2;
			bool flag = false;
			for (int i = 0; i < count; i += 2)
			{
				float num2 = vertices[i + 1];
				float num3 = vertices[num + 1];
				if ((num2 < y && num3 >= y) || (num3 < y && num2 >= y))
				{
					float num4 = vertices[i];
					if (num4 + (y - num2) / (num3 - num2) * (vertices[num] - num4) < x)
					{
						flag = !flag;
					}
				}
				num = i;
			}
			return flag;
		}

		// Token: 0x060010C3 RID: 4291 RVA: 0x0002B004 File Offset: 0x00029404
		public BoundingBoxAttachment ContainsPoint(float x, float y)
		{
			ExposedList<Polygon> polygons = this.Polygons;
			int i = 0;
			int count = polygons.Count;
			while (i < count)
			{
				if (this.ContainsPoint(polygons.Items[i], x, y))
				{
					return this.BoundingBoxes.Items[i];
				}
				i++;
			}
			return null;
		}

		// Token: 0x060010C4 RID: 4292 RVA: 0x0002B058 File Offset: 0x00029458
		public BoundingBoxAttachment IntersectsSegment(float x1, float y1, float x2, float y2)
		{
			ExposedList<Polygon> polygons = this.Polygons;
			int i = 0;
			int count = polygons.Count;
			while (i < count)
			{
				if (this.IntersectsSegment(polygons.Items[i], x1, y1, x2, y2))
				{
					return this.BoundingBoxes.Items[i];
				}
				i++;
			}
			return null;
		}

		// Token: 0x060010C5 RID: 4293 RVA: 0x0002B0AC File Offset: 0x000294AC
		public bool IntersectsSegment(Polygon polygon, float x1, float y1, float x2, float y2)
		{
			float[] vertices = polygon.Vertices;
			int count = polygon.Count;
			float num = x1 - x2;
			float num2 = y1 - y2;
			float num3 = x1 * y2 - y1 * x2;
			float num4 = vertices[count - 2];
			float num5 = vertices[count - 1];
			for (int i = 0; i < count; i += 2)
			{
				float num6 = vertices[i];
				float num7 = vertices[i + 1];
				float num8 = num4 * num7 - num5 * num6;
				float num9 = num4 - num6;
				float num10 = num5 - num7;
				float num11 = num * num10 - num2 * num9;
				float num12 = (num3 * num9 - num * num8) / num11;
				if (((num12 >= num4 && num12 <= num6) || (num12 >= num6 && num12 <= num4)) && ((num12 >= x1 && num12 <= x2) || (num12 >= x2 && num12 <= x1)))
				{
					float num13 = (num3 * num10 - num2 * num8) / num11;
					if (((num13 >= num5 && num13 <= num7) || (num13 >= num7 && num13 <= num5)) && ((num13 >= y1 && num13 <= y2) || (num13 >= y2 && num13 <= y1)))
					{
						return true;
					}
				}
				num4 = num6;
				num5 = num7;
			}
			return false;
		}

		// Token: 0x060010C6 RID: 4294 RVA: 0x0002B1EC File Offset: 0x000295EC
		public Polygon getPolygon(BoundingBoxAttachment attachment)
		{
			int num = this.BoundingBoxes.IndexOf(attachment);
			return (num != -1) ? this.Polygons.Items[num] : null;
		}

		// Token: 0x0400415C RID: 16732
		private ExposedList<Polygon> polygonPool = new ExposedList<Polygon>();

		// Token: 0x0400415D RID: 16733
		private float minX;

		// Token: 0x0400415E RID: 16734
		private float minY;

		// Token: 0x0400415F RID: 16735
		private float maxX;

		// Token: 0x04004160 RID: 16736
		private float maxY;
	}
}

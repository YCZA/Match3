using System;
using System.Collections.Generic;
using System.Linq;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.Town;
using Match3.Scripts1.Shared.DataStructures;
using Match3.Scripts1.Town;
using Match3.Scripts1.Wooga.Signals;
using UnityEngine;

// Token: 0x02000955 RID: 2389
namespace Match3.Scripts1
{
	[ExecuteInEditMode]
	public class TownPathfinding : MonoBehaviour, IPathfinder
	{
		public BuildingsController Buildings { get; private set; }

		// Token: 0x170008E3 RID: 2275
		// (get) Token: 0x06003A05 RID: 14853 RVA: 0x0011E2D4 File Offset: 0x0011C6D4
		public int UnlockedLocalArea
		{
			get
			{
				int unlockedAreaWithQuestAndEndOfContent = this.questsService.UnlockedAreaWithQuestAndEndOfContent;
				return this.configService.SbsConfig.islandareaconfig.ClampGlobalToLocalArea(this.islandId, unlockedAreaWithQuestAndEndOfContent);
			}
		}

		// Token: 0x06003A06 RID: 14854 RVA: 0x0011E30C File Offset: 0x0011C70C
		public AreaInfo GetAreaInfo(int area)
		{
			return (!this.IsAreaValid(area)) ? default(AreaInfo) : this.areaInfos[area - 1];
		}

		// Token: 0x170008E4 RID: 2276
		// (get) Token: 0x06003A07 RID: 14855 RVA: 0x0011E346 File Offset: 0x0011C746
		public int NumAreas
		{
			get
			{
				return this.areaInfos.Length;
			}
		}

		// Token: 0x06003A08 RID: 14856 RVA: 0x0011E350 File Offset: 0x0011C750
		public bool IsAreaValid(int area)
		{
			int num = area - 1;
			return num >= 0 && num < this.areaInfos.Length && this.areaInfos[num].tiles.Count > 0;
		}

		// Token: 0x06003A09 RID: 14857 RVA: 0x0011E38F File Offset: 0x0011C78F
		public IEnumerable<IntVector2> GetAreaTiles(int area)
		{
			return (!this.IsAreaValid(area)) ? null : this.areaInfos[area - 1].tiles;
		}

		// Token: 0x06003A0A RID: 14858 RVA: 0x0011E3B8 File Offset: 0x0011C7B8
		public bool FindEmptySpotInActiveAreas(out Vector2 position)
		{
			List<IntVector2> list = new List<IntVector2>();
			Map<GridValue> map = this.Grid(true);
			int unlockedLocalArea = this.UnlockedLocalArea;
			for (int i = 0; i < unlockedLocalArea; i++)
			{
				list.AddRange(this.areaInfos[i].tiles);
			}
			for (int j = 0; j < 10; j++)
			{
				IntVector2 coord = list.RandomElement(false);
				if (map.__getTileUnsafe(coord.x, coord.y) != GridValue.Blocked)
				{
					position = TownPathfinding.ConvertMapToWorldSpace(coord);
					return true;
				}
			}
			position = Vector2.zero;
			return false;
		}

		// Token: 0x06003A0B RID: 14859 RVA: 0x0011E45C File Offset: 0x0011C85C
		public bool FindEmptySpotAroundPoint(Vector2 input, float minRadius, float maxRadius, out Vector2 position)
		{
			Map<GridValue> map = this.Grid(true);
			for (int i = 0; i < 10; i++)
			{
				float f = global::UnityEngine.Random.Range(-3.1415927f, 3.1415927f);
				float d = global::UnityEngine.Random.Range(minRadius, maxRadius);
				Vector2 vector = new Vector2(Mathf.Cos(f), Mathf.Sin(f)) * d + input;
				IntVector2 vec = TownPathfinding.ConvertWorldSpaceToMap(vector);
				if (map[vec] != GridValue.Blocked)
				{
					position = vector;
					return true;
				}
			}
			position = Vector2.zero;
			return false;
		}

		public void Init(QuestService questsService, ConfigService configService, ProgressionDataService.Service progressionService, BuildingsController buildings, TownUiRoot townUi, TownOverheadUiRoot townOverheadUi)
		{
			this.questsService = questsService;
			this.configService = configService;
			this.progressionService = progressionService;
			this.Buildings = buildings;
			this.townUi = townUi;
			this.townOverheadUi = townOverheadUi;
			this.LoadMap();
		}

		private void LoadMap()
		{
			this.mapData.Fill(this.mapConfigBinary.bytes);
			Shader.SetGlobalTexture("_MapTexture", this.mapTexture);
			Shader.SetGlobalFloat("_MapSize", 128f);
		}
	
		public Texture2D BlurMapTexture(Texture2D source, int area)
		{
			this.blurMaterial.SetInt("_Area", area);
			RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height, 0);
			// RenderTexture temporary2 = RenderTexture.GetTemporary(source.width, source.height, 0);
			// RenderTexture temporary3 = RenderTexture.GetTemporary(source.width, source.height, 0);
			Graphics.Blit(source, temporary, this.blurMaterial, 0);
			// Graphics.Blit(temporary, temporary, this.blurMaterial, 0);
			// Graphics.Blit(temporary, temporary2, this.blurMaterial, 1);
			// Graphics.Blit(temporary, temporary2, this.blurMaterial, 0);
			// Graphics.Blit(temporary2, temporary3, this.blurMaterial, 2);
			// Graphics.Blit(temporary2, temporary3, this.blurMaterial, 0);
			// RenderTexture.active = temporary3;
			// RenderTexture.active = temporary2;
			Texture2D texture2D = new Texture2D(source.width, source.height)
			{
				name = string.Format("Cloud {0}", area),
				wrapMode = TextureWrapMode.Clamp,
				filterMode = FilterMode.Bilinear
			};
			texture2D.ReadPixels(new Rect(0f, 0f, (float)source.width, (float)source.height), 0, 0);
			texture2D.Apply();
			temporary.Release();	// 后来加的
			RenderTexture.ReleaseTemporary(temporary);
			// temporary2.Release();
			// RenderTexture.ReleaseTemporary(temporary2);
			// RenderTexture.ReleaseTemporary(temporary3);
			return texture2D;
		}

		// Token: 0x06003A0F RID: 14863 RVA: 0x0011E660 File Offset: 0x0011CA60
		public bool IsBuildingAreaFree(IntRect buildingArea)
		{
			for (int i = buildingArea.min.x; i < buildingArea.max.x; i++)
			{
				for (int j = buildingArea.min.y; j < buildingArea.max.y; j++)
				{
					IntVector2 position = new IntVector2(i, j);
					if (!this.IsPositionUnlocked(position) || this.HasRubble(position))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06003A10 RID: 14864 RVA: 0x0011E6E4 File Offset: 0x0011CAE4
		public bool HasRubble(IntVector2 position)
		{
			// eli key point 审核版没有碎石
#if true
			return false;
#endif
			if (this.mapData == null || this.progressionService == null || !this.IsValidPosition(position))
			{
				return false;
			}
			int localArea = this.GetLocalArea(position);
			int num = this.configService.SbsConfig.islandareaconfig.LocalAreaToGlobalArea(localArea, this.islandId);
			bool flag = this.configService.SbsConfig.islandareaconfig.CouldHaveRubble(num);
			return flag && num > this.progressionService.LastRubbleAreaCleared;
		}

		// Token: 0x06003A11 RID: 14865 RVA: 0x0011E768 File Offset: 0x0011CB68
		public bool IsPositionUnlocked(IntVector2 position)
		{
			return this.mapData != null && this.questsService != null && this.IsValidPosition(position) && this.IsPlacementAreaUnlocked(this.GetLocalArea(position));
		}

		// Token: 0x06003A12 RID: 14866 RVA: 0x0011E79B File Offset: 0x0011CB9B
		private bool IsPlacementAreaUnlocked(int buildingArea)
		{
			return buildingArea <= this.UnlockedLocalArea;
		}

		// Token: 0x06003A13 RID: 14867 RVA: 0x0011E7A9 File Offset: 0x0011CBA9
		private bool IsValidPosition(IntVector2 position)
		{
			return this.IsLocalAreaValid(this.GetLocalArea(position));
		}

		// Token: 0x06003A14 RID: 14868 RVA: 0x0011E7B8 File Offset: 0x0011CBB8
		private bool IsLocalAreaValid(int localArea)
		{
			return localArea > 0;
		}

		// Token: 0x06003A15 RID: 14869 RVA: 0x0011E7C0 File Offset: 0x0011CBC0
		private Map<GridValue> Grid(bool allowOnRubble = true)
		{
			int num = this.UnlockedLocalArea;
			if (!allowOnRubble)
			{
				num = Mathf.Min(num, this.progressionService.LastRubbleAreaCleared);
			}
			this.gridCache.Clear();
			for (int i = 0; i < num; i++)
			{
				this.areaInfos[i].tiles.ForEach(delegate(IntVector2 tile)
				{
					this.gridCache.__setUnsafe(tile.x, tile.y, GridValue.Normal);
				});
			}
			foreach (BuildingInstance buildingInstance in this.Buildings.Buildings)
			{
				if (buildingInstance.sv.ownerArea <= num)
				{
					BuildingConfig blueprint = buildingInstance.blueprint;
					bool flag = (blueprint.type & 16) != 0;
					GridValue value = (!flag) ? GridValue.Blocked : GridValue.Road;
					IntVector2 position = buildingInstance.position;
					if (!blueprint.IsRubble())
					{
						for (int j = 0; j < blueprint.size; j++)
						{
							for (int k = 0; k < blueprint.size; k++)
							{
								this.gridCache[TownPathfinding.ConvertCoordinates(position.x + j, position.y + k)] = value;
							}
						}
					}
				}
			}
			return this.gridCache;
		}

		// Token: 0x06003A16 RID: 14870 RVA: 0x0011E934 File Offset: 0x0011CD34
		private static Vector2 GetTilesFloatCenter(IEnumerable<IntVector2> tiles)
		{
			Vector2 zero = Vector2.zero;
			int num = 0;
			foreach (IntVector2 intVector in tiles)
			{
				zero.x += (float)intVector.x;
				zero.y += (float)intVector.y;
				num++;
			}
			return zero / (float)num;
		}

		// Token: 0x06003A17 RID: 14871 RVA: 0x0011E9C4 File Offset: 0x0011CDC4
		public static IntVector2 GetTilesCenter(IEnumerable<IntVector2> tiles)
		{
			Vector2 tilesFloatCenter = TownPathfinding.GetTilesFloatCenter(tiles);
			return new IntVector2(Mathf.FloorToInt(tilesFloatCenter.x), Mathf.FloorToInt(tilesFloatCenter.y));
		}

		// Token: 0x06003A18 RID: 14872 RVA: 0x0011E9F8 File Offset: 0x0011CDF8
		public Vector3 FindAreaFocusPoint(int areaId)
		{
			Vector2 vector = (!this.IsAreaValid(areaId)) ? Vector2.zero : TownPathfinding.GetTilesFloatCenter(this.areaInfos[areaId - 1].tiles);
			vector = TownPathfinding.ConvertCoordinates(vector.x, vector.y);
			return vector.x0y();
		}

		// Token: 0x06003A19 RID: 14873 RVA: 0x0011EA50 File Offset: 0x0011CE50
		public bool FindBuildLocation(IntVector2 focusPosition, BuildingConfig building, out IntVector2 locationFound)
		{
			float num = float.MaxValue;
			Map<GridValue> grid = this.Grid(false);
			locationFound = this.INVALID_POSITION;
			focusPosition = TownPathfinding.ConvertCoordinates(focusPosition);
			for (int i = 0; i < 128; i++)
			{
				for (int j = 0; j < 128; j++)
				{
					IntVector2 intVector = new IntVector2(i, j);
					if (this.CheckBuildLocation(intVector, building, grid))
					{
						int sqrMagnitude = (focusPosition - intVector).SqrMagnitude;
						if ((float)sqrMagnitude < num)
						{
							num = (float)sqrMagnitude;
							locationFound = new IntVector2(intVector.x, intVector.y);
						}
					}
				}
			}
			bool result = false;
			if (locationFound != this.INVALID_POSITION)
			{
				locationFound = TownPathfinding.ConvertCoordinates(locationFound);
				result = true;
			}
			return result;
		}

		// Token: 0x06003A1A RID: 14874 RVA: 0x0011EB28 File Offset: 0x0011CF28
		protected bool CheckBuildLocation(IntVector2 gridPosition, BuildingConfig config, Map<GridValue> grid)
		{
			bool flag = true;
			int num = 0;
			while (flag && num < config.size)
			{
				int num2 = 0;
				while (flag && num2 < config.size)
				{
					IntVector2 vec = new IntVector2(gridPosition.x + num, gridPosition.y - num2);
					flag = (grid.IsValid(vec) && grid.__getTileUnsafe(vec.x, vec.y) == GridValue.Normal);
					num2++;
				}
				num++;
			}
			return flag;
		}

		// Token: 0x06003A1B RID: 14875 RVA: 0x0011EBB4 File Offset: 0x0011CFB4
		public bool GetPath(Vector2 a, Vector2 b, out Vector2[] path)
		{
			bool result;
			try
			{
				path = this.GetPath(TownPathfinding.ConvertWorldSpaceToMap(a), TownPathfinding.ConvertWorldSpaceToMap(b), this.Grid(true));
				result = true;
			}
			catch (IndexOutOfRangeException)
			{
				path = null;
				result = false;
			}
			return result;
		}

		// Token: 0x06003A1C RID: 14876 RVA: 0x0011EC00 File Offset: 0x0011D000
		private Vector2[] GetPath(IntVector2 a, IntVector2 b, Map<GridValue> grid)
		{
			this.mapSteps.Clear();
			this.mapRoads.Clear();
			this.mapSteps[a] = byte.MaxValue;
			this.bufferSteps[0] = a;
			int num = 0;
			int num2 = 1;
			while (num2 - num < this.bufferSteps.Length && num < num2 && this.mapSteps[b] == 0)
			{
				IntVector2 intVector = this.bufferSteps[num % this.bufferSteps.Length];
				for (int i = IntVector2.Sides.Length - 1; i >= 0; i--)
				{
					IntVector2 intVector2 = intVector + IntVector2.Sides[i];
					GridValue gridValue = grid[intVector2];
					byte b2 = this.mapSteps[intVector];
					b2 -= 1;
					bool flag = gridValue != GridValue.Blocked;
					bool flag2 = this.mapSteps[intVector2] < b2;
					if (flag2 && flag)
					{
						this.mapSteps[intVector2] = b2;
						this.bufferSteps[num2++ % this.bufferSteps.Length] = intVector2;
						if (gridValue == GridValue.Road)
						{
							this.bufferRoads[0] = intVector2;
							this.mapRoads[intVector2] = b2;
							int num3 = 0;
							int num4 = 1;
							while (num4 - num3 < this.bufferRoads.Length && num3 < num4)
							{
								byte b3 = b2;
								b3 -= 1;
								IntVector2 intVector3 = this.bufferRoads[num3 % this.bufferRoads.Length];
								for (int j = 0; j < IntVector2.Sides.Length; j++)
								{
									IntVector2 intVector4 = intVector3 + IntVector2.Sides[j];
									byte b4 = this.mapRoads[intVector3];
									b4 -= 1;
									if (intVector4.x >= 0 && intVector4.y >= 0)
									{
										if (intVector4.x < 128 && intVector4.y < 128)
										{
											if (this.mapRoads[intVector4] < b4 && grid.__getTileUnsafe(intVector4.x, intVector4.y) == GridValue.Road)
											{
												this.mapRoads[intVector4] = b4;
												this.bufferRoads[num4++ % this.bufferRoads.Length] = intVector4;
												this.mapSteps[intVector4] = b3;
												this.bufferSteps[num2++ % this.bufferRoads.Length] = intVector4;
											}
										}
									}
								}
								num3++;
							}
						}
					}
				}
				num++;
			}
			Stack<Vector2> stack = new Stack<Vector2>();
			stack.Push(TownPathfinding.ConvertMapToWorldSpace(b));
			byte b5 = this.mapSteps[b];
			IL_44B:
			while ((b5 += 1) < 255)
			{
				bool flag3 = false;
				for (int k = 0; k < IntVector2.Sides.Length; k++)
				{
					IntVector2 intVector5 = b + IntVector2.Sides[k];
					if (this.mapSteps[intVector5] == b5)
					{
						b = intVector5;
						stack.Push(TownPathfinding.ConvertMapToWorldSpace(b));
						flag3 = true;
						break;
					}
				}
				if (!flag3 && grid[b.x, b.y] == GridValue.Road)
				{
					byte b6 = this.mapRoads[b];
					while ((b6 += 1) < 255)
					{
						for (int l = 0; l < IntVector2.Sides.Length; l++)
						{
							IntVector2 intVector6 = b + IntVector2.Sides[l];
							if (this.mapSteps[intVector6] == b5)
							{
								b = intVector6;
								stack.Push(TownPathfinding.ConvertMapToWorldSpace(b));
								goto IL_44B;
							}
							if (this.mapRoads[intVector6] == b6)
							{
								b = intVector6;
								stack.Push(TownPathfinding.ConvertMapToWorldSpace(b));
								break;
							}
						}
					}
				}
			}
			return stack.ToArray();
		}

		// Token: 0x06003A1D RID: 14877 RVA: 0x0011F071 File Offset: 0x0011D471
		private int GetVertexIndex(int x, int y)
		{
			return x * 129 + y;
		}

		// Token: 0x06003A1E RID: 14878 RVA: 0x0011F07C File Offset: 0x0011D47C
		private void ParseQuad(int posx, int posy, int width, int height, List<int> triangles, Func<int, int, bool> func)
		{
			bool flag = true;
			bool flag2 = false;
			for (int i = posx; i < posx + width; i++)
			{
				for (int j = posy; j < posy + height; j++)
				{
					bool flag3 = func(i, j);
					flag = (flag && flag3);
					flag2 = (flag2 || flag3);
				}
			}
			if (!flag2)
			{
				return;
			}
			if (flag || width <= 1 || height <= 1)
			{
				triangles.Add(this.GetVertexIndex(posx, posy));
				triangles.Add(this.GetVertexIndex(posx + width, posy));
				triangles.Add(this.GetVertexIndex(posx + width, posy + height));
				triangles.Add(this.GetVertexIndex(posx + width, posy + height));
				triangles.Add(this.GetVertexIndex(posx, posy + height));
				triangles.Add(this.GetVertexIndex(posx, posy));
			}
			else
			{
				width /= 2;
				height /= 2;
				this.ParseQuad(posx, posy, width, height, triangles, func);
				this.ParseQuad(posx + width, posy, width, height, triangles, func);
				this.ParseQuad(posx + width, posy + height, width, height, triangles, func);
				this.ParseQuad(posx, posy + height, width, height, triangles, func);
			}
		}

		// Token: 0x06003A1F RID: 14879 RVA: 0x0011F1A8 File Offset: 0x0011D5A8
		public Mesh BuildMeshForArea(int area, Func<int, int, bool> func)
		{
			Vector3[] array = new Vector3[16641];
			Vector2[] array2 = new Vector2[16641];
			List<int> list = new List<int>();
			this.ParseQuad(0, 0, 128, 128, list, func);
			int i = 0;
			int num = 0;
			while (i <= 128)
			{
				int j = 0;
				while (j <= 128)
				{
					IntVector2 intVector = TownPathfinding.ConvertCoordinates(i, j);
					array[num] = new Vector3((float)intVector.x, 0f, (float)intVector.y);
					array2[num] = new Vector2((float)i, (float)j);
					j++;
					num++;
				}
				i++;
			}
			int[] array3 = new HashSet<int>(list).ToArray<int>();
			int[] array4 = new int[array.Length];
			Vector3[] array5 = new Vector3[array3.Length];
			Vector2[] array6 = new Vector2[array3.Length];
			for (int k = 0; k < array3.Length; k++)
			{
				array5[k] = array[array3[k]];
				array6[k] = array2[array3[k]];
				array4[array3[k]] = k;
			}
			for (int l = 0; l < list.Count; l++)
			{
				list[l] = array4[list[l]];
			}
			Mesh mesh = new Mesh
			{
				name = string.Format("Area {0}", area),
				vertices = array5,
				uv = array6,
				triangles = list.ToArray()
			};
			mesh.RecalculateNormals();
			mesh.RecalculateBounds();
			mesh.RecalculateTangents();
			return mesh;
		}

		// Token: 0x06003A20 RID: 14880 RVA: 0x0011F378 File Offset: 0x0011D778
		private void OnDrawGizmos()
		{
			if (Application.isPlaying)
			{
				return;
			}
			for (int i = 0; i < this.areaInfos.Length; i++)
			{
				Gizmos.color = TownPathfinding.ColorFromArea(i);
				Gizmos.DrawMesh(this.areaInfos[i].areaMesh);
			}
		}

		// Token: 0x06003A21 RID: 14881 RVA: 0x0011F3CA File Offset: 0x0011D7CA
		public static Color ColorFromArea(int col)
		{
			return TownPathfinding.editorAreaColors[col % TownPathfinding.editorAreaColors.Length];
		}

		// Token: 0x06003A22 RID: 14882 RVA: 0x0011F3E4 File Offset: 0x0011D7E4
		public static IntVector2 ConvertCoordinates(IntVector2 coord)
		{
			return TownPathfinding.ConvertCoordinates(coord.x, coord.y);
		}

		// Token: 0x06003A23 RID: 14883 RVA: 0x0011F3F9 File Offset: 0x0011D7F9
		public static IntVector2 ConvertCoordinates(int x, int y)
		{
			return new IntVector2(x, 42 - y);
		}

		// Token: 0x06003A24 RID: 14884 RVA: 0x0011F405 File Offset: 0x0011D805
		public static Vector2 ConvertCoordinates(float x, float y)
		{
			return new Vector2(x, 42f - y);
		}

		// Token: 0x06003A25 RID: 14885 RVA: 0x0011F414 File Offset: 0x0011D814
		private static IntVector2 ConvertWorldSpaceToMap(Vector2 position)
		{
			return TownPathfinding.ConvertCoordinates(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y));
		}

		// Token: 0x06003A26 RID: 14886 RVA: 0x0011F434 File Offset: 0x0011D834
		private static Vector2 ConvertMapToWorldSpace(IntVector2 coord)
		{
			IntVector2 intVector = TownPathfinding.ConvertCoordinates(coord);
			return new Vector2((float)intVector.x + 0.5f, (float)intVector.y + 0.5f);
		}

		// Token: 0x06003A27 RID: 14887 RVA: 0x0011F469 File Offset: 0x0011D869
		public int GetLocalArea(IntVector2 coord)
		{
			return (int)this.mapData[TownPathfinding.ConvertCoordinates(coord)];
		}

		// Token: 0x06003A28 RID: 14888 RVA: 0x0011F47C File Offset: 0x0011D87C
		public int GetLocalArea(int x, int y)
		{
			return (int)this.mapData[TownPathfinding.ConvertCoordinates(x, y)];
		}

		// Token: 0x0400621B RID: 25115
		public const int MAP_SIZE = 128;

		// Token: 0x0400621C RID: 25116
		public int islandId;

		// Token: 0x0400621D RID: 25117
		// public TextAsset mapConfig;

		// Token: 0x0400621E RID: 25118
		public TextAsset mapConfigBinary;

		// Token: 0x0400621F RID: 25119
		public Material blurMaterial;

		// Token: 0x04006220 RID: 25120
		public readonly Signal<int> onAreaClick = new Signal<int>();

		// Token: 0x04006221 RID: 25121
		private const int BASE_CONVERSION = 42;

		// Token: 0x04006222 RID: 25122
		private const int MAX_STEPS_BUFFER = 1024;

		// Token: 0x04006223 RID: 25123
		private const int WANDER_LOCATION_TRIES = 10;

		// Token: 0x04006224 RID: 25124
		private ProgressionDataService.Service progressionService;

		// Token: 0x04006225 RID: 25125
		private QuestService questsService;

		// Token: 0x04006226 RID: 25126
		private ConfigService configService;

		// Token: 0x04006227 RID: 25127
		[SerializeField]
		[HideInInspector]
		private Texture2D mapTexture;

		// Token: 0x04006228 RID: 25128
		// [HideInInspector]
		public AreaInfo[] areaInfos;

		// Token: 0x04006229 RID: 25129
		[HideInInspector]
		public TownUiRoot townUi;

		// Token: 0x0400622A RID: 25130
		[HideInInspector]
		public TownOverheadUiRoot townOverheadUi;

		// Token: 0x0400622B RID: 25131
		private readonly Map1d<byte> mapData = new Map1d<byte>(128);

		// Token: 0x0400622C RID: 25132
		private readonly Map<GridValue> gridCache = new Map<GridValue>(128);

		// Token: 0x0400622D RID: 25133
		private readonly IntVector2[] bufferSteps = new IntVector2[1024];

		// Token: 0x0400622E RID: 25134
		private readonly IntVector2[] bufferRoads = new IntVector2[1024];

		// Token: 0x0400622F RID: 25135
		private readonly Map<byte> mapSteps = new Map<byte>(128);

		// Token: 0x04006230 RID: 25136
		private readonly Map<byte> mapRoads = new Map<byte>(128);

		// Token: 0x04006231 RID: 25137
		private readonly IntVector2 INVALID_POSITION = new IntVector2(-999, -999);

		// Token: 0x04006233 RID: 25139
		private static Color[] editorAreaColors = new Color[]
		{
			new Color(0.75f, 0.25f, 0f, 0.6f),
			new Color(0f, 0.25f, 0.75f, 0.6f),
			new Color(1f, 0f, 0f, 0.6f),
			new Color(0f, 0f, 1f, 0.6f),
			new Color(1f, 1f, 0f, 0.6f),
			new Color(0f, 1f, 0.75f, 0.6f),
			new Color(1f, 0f, 1f, 0.6f),
			new Color(0f, 0f, 0f, 0.6f)
		};
	}
}

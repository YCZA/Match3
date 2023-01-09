using System;
using System.Collections;
using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Shared.DataStructures;
using Shared.Pooling;

// Token: 0x020004E4 RID: 1252
namespace Match3.Scripts1
{
	public class FloodFillMatcher
	{
		// Token: 0x060022BF RID: 8895 RVA: 0x00099FD4 File Offset: 0x000983D4
		public List<Group> FindMatches(Fields fields)
		{
			this.fields = fields;
			List<Group> list = ListPool<Group>.Create(10);
			this.visited.Clear();
			IEnumerator enumerator = fields.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Field field = (Field)obj;
					if (field.HasGem && field.gem.IsMatchable)
					{
						Group group = GroupPool.Get(field.gem);
						this.FloodFill(field.gridPosition, group, true);
						if (group.Count >= 3)
						{
							list.Add(group);
						}
						else
						{
							group.ReturnToPool<Group>();
						}
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			return list;
		}

		// Token: 0x060022C0 RID: 8896 RVA: 0x0009A0A0 File Offset: 0x000984A0
		public List<Group> FindMatch(Move move, Fields fields)
		{
			this.fields = fields;
			List<Group> list = ListPool<Group>.Create(10);
			this.visited.Clear();
			list.AddIfNotNull(this.FindMatch(move.from));
			list.AddIfNotNull(this.FindMatch(move.to));
			return list;
		}

		// Token: 0x060022C1 RID: 8897 RVA: 0x0009A0F0 File Offset: 0x000984F0
		private Group FindMatch(IntVector2 pos)
		{
			Field field = this.fields[pos];
			if (!field.gem.IsMatchable)
			{
				return null;
			}
			Group group = GroupPool.Get(field.gem);
			this.FloodFill(group[0].position, group, true);
			if (group.Count >= 3)
			{
				return group;
			}
			group.ReturnToPool<Group>();
			return null;
		}

		// Token: 0x060022C2 RID: 8898 RVA: 0x0009A154 File Offset: 0x00098554
		private void FloodFill(IntVector2 position, Group group, bool isSeed = false)
		{
			GemColor color = group.Color;
			if (!this.fields.IsValid(position))
			{
				return;
			}
			if (this.visited[position] > 0)
			{
				return;
			}
			if (this.fields[position].GemBlocked)
			{
				return;
			}
			if (!this.fields[position].HasGem)
			{
				return;
			}
			if (!this.fields[position].gem.IsMatchable)
			{
				return;
			}
			if (this.fields[position].gem.color != color)
			{
				return;
			}
			Map<int> map;
			(map = this.visited)[position] = map[position] + 1;
			if (!isSeed)
			{
				group.Add(this.fields[position].gem);
			}
			this.FloodFill(position + IntVector2.Left, group, false);
			this.FloodFill(position + IntVector2.Up, group, false);
			this.FloodFill(position + IntVector2.Right, group, false);
			this.FloodFill(position + IntVector2.Down, group, false);
		}

		// Token: 0x04004E6D RID: 20077
		private Fields fields;

		// Token: 0x04004E6E RID: 20078
		private readonly Map<int> visited = new Map<int>(9);
	}
}

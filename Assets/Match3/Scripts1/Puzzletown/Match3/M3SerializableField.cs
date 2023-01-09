using System;
using System.Collections.Generic;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020004FA RID: 1274
	[Serializable]
	public class M3SerializableField
	{
		// Token: 0x06002314 RID: 8980 RVA: 0x0009B400 File Offset: 0x00099800
		public M3SerializableField(Field field)
		{
			this.field = field;
			IntVector2 position = (!field.HasGem) ? IntVector2.Zero : field.gridPosition;
			field.gem.position = position;
			this.modifiers = new List<SerializableFieldModifier>();
			List<IFieldModifier> list = new List<IFieldModifier>();
			if (field.isWindow)
			{
				list.Add(new Window());
			}
			else if (field.isGrowingWindow)
			{
				list.Add(new GrowingWindow());
			}
			if (field.hiddenItemId > 0)
			{
				list.Add(new HiddenItem(field.hiddenItemId));
			}
			if (field.spawnType != SpawnTypes.None)
			{
				list.Add(new Spawn((int)field.spawnType));
			}
			if (field.isDropSpawner)
			{
				list.Add(new DropItem(1));
			}
			else if (field.isDropExit)
			{
				list.Add(new DropItem(2));
			}
			if (field.isClimberSpawner)
			{
				list.Add(new Climber(1));
			}
			else if (field.IsExitClimber)
			{
				list.Add(new Climber(2));
			}
			if (field.portalId > 0)
			{
				list.Add(new Portal(field.portalId));
			}
			if (field.numChains > 0)
			{
				list.Add(new Chain(field.numChains));
			}
			if (field.IsStone)
			{
				list.Add(new Stone(field.blockerIndex));
			}
			if (field.IsResistantBlocker)
			{
				list.Add(new ResistantBlocker(field.blockerIndex));
			}
			if (field.IsColorWheel)
			{
				list.Add(new ColorWheel(field.blockerIndex));
			}
			if (field.HasCrates)
			{
				list.Add(new Crate(field.cratesIndex));
			}
			if (field.numTiles > 0)
			{
				list.Add(new Tile(field.numTiles));
			}
			foreach (IFieldModifier modifier in list)
			{
				this.modifiers.Add(new SerializableFieldModifier(modifier));
			}
		}

		// Token: 0x06002315 RID: 8981 RVA: 0x0009B640 File Offset: 0x00099A40
		public Field Deserialize()
		{
			Field field = this.field;
			foreach (SerializableFieldModifier serializableFieldModifier in this.modifiers)
			{
				if (serializableFieldModifier.type.Equals(typeof(Window).FullName))
				{
					field.isWindow = true;
				}
				else if (serializableFieldModifier.type.Equals(typeof(GrowingWindow).FullName))
				{
					field.isGrowingWindow = true;
				}
				else if (serializableFieldModifier.type.Equals(typeof(Spawn).FullName))
				{
					field.spawnType = (SpawnTypes)serializableFieldModifier.count;
				}
				else if (serializableFieldModifier.type.Equals(typeof(DropItem).FullName))
				{
					field.isDropSpawner = (serializableFieldModifier.count == 1);
					field.isDropExit = (serializableFieldModifier.count == 2);
				}
				else if (serializableFieldModifier.type.Equals(typeof(Climber).FullName))
				{
					field.isClimberSpawner = (serializableFieldModifier.count == 1);
					field.isClimberExit = (serializableFieldModifier.count == 2);
				}
				else if (serializableFieldModifier.type.Equals(typeof(Portal).FullName))
				{
					field.portalId = serializableFieldModifier.count;
				}
				else if (serializableFieldModifier.type.Equals(typeof(Chain).FullName))
				{
					field.numChains = serializableFieldModifier.count;
				}
				else if (serializableFieldModifier.type.Equals(typeof(Crate).FullName))
				{
					field.cratesIndex = serializableFieldModifier.count;
				}
				else if (serializableFieldModifier.type.Equals(typeof(Tile).FullName))
				{
					field.numTiles = serializableFieldModifier.count;
				}
				else if (serializableFieldModifier.type.Equals(typeof(Stone).FullName) || serializableFieldModifier.type.Equals(typeof(ResistantBlocker).FullName) || serializableFieldModifier.type.Equals(typeof(ColorWheel).FullName))
				{
					field.blockerIndex = serializableFieldModifier.count;
				}
				else if (serializableFieldModifier.type.Equals(typeof(HiddenItem).FullName))
				{
					field.hiddenItemId = serializableFieldModifier.count;
				}
			}
			return field;
		}

		// Token: 0x04004ECF RID: 20175
		public Field field;

		// Token: 0x04004ED0 RID: 20176
		public List<SerializableFieldModifier> modifiers;
	}
}

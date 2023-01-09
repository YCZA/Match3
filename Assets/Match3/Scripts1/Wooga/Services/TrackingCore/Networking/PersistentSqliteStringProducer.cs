using System;
using System.Collections;
using System.IO;
using Wooga.Coroutines; // using Wooga.Core.Sqlite;

namespace Match3.Scripts1.Wooga.Services.TrackingCore.Networking
{
	// Token: 0x02000473 RID: 1139
	public class PersistentSqliteStringProducer : IProducerStrategy
	{
		// Token: 0x060020F3 RID: 8435 RVA: 0x0008ACEC File Offset: 0x000890EC
		public PersistentSqliteStringProducer(string path, long maxDataSize = 1024L)
		{
			
			try
			{
				this.OpenAndInitializeDatabase(path, maxDataSize);
			}
			catch
			{
				File.Delete(path);
				this.OpenAndInitializeDatabase(path, maxDataSize);
			}
		}

		// Token: 0x060020F4 RID: 8436 RVA: 0x0008AD3C File Offset: 0x0008913C
		public void Close()
		{
			// this.db.Close();
		}

		// Token: 0x060020F5 RID: 8437 RVA: 0x0008AD4C File Offset: 0x0008914C
		private void OpenAndInitializeDatabase(string path, long maxDataSize)
		{
			// this.db = new SQLiteConnection(path, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex, false);
			// this.db.Query<PersistedData>("create table if not exists \"PersistedData\"(\n\"Id\" integer primary key autoincrement not null ,\n\"Data\" varchar(" + maxDataSize + ") ,\n\"ModificationToken\" varchar(32) ,\n\"IsConsumed\" integer )", new object[0]);
			// this.db.Query<PersistedData>("UPDATE PersistedData SET IsConsumed = 0", new object[0]);
			// this._availableCount = new AtomicCounter(this.db.Query<PersistedDataIdCount>("SELECT COUNT(Id) As IdCount FROM PersistedData", new object[0])[0].IdCount);
		}

		// Token: 0x060020F6 RID: 8438 RVA: 0x0008ADD8 File Offset: 0x000891D8
		public IEnumerator Produce(string input)
		{
			// this.db.Insert(new PersistedData
			// {
				// Data = input,
				// ModificationToken = Guid.NewGuid().ToString(),
				// IsConsumed = false
			// });
			// this._availableCount.Increment();
			yield return null;
		}

		// Token: 0x060020F7 RID: 8439 RVA: 0x0008ADFC File Offset: 0x000891FC
		public IEnumerator Consume(IConsumer consumer)
		{
			string dataToConsume = string.Empty;
			// List<PersistedData> list = this.db.Query<PersistedData>("SELECT * FROM PersistedData WHERE IsConsumed = 0 LIMIT 1", new object[0]);
			// PersistedData data = null;
			// if (list.Count == 1)
			// {
				// data = list[0];
				// string text = Guid.NewGuid().ToString();
				// this.db.Query<PersistedData>("UPDATE PersistedData SET IsConsumed = 1, ModificationToken = ? WHERE IsConsumed = 0 AND Id = ?", new object[]
				// {
					// text,
					// data.Id
				// });
				// List<PersistedData> list2 = this.db.Query<PersistedData>("SELECT * FROM PersistedData WHERE Id = ?", new object[]
				// {
					// data.Id
				// });
				// if (list2.Count != 1 || !(list2[0].ModificationToken == text))
				// {
					// return this.Consume(consumer);
				// }
				// this._availableCount.Decrement();
				// this._consumingCount.Increment();
				// dataToConsume = data.Data;
			// }
			return global::Wooga.Coroutines.Coroutines.Empty().ContinueWith(() => consumer.Consume(dataToConsume).ContinueWith(delegate(bool consumed)
			{
				// this.ResetEntry(data.Id, consumed);
			})).Catch(delegate(Exception exception)
			{
				// this.db.Query<PersistedData>("UPDATE PersistedData SET IsConsumed = 0 WHERE Id = ?", new object[]
				// {
					// data.Id
				// });
				this._availableCount.Increment();
				this._consumingCount.Decrement();
			});
		}

		// Token: 0x060020F8 RID: 8440 RVA: 0x0008AF54 File Offset: 0x00089354
		private void ResetEntry(int dataId, bool consumed)
		{
			if (consumed)
			{
				// this.db.Query<PersistedData>("DELETE FROM PersistedData WHERE Id = ?", new object[]
				// {
					// dataId
				// });
			}
			else
			{
				// this.db.Query<PersistedData>("UPDATE PersistedData SET IsConsumed = 0 WHERE Id = ?", new object[]
				// {
					// dataId
				// });
				this._availableCount.Increment();
			}
			this._consumingCount.Decrement();
		}

		// Token: 0x060020F9 RID: 8441 RVA: 0x0008AFC4 File Offset: 0x000893C4
		public IEnumerator Map(IConsumer consumer)
		{
			IEnumerator enumerator = global::Wooga.Coroutines.Coroutines.Empty();
			int num = this.AvailableCount();
			for (int i = 0; i < num; i++)
			{
				enumerator = enumerator.ContinueWith(() => this.Consume(consumer));
			}
			return enumerator;
		}

		// Token: 0x060020FA RID: 8442 RVA: 0x0008B018 File Offset: 0x00089418
		public int AvailableCount()
		{
			// return this._availableCount.Count;
			return 0;
		}

		// Token: 0x060020FB RID: 8443 RVA: 0x0008B025 File Offset: 0x00089425
		public bool IsBeingConsumed()
		{
			return this._consumingCount.Count > 0;
		}

		// Token: 0x04004BAD RID: 19373
		// private SQLiteConnection db;

		// Token: 0x04004BAE RID: 19374
		private AtomicCounter _availableCount;

		// Token: 0x04004BAF RID: 19375
		private AtomicCounter _consumingCount = new AtomicCounter();
	}
}

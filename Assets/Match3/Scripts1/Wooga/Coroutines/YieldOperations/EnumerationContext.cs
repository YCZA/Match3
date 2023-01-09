using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wooga.Coroutines.YieldOperations
{
	// Token: 0x020003DF RID: 991
	public class EnumerationContext
	{
		// Token: 0x06001DEC RID: 7660 RVA: 0x0007F892 File Offset: 0x0007DC92
		private EnumerationContext()
		{
		}

		// Token: 0x170004AA RID: 1194
		// (set) Token: 0x06001DED RID: 7661 RVA: 0x0007F8AC File Offset: 0x0007DCAC
		public bool IsTracingEnabled
		{
			set
			{
			}
		}

		// Token: 0x170004AB RID: 1195
		// (get) Token: 0x06001DEE RID: 7662 RVA: 0x0007F8AE File Offset: 0x0007DCAE
		public object Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x06001DEF RID: 7663 RVA: 0x0007F8B8 File Offset: 0x0007DCB8
		public static EnumerationContext WithHead(IEnumerator head)
		{
			EnumerationContext enumerationContext = new EnumerationContext();
			enumerationContext._operations.Add(new HeadYieldOpertation(enumerationContext, head));
			return enumerationContext;
		}

		// Token: 0x06001DF0 RID: 7664 RVA: 0x0007F8DE File Offset: 0x0007DCDE
		public void Add(IYieldOperation operation)
		{
			this._operations.Add(operation);
		}

		// Token: 0x06001DF1 RID: 7665 RVA: 0x0007F8EC File Offset: 0x0007DCEC
		public bool MoveNext()
		{
			if (this._operations.Count == 0)
			{
				return false;
			}
			if (this.Index < 0)
			{
				this.Index++;
				this._continuation = this._operations[this.Index].GetContinuation();
			}
			bool result;
			for (;;)
			{
					IL_48:
				try
				{
					if (this._continuation is YieldInstruction || this._continuation is CustomYieldInstruction)
					{
						this._value = this._continuation;
						this._producedValue = true;
						this._continuation = Nothing.AtAll;
						result = this.DoReturn();
						break;
					}
					if (!(this._continuation is Nothing))
					{
						this._currentEnumerator = (this._currentEnumerator ?? (this._continuation as IEnumerator));
						if (this._currentEnumerator != null)
						{
							if (Coroutines.FlattenMoveNext(ref this._stack, ref this._currentEnumerator, ref this._value))
							{
								this._producedValue = true;
								if (!Future.UseProbing || this._value is YieldInstruction || this._value is CustomYieldInstruction)
								{
									result = this.DoReturn();
									break;
								}
								if (Coroutines.FlattenMoveNext(ref this._stack, ref this._currentEnumerator, ref this._value))
								{
									this._producedValue = true;
									result = this.DoReturn();
									break;
								}
								this._currentEnumerator = null;
							}
							else
							{
								this._currentEnumerator = null;
							}
						}
						else
						{
							this._value = this._continuation;
							this._producedValue = true;
						}
					}
					if (++this.Index >= this._operations.Count)
					{
						this._operations[this._operations.Count - 1].Finally();
						this._operations.Clear();
						result = this.DoReturn();
						break;
					}
					this._operations[this.Index - 1].Finally();
					this._continuation = this._operations[this.Index].GetContinuation();
				}
				catch (Exception e)
				{
					this._currentEnumerator = null;
					this._producedValue = false;
					this._value = null;
					while (this.Index < this._operations.Count)
					{
						IYieldOperation yieldOperation = this._operations[this.Index];
						yieldOperation.Finally();
						if (yieldOperation.HandlesException(e))
						{
							try
							{
								this._continuation = yieldOperation.HandleException(e);
							}
							catch (Exception ex)
							{
								e = ex;
								goto IL_276;
							}
							goto IL_48;
						}
						IL_276:
						this.Index++;
					}
					throw;
				}
			}
			return result;
		}

		// Token: 0x06001DF2 RID: 7666 RVA: 0x0007FBD0 File Offset: 0x0007DFD0
		private bool DoReturn()
		{
			if (this._producedValue)
			{
				this._producedValue = false;
				return true;
			}
			return false;
		}

		// Token: 0x040049E6 RID: 18918
		public static bool GlobalTracing;

		// Token: 0x040049E7 RID: 18919
		private readonly List<IYieldOperation> _operations = new List<IYieldOperation>();

		// Token: 0x040049E8 RID: 18920
		private Stack<IEnumerator> _stack;

		// Token: 0x040049E9 RID: 18921
		private object _continuation;

		// Token: 0x040049EA RID: 18922
		private IEnumerator _currentEnumerator;

		// Token: 0x040049EB RID: 18923
		private bool _producedValue;

		// Token: 0x040049EC RID: 18924
		private object _value;

		// Token: 0x040049ED RID: 18925
		public int Index = -1;
	}
}

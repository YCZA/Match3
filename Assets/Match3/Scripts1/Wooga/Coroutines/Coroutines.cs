using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Wooga.Coroutines.YieldOperations;

namespace Wooga.Coroutines
{
	// Token: 0x020003C9 RID: 969
	public static class Coroutines
	{
		// Token: 0x06001D35 RID: 7477 RVA: 0x0007DF43 File Offset: 0x0007C343
		public static IEnumerator<TOut> ContinueWith<TOut>(this IEnumerator head, Func<TOut> next)
		{
			return new YieldWithToeOperation1<TOut>(head, next);
		}

		// Token: 0x06001D36 RID: 7478 RVA: 0x0007DF4C File Offset: 0x0007C34C
		public static IEnumerator<TOut> ContinueWith<TIn, TOut>(this IEnumerator<TIn> head, Func<TIn, TOut> next)
		{
			return new YieldWithToeOperation2<TIn, TOut>(head, next);
		}

		// Token: 0x06001D37 RID: 7479 RVA: 0x0007DF55 File Offset: 0x0007C355
		public static IEnumerator<TOut> ContinueWith<TIn, TOut>(this IEnumerator head, Func<TIn, TOut> next)
		{
			return new YieldWithToeOperation2<TIn, TOut>(head, next);
		}

		// Token: 0x06001D38 RID: 7480 RVA: 0x0007DF5E File Offset: 0x0007C35E
		public static IEnumerator ContinueWith(this IEnumerator head, Func<IEnumerator> next)
		{
			return new YieldWithTailOperation0(head, next);
		}

		// Token: 0x06001D39 RID: 7481 RVA: 0x0007DF67 File Offset: 0x0007C367
		public static IEnumerator ContinueWith<TIn>(this IEnumerator<TIn> head, Func<TIn, IEnumerator> next)
		{
			return new YieldWithTailOperation3<TIn>(head, next);
		}

		// Token: 0x06001D3A RID: 7482 RVA: 0x0007DF70 File Offset: 0x0007C370
		public static IEnumerator<TOut> ContinueWith<TIn, TOut>(this IEnumerator head, Func<TIn, IEnumerator<TOut>> next)
		{
			return new YieldWithTailOperation2<TIn, TOut>(head, next);
		}

		// Token: 0x06001D3B RID: 7483 RVA: 0x0007DF79 File Offset: 0x0007C379
		public static IEnumerator<TOut> ContinueWith<TIn, TOut>(this IEnumerator<TIn> head, Func<TIn, IEnumerator<TOut>> next)
		{
			return new YieldWithTailOperation2<TIn, TOut>(head, next);
		}

		// Token: 0x06001D3C RID: 7484 RVA: 0x0007DF82 File Offset: 0x0007C382
		public static IEnumerator<TOut> ContinueWith<TOut>(this IEnumerator head, Func<IEnumerator<TOut>> next)
		{
			return new YieldWithTailOperation1<TOut>(head, next);
		}

		// Token: 0x06001D3D RID: 7485 RVA: 0x0007DF8B File Offset: 0x0007C38B
		public static IEnumerator ContinueWith<TIn>(this IEnumerator<TIn> head, Action<TIn> next)
		{
			return head.OnComplete(next);
		}

		// Token: 0x06001D3E RID: 7486 RVA: 0x0007DF94 File Offset: 0x0007C394
		public static IEnumerator OnComplete<TIn>(this IEnumerator<TIn> head, Action<TIn> next)
		{
			return new YieldWithPostActionOperation2<TIn>(head, next);
		}

		// Token: 0x06001D3F RID: 7487 RVA: 0x0007DF9D File Offset: 0x0007C39D
		public static IEnumerator ContinueWith(this IEnumerator head, Action next)
		{
			return head.OnComplete(next);
		}

		// Token: 0x06001D40 RID: 7488 RVA: 0x0007DFA6 File Offset: 0x0007C3A6
		public static IEnumerator OnComplete(this IEnumerator head, Action next)
		{
			return new YieldWithPostActionOperation1(head, next);
		}

		// Token: 0x06001D41 RID: 7489 RVA: 0x0007DFAF File Offset: 0x0007C3AF
		public static IEnumerator ContinueWith<TIn>(this IEnumerator head, Action<TIn> next)
		{
			return head.OnComplete(next);
		}

		// Token: 0x06001D42 RID: 7490 RVA: 0x0007DFB8 File Offset: 0x0007C3B8
		public static IEnumerator OnComplete<TIn>(this IEnumerator head, Action<TIn> next)
		{
			return new YieldWithPostActionOperation2<TIn>(head, next);
		}

		// Token: 0x06001D43 RID: 7491 RVA: 0x0007DFC1 File Offset: 0x0007C3C1
		public static IEnumerator Empty()
		{
			return EmptyEnumerator.Instance;
		}

		// Token: 0x06001D44 RID: 7492 RVA: 0x0007DFC8 File Offset: 0x0007C3C8
		public static IEnumerator<T> Yield<T>(this Func<T> factory)
		{
			yield return factory();
			yield break;
		}

		// Token: 0x06001D45 RID: 7493 RVA: 0x0007DFE4 File Offset: 0x0007C3E4
		public static IEnumerator Yield(this Action action)
		{
			action();
			yield break;
		}

		// Token: 0x06001D46 RID: 7494 RVA: 0x0007E000 File Offset: 0x0007C400
		public static IEnumerator<T> Yield<T>(this object obj)
		{
			yield return (T)((object)obj);
			yield break;
		}

		// Token: 0x06001D47 RID: 7495 RVA: 0x0007E01C File Offset: 0x0007C41C
		public static IEnumerator<T> Yield<T>(this T obj)
		{
			yield return obj;
			yield break;
		}

		// Token: 0x06001D48 RID: 7496 RVA: 0x0007E037 File Offset: 0x0007C437
		public static IEnumerator EraseType<TIn>(this IEnumerator<TIn> enumerator)
		{
			return enumerator;
		}

		// Token: 0x06001D49 RID: 7497 RVA: 0x0007E03A File Offset: 0x0007C43A
		public static IEnumerator<OutT> Cast<OutT>(this IEnumerator enumerator)
		{
			return new CastOperation<OutT>(enumerator);
		}

		// Token: 0x06001D4A RID: 7498 RVA: 0x0007E042 File Offset: 0x0007C442
		public static IEnumerator<OutT> EnableTracing<OutT>(this IEnumerator<OutT> enumerator)
		{
			return new ContextOperation<OutT>(enumerator, delegate(EnumerationContext context)
			{
				context.IsTracingEnabled = true;
			});
		}

		// Token: 0x06001D4B RID: 7499 RVA: 0x0007E056 File Offset: 0x0007C456
		public static IEnumerator EnableTracing(this IEnumerator enumerator)
		{
			return new ContextOperation<object>(enumerator, delegate(EnumerationContext context)
			{
				context.IsTracingEnabled = true;
			});
		}

		// Token: 0x06001D4C RID: 7500 RVA: 0x0007E07C File Offset: 0x0007C47C
		public static bool Probe(this IEnumerator enumerator, out object value)
		{
			value = null;
			if (!enumerator.MoveNext())
			{
				return true;
			}
			value = enumerator.Current;
			if (value is YieldInstruction || value is IEnumerator)
			{
				return false;
			}
			if (enumerator.MoveNext())
			{
				value = enumerator.Current;
				return false;
			}
			return !(value is YieldInstruction) && !(value is IEnumerator);
		}

		// Token: 0x06001D4D RID: 7501 RVA: 0x0007E0EC File Offset: 0x0007C4EC
		public static bool FlattenMoveNext(ref Stack<IEnumerator> enumerators, ref IEnumerator currentEnum)
		{
			for (;;)
			{
				if (!currentEnum.MoveNext())
				{
					if (enumerators == null || enumerators.Count <= 0)
					{
						break;
					}
					currentEnum = enumerators.Pop();
				}
				else
				{
					IEnumerator enumerator = currentEnum.Current as IEnumerator;
					if (enumerator == null)
					{
						return true;
					}
					if (enumerators == null)
					{
						enumerators = new Stack<IEnumerator>();
					}
					enumerators.Push(currentEnum);
					currentEnum = enumerator;
				}
			}
			return false;
		}

		// Token: 0x06001D4E RID: 7502 RVA: 0x0007E168 File Offset: 0x0007C568
		public static bool FlattenMoveNext(ref Stack<IEnumerator> enumerators, ref IEnumerator currentEnum, ref object value)
		{
			object obj;
			for (;;)
			{
				if (!currentEnum.MoveNext())
				{
					if (enumerators == null || enumerators.Count <= 0)
					{
						break;
					}
					currentEnum = enumerators.Pop();
				}
				else
				{
					obj = currentEnum.Current;
					IEnumerator enumerator = obj as IEnumerator;
					if (enumerator == null)
					{
						goto IL_69;
					}
					if (enumerators == null)
					{
						enumerators = new Stack<IEnumerator>();
					}
					enumerators.Push(currentEnum);
					currentEnum = enumerator;
				}
			}
			return false;
			IL_69:
			value = obj;
			return true;
		}

		// Token: 0x06001D4F RID: 7503 RVA: 0x0007E1E7 File Offset: 0x0007C5E7
		public static Future<object> Start(this IEnumerator enumerator)
		{
			return CoroutineRunner.StartTask(enumerator, null, null);
		}

		// Token: 0x06001D50 RID: 7504 RVA: 0x0007E1F1 File Offset: 0x0007C5F1
		public static Future<object> Start(this IEnumerator enumerator, Action<Exception> errorHandler)
		{
			return CoroutineRunner.StartTask(enumerator, errorHandler, null);
		}

		// Token: 0x06001D51 RID: 7505 RVA: 0x0007E1FB File Offset: 0x0007C5FB
		public static Future<object> Start(this IEnumerator enumerator, ICancellationToken cancellationToken)
		{
			return CoroutineRunner.StartTask(enumerator, null, cancellationToken);
		}

		// Token: 0x06001D52 RID: 7506 RVA: 0x0007E205 File Offset: 0x0007C605
		public static Future<object> Start(this IEnumerator enumerator, Action<Exception> errorHandler, ICancellationToken cancellationToken)
		{
			return CoroutineRunner.StartTask(enumerator, errorHandler, cancellationToken);
		}

		// Token: 0x06001D53 RID: 7507 RVA: 0x0007E20F File Offset: 0x0007C60F
		public static Future<T> Start<T>(this IEnumerator<T> enumerator)
		{
			return CoroutineRunner.StartTask<T>(enumerator, null, null);
		}

		// Token: 0x06001D54 RID: 7508 RVA: 0x0007E219 File Offset: 0x0007C619
		public static Future<T> Start<T>(this IEnumerator<T> enumerator, Action<Exception> errorHandler)
		{
			return CoroutineRunner.StartTask<T>(enumerator, errorHandler, null);
		}

		// Token: 0x06001D55 RID: 7509 RVA: 0x0007E223 File Offset: 0x0007C623
		public static Future<T> Start<T>(this IEnumerator<T> enumerator, ICancellationToken cancellationToken)
		{
			return CoroutineRunner.StartTask<T>(enumerator, null, cancellationToken);
		}

		// Token: 0x06001D56 RID: 7510 RVA: 0x0007E22D File Offset: 0x0007C62D
		public static Future<T> Start<T>(this IEnumerator<T> enumerator, Action<Exception> errorHandler, ICancellationToken cancellationToken)
		{
			return CoroutineRunner.StartTask<T>(enumerator, errorHandler, cancellationToken);
		}

		// Token: 0x06001D57 RID: 7511 RVA: 0x0007E237 File Offset: 0x0007C637
		public static IEnumerator StartAndAwait(this IEnumerator enumerator)
		{
			return CoroutineRunner.StartTask(enumerator, null, null).Await();
		}

		// Token: 0x06001D58 RID: 7512 RVA: 0x0007E246 File Offset: 0x0007C646
		public static IEnumerator StartAndAwait(this IEnumerator enumerator, Action<Exception> errorHandler)
		{
			return CoroutineRunner.StartTask(enumerator, errorHandler, null).Await();
		}

		// Token: 0x06001D59 RID: 7513 RVA: 0x0007E255 File Offset: 0x0007C655
		public static IEnumerator StartAndAwait(this IEnumerator enumerator, ICancellationToken cancelationToken)
		{
			return CoroutineRunner.StartTask(enumerator, null, cancelationToken).Await();
		}

		// Token: 0x06001D5A RID: 7514 RVA: 0x0007E264 File Offset: 0x0007C664
		public static IEnumerator StartAndAwait(this IEnumerator enumerator, Action<Exception> errorHandler, ICancellationToken cancelationToken)
		{
			return CoroutineRunner.StartTask(enumerator, errorHandler, cancelationToken).Await();
		}

		// Token: 0x06001D5B RID: 7515 RVA: 0x0007E273 File Offset: 0x0007C673
		public static IEnumerator<T> StartAndAwait<T>(this IEnumerator<T> enumerator)
		{
			return CoroutineRunner.StartTask<T>(enumerator, null, null).Await();
		}

		// Token: 0x06001D5C RID: 7516 RVA: 0x0007E282 File Offset: 0x0007C682
		public static IEnumerator<T> StartAndAwait<T>(this IEnumerator<T> enumerator, Action<Exception> errorHandler)
		{
			return CoroutineRunner.StartTask<T>(enumerator, errorHandler, null).Await();
		}

		// Token: 0x06001D5D RID: 7517 RVA: 0x0007E291 File Offset: 0x0007C691
		public static IEnumerator<T> StartAndAwait<T>(this IEnumerator<T> enumerator, ICancellationToken cancelationToken)
		{
			return CoroutineRunner.StartTask<T>(enumerator, null, cancelationToken).Await();
		}

		// Token: 0x06001D5E RID: 7518 RVA: 0x0007E2A0 File Offset: 0x0007C6A0
		public static IEnumerator<T> StartAndAwait<T>(this IEnumerator<T> enumerator, Action<Exception> errorHandler, ICancellationToken cancelationToken)
		{
			return CoroutineRunner.StartTask<T>(enumerator, errorHandler, cancelationToken).Await();
		}

		// Token: 0x06001D5F RID: 7519 RVA: 0x0007E2AF File Offset: 0x0007C6AF
		public static IEnumerable<Future<object>> StartAll(this IEnumerable<IEnumerator> coroutines)
		{
			if (Coroutines._003C_003Ef__mg_0024cache0 == null)
			{
				Coroutines._003C_003Ef__mg_0024cache0 = new Func<IEnumerator, Future<object>>(Coroutines.Start);
			}
			return coroutines.Select(Coroutines._003C_003Ef__mg_0024cache0).ToList<Future<object>>();
		}

		// Token: 0x06001D60 RID: 7520 RVA: 0x0007E2D9 File Offset: 0x0007C6D9
		public static IEnumerable<Future<T>> StartAll<T>(this IEnumerable<IEnumerator<T>> coroutines)
		{
			return coroutines.Select(new Func<IEnumerator<T>, Future<T>>(Coroutines.Start<T>)).ToList<Future<T>>();
		}

		// Token: 0x06001D61 RID: 7521 RVA: 0x0007E2F4 File Offset: 0x0007C6F4
		public static IEnumerator<List<OutputT>> StartAllAndAwait<InputT, OutputT>(this IEnumerable<InputT> source, Func<InputT, IEnumerator<OutputT>> spawner)
		{
			return (from v in source
			select spawner(v).Start<OutputT>()).Await<OutputT>();
		}

		// Token: 0x06001D62 RID: 7522 RVA: 0x0007E328 File Offset: 0x0007C728
		public static IEnumerator StartAllAndAwait<InputT>(this IEnumerable<InputT> source, Func<InputT, IEnumerator> spawner)
		{
			return (from v in source
			select spawner(v).Start()).Await<object>();
		}

		// Token: 0x06001D63 RID: 7523 RVA: 0x0007E35C File Offset: 0x0007C75C
		public static IEnumerator<List<T>> Await<T>(this IEnumerable<Future<T>> tasks)
		{
			List<Future<T>> taskList = tasks.ToList<Future<T>>();
			T[] result = new T[taskList.Count];
			List<T> resultList = new List<T>(taskList.Count);
			if (taskList.Count > 0)
			{
				bool shouldContinue = true;
				bool allCompleted = false;
				while (!allCompleted && shouldContinue)
				{
					allCompleted = true;
					for (int i = 0; i < taskList.Count; i++)
					{
						Future<T> future = taskList[i];
						if (future.IsFaulted)
						{
							throw future.Exception;
						}
						if (future.IsRanToCompletion)
						{
							result[i] = future.Result;
						}
						else if (future.IsCompleted)
						{
							shouldContinue &= future.IsRanToCompletion;
						}
						else
						{
							allCompleted = false;
						}
					}
					if (!allCompleted && shouldContinue)
					{
						yield return resultList;
					}
				}
			}
			resultList.AddRange(result);
			yield return resultList;
			yield break;
		}

		// Token: 0x06001D64 RID: 7524 RVA: 0x0007E377 File Offset: 0x0007C777
		public static IEnumerator Finally(this IEnumerator enumerator, Action finallyBlock)
		{
			return new FinallyOperation<object>(enumerator, finallyBlock);
		}

		// Token: 0x06001D65 RID: 7525 RVA: 0x0007E380 File Offset: 0x0007C780
		public static IEnumerator<T> Finally<T>(this IEnumerator<T> enumerator, Action finallyBlock)
		{
			return new FinallyOperation<T>(enumerator, finallyBlock);
		}

		// Token: 0x06001D66 RID: 7526 RVA: 0x0007E389 File Offset: 0x0007C789
		public static IEnumerator Catch(this IEnumerator enumerator, Action<Exception> handler)
		{
			return new CatchOperationWithAction(enumerator, handler);
		}

		// Token: 0x06001D67 RID: 7527 RVA: 0x0007E392 File Offset: 0x0007C792
		public static IEnumerator<TOut> Catch<TOut>(this IEnumerator<TOut> enumerator, Func<Exception, TOut> handler)
		{
			return new CatchWithToeOperation<Exception, TOut>(enumerator, handler);
		}

		// Token: 0x06001D68 RID: 7528 RVA: 0x0007E39B File Offset: 0x0007C79B
		public static IEnumerator<TOut> Catch<TOut>(this IEnumerator<TOut> enumerator, Func<Exception, IEnumerator<TOut>> handler)
		{
			return new CatchWithTailOperation<Exception, TOut>(enumerator, handler);
		}

		// Token: 0x06001D69 RID: 7529 RVA: 0x0007E3A4 File Offset: 0x0007C7A4
		public static IEnumerator<TOut> Catch<ExceptionT, TOut>(this IEnumerator<TOut> enumerator, Func<ExceptionT, TOut> handler) where ExceptionT : Exception
		{
			return new CatchWithToeOperation<ExceptionT, TOut>(enumerator, handler);
		}

		// Token: 0x06001D6A RID: 7530 RVA: 0x0007E3AD File Offset: 0x0007C7AD
		public static IEnumerator<T> CatchAndThrow<T>(this IEnumerator<T> enumerator, Action<Exception> handler)
		{
			return new RethrowOperation<T>(enumerator, handler);
		}

		// Token: 0x06001D6B RID: 7531 RVA: 0x0007E3B8 File Offset: 0x0007C7B8
		public static bool TryMoveNext(ref IEnumerator enumerator, ref object nextValue, out Exception exception)
		{
			exception = null;
			bool result;
			try
			{
				if (enumerator.MoveNext())
				{
					nextValue = enumerator.Current;
					result = true;
				}
				else
				{
					result = false;
				}
			}
			catch (Exception ex)
			{
				exception = ex;
				result = false;
			}
			return result;
		}

		// Token: 0x06001D6C RID: 7532 RVA: 0x0007E408 File Offset: 0x0007C808
		public static bool TryMoveNext<T>(ref IEnumerator<T> enumerator, ref T nextValue, out Exception exception)
		{
			exception = null;
			bool result;
			try
			{
				if (enumerator.MoveNext())
				{
					nextValue = enumerator.Current;
					result = true;
				}
				else
				{
					result = false;
				}
			}
			catch (Exception ex)
			{
				exception = ex;
				result = false;
			}
			return result;
		}

		// Token: 0x040049C5 RID: 18885
		public static ILogger Log;

		// Token: 0x040049C7 RID: 18887
		[CompilerGenerated]
		private static Func<IEnumerator, Future<object>> _003C_003Ef__mg_0024cache0;
	}
}

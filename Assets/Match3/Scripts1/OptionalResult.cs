using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Wooga.Coroutines;

namespace Match3.Scripts1
{
	// Token: 0x020003D7 RID: 983
	[DebuggerDisplay("HasValue = {_hasValue}, Value = {_value}")]
	public struct OptionalResult<T> : IEquatable<OptionalResult<T>>, IEnumerable<T>, IEnumerable
	{
		// Token: 0x06001DB4 RID: 7604 RVA: 0x0007F1A2 File Offset: 0x0007D5A2
		internal OptionalResult(T value)
		{
			this._hasValue = true;
			this._value = value;
		}

		// Token: 0x06001DB5 RID: 7605 RVA: 0x0007F1B2 File Offset: 0x0007D5B2
		public static OptionalResult<T> Some(T value)
		{
			return new OptionalResult<T>(value);
		}

		// Token: 0x170004A4 RID: 1188
		// (get) Token: 0x06001DB6 RID: 7606 RVA: 0x0007F1BA File Offset: 0x0007D5BA
		public static OptionalResult<T> None
		{
			get
			{
				return OptionalResult<T>._none;
			}
		}

		// Token: 0x170004A5 RID: 1189
		// (get) Token: 0x06001DB7 RID: 7607 RVA: 0x0007F1C1 File Offset: 0x0007D5C1
		public bool HasValue
		{
			get
			{
				return this._hasValue;
			}
		}

		// Token: 0x170004A6 RID: 1190
		// (get) Token: 0x06001DB8 RID: 7608 RVA: 0x0007F1C9 File Offset: 0x0007D5C9
		public bool HasNothing
		{
			get
			{
				return !this._hasValue;
			}
		}

		// Token: 0x170004A7 RID: 1191
		// (get) Token: 0x06001DB9 RID: 7609 RVA: 0x0007F1D4 File Offset: 0x0007D5D4
		[DebuggerDisplay("{_value}")]
		public T Value
		{
			get
			{
				if (!this.HasValue)
				{
					throw new InvalidOperationException("The option does not have a value");
				}
				return this._value;
			}
		}

		// Token: 0x170004A8 RID: 1192
		// (get) Token: 0x06001DBA RID: 7610 RVA: 0x0007F1F4 File Offset: 0x0007D5F4
		public T ValueOrDefault
		{
			get
			{
				return (!this._hasValue) ? default(T) : this._value;
			}
		}

		// Token: 0x06001DBB RID: 7611 RVA: 0x0007F220 File Offset: 0x0007D620
		public T ValueOr(T val)
		{
			return (!this._hasValue) ? val : this._value;
		}

		// Token: 0x06001DBC RID: 7612 RVA: 0x0007F239 File Offset: 0x0007D639
		public T ValueOr(Func<T> val)
		{
			return (!this._hasValue) ? val() : this._value;
		}

		// Token: 0x06001DBD RID: 7613 RVA: 0x0007F257 File Offset: 0x0007D657
		public bool TryGetValue(out T result)
		{
			result = this.ValueOrDefault;
			return this.HasValue;
		}

		// Token: 0x06001DBE RID: 7614 RVA: 0x0007F26B File Offset: 0x0007D66B
		public void Match(Action None = null, Action<T> Some = null)
		{
			if (!this._hasValue)
			{
				if (None != null)
				{
					None();
				}
			}
			else if (Some != null)
			{
				Some(this._value);
			}
		}

		// Token: 0x06001DBF RID: 7615 RVA: 0x0007F29B File Offset: 0x0007D69B
		public TOut Match<TOut>(Func<TOut> None, Func<T, TOut> Some)
		{
			if (!this._hasValue)
			{
				return None();
			}
			return Some(this._value);
		}

		// Token: 0x06001DC0 RID: 7616 RVA: 0x0007F2BB File Offset: 0x0007D6BB
		public static implicit operator OptionalResult<T>(OptionalResult option)
		{
			return OptionalResult<T>.None;
		}

		// Token: 0x06001DC1 RID: 7617 RVA: 0x0007F2C2 File Offset: 0x0007D6C2
		public static implicit operator OptionalResult<T>(T value)
		{
			return OptionalResult.Create<T>(value);
		}

		// Token: 0x06001DC2 RID: 7618 RVA: 0x0007F2CA File Offset: 0x0007D6CA
		public static bool operator ==(OptionalResult<T> lhs, OptionalResult<T> rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x06001DC3 RID: 7619 RVA: 0x0007F2D4 File Offset: 0x0007D6D4
		public static bool operator !=(OptionalResult<T> lhs, OptionalResult<T> rhs)
		{
			return !lhs.Equals(rhs);
		}

		// Token: 0x06001DC4 RID: 7620 RVA: 0x0007F2E1 File Offset: 0x0007D6E1
		public T[] ToArray()
		{
			if (this._hasValue)
			{
				return new T[]
				{
					this._value
				};
			}
			return EmptyArray<T>.Array;
		}

		// Token: 0x06001DC5 RID: 7621 RVA: 0x0007F307 File Offset: 0x0007D707
		public void ForEach(Action<T> action)
		{
			if (this._hasValue)
			{
				action(this._value);
			}
		}

		// Token: 0x06001DC6 RID: 7622 RVA: 0x0007F320 File Offset: 0x0007D720
		public OptionalResult<TResult> Select<TResult>(Func<T, TResult> selector)
		{
			if (this._hasValue)
			{
				return selector(this._value);
			}
			return OptionalResult.None;
		}

		// Token: 0x06001DC7 RID: 7623 RVA: 0x0007F349 File Offset: 0x0007D749
		public OptionalResult<TResult> Select<TResult>(Func<T, int, TResult> selector)
		{
			if (this._hasValue)
			{
				return selector(this._value, 0);
			}
			return OptionalResult.None;
		}

		// Token: 0x06001DC8 RID: 7624 RVA: 0x0007F373 File Offset: 0x0007D773
		public OptionalResult<T> Where(Func<T, bool> predicate)
		{
			if (this._hasValue && predicate(this._value))
			{
				return this;
			}
			return OptionalResult.None;
		}

		// Token: 0x06001DC9 RID: 7625 RVA: 0x0007F3A2 File Offset: 0x0007D7A2
		public OptionalResult<T> Where(Func<T, int, bool> predicate)
		{
			if (this._hasValue && predicate(this._value, 0))
			{
				return this;
			}
			return OptionalResult.None;
		}

		// Token: 0x06001DCA RID: 7626 RVA: 0x0007F3D2 File Offset: 0x0007D7D2
		public bool Equals(OptionalResult<T> other)
		{
			return this.HasValue == other.HasValue && (!this.HasValue || EqualityComparer<T>.Default.Equals(this._value, other.Value));
		}

		// Token: 0x06001DCB RID: 7627 RVA: 0x0007F40C File Offset: 0x0007D80C
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06001DCC RID: 7628 RVA: 0x0007F414 File Offset: 0x0007D814
		public IEnumerator<T> GetEnumerator()
		{
			if (this._hasValue)
			{
				yield return this._value;
			}
			yield break;
		}

		// Token: 0x06001DCD RID: 7629 RVA: 0x0007F434 File Offset: 0x0007D834
		public IEnumerator<T> SelectMany()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06001DCE RID: 7630 RVA: 0x0007F43C File Offset: 0x0007D83C
		public override bool Equals(object other)
		{
			return other is OptionalResult<T> && this.Equals((OptionalResult<T>)other);
		}

		// Token: 0x06001DCF RID: 7631 RVA: 0x0007F457 File Offset: 0x0007D857
		public override int GetHashCode()
		{
			if (!this._hasValue)
			{
				return 0;
			}
			return EqualityComparer<T>.Default.GetHashCode(this._value);
		}

		// Token: 0x06001DD0 RID: 7632 RVA: 0x0007F478 File Offset: 0x0007D878
		public override string ToString()
		{
			if (!this._hasValue)
			{
				return string.Format("OptionalResult<{0}>.None", typeof(T));
			}
			return string.Format("OptionalResult<{0}>.Some={1}", typeof(T), Convert.ToString(this._value));
		}

		// Token: 0x06001DD1 RID: 7633 RVA: 0x0007F4C9 File Offset: 0x0007D8C9
		public string ToString(Func<T, string> formatter)
		{
			if (!this._hasValue)
			{
				return this.ToString();
			}
			return formatter(this._value);
		}

		// Token: 0x040049DF RID: 18911
		private readonly T _value;

		// Token: 0x040049E0 RID: 18912
		private readonly bool _hasValue;

		// Token: 0x040049E1 RID: 18913
		private static OptionalResult<T> _none = default(OptionalResult<T>);
	}
}

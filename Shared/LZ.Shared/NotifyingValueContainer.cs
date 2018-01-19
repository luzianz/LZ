using System;

namespace LZ {

	/// <summary>
	/// Wraps a value into a property, and allows you to observe its changes.
	/// </summary>
	public class NotifyingValueContainer<T> {

		#region Fields

		private readonly Action onValueChanged;

		#endregion

		#region Properties

		private T _Value;
		public T Value {
			get { return _Value; }
			set {
				if (Object.Equals(_Value, value)) return;

				_Value = value;
				onValueChanged();
			}
		}

		#endregion

		#region Constructors

		public NotifyingValueContainer(Action onValueChanged) {
			if (onValueChanged == null) throw new ArgumentNullException(nameof(onValueChanged));

			this.onValueChanged = onValueChanged;
		}

		public NotifyingValueContainer(T initialValue, Action onValueChanged)
			: this(onValueChanged) {
			_Value = initialValue;
		}

		#endregion

		#region Operators

		public static implicit operator T(NotifyingValueContainer<T> observable) {
			return observable.Value;
		}

		#endregion
	}
}

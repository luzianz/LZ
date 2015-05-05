using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using LZ.EventHandling;

namespace LZ
{
	/// <summary>
	/// Wraps a value into a property, and allows you to observe its changes.
	/// </summary>
	public class ObservableBox<T>
	{
		#region Fields

		private readonly Action onValueChanged;

		#endregion

		#region Constructors

		public ObservableBox(Action onValueChanged = null)
		{
			this.onValueChanged = onValueChanged;
		}

		public ObservableBox(T initialValue, Action onValueChanged = null)
			: this(onValueChanged)
		{
			_Value = initialValue;
		}

		#endregion

		#region Properties

		private T _Value;
		public T Value
		{
			get { return _Value; }
			set
			{
				if (Object.Equals(_Value, value)) return;

				_Value = value;
				onValueChanged();
			}
		}

		#endregion

		#region Operators

		public static implicit operator T(ObservableBox<T> observable)
		{
			return observable.Value;
		}

		#endregion
	}
}

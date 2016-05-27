using System.ComponentModel;

namespace LZ.ComponentModel {

	public interface IPropertyChangeTracking : IRevertibleChangeTracking, INotifyPropertyChanged {

		// Note that the PropertyChanged event _notifies_ when a property changed from it's _previous_ value.
		// The HasPropertyChanged method _checks_ if a value changed from its _original_ (since last commit: see AcceptChanges) value.
		// When PropertyChanged is fired, the consumer should call this method.
		bool HasPropertyChanged(string propertyName);
	}
}
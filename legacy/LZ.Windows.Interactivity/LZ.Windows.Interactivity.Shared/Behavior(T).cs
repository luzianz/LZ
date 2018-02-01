using Microsoft.Xaml.Interactivity;
using System;
using Windows.UI.Xaml;

namespace LZ.Interactivity {

	public class Behavior<AssociatedType> : Behavior where AssociatedType : FrameworkElement {

		#region Properties

		protected new AssociatedType AssociatedObject {
			get {
				return (AssociatedType)base.AssociatedObject;
			}
		}

		#endregion

		#region Behavior

		protected override void OnAttaching(DependencyObject associatedObject) {
			if (associatedObject is AssociatedType) base.OnAttaching(associatedObject);
			else throw new ArgumentException(string.Format("Invalid type. Expecting object of type {0}", typeof(AssociatedType).FullName), "associatedObject");
		}

		#endregion
	}
}

using System;
using System.Collections.Generic;

namespace LZ.Collections {

	public class WeakCollection<T> where T : class {

		#region Fields

		private readonly List<WeakReference<T>> weakRefs;

		#endregion

		#region Constructor

		public WeakCollection() {
			weakRefs = new List<WeakReference<T>>();
		}

		#endregion

		#region Public Methods

		public void Add(T item) {
			weakRefs.Add(new WeakReference<T>(item));
		}

		public void Remove(T itemToRemove) {
			var weakRefsToRemove = new List<WeakReference<T>>();

			ForEach((i, r) => {
				if (itemToRemove == i) {
					weakRefsToRemove.Add(r);
				}
			}, weakRefsToRemove);
		}

		public void ForEach(Action<T> onEach) {
			ForEach((i, r) => onEach(i), new List<WeakReference<T>>());
		}

		#endregion

		#region Private Methods

		private void ForEach(Action<T, WeakReference<T>> onEach, ICollection<WeakReference<T>> weakRefsToRemove) {
			T item;
			foreach (var weakRef in weakRefs) {
				if (weakRef.TryGetTarget(out item)) {
					onEach(item, weakRef);
				} else {
					// remove lost references
					weakRefsToRemove.Add(weakRef);
				}
			}

			foreach (var weakRef in weakRefsToRemove) {
				weakRefs.Remove(weakRef);
			}
		}

		#endregion
	}
}

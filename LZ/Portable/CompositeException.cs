using System;
using System.Collections;
using System.Collections.Generic;

namespace LZ {

	public class CompositeException : Exception, ICollection<Exception> {

		#region Fields

		private readonly IList<Exception> exceptions;

		#endregion

		#region Constructor

		public CompositeException(string message, IEnumerable<Exception> exceptions) {
			Message = message;
			this.exceptions = new List<Exception>(exceptions);
		}

		public CompositeException(string message, params Exception[] exceptions) {
			Message = message;
			this.exceptions = new List<Exception>(exceptions);
		}

		#endregion

		#region Exception

		public override string Message { get; }

		#endregion
		
		#region ICollection<Exception>

		public int Count {
			get {
				return exceptions.Count;
			}
		}

		public bool IsReadOnly {
			get {
				return exceptions.IsReadOnly;
			}
		}

		public void Add(Exception item) {
			exceptions.Add(item);
		}

		public void Clear() {
			exceptions.Clear();
		}

		public bool Contains(Exception item) {
			return exceptions.Contains(item);
		}

		public void CopyTo(Exception[] array, int arrayIndex) {
			exceptions.CopyTo(array, arrayIndex);
		}

		public bool Remove(Exception item) {
			return exceptions.Remove(item);
		}

		#endregion

		#region IEnumerable

		IEnumerator IEnumerable.GetEnumerator() {
			return exceptions.GetEnumerator();
		}

		#endregion

		#region IEnumerable<Exception>

		IEnumerator<Exception> IEnumerable<Exception>.GetEnumerator() {
			return exceptions.GetEnumerator();
		}

		#endregion
	}
}

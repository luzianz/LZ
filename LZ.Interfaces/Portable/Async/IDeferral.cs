using System;

namespace LZ.Async {

	/// <summary>
	/// A token to trigger the completion of an asynchronous task
	/// </summary>
	public interface IDeferral : IDisposable {

		void RegisterCancellationHandler(Action onCancel);
	}
}
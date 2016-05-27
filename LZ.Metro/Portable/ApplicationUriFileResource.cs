using LZ.Async;
using System;
using System.Threading.Tasks;
using Windows.Storage;
using System.Threading;

namespace LZ.Metro {

	/// <summary>
	/// Provides a <see cref="Windows.Storage.IStorageFile"/> asynchronously given a uri of the file.
	/// </summary>
	public class ApplicationUriFileResource : IAsyncResource<IStorageFile> {

		#region Fields

		private readonly Uri appResourceUri;

		#endregion

		#region Constructor

		public ApplicationUriFileResource(Uri appResourceUri) {
			if (appResourceUri == null) throw new ArgumentNullException(nameof(appResourceUri));
			this.appResourceUri = appResourceUri;
		}

		#endregion

		#region IAsyncResource<IStorageFile>

		public async Task<IStorageFile> GetAsync() {
			return await StorageFile.GetFileFromApplicationUriAsync(appResourceUri);
		}

		public async Task<IStorageFile> GetAsync(CancellationToken cancellationToken) {
			var asyncOp = StorageFile.GetFileFromApplicationUriAsync(appResourceUri);
			return await asyncOp.InvokeAsync(cancellationToken);
		}

		#endregion
	}
}

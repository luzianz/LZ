using LZ.Async;
using System;
using System.Threading.Tasks;
using Windows.Storage;
using System.Threading;
using Windows.Foundation;

namespace LZ.Metro {

	/// <summary>
	/// Provides a <see cref="Windows.Storage.IStorageFile"/> asynchronously given a folder and file name.
	/// </summary>
	public class FolderPathFileResource : IAsyncResource<IStorageFile> {

		#region Fields

		private readonly IStorageFolder folder;
		private readonly string fileName;
		private readonly bool createIfNotExists;

		#endregion

		#region Constructor

		public FolderPathFileResource(IStorageFolder folder, string fileName, bool createIfNotExists) {
			if (folder == null) throw new ArgumentNullException(nameof(folder));
			this.folder = folder;

			this.fileName = fileName;
			this.createIfNotExists = createIfNotExists;
		}

		public FolderPathFileResource(IStorageFolder folder, string fileName)
			: this(folder, fileName, false) { }

		#endregion

		#region IAsyncResource<IStorageFile>
		
		public async Task<IStorageFile> GetAsync(CancellationToken cancellationToken) {
			IAsyncOperation<StorageFile> asyncOp;
			if (createIfNotExists) {
				asyncOp = folder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
			} else {
				asyncOp = folder.GetFileAsync(fileName);
			}

			return await asyncOp.InvokeAsync(cancellationToken);
		}

		#endregion
	}
}

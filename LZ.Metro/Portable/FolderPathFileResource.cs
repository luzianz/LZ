using LZ.Async;
using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace LZ.Metro
{
	/// <summary>
	/// Provides a <see cref="Windows.Storage.IStorageFile"/> asynchronously given a folder and file name.
	/// </summary>
	public class FolderPathFileResource : IAsyncResource<IStorageFile>
	{
		#region Fields
		
		private readonly IStorageFolder folder;
		private readonly string fileName;
		private readonly bool createIfNotExists;

		#endregion

		#region Constructor

		public FolderPathFileResource(IStorageFolder folder, string fileName, bool createIfNotExists)
		{
			if (folder == null) throw new ArgumentNullException("folder");
			this.folder = folder;

			this.fileName = fileName;
			this.createIfNotExists = createIfNotExists;
		}

		public FolderPathFileResource(IStorageFolder folder, string fileName)
			: this(folder, fileName, false)
		{ }

		#endregion

		#region IAsyncResource<IStorageFile>

		public async Task<IStorageFile> GetAsync()
		{
			if (createIfNotExists)
			{
				return await folder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
			}
			else
			{
				return await folder.GetFileAsync(fileName);
			}
		}

		#endregion
	}
}

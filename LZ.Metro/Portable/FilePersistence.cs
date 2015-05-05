using LZ.Async;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Windows.Storage;

namespace LZ.Metro
{
	/// <summary>
	/// Provides key/value persistence to a file.
	/// </summary>
	public class FilePersistence : IPersistence
	{
		#region Fields

		private readonly Type dictionaryType = typeof(Dictionary<string, object>);
		private readonly IAsyncResource<IStorageFile> fileResource;
		private Dictionary<string, object> data;

		#endregion

		#region Constructor

		public FilePersistence(IAsyncResource<IStorageFile> fileResource)
		{
			if (fileResource == null) throw new ArgumentNullException("getFileAsyncResolver");
			this.fileResource = fileResource;
		}

		#endregion

		#region IPersistence

		public bool TryGetValue(string key, out object value)
		{
			return data.TryGetValue(key, out value);
		}

		public void SetValue(string key, object value)
		{
			data[key] = value;
		}

		public async Task LoadAsync()
		{
			var file = await fileResource.GetAsync();
			using (var stream = await file.OpenStreamForReadAsync())
			{
				var serializer = new DataContractSerializer(dictionaryType);
				data = (Dictionary<string, object>)serializer.ReadObject(stream);
			}
		}

		public async Task SaveAsync()
		{
			var file = await fileResource.GetAsync();
			using (var stream = await file.OpenStreamForWriteAsync())
			{
				var serializer = new DataContractSerializer(dictionaryType);
				serializer.WriteObject(stream, data);
			}
		}

		#endregion
	}
}

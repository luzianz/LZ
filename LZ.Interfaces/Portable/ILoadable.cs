using System.Threading.Tasks;

namespace LZ
{
	/// <summary>
	/// Provides the opportunity to asynchronously load data.
	/// </summary>
	public interface ILoadable
	{
		/// <summary>
		/// Loads data. Should be called after the user interface is loaded.
		/// </summary>
		Task LoadAsync();
	}
}

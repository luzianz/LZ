using System.Threading.Tasks;

namespace LZ
{
	/// <summary>
	/// Provides the opportunity to asynchronously save data.
	/// </summary>
	public interface ISavable
	{
		/// <summary>
		/// Save data. Should be called before the user interface is navigated from, or before the application closes.
		/// </summary>
		Task SaveAsync();
	}
}

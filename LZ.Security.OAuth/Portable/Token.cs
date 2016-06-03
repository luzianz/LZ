using LZ.Security;

namespace LZ.Security.OAuth {

	public class Token : ICredential {

		#region Properties

		public virtual string Key { get; set; }
		public virtual string Secret { get; set; }

		#endregion
	}
}

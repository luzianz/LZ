using System;

namespace LZ {

	public interface ITransaction : IDisposable {

		bool IsCommitted { get; }

		void Commit();
	}
}

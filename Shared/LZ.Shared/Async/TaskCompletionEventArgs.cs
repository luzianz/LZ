using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LZ.Async {

	public class TaskCompletionEventArgs : EventArgs, IDeferralProvider {

		#region Fields

		private readonly List<TaskCompletionDeferral> deferrals;
		private readonly CancellationToken cancellationToken;

		#endregion

		#region Constructor

		public TaskCompletionEventArgs(CancellationToken cancellationToken) {
			deferrals = new List<TaskCompletionDeferral>();
			this.cancellationToken = cancellationToken;
		}

		#endregion

		#region IDeferralProvider<TResult>

		public IDeferral GetDeferral() {
			var deferral = new TaskCompletionDeferral(cancellationToken);
			deferrals.Add(deferral);
			return deferral;
		}

		#endregion

		#region Methods

		public async Task AwaitAll() {
			foreach (var deferral in deferrals) {
				await deferral.Task;
			}
		}

		#endregion
	}
}

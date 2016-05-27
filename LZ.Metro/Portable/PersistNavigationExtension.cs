using LZ.EventHandling;
using System;
using Windows.UI.Xaml.Controls;
using LZ.Async;
using LZ.Metro.Serialization;

namespace LZ.Metro {

	public class PersistNavigationExtension {

		private const string NAVIGATION_STATE_KEY = "NavigationState";

		private readonly IPersistence persistence;
		private Frame rootFrame;

		public PersistNavigationExtension(IPersistence persistence, ApplicationLifeCycleManager appManager) {
			if (persistence == null) throw new ArgumentNullException(nameof(persistence));
			if (appManager == null) throw new ArgumentNullException(nameof(appManager));
			this.persistence = persistence;

			appManager.LoadingState += AppManager_LoadingState;
			appManager.SavingState += AppManager_SavingState;
			appManager.RootFrameCreated += AppManager_RootFrameCreated;
		}

		private void AppManager_RootFrameCreated(object sender, IEventArgs<Frame> e) {
			rootFrame = e.Args as Frame;
			persistence.TryGetValueAndRemove<string>(NAVIGATION_STATE_KEY, value => rootFrame.SetNavigationState(value));
		}

		private async void AppManager_SavingState(object sender, IDeferralProvider e) {
			using (var deferral = e.GetDeferral()) {
				persistence.SetValue(NAVIGATION_STATE_KEY, rootFrame.GetNavigationState());
				await persistence.SaveAsync();
			}
		}

		private async void AppManager_LoadingState(object sender, IDeferralProvider e) {
			using (var deferral = e.GetDeferral()) {
				await persistence.LoadAsync();
			}
		}
	}
}

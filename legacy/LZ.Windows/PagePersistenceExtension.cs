using LZ.EventHandling;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using LZ.Windows.Serialization;
using LZ.Async;
using LZ.ComponentModel;

namespace LZ.Windows {

	/// <summary>
	/// Injects functionality to persist page state.
	/// </summary>
	public class PagePersistenceExtension {

		#region Fields

		private readonly IPersistence persistence;

		#endregion

		#region Constructor

		public PagePersistenceExtension(IPersistence persistence, ApplicationLifeCycleManager appManager) {
			if (persistence == null) throw new ArgumentNullException(nameof(persistence));
			if (appManager == null) throw new ArgumentNullException(nameof(appManager));
			this.persistence = persistence;

			appManager.LoadingState += appManager_LoadingState;
			appManager.SavingState += appManager_SavingState;
			appManager.RootFrameCreated += appManager_RootFrameCreated;
		}

		#endregion

		#region Event Handlers

		void appManager_RootFrameCreated(object sender, IEventArgs<Frame> e) {
			e.Args.Navigated += RootFrame_Navigated;
			e.Args.Navigating += RootFrame_Navigating;
		}

		async void appManager_SavingState(object sender, IDeferralProvider e) {
			using (var deferral = e.GetDeferral()) {
				await persistence.SaveAsync();
			}
		}

		async void appManager_LoadingState(object sender, IDeferralProvider e) {
			using (var deferral = e.GetDeferral()) {
				await persistence.LoadAsync();
			}
		}

		// Navigated to
		void RootFrame_Navigated(object sender, NavigationEventArgs e) {
			(e.Content as ISerializable)?.Deserialize(persistence);
		}

		// Navigating from
		void RootFrame_Navigating(object sender, NavigatingCancelEventArgs e) {
			((sender as Frame)?.Content as ISerializable)?.Serialize(persistence);
		}

		#endregion
	}
}

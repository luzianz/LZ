using LZ.Async;
using LZ.EventHandling;
using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Threading;

namespace LZ.Metro {

	/// <summary>
	/// Manages application lifecycle.
	/// </summary>
	public class ApplicationLifeCycleManager {

		#region Events

		public event EventHandler<IEventArgs<Frame>> RootFrameCreated;
		public event EventHandler<IDeferralProvider> SavingState;
		public event EventHandler<IDeferralProvider> LoadingState;

		#endregion

		#region Constructor

		public ApplicationLifeCycleManager(Application app) {
			app.Suspending += OnSuspending;
		}

		#endregion

		/// <summary>
		/// Pass the application launch event to this method.
		/// </summary>
		public async void Launch(Type startPageType, ApplicationExecutionState previousExecutionState, string args) {
			Frame rootFrame = Window.Current.Content as Frame;

			// Do not repeat app initialization when the Window already has content,
			// just ensure that the window is active
			if (rootFrame == null) {
				// Create a Frame to act as the navigation context and navigate to the first page
				rootFrame = new Frame();
				// Set the default language
				rootFrame.Language = Windows.Globalization.ApplicationLanguages.Languages[0];

				rootFrame.NavigationFailed += OnNavigationFailed;

				if (previousExecutionState == ApplicationExecutionState.Terminated) {
					await LoadingState.InvokeAsync(this, CancellationToken.None);
				}

				// Place the frame in the current Window
				Window.Current.Content = rootFrame;
			}

			RootFrameCreated?.Invoke(this, new EventArgs<Frame>(rootFrame));

			if (rootFrame.Content == null) {
				// When the navigation stack isn't restored navigate to the first page,
				// configuring the new page by passing required information as a navigation
				// parameter
				rootFrame.Navigate(startPageType, args);
			}
			// Ensure the current window is active
			Window.Current.Activate();
		}

		#region Event Handlers

		async void OnSuspending(object sender, SuspendingEventArgs e) {
			var deferral = e.SuspendingOperation.GetDeferral();

			try {
				await SavingState.InvokeAsync(this, CancellationToken.None);
			} finally {
				deferral.Complete();
			}
		}

		void OnNavigationFailed(object sender, NavigationFailedEventArgs e) {
			throw new Exception($"Failed to load Page '{e.SourcePageType.FullName}'");
		}

		#endregion
	}
}

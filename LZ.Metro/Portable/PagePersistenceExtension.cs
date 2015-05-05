using LZ.EventHandling;
using LZ.Soft;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace LZ.Metro
{
	/// <summary>
	/// Injects functionality to persist page state.
	/// </summary>
	public class PagePersistenceExtension
	{
		#region Fields

		private readonly IPersistence persistence;

		#endregion

		#region Constructor

		public PagePersistenceExtension(IPersistence persistence, ApplicationLifeCycleManager appManager)
		{
			if (persistence == null) throw new ArgumentNullException("persistence");
			if (appManager == null) throw new ArgumentNullException("appManager");
			this.persistence = persistence;

			appManager.LoadingState += appManager_LoadingState;
			appManager.SavingState += appManager_SavingState;
			appManager.RootFrameCreated += appManager_RootFrameCreated;
		}

		#endregion

		#region Event Handlers

		void appManager_RootFrameCreated(object sender, IEventArgs<Frame> e)
		{
			e.Args.Navigated += RootFrame_Navigated;
			e.Args.Navigating += RootFrame_Navigating;
		}

		async void appManager_SavingState(object sender, ITaskCompletionEventArgs e)
		{
			try
			{
				await persistence.SaveAsync();
				e.Complete();
			}
			catch (Exception ex)
			{
				e.SetException(ex);
			}
		}

		async void appManager_LoadingState(object sender, ITaskCompletionEventArgs e)
		{
			try
			{
				await persistence.LoadAsync();
				e.Complete();
			}
			catch (Exception ex)
			{
				e.SetException(ex);
			}
		}

		// Navigated to
		void RootFrame_Navigated(object sender, NavigationEventArgs e)
		{
			e.Content.DoAs<ISerializable>(s => s.Deserialize(persistence));
		}

		// Navigating from
		void RootFrame_Navigating(object sender, NavigatingCancelEventArgs e)
		{
			var frame = (Frame)sender;
			frame.Content.DoAs<ISerializable>(s => s.Serialize(persistence));
		}

		#endregion
	}
}

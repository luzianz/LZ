using Microsoft.Xaml.Interactivity;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;

namespace LZ.Interactivity
{
	[ContentProperty(Name = "Actions")]
	public class Trigger<AssociatedType> : Behavior<AssociatedType> where AssociatedType : FrameworkElement
	{
		#region Fields

		private Lazy<ObservableCollection<IAction>> _ActionsLazy;

		#endregion

		#region Properties

		public ObservableCollection<IAction> Actions
		{
			get { return _ActionsLazy.Value; }
		}

		#endregion

		#region Constructor

		public Trigger()
		{
			_ActionsLazy = new Lazy<ObservableCollection<IAction>>(InitializeActions);
		}

		#endregion

		#region Behavior<AssociatedType>

		protected override void OnAttached()
		{
			base.OnAttached();
			this.DataContextChanged += Trigger_DataContextChanged;
			AssociatedObject.DataContextChanged += AssociatedObject_DataContextChanged;
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();
			AssociatedObject.DataContextChanged -= AssociatedObject_DataContextChanged;
			this.DataContextChanged -= Trigger_DataContextChanged;
		}

		#endregion

		#region Event Handlers

		void AssociatedObject_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
		{
			this.BindTo(AssociatedObject);
		}

		void Trigger_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
		{
			foreach (var el in Actions.OfType<FrameworkElement>())
			{
				el.BindTo(this);
			}
		}

		void Actions_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.OldItems != null)
			{
				foreach (var fe in e.OldItems.OfType<FrameworkElement>()) fe.Unbind();
			}
			if (e.NewItems != null)
			{
				foreach (var fe in e.NewItems.OfType<FrameworkElement>()) fe.BindTo(this);
			}
		}

		#endregion

		protected void ExecuteActions(object parameter)
		{
			foreach (IAction action in Actions) action.Execute(AssociatedObject, parameter);
		}

		private ObservableCollection<IAction> InitializeActions()
		{
			var actions = new ObservableCollection<IAction>();
			actions.CollectionChanged += Actions_CollectionChanged;
			return actions;
		}
	}
}

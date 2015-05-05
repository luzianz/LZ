using LZ.Interactivity;
using System.Collections;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LZ.Interactions
{
	public class SearchSuggestionsBehavior : Behavior<SearchBox>
	{
		private string previousQueryText;

		#region Dependency Properties
		public static readonly DependencyProperty CollectionProperty = DependencyProperty.Register("Collection", typeof(object), typeof(SearchSuggestionsBehavior), new PropertyMetadata(null));
		/// <summary>A collection containing all items that can be searched</summary>
		public object Collection
		{
			get { return base.GetValue(CollectionProperty); }
			set { base.SetValue(CollectionProperty, value); }
		}

		public static readonly DependencyProperty QueryActivationLengthProperty = DependencyProperty.Register("QueryActivationLength", typeof(int), typeof(SearchSuggestionsBehavior), new PropertyMetadata(3));
		/// <summary>Minimum number of characters required for query to activate.</summary>
		public int QueryActivationLength
		{
			get { return (int)GetValue(QueryActivationLengthProperty); }
			set { SetValue(QueryActivationLengthProperty, value); }
		}
		#endregion

		#region Event Handlers
		void SuggestionsRequested(SearchBox sender, SearchBoxSuggestionsRequestedEventArgs args)
		{
			// Accept only what can be enumerated.
			var collection = Collection as IEnumerable;
			if (collection == null)
			{
				return;
			}

			if (string.IsNullOrWhiteSpace(args.QueryText)
				// ignore if query isn't long enough
				|| args.QueryText.Length < QueryActivationLength
				// ignore if what's entered is the same as what was previously
				|| (previousQueryText != null && args.QueryText == previousQueryText)) return;

			previousQueryText = args.QueryText;

			var suggestions = new List<string>();

			foreach(object item in collection)
			{
				string itemString = item.ToString().ToLower();
				if (itemString.Contains(args.QueryText)) suggestions.Add(itemString);
			}

			args.Request.SearchSuggestionCollection.AppendQuerySuggestions(suggestions);
		}
		#endregion

		#region Behavior<SearchBox>
		protected override void OnAttached()
		{
			AssociatedObject.SuggestionsRequested += this.SuggestionsRequested;
		}
		protected override void OnDetaching()
		{
			AssociatedObject.SuggestionsRequested -= this.SuggestionsRequested;
		}
		#endregion

	}
}

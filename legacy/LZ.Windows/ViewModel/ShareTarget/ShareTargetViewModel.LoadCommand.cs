using LZ.Commanding;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml.Media.Imaging;
using System.Threading;

namespace LZ.Windows.ViewModel {

	public partial class ShareTargetViewModel {

		private class LoadCommand : AsyncCommand {

			#region Fields

			private readonly ILoadCommandContext context;

			#endregion

			#region Constructor

			public LoadCommand(ILoadCommandContext context) {
				this.context = context;
			}

			#endregion

			#region AsyncCommand
			
			protected override async Task ExecuteAsync(object parameter, CancellationToken cancellationToken) {
				using (var reg = cancellationToken.Register(() => context.ShareOperation.ReportError("cancelled"))) {
					try {
						var bmpImgSrc = new BitmapImage();
						var data = context.ShareOperation.Data;

						context.Description = data.Properties.Description;
						context.Title = data.Properties.Title;
						context.SourceApplicationName = data.Properties.ApplicationName;
						context.Thumbnail = bmpImgSrc;

						if (data.Properties.Thumbnail != null) {
							var stream = await data.Properties.Thumbnail.OpenReadAsync();
							bmpImgSrc.SetSource(stream);
						}

						if (data.Contains(StandardDataFormats.Text)) {
							context.TextContent = await data.GetTextAsync(StandardDataFormats.Text);
						}
						context.ShareOperation.ReportDataRetrieved();
					} catch (Exception) {
						// TODO: test scenarios that throw exceptions
						context.ShareOperation.ReportError("unknown error");
					}
				}
			}

			#endregion
		}
	}
}

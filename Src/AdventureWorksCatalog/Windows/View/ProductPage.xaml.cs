using AdventureWorksCatalog.ViewModel;
using AdventureWorksCatalog.ViewModel.Messages;
using AdventureWorksCatalog.Windows.ViewModel.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace AdventureWorksCatalog.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ProductPage : Page
    {
        public ProductPageViewModel ViewModel
        {
            get
            {
                if (DataContext == null)
                {
                    DataContext = new ProductPageViewModel();
                }
                return DataContext as ProductPageViewModel;
            }
        }

        public ProductPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var productId = (int)e.Parameter;
            ViewModel.Messenger.Subscribe<NavigateMessage>(MessageHandler.NavigateMessage);
            await ViewModel.LoadAsync(productId);

            _DataTransferManager = DataTransferManager.GetForCurrentView();
            _DataTransferManager.DataRequested += OnDataRequested;
            _DataTransferManager.TargetApplicationChosen += TargetApplicationChosen;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ViewModel.Messenger.Unsubscribe<NavigateMessage>(MessageHandler.NavigateMessage);
            _DataTransferManager.DataRequested -= OnDataRequested;
            _DataTransferManager.TargetApplicationChosen -= TargetApplicationChosen;
        }

        private DataTransferManager _DataTransferManager;
        private DataPackage _RequestData;

        private void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            if (ViewModel.Product != null)
            {
                _RequestData = args.Request.Data;
                _RequestData.Properties.Title = ViewModel.GetTitleToShare();
                _RequestData.Properties.Description = ViewModel.Product.Description;
                _RequestData.SetText(ViewModel.Product.Description);

                var imageStreamRef = RandomAccessStreamReference.CreateFromUri(new Uri(new Uri("ms-appx:///Data/"), ViewModel.Product.PhotoPath));
                if (imageStreamRef != null)
                {
                    _RequestData.Properties.Thumbnail = imageStreamRef;
                    _RequestData.SetBitmap(imageStreamRef);
                }

                var htmlToShare = ViewModel.GetHtmlToShare();
                _RequestData.SetHtmlFormat(HtmlFormatHelper.CreateHtmlFormat(htmlToShare));
            }
            else
            {
                args.Request.FailWithDisplayText("Select the item you want to share and try again.");
            }
        }

        private void TargetApplicationChosen(DataTransferManager sender, TargetApplicationChosenEventArgs args)
        {
            try
            {
                if (!(args.ApplicationName == "Email" || args.ApplicationName == "Mail"))
                {
                    _RequestData.SetWebLink(ViewModel.GetUriToShare());
                }
            }
            catch { }
        }
    }
}

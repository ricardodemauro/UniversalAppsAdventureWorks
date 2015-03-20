using AdventureWorksCatalog.Common;
using AdventureWorksCatalog.Portable.Model;
using AdventureWorksCatalog.ViewModel;
using AdventureWorksCatalog.ViewModel.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.Input;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace AdventureWorksCatalog.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : SharePage
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public HomePage()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
        }

        public HomePageViewModel ViewModel
        {
            get
            {
                if (DataContext == null)
                {
                    DataContext = new HomePageViewModel();
                }
                return DataContext as HomePageViewModel;
            }
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.Messenger.Subscribe<NavigateMessage>(MessageHandler.NavigateMessage);
            await ViewModel.LoadAsync();

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ViewModel.Messenger.Unsubscribe<NavigateMessage>(MessageHandler.NavigateMessage);

            base.OnNavigatedFrom(e);
        }

        private void Product_Click(object sender, ItemClickEventArgs e)
        {
            var product = (Product)e.ClickedItem;
            ViewModel.NavigateToProductCommand.Execute(product.Id);
        }

        protected override bool GetShareContent(DataRequest request)
        {
            bool succeeded = false;

            string dataPackageText = "Share menu";
            if (!String.IsNullOrEmpty(dataPackageText))
            {
                DataPackage requestData = request.Data;
                requestData.Properties.Title = "Share menu Title";
                requestData.Properties.Description = "Share menu Description"; // The description is optional.
                requestData.Properties.ContentSourceApplicationLink = ApplicationLink;
                requestData.SetText(dataPackageText);
                succeeded = true;
            }
            else
            {
                request.FailWithDisplayText("Enter the text you would like to share and try again.");
            }
            return succeeded;
        }

        private void MenuFlyoutItemShare_Click(object sender, RoutedEventArgs e)
        {
            // If the user clicks the share button, invoke the share flow programatically.
            DataTransferManager.ShowShareUI();
        }

        private void GridProducts_Holding(object sender, HoldingRoutedEventArgs args)
        {
            // this event is fired multiple times. We do not want to show the menu twice
            if (args.HoldingState != HoldingState.Started) return;

            FrameworkElement element = sender as FrameworkElement;
            if (element == null) return;

            // If the menu was attached properly, we just need to call this handy method
            FlyoutBase.ShowAttachedFlyout(element);
        }
    }
}

using AdventureWorksCatalog.ViewModel;
using AdventureWorksCatalog.ViewModel.Messages;
using AdventureWorksCatalog.Windows.ViewModel.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class HomePage : Page
    {
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


        public HomePage()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
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

        }
    }
}

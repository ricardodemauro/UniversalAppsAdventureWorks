using AdventureWorksCatalog.Portable.Model;
using AdventureWorksCatalog.ViewModel;
using AdventureWorksCatalog.ViewModel.Messages;
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
    public sealed partial class CategoryPage : Page
    {
        public CategoryPageViewModel ViewModel
        {
            get
            {
                if (DataContext == null)
                {
                    DataContext = new CategoryPageViewModel();
                }
                return DataContext as CategoryPageViewModel;
            }
        }

        public CategoryPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var categoryId = (int)e.Parameter;
            ViewModel.Messenger.Subscribe<NavigateMessage>(MessageHandler.NavigateMessage);
            await ViewModel.LoadAsync(categoryId);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ViewModel.Messenger.Unsubscribe<NavigateMessage>(MessageHandler.NavigateMessage);
        }

        private void Product_Click(object sender, ItemClickEventArgs e)
        {
            var product = (Product)e.ClickedItem;
            ViewModel.NavigateToProductCommand.Execute(product.Id);
        }
    }
}

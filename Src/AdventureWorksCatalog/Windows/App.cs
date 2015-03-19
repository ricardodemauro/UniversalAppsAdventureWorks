using AdventureWorksCatalog.Portable.DataSources;
using AdventureWorksCatalog.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AdventureWorksCatalog
{
    public partial class App
    {
        protected async override void OnSearchActivated(SearchActivatedEventArgs args)
        {
            await DataSource.Instance.LoadAsync();

            var previousContent = Window.Current.Content;
            var frame = previousContent as Frame;

            if (frame == null)
            {
                frame = new Frame();
                frame.Navigate(typeof(HomePage));
            }

            if (frame.CurrentSourcePageType == typeof(SearchPage))
            {
                var searchPage = frame.Content as SearchPage;
                await searchPage.ViewModel.LoadAsync(args.QueryText);
            }
            else
            {
                frame.Navigate(typeof(SearchPage), args.QueryText);
            }

            Window.Current.Content = frame;
            Window.Current.Activate();
        }

    }
}

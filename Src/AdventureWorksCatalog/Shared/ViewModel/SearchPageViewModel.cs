using AdventureWorksCatalog.Portable.DataSources;
using AdventureWorksCatalog.Portable.Model;
using AdventureWorksCatalog.ViewModel.Commands;
using AdventureWorksCatalog.ViewModel.Messages;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AdventureWorksCatalog.ViewModel
{
    public class SearchPageViewModel : ViewModelBase
    {
        public ICommand NavigateToCategoryCommand { get; private set; }
        public ICommand NavigateHomeCommand { get; private set; }
        public ICommand NavigateToProductCommand { get; private set; }

        private string _Query;
        public string Query
        {
            get { return _Query; }
            set { SetProperty(ref this._Query, value); }
        }

        private Company _Company;
        public Company Company
        {
            get { return _Company; }
            set { SetProperty(ref this._Company, value); }
        }

        private ObservableCollection<Category> _Categories;
        public ObservableCollection<Category> Categories
        {
            get { return _Categories; }
            set { SetProperty(ref this._Categories, value); }
        }

        public SearchPageViewModel()
        {
            NavigateHomeCommand = new DelegateCommand(OnNavigateHomeCommand);
            NavigateToCategoryCommand = new DelegateCommand(OnNavigateToCategoryCommand);
            NavigateToProductCommand = new DelegateCommand(OnNavigateToProductCommand);
        }

        private void OnNavigateHomeCommand(object parameter)
        {
            PublishMessage(new NavigateMessage("HomePage", parameter));
        }

        private void OnNavigateToProductCommand(object parameter)
        {
            PublishMessage(new NavigateMessage("ProductPage", parameter));
        }

        private void OnNavigateToCategoryCommand(object parameter)
        {
            PublishMessage(new NavigateMessage("CategoryPage", parameter));
        }

        public async Task LoadAsync(string query)
        {
            Query = query;
            var categories = await DataSource.Instance.SearchCategoriesAndItemsAsync(query);
            Categories = new ObservableCollection<Category>();
            foreach (var category in categories)
            {
                Categories.Add(category);
            }
            Company = await DataSource.Instance.GetCompanyAsync();
        }
    }
}

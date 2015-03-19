using AdventureWorksCatalog.Portable.Model;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using AdventureWorksCatalog.ViewModel.Commands;
using AdventureWorksCatalog.ViewModel.Messages;
using AdventureWorksCatalog.Portable.DataSources;

namespace AdventureWorksCatalog.ViewModel
{
    public class HomePageViewModel : ViewModelBase
    {
        public ICommand NavigateToCategoryCommand { get; private set; }
        public ICommand NavigateToProductCommand { get; private set; }

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

        public HomePageViewModel()
        {
            NavigateToCategoryCommand = new DelegateCommand(OnNavigateToCategoryCommand);
            NavigateToProductCommand = new DelegateCommand(OnNavigateToProductCommand);
        }

        private void OnNavigateToProductCommand(object parameter)
        {
            PublishMessage(new NavigateMessage("ProductPage", parameter));
        }

        private void OnNavigateToCategoryCommand(object parameter)
        {
            PublishMessage(new NavigateMessage("CategoryPage", parameter));
        }

        public async Task LoadAsync()
        {
            var categories = await DataSource.Instance.GetCategoriesAndItemsAsync(4);
            Categories = new ObservableCollection<Category>();
            foreach (var category in categories)
            {
                Categories.Add(category);
            }
            Company = await DataSource.Instance.GetCompanyAsync();
        }
    }
}

using AdventureWorksCatalog.Portable.Model;
using System.Threading.Tasks;
using System.Windows.Input;
using AdventureWorksCatalog.ViewModel.Commands;
using AdventureWorksCatalog.ViewModel.Messages;
using AdventureWorksCatalog.Portable.DataSources;

namespace AdventureWorksCatalog.ViewModel
{
    public class CategoryPageViewModel : ViewModelBase
    {
        public ICommand NavigateHomeCommand { get; private set; }
        public ICommand NavigateToProductCommand { get; private set; }

        private Company _Company;
        public Company Company
        {
            get { return _Company; }
            set { SetProperty(ref this._Company, value); }
        }

        private Category _Category;
        public Category Category
        {
            get { return _Category; }
            set { SetProperty(ref this._Category, value); }
        }

        public CategoryPageViewModel()
        {
            NavigateHomeCommand = new DelegateCommand(OnNavigateHomeCommand);
            NavigateToProductCommand = new DelegateCommand(OnNavigateToProductCommand);
        }

        private void OnNavigateToProductCommand(object parameter)
        {
            PublishMessage(new NavigateMessage("ProductPage", parameter));
        }

        private void OnNavigateHomeCommand(object parameter)
        {
            PublishMessage(new NavigateMessage("HomePage", parameter));
        }

        public async Task LoadAsync(int categoryId)
        {
            Category = await DataSource.Instance.GetCategoryAsync(categoryId);
            Company = await DataSource.Instance.GetCompanyAsync();
        }
    }
}

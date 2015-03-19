using AdventureWorksCatalog.Portable.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdventureWorksCatalog.Portable.DataSources
{
    public abstract class DataSource
    {
        private static DataSource _Instance;
        public static DataSource Instance
        {
            get
            {
                return _Instance;
            }
        }

        public static void Initialize(DataSource instance)
        {
            _Instance = instance;
        }

        private Dictionary<int, Product> _Products;
        private Company _Company;
        private Dictionary<int, Category> _Categories;

        public async Task<List<Category>> GetCategoriesAndItemsAsync(int? maxItemsPerCategory = null)
        {
            await LoadAsync();

            return (from category in _Categories.Values
                    select new Category()
                    {
                        Id = category.Id,
                        Name = category.Name,
                        Products = new ObservableCollection<Product>(category.Products.Take(maxItemsPerCategory.GetValueOrDefault(int.MaxValue))),
                    }
                    ).ToList();
        }

        public async Task<List<Category>> SearchCategoriesAndItemsAsync(string query)
        {
            await LoadAsync();

            var words = query.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            var result = new List<Category>();
            foreach (var category in _Categories.Values)
            {
                Category searchCategory = null;
                bool found;

                foreach (Product product in category.Products)
                {
                    found = false;

                    if (words.All((w) => product.Name.IndexOf(w, StringComparison.CurrentCultureIgnoreCase) > -1)
                        || words.All((w) => category.Name.IndexOf(w, StringComparison.CurrentCultureIgnoreCase) > -1)
                    )
                    {
                        found = true;
                    }

                    if (found)
                    {
                        if (searchCategory == null)
                        {
                            searchCategory = new Category()
                            {
                                Id = category.Id,
                                Name = category.Name,
                                Products = new ObservableCollection<Product>(),
                            };
                        }
                        searchCategory.Products.Add(product);
                    }
                }

                if (searchCategory != null)
                {
                    result.Add(searchCategory);
                }
            }
            return result;
        }

        public async Task LoadAsync()
        {
            if (_Categories == null)
            {
                var categories = await InternalLoadAllCategoriesAndProductsAsync();

                foreach (var category in categories)
                {
                    if (category.Products == null)
                    {
                        category.Products = new ObservableCollection<Product>();
                    }
                    foreach (var product in category.Products)
                    {
                        product.Category = category;
                    }
                }

                _Categories = categories.ToDictionary((c) => c.Id);
                _Products = categories.SelectMany((c) => c.Products).ToDictionary((i) => i.Id);
            }
            if (_Company == null)
            {
                _Company = await InternalLoadCompanyAsync();
            }
        }

        public async Task<Company> GetCompanyAsync()
        {
            await LoadAsync();
            return _Company;
        }

        public async Task<Product> GetProductAsync(int productId)
        {
            await LoadAsync();
            if (_Products.ContainsKey(productId))
            {
                return _Products[productId];
            }
            return null;
        }

        public async Task<Category> GetCategoryAsync(int categoryId)
        {
            await LoadAsync();
            if (_Categories.ContainsKey(categoryId))
            {
                return _Categories[categoryId];
            }
            return null;
        }

        private async Task<List<Category>> InternalLoadAllCategoriesAndProductsAsync()
        {
            var result = new List<Category>();
            var categoryFilePaths = await ListFiles(@"Data\Categories\", FilePathKind.DataFolder);
            if (categoryFilePaths.Count == 0)
            {
                categoryFilePaths = await ListFiles(@"Data\Categories\", FilePathKind.InstalledFolder);
            }
            foreach (var categoryFilePath in categoryFilePaths)
            {
                using (var streamReader = new StreamReader(await OpenFileReadAsync(categoryFilePath, FilePathKind.AbsolutePath)))
                {
                    var json = await streamReader.ReadToEndAsync();
                    var category = JsonConvert.DeserializeObject<Category>(json);
                    result.Add(category);
                }
            }
            return result;
        }

        private async Task<Company> InternalLoadCompanyAsync()
        {
            var stream = await OpenFileReadAsync(@"Data\Company.json", FilePathKind.DataFolder);
            if (stream == null)
            {
                stream = await OpenFileReadAsync(@"Data\Company.json", FilePathKind.InstalledFolder);
            }
            using (var streamReader = new StreamReader(stream))
            {
                var json = await streamReader.ReadToEndAsync();
                var company = JsonConvert.DeserializeObject<Company>(json);
                return company;
            }
        }

        public enum FilePathKind
        {
            InstalledFolder,
            DataFolder,
            AbsolutePath,
        }

        protected abstract Task<List<string>> ListFiles(string directoryPath, FilePathKind filePathKind);
        protected abstract Task<Stream> OpenFileReadAsync(string filePath, FilePathKind filePathKind);

        #region Azure Mobile Services
        protected abstract Task<Stream> OpenFileWriteAsync(string filePath, FilePathKind filePathKind);
        protected abstract Task DeleteFileAsync(string categoryFilePath, FilePathKind filePathKind);
        public abstract Task<DateTimeOffset?> GetFileModifiedDateAsync(string filePath, FilePathKind filePathKind);

        public async Task SaveCompanyAsync(Company company)
        {
            using (var streamWriter = new StreamWriter(await OpenFileWriteAsync(@"Data\Company.json", FilePathKind.DataFolder)))
            {
                var json = JsonConvert.SerializeObject(company, Formatting.Indented);
                await streamWriter.WriteAsync(json);
                await streamWriter.FlushAsync();
            }
        }

        public async Task SaveCategoriesAsync(List<Category> categories)
        {
            var categoryFilePaths = await ListFiles(@"Data\Categories\", FilePathKind.DataFolder);
            foreach (var categoryFilePath in categoryFilePaths)
            {
                await DeleteFileAsync(categoryFilePath, FilePathKind.AbsolutePath);
            }
            foreach (var category in categories)
            {
                var categoryFilePath = string.Format(@"Data\Categories\{0}.json", category.Name);
                using (var streamWriter = new StreamWriter(await OpenFileWriteAsync(categoryFilePath, FilePathKind.DataFolder)))
                {
                    var json = JsonConvert.SerializeObject(category, Formatting.Indented);
                    await streamWriter.WriteAsync(json);
                    await streamWriter.FlushAsync();
                }
            }
        }

        public async Task SaveFileAsync(string filePath, Stream stream)
        {
            filePath = Path.Combine("Data", filePath).Replace("/", @"\");
            using (var writingStream = await OpenFileWriteAsync(filePath, FilePathKind.DataFolder))
            {
                await stream.CopyToAsync(writingStream);
                await writingStream.FlushAsync();
            }
        }

        public Task<DateTimeOffset?> GetFileModifiedDateAsync(string filePath)
        {
            filePath = Path.Combine("Data", filePath).Replace("/", @"\");
            return GetFileModifiedDateAsync(filePath, FilePathKind.DataFolder);
        }

        #endregion
    }
}

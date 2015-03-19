using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace AdventureWorksCatalog.Portable.Model
{
    [DataContract]
    public class Category : ModelBase
    {
        private int _Id;
        [DataMember]
        public int Id
        {
            get { return _Id; }
            set { SetProperty(ref _Id, value); }
        }

        private string _Name;
        [DataMember]
        public string Name
        {
            get { return _Name; }
            set { SetProperty(ref _Name, value); }
        }

        private ObservableCollection<Product> _Products;
        [DataMember]
        public ObservableCollection<Product> Products
        {
            get { return _Products; }
            set { SetProperty(ref _Products, value); }
        }
    }
}

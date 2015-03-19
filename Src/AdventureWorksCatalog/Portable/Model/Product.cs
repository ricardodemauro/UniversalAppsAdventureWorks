using System.Runtime.Serialization;

namespace AdventureWorksCatalog.Portable.Model
{
    [DataContract]
    public class Product : ModelBase
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

        private string _ProductNumber;
        [DataMember]
        public string ProductNumber
        {
            get { return _ProductNumber; }
            set { SetProperty(ref _ProductNumber, value); }
        }

        private string _ThumbnailPath;
        [DataMember]
        public string ThumbnailPath
        {
            get { return _ThumbnailPath; }
            set { SetProperty(ref _ThumbnailPath, value); }
        }

        private string _PhotoPath;
        [DataMember]
        public string PhotoPath
        {
            get { return _PhotoPath; }
            set { SetProperty(ref _PhotoPath, value); }
        }

        private string _Description;
        [DataMember]
        public string Description
        {
            get { return _Description; }
            set { SetProperty(ref _Description, value); }
        }

        private string _ProductUrl;
        [DataMember]
        public string ProductUrl
        {
            get { return _ProductUrl; }
            set { SetProperty(ref _ProductUrl, value); }
        }

        private int _CategoryId;
        [DataMember]
        public int CategoryId
        {
            get { return _CategoryId; }
            set { SetProperty(ref _CategoryId, value); }
        }

        private double _Price;
        [DataMember]
        public double Price
        {
            get { return _Price; }
            set { SetProperty(ref _Price, value); }
        }

        private Category _Category;
        [IgnoreDataMember]
        public Category Category
        {
            get { return _Category; }
            set { SetProperty(ref _Category, value); }
        }
    }
}

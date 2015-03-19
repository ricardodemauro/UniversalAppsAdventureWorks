using System.Runtime.Serialization;

namespace AdventureWorksCatalog.Portable.Model
{
    [DataContract]
    public class Company : ModelBase
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

        private string _Address;
        [DataMember]
        public string Address
        {
            get { return _Address; }
            set { SetProperty(ref _Address, value); }
        }

        private string _Telephone;
        [DataMember]
        public string Telephone
        {
            get { return _Telephone; }
            set { SetProperty(ref _Telephone, value); }
        }

        private string _Website;
        [DataMember]
        public string Website
        {
            get { return _Website; }
            set { SetProperty(ref _Website, value); }
        }

        private string _ContactEmail;
        [DataMember]
        public string ContactEmail
        {
            get { return _ContactEmail; }
            set { SetProperty(ref _ContactEmail, value); }
        }

        private string _DeveloperName;
        [DataMember]
        public string DeveloperName
        {
            get { return _DeveloperName; }
            set { SetProperty(ref _DeveloperName, value); }
        }

        private string _DeveloperEmail;
        [DataMember]
        public string DeveloperEmail
        {
            get { return _DeveloperEmail; }
            set { SetProperty(ref _DeveloperEmail, value); }
        }

        private string _PrivacyPolicy;
        [DataMember]
        public string PrivacyPolicy
        {
            get { return _PrivacyPolicy; }
            set { SetProperty(ref _PrivacyPolicy, value); }
        }

        private string _LogoPath;
        [DataMember]
        public string LogoPath
        {
            get { return _LogoPath; }
            set { SetProperty(ref _LogoPath, value); }
        }

        private string _BackgroundPath;
        [DataMember]
        public string BackgroundPath
        {
            get { return _BackgroundPath; }
            set { SetProperty(ref _BackgroundPath, value); }
        }

        private string _ImagePath;
        [DataMember]
        public string ImagePath
        {
            get { return _ImagePath; }
            set { SetProperty(ref _ImagePath, value); }
        }
    }
}

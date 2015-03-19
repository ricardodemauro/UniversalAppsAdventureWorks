using AdventureWorksCatalog.Portable.Model;
using AdventureWorksCatalog.ViewModel.Commands;
using AdventureWorksCatalog.ViewModel.Messages;
using System.Windows.Input;

namespace AdventureWorksCatalog.ViewModel
{
    public class ViewModelBase : ModelBase
    {
        public ICommand NavigateBackCommand { get; private set; }

        public ViewModelBase()
        {
            Subscribe();
            NavigateBackCommand = new DelegateCommand(OnNavigateBackCommand);
        }

        private void OnNavigateBackCommand(object obj)
        {
            PublishMessage(NavigateMessage.Back);
        }

        public Messenger Messenger
        {
            get { return Messenger.Instance; }
        }

        protected virtual void Subscribe()
        {
        }

        protected void PublishMessage<TMessage>(TMessage message)
        {
            Messenger.Publish<TMessage>(message);
        }
    }
}

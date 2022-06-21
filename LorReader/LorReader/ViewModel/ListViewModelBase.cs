using System.Collections.ObjectModel;
using ReactiveUI.Fody.Helpers;
using Xamarin.Forms;

namespace LorReader.ViewModel
{
    public abstract class ListViewModelBase<T> : BaseViewModel
    {
        public ListViewModelBase(INavigation navigation) : base(navigation)
        {
            Data = new ObservableCollection<T>();
        }

        [Reactive] public T SelectedItem { get; set; }

        [Reactive] public ObservableCollection<T> Data { get; set; }

        public abstract void Load();
    }
}
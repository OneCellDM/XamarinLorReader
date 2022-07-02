using System.Collections.ObjectModel;
using System.Threading.Tasks;

using OneCellDM.Collections;

using ReactiveUI.Fody.Helpers;
using Xamarin.Forms;

namespace LorReader.ViewModel
{
    public abstract class ListViewModelBase<T> : BaseViewModel
    {
        public ListViewModelBase(INavigation navigation) : base(navigation)
        {
            Data = new ConcurrentObservableCollection<T>();
        }

        [Reactive] public T SelectedItem { get; set; }

        [Reactive] public ConcurrentObservableCollection<T> Data { get; set; }

        public abstract void Load();
    }
}
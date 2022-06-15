using ReactiveUI;
using ReactiveUI.Fody.Helpers;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

using Xamarin.Forms;

namespace LorReader.ViewModel
{
    public abstract class ListViewModelBase <T> : BaseViewModel
    {
        [Reactive]
        public T SelectedItem { get; set; }

        [Reactive]
        public ObservableCollection<T> Data { get; set; }
        public ListViewModelBase(INavigation navigation):base(navigation)
        {
            Data = new ObservableCollection<T>();
        }

        public abstract void Load();
    }
}

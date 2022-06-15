using ReactiveUI;

using System;
using System.Collections.Generic;
using System.Text;

using Xamarin.Forms;

namespace LorReader.ViewModel
{
    public class BaseViewModel:ReactiveObject
    {
        public INavigation Navigation { get; set; }
        public BaseViewModel(INavigation navigation)        {
            Navigation = navigation;
        }

    }
}

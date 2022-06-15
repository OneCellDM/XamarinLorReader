using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Xamarin.Forms;

namespace LorReader.ViewModel
{
    public class ReadNewsViewModel : ViewModel.BaseViewModel
    {
        public ReadNewsViewModel(INavigation navigation, string url) : base(navigation)
        {
            Debug.WriteLine(url);
        }
    }
}

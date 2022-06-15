using LorReader.ViewModel;

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LorReader.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewsPage : ContentPage
    {
       

        public NewsPage()
        {
            InitializeComponent();
            this.BindingContext = new NewsViewModel(this.Navigation);

        }

       
    }
}

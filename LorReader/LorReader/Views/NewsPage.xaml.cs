using LorReader.ViewModel;
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
            BindingContext = new NewsViewModel(Navigation);
        }
    }
}
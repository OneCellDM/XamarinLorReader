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

            var context= new NewsViewModel(Navigation);

            BindingContext = context;

            NewsCollectionView.Scrolled += context.CollectionView_Scrolled;
           
        }

        
    }
}
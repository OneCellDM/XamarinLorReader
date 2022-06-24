using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LorReader.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CommentariesView : ContentView
    {
        public CommentariesView()
        {
            InitializeComponent();
        }

        private void CollectionView_Scrolled(object sender, ItemsViewScrolledEventArgs e)
        {

        }
    }
}
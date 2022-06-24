using ReactiveUI;
using Xamarin.Forms;

namespace LorReader.ViewModel
{
    public class BaseViewModel : ReactiveObject
    {
        public BaseViewModel(INavigation navigation)
        {
            Navigation = navigation;
        }
        

        public INavigation Navigation { get; set; }
    }
}
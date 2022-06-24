using LorParser.Models.Pages;

using LorReader.Views;

using ReactiveUI.Fody.Helpers;

using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace LorReader.ViewModel
{
    public class ReadNewsViewModel : BaseViewModel
    {
        [Reactive]
        public LorParser.Models.FullNews News { get; set; }

        [Reactive]
        public CommentariesView ReadCommentariesView { get; set; }
        
        public ReadNewsViewModel(INavigation navigation, string url) : base(navigation)
        {
            Load(url);
        }
        private async void Load(string url)
        {
            int errcount = 0;
            Exception _ex = null;
            try
            {
                News = await LorParser.LOR.GetNews(url);

            }
            catch(Exception ex) {  _ex = ex; errcount++; }
            try
            {
                ReadCommentariesView = new CommentariesView()
                {
                    BindingContext = new CommentariesViewModel(Navigation, News.commentPage)
                    {
                        IsScrollable = false,
                        IsMain = false,
                    }
                };
            }
            catch (Exception ex) 
            {
                if(_ex!=null) _ex = ex;
                errcount++;
               
            }
            if (errcount == 2)
            {
               await App.Current.MainPage.DisplayActionSheet("Error", _ex?.Message??"Неизвестная ошибка", "cancel");
               await Navigation.PopAsync();
            }
        }
    }
}
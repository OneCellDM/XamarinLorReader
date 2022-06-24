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
            try
            {
                News = await LorParser.LOR.GetNews(url);
            }
            catch(Exception ex)
            {

            }

            ReadCommentariesView = new CommentariesView()
            {
                BindingContext = new ReadCommentariesViewModel(Navigation, News.commentPage)
                {
                    IsScrollable=false
                }
            };
        }
    }
}
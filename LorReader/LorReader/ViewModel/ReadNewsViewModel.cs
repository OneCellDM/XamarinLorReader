using LorParser.Models.Pages;
using ReactiveUI.Fody.Helpers;
using System.Diagnostics;
using Xamarin.Forms;

namespace LorReader.ViewModel
{
    public class ReadNewsViewModel : BaseViewModel
    {
        [Reactive]
        public LorParser.Models.FullNews News { get; set; }
        [Reactive]
        public int CurrentPage { get; set; }
        [Reactive]
        public int LastPage { get; set; }
        [Reactive]
        public CommentPage commentPage { get; set; }
        public ReadNewsViewModel(INavigation navigation, string url) : base(navigation)
        {
            Load(url);
        }
        private async void Load(string url)
        {
            News = await LorParser.LOR.GetNews(url);
            commentPage = News.commentPage;
        }
    }
}
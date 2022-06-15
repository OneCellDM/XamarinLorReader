using LorParser.Models;

using LorReader.Views;

using ReactiveUI;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace LorReader.ViewModel
{
    public class NewsViewModel : ListViewModelBase<NewsPreview>
    {
        public IReactiveCommand OpenNewsCommand { get; set; }
        public IReactiveCommand OpenCommentaries { get; set; }
        
        public NewsViewModel (INavigation navigation) : base(navigation)
        {
            OpenCommentaries = ReactiveCommand.Create((string id) =>
            {
                navigation.PushAsync(new ReadCommentariesPage()
                {
                    BindingContext = new ReadCommentariesViewModel(navigation, id)
                });
            });

            OpenNewsCommand = ReactiveCommand.Create((string Url) =>
            {
                navigation.PushAsync(new ReadNewPage()
                {
                    BindingContext = new ReadNewsViewModel(navigation, Url)
                });
            });

            Load();
        }

        public override void Load()
        {
            Task.Run(async () =>
            {
                var res = await LorParser.LOR.GetNewsList();
                foreach (var item in res.Items)
                {
                    App.Current.Dispatcher.BeginInvokeOnMainThread(() => Data.Add(item));
                }
            });
        }

    }
}

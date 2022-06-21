using System;
using System.Threading.Tasks;
using LorParser;
using LorParser.Models;
using LorReader.Views;
using ReactiveUI;
using Xamarin.Forms;


namespace LorReader.ViewModel
{
    public class NewsViewModel : ListViewModelBase<NewsPreview>
    {
        public IReactiveCommand OpenNewsCommand { get; set; }
        public IReactiveCommand OpenCommentaries { get; set; }

        public NewsViewModel(INavigation navigation) : base(navigation)
        {
            OpenCommentaries = ReactiveCommand.Create((NewsPreview preview) =>
            {
                try
                {
                    navigation.PushAsync(new ReadCommentariesPage
                    {
                        Title = "Комментарии к записи:" + preview.Title,
                        BindingContext = new ReadCommentariesViewModel(navigation, preview.NewsUri)
                    }, true);
                }
                catch(Exception ex)
                {
                 
                }
            });

            OpenNewsCommand = ReactiveCommand.Create((NewsPreview preview) =>
            {
                navigation.PushAsync(new ReadNewPage
                {
                    BindingContext = new ReadNewsViewModel(navigation, preview.NewsUri)
                });
            });

            Load();
        }

       

        public override void Load()
        {
            Task.Run(async () =>
            {
                var res = await LOR.GetNewsList();
                foreach (var item in res.Items)
                    Application.Current.Dispatcher.BeginInvokeOnMainThread(() => Data.Add(item));
            });
        }
    }
}
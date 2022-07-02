using System;
using System.Diagnostics;
using System.Threading.Tasks;
using LorParser;
using LorParser.Models;
using LorReader.Views;
using ReactiveUI;
using Xamarin.Forms;
using System.Linq;
using System.Linq.Expressions;

namespace LorReader.ViewModel
{
    public class NewsViewModel : ListViewModelBase<NewsPreview>
    {
        public IReactiveCommand OpenNewsCommand { get; set; }
        public IReactiveCommand OpenCommentaries { get; set; }
        public IReactiveCommand ScrollCommand { get; set; }
        bool _loading { get; set; }
        bool EndData { get; set; }
        public void CollectionView_Scrolled(object sender, ItemsViewScrolledEventArgs e)
        {
          
                if (EndData is false && 
                    _loading is false && 
                    Data.Count > 0 && 
                    e.LastVisibleItemIndex == Data.Count - 1)
                {
                     Load();
                }
            
        }
        public NewsViewModel(INavigation navigation) : base(navigation)
        {
            
            OpenCommentaries = ReactiveCommand.Create((NewsPreview preview) =>
            {
                
                    navigation.PushAsync(new ReadCommentariesPage()
                    {
                        Title = "Комментарии:" + preview.Title,

                        Content = new CommentariesView()
                        {
                            BindingContext = new CommentariesViewModel(navigation, preview.NewsUri)
                            {
                                IsScrollable = true,
                                IsMain = true

                            }
                        }

                    }, true); ;
                
               
                
                
            });

            OpenNewsCommand = ReactiveCommand.Create((NewsPreview preview) =>
            {
                   navigation.PushAsync(new ReadNewPage
                    {
                        Title = "Чтение:" + preview.Title,
                        BindingContext = new ReadNewsViewModel(navigation, preview.NewsUri)

                    });
               
            });

            Load();
        }

       

        public override void Load()
        {
             if(_loading) return;

            _loading = true;

            Task.Run(async () =>
            {
                try
                {
                    var res = await LOR.GetNewsList(Data.Count);
                    bool IsEnd = false;
                    foreach (var item in res.Items)
                    {
                        if (Data.Select(x => x.NewsUri).Contains(item.NewsUri))
                        {
                            IsEnd = true; 
                        }
                        else
                        {
                            Data.Add(item);
                            IsEnd = false;
                        }
                    }
                    if (IsEnd)
                    {
                        EndData = true;
                    }
                }
                catch(Exception ex)
                {
                    await  App.Current.MainPage.DisplayAlert("Error", ex.Message,"Cancel");
                }
                finally
                {
                   
                    _loading = false;
                }
            });
        }
    }
}
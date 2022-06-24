using LorParser.Models;
using LorParser.Models.Pages;

using ReactiveUI.Fody.Helpers;

using System.Collections.ObjectModel;
using Xamarin.Forms;
using ReactiveUI;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Diagnostics;
using System;
using LorReader.Models;

namespace LorReader.ViewModel
{
    public class CommentariesViewModel : ListViewModelBase<Comment>
    {

        private CommentPage _commentpage;
        public CommentPage CommentPage { get=>_commentpage; set=>this.RaiseAndSetIfChanged(ref _commentpage,value); }

        [Reactive]
        private int currentPage { get; set; } = 0;
        public string Uri { get; private set; }
        [Reactive]
        public List<PageModel> Pages { get; set; }
        [Reactive]
        public PageModel SelectedPage { get; set; }
        [Reactive]
        public bool IsScrollable { get; set; } = false;
        public bool IsMain { get; set; }
        public ScrollBarVisibility ScrollBarVisibility { get=>IsScrollable?ScrollBarVisibility.Always:ScrollBarVisibility.Never;  }
     
        
       public void Subscribe()
        {
            this.WhenAnyValue(x => x.SelectedPage).WhereNotNull()

              .Where(x => x.IsSelected is false && x.Number > 0)
              .Subscribe(value =>
              {
                  currentPage = (SelectedPage.Number - 1);
                  Load();
              });

            this.WhenAnyValue(x => x.CommentPage).WhereNotNull()

                .Where(x => x.PageCount > 0).Subscribe(value =>
                {

                    Pages = Enumerable.Range(1, (value.PageCount)).Select(x =>
                    {
                        var pageTmp = new PageModel(x);

                        if (x == (value.PageNumber))
                            pageTmp.IsSelected = true;

                        return pageTmp;

                    }).ToList();

                });

        }
        public CommentariesViewModel(INavigation navigation, string Uri):base(navigation)
        {

            Subscribe();
            this.Uri = Uri;
            Load();
            
        }
        public CommentariesViewModel(INavigation navigation, CommentPage commentPage):base(navigation)
        {
            Subscribe();
            this.CommentPage = commentPage;
            this.Uri = this.CommentPage.NewsUrl;
            Load();
            
        }

        public override async void Load()
        {
            try
            {
                CommentPage = await LorParser.LOR.GetCommentPage(this.Uri, currentPage);
            }
            catch(Exception ex)
            {
                if (IsMain)
                {
                    await App.Current.MainPage.DisplayActionSheet("Error", ex.Message, "cancel");

                    await Navigation.PopAsync();
                }
                else throw ex;
            }
        }
    }
}
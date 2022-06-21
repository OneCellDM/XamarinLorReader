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

namespace LorReader.ViewModel
{
    public class ReadCommentariesViewModel : ListViewModelBase<Comment>
    {
        public class PageModel:ReactiveObject
        {
            public bool IsSelected { get; set; }
            public int Number { get; set; }
            public PageModel()
            {

            }

            public PageModel(int number)
            {
                this.Number = number;
            }
            public PageModel(int number, bool isSelected = false) : this(number)
            {
                this.IsSelected = isSelected;
            }
        }

        [Reactive]
        public CommentPage CommentPage { get; set; }
        private int currentPage { get => CommentPage?.PageNumber ?? 0; }
        public string Uri { get; private set; }
        [Reactive]
        public List<PageModel> Pages { get; set; }

        public ReadCommentariesViewModel(INavigation navigation) : base(navigation)
        {
   
            this.WhenAnyValue(x => x.CommentPage).WhereNotNull().Subscribe(value =>
            {
                if (value.PageCount > 0)
                {
                    Pages = Enumerable.Range(1, (value.PageCount + 1)).Select(x =>
                    {
                        var pageTmp = new PageModel(x);

                        if (x == (value.PageNumber))
                            pageTmp.IsSelected = true;
                        
                        return pageTmp;
                    }).ToList();
                }
                else Pages = null;
            }); 
        }
     
        
     
        public ReadCommentariesViewModel(INavigation navigation, string Uri) : this(navigation)
        {
            this.Uri = Uri;
            Load();
        }
        public ReadCommentariesViewModel(INavigation navigation, CommentPage commentPage): this(navigation)
        {
            this.CommentPage = commentPage;
        }

        public override async void Load()
        {
          CommentPage = await  LorParser.LOR.GetCommentPage(this.Uri, currentPage);
        }
    }
}
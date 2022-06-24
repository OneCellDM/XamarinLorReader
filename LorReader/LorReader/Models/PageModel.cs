using ReactiveUI;

namespace LorReader.Models
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
    
}
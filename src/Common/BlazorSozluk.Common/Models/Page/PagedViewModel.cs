using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSozluk.Common.Models.Page
{
    // Herhangi bir enttiy'nin sayfalama yapisini kuravagimiz icin generic yapi kurduk
    public class PagedViewModel<T> where T : class
    {
        // Nesne olusturulurken bilgiler gelmezse otomatik olarak bu alanlarin set edilebilmesi icin this(...) icerisinde belirtiyoruz
        public PagedViewModel() : this(new List<T>(), new Page())
        {

        }
        public PagedViewModel(IList<T> results, Page pageInfo)
        {
            Results = results;
            PageInfo = pageInfo;
        }

        public IList<T> Results { get; set; }
        public Page PageInfo { get; set; }
    }
}

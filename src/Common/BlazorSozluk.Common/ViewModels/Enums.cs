using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSozluk.Common.ViewModels
{
    public enum VoteType
    {
        None = -1, // ilgili entry'e ait herhangi bir oy kullanilmamissa
        DownVote = 0, // ilgili entry'e ait dusuk bir oy kullanilmissa
        UpVote = 1 // ilgili entry'e ait yuksek bir oy kullanilmissa
    }
}

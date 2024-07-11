using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSozluk.Common
{
    public class SozlukConstants
    {
#if DEBUG // Bu peroje debug ediliyorsa bu kisim dikkate alinsin
    public const string RabbitMQHost = "localhost";
    
    // Proje docker icerisinde calisiyorsa yani debug etmiyorsak c_rabbitmq'ya ulasacak
#else
    public const string RabbitMQHost = "c_rabbitmq"; 

#endif

        public const string DefaultExchangeType = "direct";

        public const string UserExchangeName = "UserExchange";
        public const string UserEmailChangedQueueName = "UserEmailChangedQueue";

        public const string FavExchangeName = "FavExchange";
        public const string VoteExchangeName = "VoteExchange";

        public const string CreateEntryFavQueueName = "CreateEntryFavQueueName";
        public const string CreateEntryVoteQueueName = "CreateEntryVoteQueue";
        public const string DeleteEntryFavQueueName = "DeleteEntryFavQueue";
        public const string DeleteEntryVoteQueueName = "DeleteEntryVoteQueue";

        public const string CreateEntryCommentFavQueueName = "CreateEntryCommentFavQueue";
        public const string CreateEntryCommentVoteQueueName = "CreateEntryCommentVoteQueue";
        public const string DeleteEntryCommentFavQueueName = "DeleteEntryCommentFavQueue";
        public const string DeleteEntryCommentVoteQueueName = "DeleteEntryCommentVoteQueue"; 

    }
}

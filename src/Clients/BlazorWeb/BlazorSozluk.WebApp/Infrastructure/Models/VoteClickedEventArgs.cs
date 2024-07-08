namespace BlazorSozluk.WebApp.Infrastructure.Models
{
    public class VoteClickedEventArgs : EventArgs
    {
        public Guid EntryId { get; set; }
        public bool IsUpVoteClicked { get; set; } // Yukari oka tiklandi
        public bool UpVoteDeleted { get; set; } // Yukari oka tiklanmasi kaldirildi
        public bool IsDownVoteClicked { get; set; }
        public bool DownVoteDeleted { get; set; }
    }
}

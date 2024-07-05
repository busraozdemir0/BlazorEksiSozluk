namespace BlazorSozluk.WebApp.Infrastructure.Services.Interfaces
{
    public interface IVoteService
    {
        Task CreateCommentEntryUpVote(Guid entryCommentId);
        Task CreateEntryCommentDownVote(Guid entryCommentId);
        Task CreateEntryDownVote(Guid entryId);
        Task CreateEntryUpVote(Guid entryId);
        Task DeleteEntryCommentVote(Guid entryCommentId);
        Task DeleteEntryVote(Guid entryId);
    }
}
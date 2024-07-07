using BlazorSozluk.Common.Models.Page;
using BlazorSozluk.Common.Models.Queries;
using BlazorSozluk.Common.Models.RequestModels;
using BlazorSozluk.WebApp.Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System.Net.Http.Json;

namespace BlazorSozluk.WebApp.Infrastructure.Services
{
    public class EntryService : IEntryService
    {
        private readonly HttpClient client;

        public EntryService(HttpClient client)
        {
            this.client = client;
        }

        public async Task<List<GetEntriesViewModel>> GetEntries()
        {
            var result = await client.GetFromJsonAsync<List<GetEntriesViewModel>>("https://localhost:7068/api/Entry?TodayEntries=false&Count=20");
            return result;
        }

        public async Task<GetEntryDetailViewModel> GetEntryDetail(Guid entryId)
        {
            var result = await client.GetFromJsonAsync<GetEntryDetailViewModel>($"https://localhost:7068/api/entry/{entryId}");
            return result;
        }

        public async Task<PagedViewModel<GetEntryDetailViewModel>> GetMainPageEntries(int page, int pageSize)
        {
            var result = await client.GetFromJsonAsync<PagedViewModel<GetEntryDetailViewModel>>($"https://localhost:7068/api/entry/mainpageentries?page={page}&pageSize={pageSize}");
            return result;
        }

        public async Task<PagedViewModel<GetEntryDetailViewModel>> GetProfilePageEntries(int page, int pageSize, string userName = null)
        {
            var result = await client.GetFromJsonAsync<PagedViewModel<GetEntryDetailViewModel>>($"https://localhost:7068/api/entry/UserEntries?userName={userName}&page={page}&pageSize={pageSize}");
            return result;
        }

        public async Task<PagedViewModel<GetEntryCommentsViewModel>> GetEntryComments(Guid entryId, int page, int pageSize)
        {
            var result = await client.GetFromJsonAsync<PagedViewModel<GetEntryCommentsViewModel>>($"https://localhost:7068/api/entry/comments/{entryId}&page={page}&pageSize={pageSize}");
            return result;
        }

        public async Task<Guid> CreateEntry(CreateEntryCommand command)
        {
            var result = await client.PostAsJsonAsync("https://localhost:7068/api/Entry/CreateEntry", command);

            if (!result.IsSuccessStatusCode)
                return Guid.Empty;

            var guidStr = await result.Content.ReadAsStringAsync();

            return new Guid(guidStr.Trim('"'));
        }

        public async Task<Guid> CreateEntryComment(CreateEntryCommentCommand command)
        {
            var result = await client.PostAsJsonAsync("https://localhost:7068/api/Entry/CreateEntryComment", command);

            if (!result.IsSuccessStatusCode)
                return Guid.Empty;

            var guidStr = await result.Content.ReadAsStringAsync();

            return new Guid(guidStr.Trim('"'));
        }

        public async Task<List<SearchEntryViewModel>> SearchBySubject(string searchText)
        {
            var result = await client.GetFromJsonAsync<List<SearchEntryViewModel>>($"https://localhost:7068/api/entry/Search?searchText={searchText}");

            return result;
        }
    }
}

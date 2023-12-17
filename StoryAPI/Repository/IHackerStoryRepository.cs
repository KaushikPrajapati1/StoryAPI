namespace StoryAPI.Repository
{
    public interface IHackerStoryRepository
    {
        public  Task<PagingParameterModel> GetHackerStoriesByMemoryCache(int pageNumber, int pageSize);
    }
}

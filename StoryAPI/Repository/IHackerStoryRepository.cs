namespace StoryAPI.Repository
{
    public interface IHackerStoryRepository
    {
        public  Task<PagingParameterModel> GetHackerStoriesByMemoryCache(int pageSize);

    }
}

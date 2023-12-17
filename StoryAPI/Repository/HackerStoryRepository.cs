
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Text;

namespace StoryAPI.Repository
{
    public class HackerStoryRepository :IHackerStoryRepository
    {
        private readonly IMemoryCache _memoryCache;
        public HackerStoryRepository(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public List<T> Deserialize<T>(string SerializedJSONString)
        {
            var stuff = JsonConvert.DeserializeObject<List<T>>(SerializedJSONString);
            return stuff;
        }

        public async Task<PagingParameterModel> GetHackerStoriesByMemoryCache(int pageNumber, int pageSize)
        {
            var cacheKey = "storyist";
            PagingParameterModel paginationMetadata;
            if (_memoryCache.TryGetValue(cacheKey, out paginationMetadata))
            {
                if (pageSize != paginationMetadata.pageSize)
                {
                    _memoryCache.Remove(cacheKey);
                }
            }
            if (!_memoryCache.TryGetValue(cacheKey, out paginationMetadata))
            {

                paginationMetadata =
            await GetHackerStories(pageNumber, pageSize);
                var cacheExpiryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddSeconds(300),
                    Priority = CacheItemPriority.High,
                    SlidingExpiration = TimeSpan.FromSeconds(20)
                };
                _memoryCache.Set(cacheKey, paginationMetadata, cacheExpiryOptions);
            }
            return (paginationMetadata);
        }
        public async Task<PagingParameterModel> GetHackerStories(int pageNumber, int pageSize)
        {
            
            List<int> reservationList = new List<int>();
            List<HackerStory> storyList = new List<HackerStory>();
            PagingParameterModel paginationMetadata = null;
            
            using (var httpClient = new HttpClient() { Timeout = TimeSpan.FromMinutes(20) })
            {
                using (var response = await httpClient.GetAsync(" https://hacker-news.firebaseio.com/v0/topstories.json?print=pretty"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    //dynamic dObject = JObject.Parse(apiResponse);
                    reservationList = Deserialize<int>(apiResponse);

                    int count = reservationList.Count();

                    // Parameter is passed from Query string if it is null then it default Value will be pageNumber:1  
                    int CurrentPage = pageNumber;

                    // Parameter is passed from Query string if it is null then it default Value will be pageSize:20  
                    int PageSize = pageSize;

                    // Display TotalCount to Records to User  
                    int TotalCount = count;

                    // Calculating Totalpage by Dividing (No of Records / Pagesize)  
                    int TotalPages = (int)Math.Ceiling(count / (double)PageSize);

                    // Returns List of Customer after applying Paging   
                    var items = reservationList.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

                    // if CurrentPage is greater than 1 means it has previousPage  
                    var previousPage = CurrentPage > 1 ? "Yes" : "No";

                    // if TotalPages is greater than CurrentPage means it has nextPage  
                    var nextPage = CurrentPage < TotalPages ? "Yes" : "No";

                    // Object which we are going to send in header   


                    // Setting Header  
                    //  HttpContext.Current.Response.Headers.Add("Paging-Headers", JsonConvert.SerializeObject(paginationMetadata));
                    StringBuilder strappend = new StringBuilder();
                    strappend.Append("[");
                    foreach (var item in items)
                    {
                        using (var httpClient1 = new HttpClient())
                        {
                            var strUrl = "https://hacker-news.firebaseio.com/v0/item/" + item.ToString() + ".json?print=pretty";
                            using (var response1 = await httpClient1.GetAsync(strUrl))
                            {
                                string apiResponse1 = await response1.Content.ReadAsStringAsync();
                                strappend.Append(apiResponse1 + ",");
                                //dynamic dObject = JObject.Parse(apiResponse);

                            }
                        }
                    }
                    strappend.Append("]");
                    storyList = JsonConvert.DeserializeObject<List<HackerStory>>(strappend.ToString());

                        paginationMetadata = new PagingParameterModel()
                        {
                            totalCount = TotalCount,
                            pageSize = PageSize,
                            currentPage = CurrentPage,
                            totalPages = TotalPages,
                            previousPage = previousPage,
                            nextPage = nextPage,
                            hackerstory = storyList
                        };
                }
            }

        
            return (paginationMetadata);
        }
    }
}

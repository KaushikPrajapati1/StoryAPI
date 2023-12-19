
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text;

namespace StoryAPI.Repository
{
    public class HackerStoryRepository : IHackerStoryRepository
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

        public async Task<PagingParameterModel> GetHackerStoriesByMemoryCache(int pageSize)
        {
            var cacheKey = "storyist";
            var pageNumber = 1;
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
                   AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(180),
                    //AbsoluteExpiration = DateTime.Now.AddSeconds(300),
                    //Priority = CacheItemPriority.High,
                    //SlidingExpiration = TimeSpan.FromSeconds(20)
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
                //   using (var response = await httpClient.GetAsync(" https://hacker-news.firebaseio.com/v0/topstories.json?print=pretty"))
                using (var response = await httpClient.GetAsync(" https://hacker-news.firebaseio.com/v0/newstories.json"))
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
                    var tasks = new List<Task<HackerStory>>();

                    StringBuilder strappend = new StringBuilder();
                 //   strappend.Append("[");
                    foreach (var item in items)
                    {
                       
                        using (var httpClient1 = new HttpClient())
                        {
                            tasks.Add(GetHackerStoryId(item.ToString()));
                        }
                    }

                    foreach(var task in await Task.WhenAll(tasks))
                    {
                        storyList.Add(task);
                    }
                   
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
     

        public async Task<HackerStory> GetHackerStoryId(string Id)
        {
            HackerStory hackerStory = null;
            using (var httpClient = new HttpClient())
            {
                //StringBuilder strappend = new StringBuilder();
               
                var strUrl = "https://hacker-news.firebaseio.com/v0/item/" + Id.ToString() + ".json?print=pretty";
                using (var response1 = await httpClient.GetAsync(strUrl))
                {
                    string apiResponse1 = await response1.Content.ReadAsStringAsync();
                    hackerStory = JsonConvert.DeserializeObject<HackerStory>(apiResponse1);
                }
            // var  strFinal = strappend.ToString().Replace("{", "[").Replace("}", "]");
                
            }
            return (hackerStory);
        }
        
    }
}

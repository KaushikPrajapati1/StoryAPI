using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StoryAPI.Repository;
using System.Collections;
using System.Text;
using System.Text.Json.Nodes;
//using System.Web.Http;

namespace StoryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    // [ApiVersion("2.0")]
    //[Route("[controller]")]
    public class HackerStoryAPIController : ControllerBase
    {
        private IHackerStoryRepository hackerStoryRepository { get; set; }

        private readonly IMemoryCache _memoryCache;
        public class StoryData { public int storynumber; }



        public HackerStoryAPIController(IHackerStoryRepository hackerStoryRepository, IMemoryCache memoryCache)
        {
            this.hackerStoryRepository = hackerStoryRepository;
            _memoryCache = memoryCache;
        }

        public List<T> Deserialize<T>(string SerializedJSONString)
        {
            var stuff = JsonConvert.DeserializeObject<List<T>>(SerializedJSONString);
            return stuff;
        }

        [HttpGet]
        //[ActionName("Post02")]
        public async Task<PagingParameterModel> GetHackerStories(int pageSize)
        {
            PagingParameterModel paginationMetadata;
            paginationMetadata =
        await hackerStoryRepository.GetHackerStoriesByMemoryCache(pageSize);

            return (paginationMetadata);
        }

    }
}
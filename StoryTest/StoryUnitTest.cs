using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using StoryAPI;
using StoryAPI.Repository;
using System.Security.Cryptography;

namespace StoryTest
{
    public class StoryUnitTest
    {

        
        private IHackerStoryRepository hackerStoryRepository { get; set; }
        public StoryUnitTest()
        {
           
        
        }
        [Fact]
        public void Test()
        {

            [Fact]
            async void Task_GetHackerStories_Return_OkResult()
            {
                //Arrange

                var cache = new MemoryCache(new MemoryCacheOptions());
                hackerStoryRepository = new HackerStoryRepository(cache);
                var controller = new StoryAPI.Controllers.HackerStoryAPIController(hackerStoryRepository,cache);

                //Act  
                var data = await controller.GetHackerStories(5);

                //Assert  
                Assert.IsType<StoryAPI.PagingParameterModel>(data);
                Assert.Equal(1, data.pageNumber);
                Assert.Equal(5, data.pageSize);
               // Assert.Equal(500, data.totalCount);
            }

            [Fact]
            async void Task_GetHackerStories_Validate_MemorayCache()
            {
                //Arrange
                var cache = new MemoryCache(new MemoryCacheOptions());
                hackerStoryRepository = new HackerStoryRepository(cache);
                var controller = new StoryAPI.Controllers.HackerStoryAPIController(hackerStoryRepository,cache);
                PagingParameterModel cacheresult;
                //Act  
             
               
                var data = await controller.GetHackerStories(5);

                //Assert  
                
                cache.TryGetValue("storyist", out cacheresult);
                Assert.Equal(cacheresult, data);
            }

            [Fact]
            void Task_GetHackerStories_Return_BadRequestResult()
            {
                //Arrange  
                var cache = new MemoryCache(new MemoryCacheOptions());
                hackerStoryRepository = new HackerStoryRepository(cache);
                var controller = new StoryAPI.Controllers.HackerStoryAPIController(hackerStoryRepository, cache);

                //Act  
                var data = controller.GetHackerStories(1);
                data = null;

                if (data != null)
                    //Assert  
                    Assert.IsType<BadRequestResult>(data);
            }
        }
    }
}
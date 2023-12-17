# StoryAPI

# Set as a Startup Project 
StoryAPI
# Default URL
http://localhost:5222/swagger/index.html
# Project Information
WebAPI- .net core - C#, Get 200 storylist from  the GetMethod (GetHackerStories - need to assign default pageno- 1 , pagesize is dynamic any from 1 to max...), 1) Code List coming from ( https://hacker-news.firebaseio.com/v0/topstories.json?print=pretty) 2) Per Code will get details from (https://hacker-news.firebaseio.com/v0/item/38665911.json?print=pretty) , worked on several features like dependency injection, interface, service, memorary caches and created unit test cases.
# Get - GetHackerStories ( call two api internally to get storylist according to code lists)
http://localhost:5222/api/HackerStoryAPI/GetHackerStories?pageNumber=1&pageSize=200

# Unit Test
Task_GetHackerStories_Validate_MemorayCache() , Task_GetHackerStories_Return_OkResult(),  Task_GetHackerStories_Return_BadRequestResult()
# Schema 
{
  "pageNumber": 0,
  "pageSize": 0,
  "totalCount": 0,
  "currentPage": 0,
  "totalPages": 0,
  "previousPage": "string",
  "nextPage": "string",
  "hackerstory": [
    {
      "id": 0,
      "title": "string",
      "by": "string",
      "url": "string"
    }
  ]
}

# Response Body - 10 Storylist (Example)

{
  "pageNumber": 1,
  "pageSize": 10,
  "totalCount": 500,
  "currentPage": 1,
  "totalPages": 50,
  "previousPage": "No",
  "nextPage": "Yes",
  "hackerstory": [
    {
      "id": 38673292,
      "title": "The Final Speech from The Great Dictator",
      "by": "hypertexthero",
      "url": "https://www.charliechaplin.com/en/articles/29-the-final-speech-from-the-great-dictator-"
    },
    {
      "id": 38675258,
      "title": "AMD's CDNA 3 Compute Architecture",
      "by": "ksec",
      "url": "https://chipsandcheese.com/2023/12/17/amds-cdna-3-compute-architecture/"
    },
    {
      "id": 38674158,
      "title": "Misra C++:2023 Published",
      "by": "ksec",
      "url": "https://forum.misra.org.uk/thread-1668.html"
    },
    {
      "id": 38665911,
      "title": "Where Johnny Cash Came From",
      "by": "tintinnabula",
      "url": "https://www.neh.gov/article/where-johnny-cash-came"
    },
    {
      "id": 38673854,
      "title": "BrainGPT turns thoughts into text",
      "by": "11thEarlOfMar",
      "url": "https://www.iflscience.com/new-mind-reading-braingpt-turns-thoughts-into-text-on-screen-72054"
    },
    {
      "id": 38673954,
      "title": "microsoft/promptbase: All things prompt engineering",
      "by": "CharlesW",
      "url": "https://github.com/microsoft/promptbase"
    },
    {
      "id": 38673608,
      "title": "Everyone on Earth is your cousin (2014)",
      "by": "bookofjoe",
      "url": "https://qz.com/557639/everyone-on-earth-is-actually-your-cousin"
    },
    {
      "id": 38673392,
      "title": "How does Base32 (or any Base2^n) work exactly?",
      "by": "pchm",
      "url": "https://ptrchm.com/posts/base32-explained/"
    },
    {
      "id": 38666045,
      "title": "Two interesting XOR circuits inside the Intel 386 processor",
      "by": "_Microft",
      "url": "https://www.righto.com/2023/12/386-xor-circuits.html"
    },
    {
      "id": 38674240,
      "title": "Motion (YC W20) Is Hiring Front End Engineers",
      "by": "ethanyu94",
      "url": "https://jobs.ashbyhq.com/motion/4f5f6a29-3af0-4d79-99a4-988ff7c5ba05?utm_source=hn"
    }
  ]
}
# Response headers
 content-type: application/json; charset=utf-8 
 date: Sun,17 Dec 2023 19:20:29 GMT 
 server: Kestrel 
 transfer-encoding: chunked 

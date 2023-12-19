namespace StoryAPI
{
    public class PagingParameterModel
    {
        const int maxPageSize = 200;

        public int pageNumber { get; set; } = 1;

        private int _pageSize { get; set; } = 10;

        public int pageSize
        {

            get { return _pageSize; }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
        public int totalCount { get; set; }
        public int currentPage { get; set;  }
        public int totalPages { get; set; }
        public string previousPage { get; set; }
        public string nextPage { get; set; }
        public List<HackerStory> hackerstory { get; set; }
    }
}

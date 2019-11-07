namespace HotelSystem.SharedKernel.Models
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    /// <summary>
    /// Defines the <see cref="PagingHeader" />
    /// </summary>
    public class PagingHeader
    {
        /// <summary>
        /// Defines the MAX_PAGE_SIZE
        /// </summary>
        public const int MAX_PAGE_SIZE = 10;

        /// <summary>
        /// Defines the _pageSize
        /// </summary>
        private int _pageSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="PagingHeader"/> class.
        /// </summary>
        /// <param name="totalCount">The totalItems<see cref="int"/></param>
        /// <param name="pageIndex">The pageNumber<see cref="int"/></param>
        /// <param name="pageSize">The pageSize<see cref="int"/></param>
        /// <param name="totalPages">The totalPages<see cref="int"/></param>
        public PagingHeader(int totalCount, int pageIndex, int pageSize, int totalPages)
        {
            this.TotalCount = totalCount;
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
            this.TotalPages = totalPages;
        }

        /// <summary>
        /// Gets a value indicating whether HasNextPage
        /// </summary>
        public bool HasNextPage => (PageIndex < TotalPages);

        /// <summary>
        /// Gets a value indicating whether HasPreviousPage
        /// </summary>
        public bool HasPreviousPage => (PageIndex > 1);

        /// <summary>
        /// Gets the PageIndex
        /// </summary>
        public int PageIndex { get; }

        /// <summary>
        /// Gets or sets the PageSize
        /// Gets the PageSize
        /// </summary>
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MAX_PAGE_SIZE) ? MAX_PAGE_SIZE : value;
        }

        /// <summary>
        /// Gets the TotalCount
        /// </summary>
        public int TotalCount { get; }

        /// <summary>
        /// Gets the TotalPages
        /// </summary>
        public int TotalPages { get; }

        public static string HEADER_KEY = "X-Pagination";
    }
}

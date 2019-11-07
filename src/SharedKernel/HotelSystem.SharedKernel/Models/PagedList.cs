namespace HotelSystem.SharedKernel.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Defines the <see cref="PaginatedList{T}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PaginatedList<T> : List<T>
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
        /// Initializes a new instance of the <see cref="PaginatedList{T}"/> class.
        /// </summary>
        public PaginatedList()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaginatedList{T}"/> class.
        /// </summary>
        /// <param name="items">The items<see cref="List{T}"/></param>
        /// <param name="count">The count<see cref="int"/></param>
        /// <param name="pageIndex">The pageIndex<see cref="int"/></param>
        /// <param name="pageSize">The pageSize<see cref="int"/></param>
        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            _pageSize = pageSize;
            TotalCount = count;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            this.AddRange(items);
        }

        /// <summary>
        /// Gets a value indicating whether HasNextPage
        /// </summary>
        public bool HasNextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }

        /// <summary>
        /// Gets a value indicating whether HasPreviousPage
        /// </summary>
        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }

        /// <summary>
        /// Gets the PageIndex
        /// </summary>
        public int PageIndex { get; private set; }

        /// <summary>
        /// Gets or sets the PageSize
        /// </summary>
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MAX_PAGE_SIZE) ? MAX_PAGE_SIZE : value;
        }

        /// <summary>
        /// Gets the TotalCount
        /// </summary>
        public int TotalCount { get; private set; }

        /// <summary>
        /// Gets the TotalPages
        /// </summary>
        public int TotalPages { get; private set; }

        /// <summary>
        /// The CreateAsync
        /// </summary>
        /// <param name="source">The source<see cref="IQueryable{T}"/></param>
        /// <param name="pageIndex">The pageIndex<see cref="int"/></param>
        /// <param name="pageSize">The pageSize<see cref="int"/></param>
        /// <returns>The <see cref="PaginatedList{T}"/></returns>
        public static PaginatedList<T> Create(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }

        public void SetValues(PagingHeader pagingHeader)
        {
            this.TotalCount = pagingHeader.TotalCount;
            this.PageIndex = pagingHeader.PageIndex;
            this.PageSize = pagingHeader.PageSize;
            this.TotalPages = pagingHeader.TotalPages;
        }

        /// <summary>
        /// The PagingHeader
        /// </summary>
        /// <returns>The <see cref="PagingHeader"/></returns>
        public PagingHeader PagingHeader()
        {
            return new PagingHeader(TotalCount, PageIndex, PageSize, TotalPages);
        }
    }
}

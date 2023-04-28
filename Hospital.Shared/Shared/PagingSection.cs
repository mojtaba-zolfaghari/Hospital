namespace Inventory.Search.DTO.Stay.Shared
{
    /// <summary>
    /// get list of data based on entity
    /// </summary>
    public class PagingSection
    {
        /// <summary>
        /// "ASC"  or "DESC"
        /// </summary>
        public string SortType { get; set; }
        /// <summary>
        /// order by 
        /// </summary>
        public string OrderBy { get; set; }
        /// <summary>
        /// how many data you want to skip
        /// </summary>
        public int Skip { get; set; }

        /// <summary>
        /// TotalRowsCount
        /// </summary>
        public long TotalRowsCount { get; set; }
        /// <summary>
        /// how many records you want to take (by default(if pass ZERO) 10 record returns)
        /// </summary>

        private int _take;
        /// <summary>
        /// how many records you want to take (by default(if pass ZERO) 10 record returns)
        /// </summary>
        public int Take
        {
            get => _take;
            set
            {
                if (value < 1) _take = 10;
                _take = value;
            }
        }

    }
}

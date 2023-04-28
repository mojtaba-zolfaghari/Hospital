namespace Hospital.Api.QueueManagement.DTO.Shared
{
    /// <summary>
    /// get list of data based on entity
    /// </summary>
    public class RequestBase
    {
        /// <summary>
        /// filter
        /// </summary>
        public Dictionary<string, string> Filters { get; set; } = new Dictionary<string, string>();
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
        /// <summary>
        /// how many records you want to take (by default(if pass ZERO) 10 record returns)
        /// </summary>
        private int _take = 10;

        /// <summary>
        /// how many records you want to take (by default(if pass ZERO) 10 record returns)
        /// </summary>
        public int Take
        {
            get => _take;
            set => _take = value < 1 ? 10 : value;
        }
    }
}

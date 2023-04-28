namespace Hospital.Shared.Utitlities
{
    /// <summary>
    /// ListResult
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ListResult<T>
    {
        /// <summary>
        /// ListResult
        /// </summary>
        public ListResult()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="totalRowCount"></param>
        public ListResult(IEnumerable<T> data, long totalRowCount)
        {
            Data = data;
            TotalRowCount = totalRowCount;
        }
        /// <summary>
        /// Data
        /// </summary>
        public IEnumerable<T> Data { get; set; }
        /// <summary>
        /// TotalRowCount
        /// </summary>
        public long TotalRowCount { get; set; }
    }
}

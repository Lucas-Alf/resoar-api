namespace Domain.Models
{
    public class PaginationModel<TEntity>
    {
        public int Page { get; set; }

        public int PageSize { get; set; }

        public int TotalRecords { get; set; }

        public IEnumerable<TEntity> Records { get; set; }
    }
}

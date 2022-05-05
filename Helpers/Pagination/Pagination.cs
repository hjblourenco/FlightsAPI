    
    namespace Helpers.Pagination;
    public class Pagination<T> : List<T>
    {
        public MetaData MetaData { get; set; }
        public List<T> Items { get; set; }
        public Pagination(List<T> items, int count, int pageNumber, int pageSize)
        {
            MetaData = new MetaData
            {
                TotalCount = count,
                PageSize = pageSize,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize)
            };

            Items=items;
        }

        public static Pagination<T> ToPagedList(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();

            var items = source.Skip((pageNumber-1)*pageSize).Take(pageSize).ToList();

            return new Pagination<T>(items, count, pageNumber, pageSize);

        }

    }
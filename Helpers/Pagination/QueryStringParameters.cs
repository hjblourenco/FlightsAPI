namespace Helpers.QueryStringParameters;
public  class QueryStringParameters
{
    const int maxPageSize = 50;
    public int PageNumber {get; set;}=1;
    public int _pageSize = 50;

    public QueryStringParameters()
    {

    }
    public QueryStringParameters(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
    
    public int PageSize
    {
        get
        {
            return _pageSize;
        }
        set
        {
            _pageSize = (value>maxPageSize) ? maxPageSize : value;
        }
    }

}
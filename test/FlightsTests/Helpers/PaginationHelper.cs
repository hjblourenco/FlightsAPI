using System.Collections.Generic;
using System.Linq;
using Helpers.Pagination;
using Helpers.QueryStringParameters;
using Xunit;

public class PaginationHelper
{

    [Fact]
    public void PaginationTotalPagesAndItemsTest()
    {

        
        var list = new List<string>();
        for (int i = 0; i < 100; i++)
        {
            list.Add(i.ToString());
        }

        int PageNumber = 1;
        int PageSize = 10;
        var queryStringParameters = new QueryStringParameters(PageNumber,PageSize);

        var resultPaged = Pagination<string>.ToPagedList(
            list.AsQueryable<string>(),
            queryStringParameters.PageNumber,
            queryStringParameters.PageSize
        );
        

        Assert.Equal(10, resultPaged.MetaData.TotalPages);
        Assert.Equal(10, resultPaged.Items.Count());
    }

    [Fact]
    public void PaginationTotalPagesAndItemsTest2()
    {

        
        var list = new List<string>();
        for (int i = 0; i < 91; i++)
        {
            list.Add(i.ToString());
        }

        int PageNumber = 2;
        int PageSize = 10;
        var queryStringParameters = new QueryStringParameters(PageNumber,PageSize);

        var resultPaged = Pagination<string>.ToPagedList(
            list.AsQueryable<string>(),
            queryStringParameters.PageNumber,
            queryStringParameters.PageSize
        );
        

        Assert.Equal(10, resultPaged.MetaData.TotalPages);
        Assert.Equal(10, resultPaged.Items.Count());
    }

    [Fact]
    public void PaginationTotalPagesAndItemsTest3()
    {

        var list = new List<string>();

        int numberOfItems = 81;
        for (int i = 0; i < numberOfItems; i++)
        {
            list.Add(i.ToString());
        }

        int PageNumber = 9;
        int PageSize = 10;
        var queryStringParameters = new QueryStringParameters(PageNumber,PageSize);

        var resultPaged = Pagination<string>.ToPagedList(
            list.AsQueryable<string>(),
            queryStringParameters.PageNumber,
            queryStringParameters.PageSize
        );
        
        //has 81 items and have 9 pages, current page is 9 and total pages is 9 too, don't have next page but has previous page
        Assert.Equal(9, resultPaged.MetaData.TotalPages);
        Assert.Equal(9, resultPaged.MetaData.CurrentPage);
        Assert.Equal(numberOfItems, resultPaged.MetaData.TotalCount);
        Assert.Equal(PageSize, resultPaged.MetaData.PageSize);

        Assert.Equal(false, resultPaged.MetaData.HasNext);
        Assert.Equal(true, resultPaged.MetaData.HasPrevious);

        //Only have one item in the last page
        Assert.Equal(1, resultPaged.Items.Count());
    }
}



using System.Collections.Generic;
using System.Linq;
using Xunit;

public class SortHelper
{

    [Fact]
    public void SimpleOrderTest()
    {

        var testData = test.generate(3);
        
        var sortedDataDesc = testData.OrderByDescending(x => x.Id);
        
        ISort<test> sort = new Sort<test>();
        var orderedDataAsc = sort.ApplySort(testData.AsQueryable<test>(), "Id");
        Assert.Equal(testData, orderedDataAsc);

        var orderedDataDesc = sort.ApplySort(testData.AsQueryable<test>(), "Id desc");
        Assert.Equal(sortedDataDesc, orderedDataDesc);

    }

    [Fact]
    public void OrderTestTwoitems()
    {

        var testData = testTwoItems.generate(3);
        
        var sortedDataDesc = testData.OrderBy(x => x.Id).OrderBy(x => x.Id2);
        
        ISort<testTwoItems> sort = new Sort<testTwoItems>();

        var orderedDataAsc = sort.ApplySort(testData.AsQueryable<testTwoItems>(), "Id1,Id2");
        Assert.Equal(sortedDataDesc, orderedDataAsc);

        var orderedDataDesc = sort.ApplySort(testData.AsQueryable<testTwoItems>(), "Id1 desc,Id2 desc");
        Assert.Equal(testData, orderedDataDesc);

    }

    [Fact]
    public void SimpleOrderTestWithNull()
    {

        var testData = test.generate(3);
        

        ISort<test> sort = new Sort<test>();
        var orderedDataAsc = sort.ApplySort(testData.AsQueryable<test>(), null);
        //return the sane
        Assert.Equal(testData, orderedDataAsc);


    }

    private class test
    {
        public static  List<test> generate(int size)
        {
            var testList = new List<test>();
            for (int i = 0; i < size; i++)
            {
                var test = new test();
                test.Id = i;
                testList.Add(test);
            }
            return testList;
        }
        public int Id { get; set; }    
    }

    private class testTwoItems
    {
        public static  List<testTwoItems> generate(int size)
        {
            var testList = new List<testTwoItems>();
            for (int i = size; i >0; i--)
            {
                var testTwoItems = new testTwoItems();
                testTwoItems.Id = 1;
                testTwoItems.Id2 = i;                
                testList.Add(testTwoItems);
            }
            return testList;
        }
        public int Id { get; set; }    
        public int Id2 { get; set; }   
    }
}
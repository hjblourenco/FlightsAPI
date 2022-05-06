using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Xunit;



    public class SearchHelper
    {


        [Fact]
        public void OneItemSearch()
        {

           var Test1 = new test
            {
                TestVar1 = "aa",
                TestVar2 = "bb",
                TestVar3 = "cc"
            };

            var Test2 = new test
            {
                TestVar1 = "dd",
                TestVar2 = "ee",
                TestVar3 = "ff"
            };

            List<test> TestList = new List<test>();
            TestList.Add(Test1);
            TestList.Add(Test2);
            

            IEnumerable<test> TestListEnumerable = TestList.AsQueryable();

            ISearch<test> Search =  new Search<test>();

            var searchAlgorythmResult = Search.ApplySearch(TestListEnumerable,"bb");


            Assert.Equal(1, searchAlgorythmResult?.Count());
        }

        [Fact]
        public void NinetyNineItemsSearch()
        {
            List<BigTestClass> test = new List<BigTestClass>();

            test = BigTestClass.BigTestClassGenerate(33);
            string TestString = GenerateRandomStrings.RandomString(30);
            for (int i = 0; i < 33; i++)
            {
                test.Add(new BigTestClass
                {
                    Id = i,
                    Name = TestString,
                    Description = GenerateRandomStrings.RandomString(50),
                    Guid = Guid.NewGuid()
                });
                test.Add(new BigTestClass
                {
                    Id = i,
                    Name = GenerateRandomStrings.RandomString(10),
                    Description = TestString,
                    Guid = Guid.NewGuid()
                });
            }

            IEnumerable<BigTestClass> TestListEnumerable = test.AsQueryable();

            ISearch<BigTestClass> Search =  new Search<BigTestClass>();

            var searchAlgorythmResult = Search.ApplySearch(TestListEnumerable,TestString);


            Assert.Equal(66, searchAlgorythmResult?.Count());
        }

        [Fact]
        public void SearchWithNull()
        {
            List<BigTestClass> test = new List<BigTestClass>();

            string TestString = GenerateRandomStrings.RandomString(60);

            test.Add(new BigTestClass
            {
                Id = 1,
                Name = null,
                Description = GenerateRandomStrings.RandomString(50),
                Guid = Guid.NewGuid()
            });
            test.Add(new BigTestClass
            {
                Id = 2,
                Name = GenerateRandomStrings.RandomString(10),
                Description = null,
                Guid = Guid.NewGuid()
            });
            

            IEnumerable<BigTestClass> TestListEnumerable = test.AsQueryable();

            ISearch<BigTestClass> Search =  new Search<BigTestClass>();

            var searchAlgorythmResult = Search.ApplySearch(TestListEnumerable,TestString);

            Assert.Equal(0, searchAlgorythmResult?.Count());
        }

        [Fact]
        public void SearchWithEmptyString()
        {
            List<BigTestClass> test = new List<BigTestClass>();

            string TestString = "";

            test.Add(new BigTestClass
            {
                Id = 1,
                Name = null,
                Description = GenerateRandomStrings.RandomString(50),
                Guid = Guid.NewGuid()
            });
            test.Add(new BigTestClass
            {
                Id = 2,
                Name = GenerateRandomStrings.RandomString(10),
                Description = null,
                Guid = Guid.NewGuid()
            });

            IEnumerable<BigTestClass> TestListEnumerable = test.AsQueryable();

            ISearch<BigTestClass> Search =  new Search<BigTestClass>();

            var searchAlgorythmResult = Search.ApplySearch(TestListEnumerable,TestString);

            Assert.Equal(TestListEnumerable.Count(), searchAlgorythmResult?.Count());   
        }

        [Fact]
        public void SearchWithAutoMapper()
        {
            string compareString = "cc";
            var Test1 = new test
            {
                TestVar1 = "aa",
                TestVar2 = "bb",
                TestVar3 = compareString
            };
            List<test> TestList = new List<test>();
            TestList.Add(Test1);


            //auto mapper configuration
            var configuration = new MapperConfiguration(cfg =>
            cfg.CreateMap<test, testDto>());

            configuration.AssertConfigurationIsValid();

            var mapper = configuration.CreateMapper();

            var testWithAutoMapper = mapper.Map<List<testDto>>(TestList);

            IEnumerable<testDto> TestListEnumerable = testWithAutoMapper.AsQueryable();

            ISearch<testDto> Search =  new Search<testDto>();

            var searchAlgorythmResult = Search.ApplySearch(TestListEnumerable,compareString);

            Assert.Equal(1, searchAlgorythmResult?.Count());   

        }

        [Fact]
        public void OtherSearchWithAutoMapper()
        {
            string compareString = "cc";
            var Test1 = new test
            {
                TestVar1 = "aa",
                TestVar2 = "bb",
                TestVar3 = compareString
            };
            var Test2 = new test
            {
                TestVar1 = "aa",
                TestVar2 = "bb",
                TestVar3 = "Diferent"
            };
            List<test> TestList = new List<test>();
            TestList.Add(Test1);
            TestList.Add(Test2);

            //auto mapper configuration
            var configuration = new MapperConfiguration(cfg =>
            cfg.CreateMap<test, testDto>());

            configuration.AssertConfigurationIsValid();

            var mapper = configuration.CreateMapper();

            var testWithAutoMapper = mapper.Map<List<testDto>>(TestList);

            IEnumerable<testDto> TestListEnumerable = testWithAutoMapper.AsQueryable();

            ISearch<testDto> Search =  new Search<testDto>();

            var searchAlgorythmResult = Search.ApplySearch(TestListEnumerable,compareString);

            Assert.Equal(1, searchAlgorythmResult?.Count());   

        } 

        private class BigTestClass
        {
            public static List<BigTestClass> BigTestClassGenerate(int size)
            {
                List<BigTestClass> test = new List<BigTestClass>();
                Random rnd = new Random();

                for (int i = 0; i < size; i++)
                {
                    test.Add(new BigTestClass
                    {
                        Id = rnd.Next(),
                        Name = GenerateRandomStrings.RandomString(10),
                        Description = GenerateRandomStrings.RandomString(50),
                        Guid = Guid.NewGuid()
                    });
                }
                return test;
            }
            public int Id { get; set; }
            public string Name { get; set; }

            public string Description { get; set; }
            public Guid Guid { get; set; }


        }

        private class test
        {
            public string TestVar1 { get; set; }
            public string TestVar2 { get; set; }
            public string TestVar3 { get; set; }
        }

        private class testDto
        {
            public string TestVar3 { get; set; }
        }



}

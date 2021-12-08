using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Dapper;
using System.Data.SqlClient;

namespace CodingTest
{

    /* Write a function in the language of your choice which supports the following requirements:
 • Write a function that prints the numbers from 1 to 100.
 • But for multiples of 5 print “FIVE” instead of the number.
 • For the multiples of 7 print “SEVEN”.
 • For numbers which are multiples of both 5 and 7 print “FIVESEVEN”*/
    class Program
    {

        /* Write a function in the language of your choice which supports the following requirements:
            • Write a function that prints the numbers from 1 to 100.
            • But for multiples of 5 print “FIVE” instead of the number.
            • For the multiples of 7 print “SEVEN”.
            • For numbers which are multiples of both 5 and 7 print “FIVESEVEN”
        */
        public static void question1()
        {
            for (int i = 1; i <= 100; i++)
            {
                if (i % 5 == 0) Console.Write("FIVE");
                if (i % 7 == 0) Console.Write("SEVEN");
                if (i % 5 != 0 && i % 7 != 0) Console.Write(i);
                Console.Write(" ");
            }
        }
        /*  QUESTION 2
         *   You have a database storing data points in a hierarchical manner
             Given the following classes provided by the database vendor to access the database:*/
        public class DataPoint
        {
            public Guid Id { get; set; }
            public int? Value { get; set; }
            public string Name { get; set; }
        }
        public List<DataPoint> datapoints = new List<DataPoint>();
        public class PointDatabaseAccessor:IMyClass
        {
            private static string _connectionString = @"Data Source=.\SQLEXPRESS;AttachDbFilename=D:\Repos\CodingTest\DBCodingTest.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True";

            /// <summary>
            /// Gets the main item.
            /// </summary>
            public  async Task GetRootAsync()
            {
                var query = "select top 1 * from DataPoint where Parent is null";
                using (var db = new SqlConnection(_connectionString))
                {
                    var root = await db.QueryAsync<DataPoint>(query).ConfigureAwait(false);
                }

            }
            /// <summary>
            /// Gets the parent data point. Can return a null value.
            /// </summary>
            public async Task GetParentAsync(Guid dataPointId)
            {
                var query = "select top 1 * from DataPoint where Id=(select ParentId from DataPoint where Id=0"+
                    dataPointId+")";
                using (var db = new SqlConnection(_connectionString))
                {
                    var parent = await db.QueryAsync<DataPoint>(query).ConfigureAwait(false);
                }
            }
            /// <summary>
            /// Gets the parent data point. Returns an empty array if no children.
            /// </summary>
            public async Task GetChildrenAsync(Guid dataPointId)
            {
                var query = "select top 1 * from DataPoint where ParentId=(select ParentId from DataPoint where Id=0" +
                    dataPointId + ")";
                DataPoint[] result;
                using (var db = new SqlConnection(_connectionString))
                {
                    
                    var children = await db.QueryAsync<DataPoint>(query).ConfigureAwait(false);
                    result = children.ToArray();
                }
                
            }

            public async Task GetAllPointsWithNullValueUnderAsync(Guid parentPointId)
            {
                var query = "select * from DataPoint where Value is Null";
                using (var db = new SqlConnection(_connectionString))
                {
                    //var result = (List<DataPoint>)db.Query(query);
                    var result = await db.QueryAsync<DataPoint>(query).ConfigureAwait(false);
                }
            }

            public int SummationOfAllValuesInWholeHierarchy()
            {
                var query = "select Sum(Value) from DataPoint where Value is not Null";
                using (var db = new SqlConnection(_connectionString))
                {
                    var result = (List<int>)db.Query(query);
                    return result[0];
                }
            }
        }
        /*********** Implements the following interface (2 methods to implement):****/
        public interface IMyClass
        {
            /// <summary>
            /// Gets all point names with a null value under the parent data point id.
            ///
            /// IMPORTANT NOTE: Implements this function ASYNCHRONOUSLY.
            /// </summary>
            /// <param name="parentPointId">The parent id to start the search.</param>
            /// <returns>Returns a list of point names with null values.</returns>
            Task  GetAllPointsWithNullValueUnderAsync(Guid parentPointId);
            Task GetRootAsync();

            /// <summary>
            /// Do the summation of all non-null values for the whole hierarchy.
            ///
            /// IMPORTANT NOTE: Implements this function SYNCRONOUSLY.
            /// <summary>
            /// <returns>Returns the sum of all points.</returns>
            int SummationOfAllValuesInWholeHierarchy();
        }
      

        public class Asset
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }
        class AssetComparer : IEqualityComparer<Asset>
        {
            public bool Equals(Asset a, Asset b)
            {
                return a.Id == b.Id;
            }

            public int GetHashCode(Asset a)
            {
                return a.Id.GetHashCode();
            }


        }
        /// <summary> QUESTION 4 
        ///  Given two lists holding the following object
        /// Merge efficiently these two lists together without duplicates(no asset with the same id in themerged list).

        /// </summary>
        public static void doMerge()
        {
            List<Asset> list1 = new List<Asset>
            {   new Asset(){ Id = Guid.NewGuid(), Name = "One" },
                new Asset(){ Id = Guid.NewGuid(), Name = "Two" },
                new Asset(){ Id = Guid.NewGuid(), Name = "Three" },
                new Asset(){ Id = Guid.NewGuid(), Name = "Four" },
                new Asset(){ Id = Guid.NewGuid(), Name = "Five" },
                
            };
            List<Asset> list2 = new List<Asset>
            {  
                new Asset(){ Id = Guid.NewGuid(), Name = "Six" },
                new Asset(){ Id = Guid.NewGuid(), Name = "Seven" },
                new Asset(){ Id = Guid.NewGuid(), Name = "Eight" },
                new Asset(){ Id = Guid.NewGuid(), Name = "Nine" },
                new Asset(){ Id = Guid.NewGuid(), Name = "Ten" }
            };
            list2.Add(list1.FirstOrDefault(e => e.Name == "Two"));
            list2.Add(list1.FirstOrDefault(e => e.Name == "Four"));
            list1.Add(list2.FirstOrDefault(e => e.Name == "Six"));
            list1.Add(list2.FirstOrDefault(e => e.Name == "Seven"));
            List<Asset> merge_list = list1.Union(list2,new AssetComparer()).ToList();
            foreach(var l in merge_list)
            {
                Console.WriteLine("{0} {1}", l.Id, l.Name);
            }
        }
       

        static void Main(string[] args)
        {
            Console.WriteLine("CODING TEST");
            question1();
            doMerge();  //question 4
            
        }
    }
}

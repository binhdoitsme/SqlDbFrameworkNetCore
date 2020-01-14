using FinalProjectEMS.Models;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using SqlDbFrameworkNetCore.Helpers;
using SqlDbFrameworkNetCore.Linq;
using SqlDbFrameworkNetCore.TestModels;
using System;
using System.Data.Common;
using System.Data.SqlClient;

namespace SqlDbFrameworkNetCore
{
    class Program
    {
        static void Main()
        {
            //IQueryBuilder queryBuilder = new QueryBuilder();
            //queryBuilder.Select<Phone>(p => p.Model, p => p.Name)
            //    .Where(p => p.Year <= 2016 && p.Name == "LG V20")
            //    .OrderBy(p => p.Model);
            //Console.WriteLine(queryBuilder);
            //queryBuilder.Clear();
            //queryBuilder.DeleteFrom<Phone>().Where(p => p.Name == "LG V20").Limit(20);
            //Console.WriteLine(queryBuilder);
            //queryBuilder.Clear();
            //queryBuilder.Update<Phone>().Set(new { Year = 2019 }).Where(p => p.Name == "LG V20");
            //Console.WriteLine(queryBuilder);
            //System.Data.Common.DbConnection dbConnection = new System.Data.SqlClient.SqlConnection();
            //System.Data.Common.DbParameterCollection dbParameterCollection = dbConnection.CreateCommand()
            //    .CreateParameters(new { Year = 2019 });
            //queryBuilder.Clear();
            //queryBuilder.Select<Phone>()
            //    .InnerJoin<PhoneManufacturer>("m").On((p, m) => p.ManufacturerId == m.Id)
            //    //.InnerJoin<Phone>().On((p1, m1, p2) => p1.ManufacturerId == m1.Id)
            //    //.InnerJoin<PhoneManufacturer>().On((p1, m1, p2, m2) => p2.ManufacturerId == m1.Id)
            //    .Where((p, m) => p.ManufacturerId == m.Id);
            //Console.WriteLine(queryBuilder);

            //var obj = new[]
            //{ 
            //    new {
            //        name = "LG V20",
            //        model = "F800L",
            //        year = 2017,
            //        manufacturer_id = 1,
            //        id = 1
            //    }
            //};
            //try
            //{
            //    var o = ObjectMapper.ToCompositeObjectCollection<Phone, PhoneManufacturer>(obj);
            //    Console.WriteLine(o);
            //} catch
            //{
            //    Console.WriteLine("Cannot Cast!");
            //}   

            //Console.WriteLine($"Today is {DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}");

            //queryBuilder.Clear();
            //queryBuilder.InsertInto<Phone>().Values(new Phone[]
            //{
            //    new Phone
            //    {
            //        Name = "VSmart Live",
            //        Model = "VSL'212",
            //        Year = 2019
            //    },
            //    new Phone
            //    {
            //        Name = "LG V50",
            //        Model = "F960L",
            //        Year = 2019
            //    }
            //});
            //Console.WriteLine(queryBuilder);
            string connectionStr = "Server=localhost;Port=3306;Database=final_project_test_database;User Id=aspnet;Password=aspnet123;";
            DbConnection connection = new MySqlConnection(connectionStr);
            connection.Open();
            var builder = connection.CreateQueryBuilder();
            var result = builder.Select<Course>()
                                .InnerJoin<CourseClass>("cc").On((c, cc) => c.CourseId == cc.CourseId)
                                .Where((c, cc) => c.Required == true)
                                .ExecuteQuery();
            Console.WriteLine(result);

            builder.Clear();

            //var affectedRows = builder.InsertInto<Semester>()
            //                        .Values(new Semester[]
            //                        {
            //                            new Semester
            //                            {
            //                                SemesterId = 0,
            //                                SemesterName = "Học kì 3 - 2019-2020",
            //                                StartDate = DateTime.Today,
            //                                EndDate = DateTime.Today + TimeSpan.FromDays(365)
            //                            }
            //                        })
            //                        .ExecuteNonQuery();
            var affectedRows = builder.DeleteFrom<Semester>().Where(s => s.SemesterId == 6).ExecuteNonQuery();

            //var affectedRows = builder.Update<Semester>()
            //    .Set(new
            //    {
            //        SemesterName = "Học kì 3 - Unknown"
            //    })
            //    .Where(s => s.SemesterId == 6)
            //    .ExecuteNonQuery();

            Console.WriteLine($"Query finished, {affectedRows} row(s) affected.");
        }
    }
}

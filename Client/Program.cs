using Newtonsoft.Json;
using SQLQueryBuilder.Business.Concrete;
using SQLQueryBuilder.Business.Contract;
using SQLQueryBuilder.Models;
using System;
using System.Diagnostics;

namespace JsonToSqlQueryBuilder
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("------------------------------------------");
            Console.WriteLine("Convert JSON to SQL Query!!");
            Console.WriteLine("------------------------------------------\n\n\n");

            var jsonData = string.Empty;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            IQueryBuilder queryBuilder = new QueryBuilder();
           
            try
            {
                // Read Json File
                jsonData = queryBuilder.ReadJsonFIle(@"../../../jsonFile/JsonFile.json");

                // Check if file is empty or not
                if (string.IsNullOrEmpty(jsonData))
                {
                    Console.WriteLine("File is empty");
                    return;
                }

                // Deserialize Data and convert the json to sql 
                var deserializedData = JsonConvert.DeserializeObject<JsonData>(jsonData);
                var result = queryBuilder.CreateSqlQuery(deserializedData);
                Console.WriteLine(result);

                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;

                // TimeSpan value.
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                Console.WriteLine("RunTime " + elapsedTime);

            }
            catch (JsonException jsonException)
            {
                Console.WriteLine($"exception in json deserialization : {jsonException.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"exception occurred : {ex.StackTrace}");
            }
        }
    }
}

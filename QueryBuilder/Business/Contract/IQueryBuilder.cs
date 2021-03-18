using SQLQueryBuilder.Models;

namespace SQLQueryBuilder.Business.Contract
{
    public interface IQueryBuilder
    {
        /// <summary>
        /// Create the SQL query from Json File
        /// </summary>
        /// <param name="jsonData">
        /// The json data file
        /// </param>
        /// <returns> SQL query </returns>
        public string CreateSqlQuery(JsonData jsonData);

        /// <summary>
        /// Reads the json file from path
        /// </summary>
        /// <param name="path">path to the json file</param>
        /// <returns>File read</returns>
        public string ReadJsonFIle(string path);
    }

}

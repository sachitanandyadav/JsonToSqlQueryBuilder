using SQLQueryBuilder.Business.Contract;
using SQLQueryBuilder.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SQLQueryBuilder.Business.Concrete
{
    public class QueryBuilder : IQueryBuilder
    {
        static Dictionary<string, string> operatorSigns = new Dictionary<string, string> {
                {"NOTEQUALS","!=" },
                {"EQUALS","=" },
                {"LESSTHAN","<" },
                {"LESSTHANEQUAL","<=" },
                {"GREATERTHAN",">" },
                {"GREATERTHANEQUAL",">=" },
                {"LIKE","Like" },
                {"NOT","<>" },
            };

        #region PublicMethods
        /// <summary>
        /// Create the SQL query from Json File
        /// </summary>
        /// <param name="jsonData">
        /// The json data file
        /// </param>
        /// <returns> SQL query </returns>
        public string CreateSqlQuery(JsonData jsonData)
        {
            try
            {
                var sqlQuery = new StringBuilder();
                sqlQuery.Append("SELECT ");
                foreach (var item in jsonData.Selects)
                {
                    sqlQuery.Append($"{ item.TableName}.{item.FieldName} ,");
                }
                string query = sqlQuery.ToString();
                query = query.Remove(query.LastIndexOf(','));
                query += "\nFROM " + jsonData.TableName;
                query += "\n";

                //check if joins are there
                if (jsonData.Joins.Any())
                {
                    query = WriteJoinQueries(jsonData, query);
                }

                //check For where conditions
                if (jsonData.Where.Columns.Any())
                {
                    var columns = jsonData.Where.Columns.ToList();
                    query += "WHERE ";
                    query = WriteInClauseQuery(columns, query);
                    var opererators = columns.Where(x => x.Operator != "IN").Select(x => x.Operator).Distinct();
                    foreach (var item in opererators)
                    {
                        query = WriteWhereClauseQueries(query, columns, item.ToUpper());
                    }
                }
                return query;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred in building query {ex.StackTrace}");
                throw;
            }
        }

        /// <summary>
        /// Reads the json file from path
        /// </summary>
        /// <param name="path">path to the json file</param>
        /// <returns>File read</returns>
        public string ReadJsonFIle(string path)
        {
            try
            {
                return File.ReadAllText(path);
            }
            catch (IOException ioException)
            {
                Console.WriteLine($"IO exception occurred : {ioException.StackTrace}");
                throw;
            }
        } 

        #endregion
        #region PrivateMethods
        private string WriteJoinQueries(JsonData newJsonData, string query)
        {
            foreach (var item in newJsonData.Joins)
            {
                switch (item.JoinType)
                {
                    case JoinType.Inner:
                        query += $"INNER JOIN {item.SecondTableName} ON {newJsonData.TableName}.{item.PrimaryKey} = {item.SecondTableName}.{item.ForeingKey}\n";
                        break;

                    case JoinType.Left:
                        query += $"LEFT JOIN {item.SecondTableName} ON  {newJsonData.TableName}.{item.PrimaryKey} = {item.SecondTableName}.{item.ForeingKey}\n";
                        break;

                    case JoinType.Right:
                        query += $"RIGHT JOIN {item.SecondTableName} ON  {newJsonData.TableName}.{item.PrimaryKey} = {item.SecondTableName}.{item.ForeingKey}\n";
                        break;

                    default:
                        break;
                }
            }

            return query;
        }
        private string WriteWhereClauseQueries(string query, List<Column> columnsInClause, string operatorName)
        {
            var clause = columnsInClause.Where(x => x.Operator.Equals(operatorName, StringComparison.OrdinalIgnoreCase)).ToList();
            if (clause.Any())
            {
                query += " AND ";
                var numberOfClause = clause.Count;

                foreach (var item in clause)
                {
                    if (numberOfClause <= 1)
                    {

                        query = CheckOperatorCase(query, operatorName, item);
                    }
                    else
                    {
                        numberOfClause--;
                        query = CheckOperatorCase(query, operatorName, item);
                        query += " AND ";
                    }
                }

            }

            return query;
        }
        private string CheckOperatorCase(string query, string operatorName, Column item)
        {
            switch (operatorName)
            {
                case "LIKE":
                    query += $" {item.FieldName} {operatorSigns[operatorName]} '%{item.FieldValue}'\n";
                    break;

                case "BETWEEN":
                    query += $" {item.FieldName} BETWEEN {GetDataType(item.FieldValue)} AND {GetDataType(item.RangeValue)}  \n";
                    break;

                default:
                    query += $" {item.FieldName} {(!operatorSigns.ContainsKey(operatorName) ? "=" : operatorSigns[operatorName])} {GetDataType(item.FieldValue)}\n";
                    break;
            }

            return query;
        }
        private string WriteInClauseQuery(List<Column> columnsInClause, string query)
        {
            var Clause = columnsInClause.Where(x => x.Operator.Equals("in", StringComparison.OrdinalIgnoreCase)).ToList();
            if (Clause.Any())
            {
                query += $" {Clause.First().FieldName } in (";
                foreach (var item in Clause)
                {
                    query += GetDataType(item.FieldValue) + ", ";
                }
                query = query.Remove(query.LastIndexOf(','));
                query += ") ";
            }
            return query;
        }
        private dynamic GetDataType(object val)
        {
            if (val.GetType() == typeof(Int64))
            {
                return val;
            }
            if (val.GetType() == typeof(string))
            {
                return $"'{val}'"; ;
            }
            if (val.GetType() == typeof(DateTime))
            {
                return val;
            }
            return null;
        }


        #endregion
    }
}

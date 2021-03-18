using System.Collections.Generic;

namespace SQLQueryBuilder.Models
{

    public class JsonData
    {
        /// <summary>
        /// Gets or Sets TableName.
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// Gets or Sets where condition.
        /// </summary>
        public Where Where { get; set; }
        /// <summary>
        /// Gets or Sets TableName for joins along with foreign key and primary key.
        /// </summary>
        public List<Join> Joins { get; set; }
        /// <summary>
        /// Gets or Sets list of columns in Having clause.
        /// </summary>
        public List<Column> Havings { get; set; }
        /// <summary>
        /// Gets or Sets list of columns in group by clause.
        /// </summary>
        public List<Column> Orderbys { get; set; }
        /// <summary>
        /// Gets or Sets list of columns in group by clause.
        /// </summary>
        public List<Select> GroupBys { get; set; }
        /// <summary>
        /// Gets or Sets list of columns in select.
        /// </summary>
        public List<Select> Selects  { get; set; }
    }


}

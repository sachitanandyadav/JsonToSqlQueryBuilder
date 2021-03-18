namespace SQLQueryBuilder.Models
{
    public class Join
    {
        /// <summary>
        /// Gets or Sets Join type
        /// </summary>
        public JoinType JoinType { get; set; }
        /// <summary>
        /// Gets or Sets Second Table name in join query
        /// </summary>
        public string SecondTableName { get; set; }
        /// <summary>
        /// Gets or Sets primary key name
        /// </summary>
        public string PrimaryKey { get; set; }
        /// <summary>
        /// Gets or Sets foreign key name 
        /// </summary>
        public string ForeingKey { get; set; }
    }


}

namespace SQLQueryBuilder.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Column: Select
    {
        /// <summary>
        /// Gets or Sets Operator
        /// </summary>
        public string Operator { get; set; }
        /// <summary>
        /// Gets or Sets field value
        /// </summary>
        public object FieldValue { get; set; }
        /// <summary>
        /// Gets or Sets Range value
        /// </summary>
        public object RangeValue{ get; set; }
    }


}

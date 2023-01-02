using System.ComponentModel.DataAnnotations;

namespace sqliteinwebapi
{
    public class Customer
    {
        [Key]
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public int Age { get; set; }
    }
}

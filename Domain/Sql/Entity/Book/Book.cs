namespace Domain.Sql.Entity
{
    public class Book : Base
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsExist { get; set; }
    }
}

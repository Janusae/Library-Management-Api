namespace Domain.Sql.Entity.Book
{
    public class Book 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsExist { get; set; }
        public bool IsDeleted { get; set; }
    }
}

namespace WebApplication3.Entities
{
    public class Student:BaseEntity
    {
        
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public School School { get; set; }
        public int SchoolId { get; set; }//navigation property, foreign key

    }
}

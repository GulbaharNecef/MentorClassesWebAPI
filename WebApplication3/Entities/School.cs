namespace WebApplication3.Entities
{
    public class School:BaseEntity
    {
        
        public string Number { get; set; }
        public string Name { get; set; }    
        public ICollection<Student> Students { get; set; }//navigation property

    }
}

using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Entities
{
    public class School:BaseEntity
    {
        //[MaxLength(300)] Data annotation
        public string Number { get; set; }
        public string Name { get; set; }    
        public ICollection<Student> Students { get; set; }//navigation property

    }
}

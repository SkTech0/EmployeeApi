using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class District
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public int StateId { get; set; }

        public State State { get; set; }

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}

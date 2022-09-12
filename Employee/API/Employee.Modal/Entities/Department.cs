using Employee.Modal.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Employee.Modal.Entities
{
    public class Department : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string DepartmentName { get; set; }
        public virtual ICollection<EmployeeEntity> Employees { get; set; }
    }
}

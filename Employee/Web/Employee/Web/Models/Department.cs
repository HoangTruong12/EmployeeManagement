using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Web.Models;

namespace Web.Models
{
    public class Department
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string DepartmentName { get; set; }
        public int Value { get; set; }
        /// <summary>  
        /// get and set the text  
        /// </summary>  
        public string Text { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
    }
}

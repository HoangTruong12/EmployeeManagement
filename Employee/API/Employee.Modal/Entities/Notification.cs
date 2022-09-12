using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Employee.Modal.Interfaces;

namespace Employee.Modal.Entities
{
    public class Notification : IEntity
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Sender { get; set; }
        public string Reciver { get; set; }
        [DataType(DataType.Date)]
        public string DateCreate { get; set; }
    }
}

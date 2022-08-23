using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Web.Models;

namespace Test.Modal.Entities
{
    public class Notification
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

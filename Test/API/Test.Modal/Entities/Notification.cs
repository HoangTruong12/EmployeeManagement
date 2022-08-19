using System;
using System.Collections.Generic;
using System.Text;
using Test.Modal.Interfaces;

namespace Test.Modal.Entities
{
    public class Notification : IEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Sender { get; set; }
        public int Reciver { get; set; }
        public Employee Employee { get; set; }
    }
}

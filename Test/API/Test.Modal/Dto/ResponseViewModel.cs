using System;
using System.Collections.Generic;
using System.Text;
using Test.Modal.Interfaces;

namespace Test.Modal.Dto
{
    public class ResponseViewModel
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
        public string Name { get; set; }

        public string Birthday { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; }
    }
}

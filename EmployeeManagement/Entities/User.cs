﻿using EmployeeManagement.Models;
using System.Text.Json.Serialization;

namespace EmployeeManagement.Entities
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }

        [JsonIgnore]
        public string Password { get; set; }
    }
}
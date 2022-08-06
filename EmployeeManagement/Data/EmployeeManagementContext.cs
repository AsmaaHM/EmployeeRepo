using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EmployeeManagement.Models;
using EmployeeManagement.Entities;

namespace EmployeeManagement.Data
{
    public class EmployeeManagementContext : DbContext
    {
        public EmployeeManagementContext (DbContextOptions<EmployeeManagementContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employee { get; set; }
        public DbSet<User> User { get; set; }

    }
}

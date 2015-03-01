using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;

namespace Sqlite_EntityFramework.Databases
{
    internal class CourseraContext : DbContext
    {
        public DbSet<Course> Courses { get; set; }

        public DbSet<Student> Students { get; set; }
    }
}
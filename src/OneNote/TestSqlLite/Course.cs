using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sqlite_EntityFramework.Databases
{
    internal class Course
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public virtual List<Student> Students { get; set; }
    }
}

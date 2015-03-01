using Sqlite_EntityFramework.Databases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSqlLite
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new CourseraContext())
            {
                //Notification notif = new Notification() { Criticity = CriticityEnum.High, NotificationType = NotificationTypeEnum.Message, Sender = "My ass !", Text = "Go fuck yourself !" };
                //db.Notifications.Add(notif);
                db.Courses.Count();
                db.SaveChanges();
            }
        }
    }
}

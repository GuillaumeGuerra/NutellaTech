using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanbanBoard.Entities
{
    public class UserStoryCreator
    {
        private List<string> Colors { get; set; }
        private List<string> IWantTo { get; set; }
        private List<string> InOrderTo { get; set; }
        private List<string> Epic { get; set; }
        private List<string> UsId { get; set; }
        private List<string> Project { get; set; }
        private List<bool> StopDev { get; set; }
        public Random Rand { get; set; }

        public UserStoryCreator()
        {
            Colors = new List<string>() { "LightGray", "LightSteelBlue", "LightCoral", "LightYellow" };
            IWantTo = new List<string>() { "Update the component", "Fix the bug ABC", "Be able to view the results", "Have a button to show details" };
            InOrderTo = new List<string>() { "See computation details", "Explain the computation", "Enhance the user experience", "Save some money" };
            Epic = new List<string>() { "Infragistics 2013 migration", "Stress visual analysis", "RAD 13", "CFH Daily Engine" };
            UsId = new List<string>() { "HISTO.10.4", "RAD13.5.3", "LIQP4.3.12", "LSR2.56.4" };
            Project = new List<string>() { "HISTO", "RAD13", "LIQP4", "LSR2" };
            StopDev = new List<bool>() { true, false };
            Rand = new Random();
        }

        public UserStory GetNewUserStory()
        {
            return new UserStory()
            {
                Color = Colors[Rand.Next(4)],
                Epic = Epic[Rand.Next(4)],
                InOrderTo = InOrderTo[Rand.Next(4)],
                IWantTo = IWantTo[Rand.Next(4)],
                Project = Project[Rand.Next(4)],
                UsId = UsId[Rand.Next(4)],
                IsStopDev = StopDev[Rand.Next(2)],
                StartDate = new DateTime(2013, 12, Rand.Next(30) + 1),
                EndDate = new DateTime(2014, 12, Rand.Next(30) + 1)
            };
        }
    }
}

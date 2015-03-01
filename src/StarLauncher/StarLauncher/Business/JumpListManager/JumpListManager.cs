using StarLauncher.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Shell;

namespace StarLauncher.Business
{
    [Export(typeof(IJumpListManager))]
    public class JumpListManager : IJumpListManager
    {
        public void UpdateJumpList(List<StarEnvironment> environments)
        {
            JumpList jumpList = new JumpList();
            JumpList.SetJumpList(App.Current, jumpList);

            jumpList.ShowFrequentCategory = false;
            jumpList.ShowRecentCategory = true;

            foreach (var env in environments)
            {
                var task = GetJumpTask(env);

                jumpList.JumpItems.Add(task);
            }

            jumpList.Apply();

        }

        public void UpdateRecentList(StarEnvironment environment)
        {
            JumpList.AddToRecentCategory(GetJumpTask(environment));
        }

        private JumpTask GetJumpTask(StarEnvironment env)
        {
            var task = new JumpTask();

            task.ApplicationPath = Assembly.GetEntryAssembly().CodeBase;
            task.Title = env.Name;
            task.IconResourcePath = task.ApplicationPath;
            task.Description = "toutotu";
            task.Arguments = "\"" + env.SourceDirectoryName + "\"";
            task.CustomCategory = "Environments";
            task.WorkingDirectory = Environment.CurrentDirectory;

            return task;
        }
    }
}

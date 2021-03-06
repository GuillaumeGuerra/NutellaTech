﻿using System;
using System.Threading;
using PatchManager.Framework;
using PatchManager.Model.Services;
using PatchManager.Models;

namespace PatchManager.Services.Gerrit
{
    public class DemoGerritService : IGerritService
    {
        public GerritInformation GetGerritInformation(int gerritId)
        {
            WaitHelper.Sleep();

            var random = new Random((int)DateTime.Now.Ticks);

            var owners = new[] { "Obi-Wan Kenobi", "Luke Skywalker", "Han Solo", "Leia Organa", "R2D2", "C3PO" };
            var titles = new[]
            {
                "Code cleanup : Jar-Jar has been erased",
                "Luke is now able to handle a lightsaber",
                "Han has been upgraded, he went from scum to rebel hero",
                "New fancy haircut for Leia"
            };
            var values = Enum.GetValues(typeof(GerritStatus));
            return new GerritInformation()
            {
                JiraId = "STW-" + random.Next(100),
                Owner = owners[random.Next(owners.Length)],
                Title = titles[random.Next(titles.Length)],
                Status = (GerritStatus)values.GetValue(random.Next(values.Length))
            };
        }

        public bool Merge(int gerritId)
        {
            WaitHelper.Sleep();

            return true;
        }
    }
}
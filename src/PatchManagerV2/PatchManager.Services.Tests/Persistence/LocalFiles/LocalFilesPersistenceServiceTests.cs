using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using PatchManager.Config;
using PatchManager.Models;
using PatchManager.Services.Persistence;
using PatchManager.TestFramework.Context;
using PatchManager.TestFramework.Utils;
using TestStatus = PatchManager.Models.TestStatus;

namespace PatchManager.Services.Tests.Persistence.LocalFiles
{
    [TestFixture]
    public class LocalFilesPersistenceServiceTests
    {
        [Test]
        public void ShouldLoadNoPatchesWhenDirectoryIsEmpty()
        {
            RunTestOnTemporaryDirectory((service, directory) =>
            {
                var allReleases = service.GetAllReleases();
                Assert.That(allReleases, Is.Not.Null);
                Assert.That(allReleases.Count(), Is.EqualTo(0));
            });
        }

        [Test]
        public void ShouldLoadNoPatchesWhenNoFileWithRightPatternAreThere()
        {
            RunTestOnTemporaryDirectory((service, directory) =>
            {
                // File should have .json extension, this one has to be ignored by the service
                File.WriteAllText(Path.Combine(directory.Location, "wrongName.wrongExtension"), "{\"version\":\"42.0\"}");

                var allReleases = service.GetAllReleases();
                Assert.That(allReleases, Is.Not.Null);
                Assert.That(allReleases.Count(), Is.EqualTo(0));
            });
        }

        [Test]
        public void ShouldThrowWhenDirectoryIsUnknown()
        {
            var context = new PatchManagerContextMock()
                .WithSettings(new SettingsConfiguration()
                {
                    PersistenceDirectoryPath = "d:/these_are_not_the_droids_you_re_looking_for"
                });

            var ex = Assert.Throws<InvalidOperationException>(() => new LocalFilesPersistenceService(context));
            Assert.That(ex.Message, Is.EqualTo("Unknown directory for persistence [d:/these_are_not_the_droids_you_re_looking_for]"));
        }

        [Test]
        public void ShouldThrowWhenOneReleaseCannotBeRead()
        {
            RunTestOnTemporaryDirectory((service, directory) =>
            {
                File.WriteAllText(Path.Combine(directory.Location, "20.2.json"), "even Yoda can't read this json ...");
                
                var ex = Assert.Throws<InvalidOperationException>(() => service.GetAllReleases().ToList());
                Assert.That(ex.Message, Is.EqualTo($"Unable to read json content from file [{Path.Combine(directory.Location, "20.2.json")}]"));
            });
        }

        [Test]
        public void ShouldLoadReleasesAndPatchesWhenFilesCanBeLoaded()
        {
            var context = new PatchManagerContextMock()
                   .WithSettings(new SettingsConfiguration()
                   {
                       PersistenceDirectoryPath = "Persistence/LocalFiles/Data/Folder1"
                   });

            var service = new LocalFilesPersistenceService(context);

            var actualReleases = service.GetAllReleases().OrderBy(release => release.Version).ToList();
            Assert.That(actualReleases.Count, Is.EqualTo(3));

            var first = actualReleases[0];
            Assert.That(first.Version, Is.EqualTo("42.0"));
            Assert.That(first.Date, Is.EqualTo(new DateTime(2015, 12, 16))); // Star Wars VII
            Assert.That(first.IsCurrent, Is.False);
            Assert.That(first.ReleaseManager, Is.EqualTo("Yoda"));
            Assert.That(first.ReleaseManagerMail, Is.EqualTo("yoda@the-green-dwarf-with-the-laser-tooth-stick.com"));

            // This one has no corresponding directory, so we don't expect to load any patches
            Assert.That(service.GetPatches("42.0").Count(), Is.EqualTo(0));


            var second = actualReleases[1];
            Assert.That(second.Version, Is.EqualTo("42.1"));
            Assert.That(second.Date, Is.EqualTo(new DateTime(2016, 12, 14))); // Star Wars Rogue One
            Assert.That(second.IsCurrent, Is.False);
            Assert.That(second.ReleaseManager, Is.EqualTo("Obiwan"));
            Assert.That(second.ReleaseManagerMail, Is.EqualTo("obiwan@the-actor-has-a-beer-name.com"));

            // This one has a corresponding directory but with no files, no patches for this one too
            Assert.That(service.GetPatches("42.1").Count(), Is.EqualTo(0));


            var third = actualReleases[2];
            Assert.That(third.Version, Is.EqualTo("42.2"));
            Assert.That(third.Date, Is.EqualTo(new DateTime(2017, 12, 15))); // Star Wars VIII
            Assert.That(third.IsCurrent, Is.True);
            Assert.That(third.ReleaseManager, Is.EqualTo("Luke"));
            Assert.That(third.ReleaseManagerMail, Is.EqualTo("luke@i-dont-really-walk-among-the-clouds.com"));

            // This one has a corresponding directory with actual files, we'll load patches
            var actualPatches = service.GetPatches("42.2").OrderBy(patch => patch.Gerrit.Id).ToList();
            Assert.That(actualPatches.Count(), Is.EqualTo(2));

            var firstPatch = actualPatches[0];
            Assert.That(firstPatch.Owner, Is.EqualTo("Rey"));
            Assert.That(firstPatch.Gerrit, Is.Not.Null);
            Assert.That(firstPatch.Gerrit.Id, Is.EqualTo(123));
            Assert.That(firstPatch.Gerrit.Description, Is.EqualTo("Did you know that Jar-Jar ruined the entire trilogy ??"));
            Assert.That(firstPatch.Gerrit.Author, Is.EqualTo("Finn"));
            Assert.That(firstPatch.Jira, Is.Null);
            Assert.That(firstPatch.Status, Is.Not.Null);
            Assert.That(firstPatch.Status.Registration, Is.EqualTo(RegistrationStatus.Accepted));
            Assert.That(firstPatch.Status.Jira, Is.EqualTo(JiraStatus.Unknown));
            Assert.That(firstPatch.Status.Gerrit, Is.EqualTo(GerritStatus.Merged));
            Assert.That(firstPatch.Status.Test, Is.EqualTo(TestStatus.ToTest));
            Assert.That(firstPatch.Asset, Is.EqualTo(RiskOneAsset.Core));

            // TODO
            var secondPatch = actualPatches[1];
            Assert.That(secondPatch.Owner, Is.EqualTo("Poe"));
            Assert.That(secondPatch.Gerrit, Is.Not.Null);
            Assert.That(secondPatch.Gerrit.Id, Is.EqualTo(456));
            Assert.That(secondPatch.Gerrit.Description, Is.Null);
            Assert.That(secondPatch.Gerrit.Author, Is.Null);
            Assert.That(secondPatch.Jira, Is.Not.Null);
            Assert.That(secondPatch.Jira.Id, Is.EqualTo("STW-42"));
            Assert.That(secondPatch.Jira.Description, Is.EqualTo("Star Trek sucks"));
            Assert.That(secondPatch.Status, Is.Not.Null); // It's not in the json, but the status should have default values
            Assert.That(secondPatch.Status.Registration, Is.EqualTo(RegistrationStatus.Unknown));
            Assert.That(secondPatch.Status.Jira, Is.EqualTo(JiraStatus.Unknown));
            Assert.That(secondPatch.Status.Gerrit, Is.EqualTo(GerritStatus.Unknown));
            Assert.That(secondPatch.Status.Test, Is.EqualTo(TestStatus.Unknown));
            Assert.That(secondPatch.Asset, Is.EqualTo(RiskOneAsset.Official));
        }

        [Test]
        public void ShouldCreateDirectoryAndAddFileWhenAddingFileToDirectory()
        {
            RunTestOnTemporaryDirectory((service, directory) =>
            {
                // To have a consistent setup
                File.WriteAllText(Path.Combine(directory.Location, "42.0.json"), "{\"version\":\"42.0\"}");

                Assert.That(Directory.Exists(Path.Combine(directory.Location, "42.0")), Is.False);
                var allReleases = service.GetAllReleases().ToList();
                Assert.That(allReleases.Count, Is.EqualTo(1));

                // TODO : add properties
                var addedFirstPatch = new Patch()
                {
                    Gerrit = new Models.Gerrit()
                    {
                        Id = 123
                    }
                };
                service.AddPatchToRelease(allReleases[0],addedFirstPatch);

                // The patches directory should have been created by now
                Assert.That(Directory.Exists(Path.Combine(directory.Location, "42.0")), Is.False);
                
                // TODO : assert a file exists, with the right name
                // TODO : test the content of the patch
                // TODO : add another patch and assert it (file and content)
            });
        }

        private void RunTestOnTemporaryDirectory(Action<LocalFilesPersistenceService,TemporaryDirectory> action)
        {
            using (var directory = new TemporaryDirectory())
            {
                var context = new PatchManagerContextMock()
                    .WithSettings(new SettingsConfiguration()
                    {
                        PersistenceDirectoryPath = directory.Location
                    });

                var service = new LocalFilesPersistenceService(context);

                action(service, directory);
            }
        }
    }
}

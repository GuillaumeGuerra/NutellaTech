using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using PatchManager.Config;
using PatchManager.Framework;
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

                var expectedPatchesDirectory = Path.Combine(directory.Location, "42.0");

                // It's important to make sure the related directory doesn't exist
                // We expect the service to create it, when the first patch is added
                Assert.That(Directory.Exists(expectedPatchesDirectory), Is.False);
                var allReleases = service.GetAllReleases().ToList();
                Assert.That(allReleases.Count, Is.EqualTo(1));

                // So far, we should have no patch in it
                Assert.That(service.GetPatches("42.0").Count(), Is.EqualTo(0));

                var expectedAddedFirstPatch = new Patch()
                {
                    Gerrit = new Models.Gerrit()
                    {
                        Id = 123,
                        Description = "I killed my father, should I kiss my mother now ?",
                        Author = "Kylo Ren"
                    },
                    Asset = RiskOneAsset.Pricing,
                    Jira = new Models.Jira()
                    {
                        Id = "STW-42",
                        Description = "Oedipus complex !!!"
                    },
                    Status = new PatchStatus()
                    {
                        Jira = JiraStatus.InProgress,
                        Gerrit = GerritStatus.MissingBuild,
                        Registration = RegistrationStatus.Refused,
                        Test = TestStatus.ToTest
                    },
                    Owner = "Leader Snoke"
                };
                service.AddPatchToRelease(allReleases[0], expectedAddedFirstPatch);

                // The patches directory should have been created by now
                Assert.That(Directory.Exists(expectedPatchesDirectory), Is.True);
                // We should also have created the json file, named after the gerrit id
                Assert.That(File.Exists(Path.Combine(expectedPatchesDirectory, "123.json")), Is.True);
                // No other files than this one are expected
                Assert.That(Directory.GetFiles(expectedPatchesDirectory).Length, Is.EqualTo(1));

                // Let's make sure the load of the patch happens properly
                var actualPatches = service.GetPatches("42.0").ToList();
                Assert.That(actualPatches.Count, Is.EqualTo(1));
                var actualAddedFirstPatch = actualPatches[0];
                ComparePatches(expectedAddedFirstPatch, actualAddedFirstPatch);

                // Now we'll add another one, to check the addition works also when the directory already exists
                var expectedAddedSecondPatch = new Patch()
                {
                    Gerrit = new Models.Gerrit()
                    {
                        Id = 321,
                        Description = "Did you see how I woke up at the exact perfect moment ??",
                        Author = "R2D2"
                    },
                    Asset = RiskOneAsset.Pricing,
                    Jira = new Models.Jira()
                    {
                        Id = "STW-42",
                        Description = "I had a long night, now I'm ready for episode VIII and IX"
                    },
                    Status = new PatchStatus()
                    {
                        Jira = JiraStatus.Approved,
                        Gerrit = GerritStatus.BuildFailed,
                        Registration = RegistrationStatus.Reverted,
                        Test = TestStatus.Issue
                    },
                    Owner = "C3PO"
                };
                service.AddPatchToRelease(allReleases[0], expectedAddedSecondPatch);

                // Once again, we should have created the file
                Assert.That(File.Exists(Path.Combine(expectedPatchesDirectory, "321.json")), Is.True);
                // The previous one should still be there
                Assert.That(File.Exists(Path.Combine(expectedPatchesDirectory, "123.json")), Is.True);
                // And we should have no other files
                Assert.That(Directory.GetFiles(expectedPatchesDirectory).Length, Is.EqualTo(2));

                actualPatches = service.GetPatches("42.0").OrderBy(patch => patch.Gerrit.Id).ToList();
                Assert.That(actualPatches.Count, Is.EqualTo(2));

                // To make sure the first patch hasn't been altered, we'll assert both patch again
                ComparePatches(expectedAddedFirstPatch, actualPatches[0]);
                ComparePatches(expectedAddedSecondPatch, actualPatches[1]);
            });
        }

        [Test]
        public void ShouldOverridePatchFileContentWhenUpdatingAPatch()
        {
            RunTestOnTemporaryDirectory((service, directory) =>
            {
                // To have a consistent setup
                File.WriteAllText(Path.Combine(directory.Location, "42.0.json"), "{\"version\":\"42.0\"}");
                Directory.CreateDirectory(Path.Combine(directory.Location, "42.0"));

                // Now we create one file for the patch (id=123) that we'll update
                var initialFileContent = "{\"gerrit\":{\"id\":123}}";
                var patchDirectoryPath = Path.Combine(directory.Location, "42.0");
                var initialFilePath = Path.Combine(patchDirectoryPath, "123.json");
                File.WriteAllText(initialFilePath, initialFileContent);

                // To make sure the release setup is ok
                var actualReleases = service.GetAllReleases().ToList();
                Assert.That(actualReleases.Count, Is.EqualTo(1));
                Assert.That(actualReleases[0].Version, Is.EqualTo("42.0"));

                // Same for the patch one
                var actualPatches = service.GetPatches("42.0").ToList();
                Assert.That(actualPatches.Count, Is.EqualTo(1));
                Assert.That(actualPatches[0].Gerrit.Id, Is.EqualTo(123));

                var expectedUpdatePatch = new Patch()
                {
                    Gerrit = new Models.Gerrit()
                    {
                        Id = 123, // Make sure we have the same patch id, otherwise the update won't work
                        Description = "I have a nice way to move, but it ain't easy for stairs",
                        Author = "BB-8"
                    },
                    Asset = RiskOneAsset.Pricing,
                    Jira = new Models.Jira()
                    {
                        Id = "STW-42",
                        Description = "I'm basically disabled :("
                    },
                    Status = new PatchStatus()
                    {
                        Jira = JiraStatus.InProgress,
                        Gerrit = GerritStatus.MissingBuild,
                        Registration = RegistrationStatus.Refused,
                        Test = TestStatus.ToTest
                    },
                    Owner = "Poe Dameron"
                };

                service.UpdateReleasePatch(new Release() { Version = "42.0" }, expectedUpdatePatch);

                // The file should still be there
                Assert.That(File.Exists(initialFilePath), Is.True);
                // We should not have created new files
                Assert.That(Directory.GetFiles(patchDirectoryPath).Length, Is.EqualTo(1));
                // Its content should have changed
                Assert.That(File.ReadAllText(initialFilePath), Is.Not.EqualTo(initialFileContent));

                actualPatches = service.GetPatches("42.0").ToList();
                Assert.That(actualPatches.Count, Is.EqualTo(1));
                ComparePatches(expectedUpdatePatch, actualPatches[0]);
            });
        }

        [Test]
        public void ShouldThrowWhenCreatingTheSamePatchTwice()
        {
            RunTestOnTemporaryDirectory((service, directory) =>
            {
                // To have a consistent setup
                File.WriteAllText(Path.Combine(directory.Location, "42.0.json"), "{\"version\":\"42.0\"}");
                Directory.CreateDirectory(Path.Combine(directory.Location, "42.0"));

                // Now we create one file for the patch (id=123) that we'll update
                File.WriteAllText(Path.Combine(directory.Location, "42.0/123.json"), "{\"gerrit\":{\"id\":123}}");

                // To make sure the release setup is ok
                var actualReleases = service.GetAllReleases().ToList();
                Assert.That(actualReleases.Count, Is.EqualTo(1));
                Assert.That(actualReleases[0].Version, Is.EqualTo("42.0"));

                // Same for the patch one
                var actualPatches = service.GetPatches("42.0").ToList();
                Assert.That(actualPatches.Count, Is.EqualTo(1));
                Assert.That(actualPatches[0].Gerrit.Id, Is.EqualTo(123));

                // Now we'll try to add another patch with the same gerrit id
                var patchAddedTwice = new Patch()
                {
                    Gerrit = new Models.Gerrit()
                    {
                        Id = 123 // To make sure it's the same id
                    }
                };
                var ex = Assert.Throws<InvalidOperationException>(() => service.AddPatchToRelease(actualReleases[0], patchAddedTwice));
                Assert.That(ex.Message, Is.EqualTo("Patch [123] has already been added to release [42.0]"));
            });
        }

        [Test]
        public void ShouldThrowWhenUpdatingAnUnknownPatch()
        {
            RunTestOnTemporaryDirectory((service, directory) =>
            {
                // To have a consistent setup
                File.WriteAllText(Path.Combine(directory.Location, "42.0.json"), "{\"version\":\"42.0\"}");
                Directory.CreateDirectory(Path.Combine(directory.Location, "42.0"));
                
                // To make sure the release setup is ok
                var actualReleases = service.GetAllReleases().ToList();
                Assert.That(actualReleases.Count, Is.EqualTo(1));
                Assert.That(actualReleases[0].Version, Is.EqualTo("42.0"));

                // Same for the patch one (we shouldn't have any)
                var actualPatches = service.GetPatches("42.0").ToList();
                Assert.That(actualPatches.Count, Is.EqualTo(0));

                // Now we'll try to add another patch with the same gerrit id
                var unknownPatchUpdated = new Patch()
                {
                    Gerrit = new Models.Gerrit()
                    {
                        Id = 123 // To make sure it's the same id
                    }
                };
                var ex = Assert.Throws<InvalidOperationException>(() => service.UpdateReleasePatch(actualReleases[0], unknownPatchUpdated));
                Assert.That(ex.Message, Is.EqualTo("Unable to update patch [123] for release [42.0], it hasn't been added yet"));
            });
        }


        private void ComparePatches(Patch expected, Patch actual)
        {
            Assert.That(actual.Owner, Is.EqualTo(expected.Owner));

            if (expected.Gerrit != null)
            {
                Assert.That(actual.Gerrit, Is.Not.Null);
                Assert.That(actual.Gerrit.Id, Is.EqualTo(expected.Gerrit.Id));
                Assert.That(actual.Gerrit.Description, Is.EqualTo(expected.Gerrit.Description));
                Assert.That(actual.Gerrit.Author, Is.EqualTo(expected.Gerrit.Author));
            }
            else
                Assert.That(actual.Gerrit, Is.Null);

            if (expected.Jira != null)
            {
                Assert.That(actual.Jira, Is.Not.Null);
                Assert.That(actual.Jira.Id, Is.EqualTo(expected.Jira.Id));
                Assert.That(actual.Jira.Description, Is.EqualTo(expected.Jira.Description));
            }
            else
                Assert.That(actual.Jira, Is.Null);

            if (expected.Status != null)
            {
                Assert.That(actual.Status, Is.Not.Null);
                Assert.That(actual.Status.Registration, Is.EqualTo(expected.Status.Registration));
                Assert.That(actual.Status.Jira, Is.EqualTo(expected.Status.Jira));
                Assert.That(actual.Status.Gerrit, Is.EqualTo(expected.Status.Gerrit));
                Assert.That(actual.Status.Test, Is.EqualTo(expected.Status.Test));
            }
            else
                Assert.That(actual.Status, Is.Null);

            Assert.That(actual.Asset, Is.EqualTo(expected.Asset));
        }

        private void RunTestOnTemporaryDirectory(Action<LocalFilesPersistenceService, TemporaryDirectory> action)
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

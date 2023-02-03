﻿namespace Sdk4me.GraphQL.Tests
{
    [TestClass]
    public class ReleaseTest
    {
        private readonly Sdk4meClient client = Client.Get();

        [TestMethod]
        public void Get()
        {
            DataList<Release> releases = client.Get(Query.Release
                .View(ReleaseView.All)
                .SelectAll()
                .SelectCustomFieldsAttachments(new AttachmentQuery()
                    .SelectAll())
                .SelectNotes(new NoteQuery()
                    .SelectAll())
                .SelectWorkflows(new WorkflowQuery()
                    .SelectAll())
                ).Result;

            Assert.IsNotNull(releases);
            Console.WriteLine($"Count: {releases.Count}");
        }
    }
}
﻿namespace Sdk4me.GraphQL.Tests
{
    [TestClass]
    public class ProjectCategoryTest
    {
        private readonly Sdk4meClient client = Client.Get();

        [TestMethod]
        public void Get()
        {
            DataList<ProjectCategory> projectCategories = client.Get(Query.ProjectCategory
                .View(DefaultView.None)
                .SelectAll()
                .SelectInformationAttachments(new AttachmentQuery()
                    .SelectAll())
                ).Result;

            Assert.IsNotNull(projectCategories);
            Console.WriteLine($"Count: {projectCategories.Count}");
        }
    }
}

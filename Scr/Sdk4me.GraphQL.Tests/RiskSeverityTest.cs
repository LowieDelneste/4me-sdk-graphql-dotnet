﻿namespace Sdk4me.GraphQL.Tests
{
    [TestClass]
    public class RiskSeverityTest
    {
        private readonly Sdk4meClient client = Client.Get();

        [TestMethod]
        public void Get()
        {
            DataList<RiskSeverity> riskSeverities = client.Get(Query.RiskSeverity
                .View(DefaultView.None)
                .SelectAll()
                .SelectInformationAttachments(new AttachmentQuery()
                    .SelectAll())
                .SelectTranslations(new TranslationQuery()
                    .SelectAll()
                    .SelectOwner(new HasTranslationsQuery()
                        .SelectAll()
                        .ItemsPerRequest(1)))
                ).Result;

            Assert.IsNotNull(riskSeverities);
            Console.WriteLine($"Count: {riskSeverities.Count}");
        }
    }
}

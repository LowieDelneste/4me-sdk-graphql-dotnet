﻿namespace Sdk4me.GraphQL.Tests
{
    [TestClass]
    public class HolidayTest
    {
        private readonly Sdk4meClient client = Client.Get();

        [TestMethod]
        public void Get()
        {
            DataList<Holiday> holidays = client.Get(Query.Holiday
                .View(DefaultView.None)
                .SelectAll()
                .SelectCalendars(new CalendarQuery()
                    .SelectAll())
                ).Result;

            Assert.IsNotNull(holidays);
            Console.WriteLine($"Count: {holidays.Count}");
        }
    }
}

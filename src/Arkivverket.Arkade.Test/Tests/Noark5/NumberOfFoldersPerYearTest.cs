﻿using Arkivverket.Arkade.Core.Base;
using Arkivverket.Arkade.Core.Tests.Noark5;
using FluentAssertions;
using Xunit;

namespace Arkivverket.Arkade.Test.Tests.Noark5
{
    public class NumberOfFoldersPerYearTest
    {
        [Fact]
        public void ShouldReturnNumberOfFoldersForSingleYear()
        {
            XmlElementHelper helper = new XmlElementHelper().Add("arkiv",
                new XmlElementHelper()
                    .Add("arkivdel", new XmlElementHelper()
                            .Add("systemID", "someSystemId_1")
                            .Add("klassifikasjonssystem",
                            new XmlElementHelper().Add("klasse",
                                new XmlElementHelper().Add("mappe",
                                    new XmlElementHelper().Add("opprettetDato", "1863-10-18T00:00:00Z"))))));

            TestRun testRun = helper.RunEventsOnTest(new NumberOfFoldersPerYear());

            testRun.Results[0].Message.Should().Contain("1863: 1");
        }

        [Fact]
        public void ShouldReturnNumberOfFoldersForSeveralYears()
        {
            XmlElementHelper helper = new XmlElementHelper().Add("arkiv",
                new XmlElementHelper()
                    .Add("arkivdel",
                        new XmlElementHelper()
                            .Add("systemID", "someSystemId_1").Add("klassifikasjonssystem",
                            new XmlElementHelper().Add("klasse",
                                new XmlElementHelper()
                                    .Add("mappe",
                                        new XmlElementHelper().Add("opprettetDato", "1863-10-18T00:00:00Z"))
                                    .Add("mappe",
                                        new XmlElementHelper().Add("opprettetDato", "1863-10-18T00:00:00Z")))))
                    .Add("arkivdel",
                        new XmlElementHelper().Add("systemID", "someSystemId_2")
                        .Add("klassifikasjonssystem",
                            new XmlElementHelper().Add("klasse",
                                new XmlElementHelper().Add("klasse",
                                    new XmlElementHelper()
                                        .Add("mappe",
                                            new XmlElementHelper().Add("opprettetDato", "1865-10-18T00:00:00Z"))
                                        .Add("mappe",
                                            new XmlElementHelper().Add("opprettetDato", "1864-10-18T00:00:00Z")))))));

            TestRun testRun = helper.RunEventsOnTest(new NumberOfFoldersPerYear());

            testRun.Results[0].Message.Should().Contain("1863: 2");
            testRun.Results[1].Message.Should().Contain("1864: 1");
            testRun.Results[2].Message.Should().Contain("1865: 1");
        }

        [Fact]
        public void ShouldReturnNumberOfFoldersForNoYears()
        {
            XmlElementHelper helper = new XmlElementHelper().Add("arkiv",
                new XmlElementHelper()
                    .Add("arkivdel", new XmlElementHelper()
                            .Add("systemID", "someSystemId_1") 
                            .Add("klassifikasjonssystem", new XmlElementHelper()
                            .Add("klasse", new XmlElementHelper()
                                .Add("mappe", new XmlElementHelper()
                                    .Add("title", "Some title"))))));

            TestRun testRun = helper.RunEventsOnTest(new NumberOfFoldersPerYear());

            testRun.Results.Count.Should().Be(0);
        }

        [Fact]
        public void ShouldReturnTwoFoldersForSeveralYearsInTwoDifferentArchiveParts()
        {
            XmlElementHelper helper = new XmlElementHelper().Add("arkiv",
                new XmlElementHelper()
                    .Add("arkivdel", new XmlElementHelper()
                        .Add("systemID", "someSystemId_1")
                        .Add("klassifikasjonssystem", new XmlElementHelper()
                            .Add("klasse", new XmlElementHelper()
                                .Add("mappe", new XmlElementHelper().Add("opprettetDato", "1863-10-18T00:00:00Z"))
                                .Add("mappe", new XmlElementHelper().Add("opprettetDato", "1863-10-18T00:00:00Z")))))
                    .Add("arkivdel", new XmlElementHelper()
                        .Add("systemID", "someSystemId_2")
                        .Add("klassifikasjonssystem", new XmlElementHelper()
                            .Add("klasse", new XmlElementHelper()
                                .Add("klasse", new XmlElementHelper()
                                    .Add("mappe", new XmlElementHelper().Add("opprettetDato", "1865-10-18T00:00:00Z"))
                                    .Add("mappe",
                                        new XmlElementHelper().Add("opprettetDato", "1864-10-18T00:00:00Z")))))));

            TestRun testRun = helper.RunEventsOnTest(new NumberOfFoldersPerYear());

            testRun.Results.Should().Contain(r => r.Message.Equals("Arkivdel (systemID) someSystemId_1 - 1863: 2"));
            testRun.Results.Should().Contain(r => r.Message.Equals("Arkivdel (systemID) someSystemId_2 - 1864: 1"));
            testRun.Results.Should().Contain(r => r.Message.Equals("Arkivdel (systemID) someSystemId_2 - 1865: 1"));
        }
    }
}

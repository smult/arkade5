﻿using Arkivverket.Arkade.Core.Base;
using Arkivverket.Arkade.Core.Tests.Noark5;
using FluentAssertions;
using Xunit;

namespace Arkivverket.Arkade.Test.Tests.Noark5
{
    public class NumberOfPrecedentsTest
    {
        [Fact]
        public void NumberOfPrecedentsIsZero()
        {
            XmlElementHelper helper = new XmlElementHelper()
                .Add("arkiv", new XmlElementHelper()
                    .Add("arkivdel", new XmlElementHelper()
                        .Add("systemID", "someArchiveSystemId_1")
                        .Add("klassifikasjonssystem", new XmlElementHelper()
                            .Add("klasse", new XmlElementHelper()
                                .Add("mappe", new XmlElementHelper()
                                    .Add("registrering", new[] {"xsi:type", "journalpost"}, new XmlElementHelper()
                                    ))))));

            TestRun testRun = helper.RunEventsOnTest(new NumberOfPrecedents());

            testRun.Results.Should().Contain(r => r.Message.Equals("Antall presedenser i journalposter: 0"));
            testRun.Results.Should().Contain(r => r.Message.Equals("Antall presedenser i saksmapper: 0"));
        }

        [Fact]
        public void NumberOfPrecedentsInJournalpostsIsOneInEachArchivePart()
        {
            XmlElementHelper helper = new XmlElementHelper()
                .Add("arkiv", new XmlElementHelper()
                    .Add("arkivdel", new XmlElementHelper()
                        .Add("systemID", "someSystemId_1")
                        .Add("klassifikasjonssystem", new XmlElementHelper()
                            .Add("klasse", new XmlElementHelper()
                                .Add("mappe", new XmlElementHelper()
                                    .Add("registrering", new[] {"xsi:type", "journalpost"}, new XmlElementHelper()
                                        .Add("presedens", new XmlElementHelper()))
                                    .Add("registrering", new XmlElementHelper()
                                        .Add("presedens", new XmlElementHelper()))))))
                    .Add("arkivdel", new XmlElementHelper()
                        .Add("systemID", "someSystemId_2")
                        .Add("klassifikasjonssystem", new XmlElementHelper()
                            .Add("klasse", new XmlElementHelper()
                                .Add("mappe", new XmlElementHelper()
                                    .Add("registrering", new[] {"xsi:type", "journalpost"}, new XmlElementHelper()
                                        .Add("presedens", new XmlElementHelper())))))));

            TestRun testRun = helper.RunEventsOnTest(new NumberOfPrecedents());

            testRun.Results.Should().Contain(r =>
                r.Message.Equals("Arkivdel (systemID) someSystemId_1 - Antall presedenser i journalposter: 1"));
            testRun.Results.Should().Contain(r =>
                r.Message.Equals("Arkivdel (systemID) someSystemId_2 - Antall presedenser i journalposter: 1"));
        }

        [Fact]
        public void NumberOfPrecedents()
        {
            XmlElementHelper helper = new XmlElementHelper()
                .Add("arkiv", new XmlElementHelper()
                    .Add("arkivdel", new XmlElementHelper()
                        .Add("systemID", "someSystemId_1")
                        .Add("klassifikasjonssystem", new XmlElementHelper()
                            .Add("klasse", new XmlElementHelper()
                                .Add("mappe", new XmlElementHelper()
                                    .Add("registrering", new[] {"xsi:type", "journalpost"}, new XmlElementHelper()
                                        .Add("presedens", new XmlElementHelper()))
                                    .Add("registrering", new[] { "xsi:type", "journalpost" }, new XmlElementHelper()
                                        .Add("presedens", new XmlElementHelper()
                                        )))))))
                .Add("arkivdel", new XmlElementHelper()
                    .Add("systemID", "someSystemId_2")
                    .Add("klassifikasjonssystem", new XmlElementHelper()
                        .Add("klasse", new XmlElementHelper()
                            .Add("mappe", new[] {"xsi:type", "saksmappe"}, new XmlElementHelper()
                                .Add("presedens", new XmlElementHelper()
                                )))));

            TestRun testRun = helper.RunEventsOnTest(new NumberOfPrecedents());

            testRun.Results.Should().Contain(r =>
                r.Message.Equals("Arkivdel (systemID) someSystemId_1 - Antall presedenser i journalposter: 2"));
            testRun.Results.Should().Contain(r =>
                r.Message.Equals("Arkivdel (systemID) someSystemId_2 - Antall presedenser i saksmapper: 1"));
        }
    }
}
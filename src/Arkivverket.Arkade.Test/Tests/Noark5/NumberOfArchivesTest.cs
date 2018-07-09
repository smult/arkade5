using System.Linq;
using Arkivverket.Arkade.Core.Base;
using Arkivverket.Arkade.Core.Tests.Noark5;
using FluentAssertions;
using Xunit;

namespace Arkivverket.Arkade.Test.Tests.Noark5
{
    public class NumberOfArchivesTest
    {
        [Fact]
        public void TwoArchivesAreFound()
        {
            XmlElementHelper helper = new XmlElementHelper()
                .Add("arkiv", string.Empty)
                .Add("arkiv", string.Empty);

            TestRun testRun = helper.RunEventsOnTest(new NumberOfArchives());

            testRun.Results.First().Message.Should().Be("Antall arkiv: 2");

            testRun.Results.Count.Should().Be(1);
        }

        [Fact]
        public void FiveArchivesOnThreeLevelsAreFound()
        {
            XmlElementHelper helper = new XmlElementHelper()
                .Add("arkiv", string.Empty)
                .Add("arkiv", new XmlElementHelper()
                    .Add("arkiv", new XmlElementHelper()
                        .Add("arkiv", string.Empty))
                    .Add("arkiv", string.Empty));

            TestRun testRun = helper.RunEventsOnTest(new NumberOfArchives());

            testRun.Results.Should().Contain(r => r.Message.Equals("Antall arkiv: 5"));
            testRun.Results.Should().Contain(r => r.Message.Equals("Antall arkiv p� niv� 1: 2"));
            testRun.Results.Should().Contain(r => r.Message.Equals("Antall arkiv p� niv� 2: 2"));
            testRun.Results.Should().Contain(r => r.Message.Equals("Antall arkiv p� niv� 3: 1"));

            testRun.Results.Count.Should().Be(4);
        }
    }
}

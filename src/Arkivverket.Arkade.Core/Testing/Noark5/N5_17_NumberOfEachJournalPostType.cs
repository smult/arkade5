using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arkivverket.Arkade.Core.Base.Noark5;
using Arkivverket.Arkade.Core.Resources;
using Arkivverket.Arkade.Core.Util;

namespace Arkivverket.Arkade.Core.Testing.Noark5
{
    public class N5_17_NumberOfEachJournalPostType : Noark5XmlReaderBaseTest
    {
        private readonly TestId _id = new TestId(TestId.TestKind.Noark5, 17);

        private readonly List<JournalPost> _journalPosts = new List<JournalPost>();
        private readonly List<TestResult> _testResults = new List<TestResult>();
        private N5_17_ArchivePart _currentArchivePart = new N5_17_ArchivePart();
        private string _currentJournalPostSystemId;
        private bool _journalPostAttributeIsFound;

        public override TestId GetId()
        {
            return _id;
        }

        public override TestType GetTestType()
        {
            return TestType.ContentAnalysis;
        }

        protected override List<TestResult> GetTestResults()
        {
            var journalPostQuery = from journalPost in _journalPosts
                group journalPost by new
                {
                                       ArchivePartSystemId = journalPost.ArchivePart.SystemId,
                                       ArchivePartName = journalPost.ArchivePart.Name,
                                       journalPost.JournalpostType
                                   }
                into grouped
                select new
                {
                    grouped.Key.ArchivePartSystemId,
                                       grouped.Key.ArchivePartName,
                    grouped.Key.JournalpostType,
                    Count = grouped.Count()
                };


            bool multipleArchiveParts = _journalPosts.GroupBy(j => j.ArchivePart.SystemId).Count() > 1;

            foreach (var item in journalPostQuery)
            {
                var message = new StringBuilder(
                    string.Format(Noark5Messages.NumberOfEachJournalPostTypeMessage_TypeAndCount,
                        item.JournalpostType, item.Count
                    )
                );

                if (multipleArchiveParts)
                    message.Insert(0,
                        string.Format(Noark5Messages.ArchivePartSystemId, item.ArchivePartSystemId, item.ArchivePartName) + " - ");

                _testResults.Add(new TestResult(ResultType.Success, new Location(""), message.ToString()));
            }

            return _testResults;
        }

        protected override void ReadStartElementEvent(object sender, ReadElementEventArgs eventArgs)
        {
        }

        protected override void ReadAttributeEvent(object sender, ReadElementEventArgs eventArgs)
        {
            if (Noark5TestHelper.IdentifiesJournalPostRegistration(eventArgs))
                _journalPostAttributeIsFound = true;
        }

        protected override void ReadElementValueEvent(object sender, ReadElementEventArgs eventArgs)
        {
            if (eventArgs.Path.Matches("systemID", "arkivdel"))
                _currentArchivePart.SystemId = eventArgs.Value;

            if (eventArgs.Path.Matches("tittel", "arkivdel"))
                _currentArchivePart.Name = eventArgs.Value;

            if (eventArgs.Path.Matches("systemID", "registrering") && _journalPostAttributeIsFound)
                _currentJournalPostSystemId = eventArgs.Value;

            if (eventArgs.Path.Matches("journalposttype", "registrering") && _journalPostAttributeIsFound)
                _journalPosts.Add(new JournalPost(eventArgs.Value, _currentArchivePart));

        }

        protected override void ReadEndElementEvent(object sender, ReadElementEventArgs eventArgs)
        {
            if (eventArgs.NameEquals("arkivdel"))
            {
                _currentArchivePart = new N5_17_ArchivePart();
            }

            if (!eventArgs.NameEquals("registrering"))
                return;

            _journalPostAttributeIsFound = false; // reset
            _currentJournalPostSystemId = ""; // reset
        }

        internal class JournalPost
        {
            public N5_17_ArchivePart ArchivePart { get; }
            public string JournalpostType { get; }

            public JournalPost(string journalpostType, N5_17_ArchivePart archivePart)
            {
                JournalpostType = journalpostType;
                ArchivePart = archivePart;
            }
        }

        internal class N5_17_ArchivePart : ArchivePart { }
    }
}

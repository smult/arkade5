using System.Collections.Generic;
using Arkivverket.Arkade.Core.Base.Noark5;
using Arkivverket.Arkade.Core.Resources;
using Arkivverket.Arkade.Core.Util;

namespace Arkivverket.Arkade.Core.Testing.Noark5
{
    public class N5_14_NumberOfFoldersWithoutRegistrationsOrSubfolders : Noark5XmlReaderBaseTest
    {
        private readonly TestId _id = new TestId(TestId.TestKind.Noark5, 14);

        private bool _registrationIsFound;
        private bool _subfolderIsJustProcessed;
        private int _noRegistrationOrSubfolderCount;
        private N5_14_ArchivePart _currentArchivePart = new N5_14_ArchivePart();

        private readonly Dictionary<N5_14_ArchivePart, int> _noRegistrationOrSubfolderCountPerArchivePart =
            new Dictionary<N5_14_ArchivePart, int>();

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
            var testResults = new List<TestResult>
            {
                new TestResult(ResultType.Success, new Location(""), string.Format(Noark5Messages.TotalResultNumber, _noRegistrationOrSubfolderCount))
            };

            if (_noRegistrationOrSubfolderCountPerArchivePart.Count > 1)
            {
                foreach (KeyValuePair<N5_14_ArchivePart, int> noRegistrationOrSubfolderCount in
                    _noRegistrationOrSubfolderCountPerArchivePart)
                {
                    if (noRegistrationOrSubfolderCount.Value > 0)
                    {
                        var testResult = new TestResult(ResultType.Success, new Location(string.Empty),
                            string.Format(Noark5Messages.NumberOf_PerArchivePart, noRegistrationOrSubfolderCount.Key.SystemId, noRegistrationOrSubfolderCount.Key.Name,
                                noRegistrationOrSubfolderCount.Value));

                        testResults.Add(testResult);
                    }
                }
            }
            return testResults;
        }

        protected override void ReadStartElementEvent(object sender, ReadElementEventArgs eventArgs)
        {
            if (eventArgs.NameEquals("registrering"))
                _registrationIsFound = true;
        }

        protected override void ReadAttributeEvent(object sender, ReadElementEventArgs eventArgs)
        {
        }

        protected override void ReadElementValueEvent(object sender, ReadElementEventArgs eventArgs)
        {
            if (eventArgs.Path.Matches("systemID", "arkivdel"))
            {
                _currentArchivePart.SystemId = eventArgs.Value;
                _noRegistrationOrSubfolderCountPerArchivePart.Add(_currentArchivePart, 0);
            }
              
            if (eventArgs.Path.Matches("tittel", "arkivdel"))
                _currentArchivePart.Name = eventArgs.Value;
        }

        protected override void ReadEndElementEvent(object sender, ReadElementEventArgs eventArgs)
        {
            if (eventArgs.NameEquals("arkivdel"))
            {
                _currentArchivePart = new N5_14_ArchivePart();
            }

            if (!eventArgs.NameEquals("mappe"))
                return;

            if (!_registrationIsFound && !_subfolderIsJustProcessed)
            {
                _noRegistrationOrSubfolderCount++;

                if (_noRegistrationOrSubfolderCountPerArchivePart.Count > 0)
                {
                    if (_noRegistrationOrSubfolderCountPerArchivePart.ContainsKey(_currentArchivePart))
                        _noRegistrationOrSubfolderCountPerArchivePart[_currentArchivePart]++;
                }
            }

            _registrationIsFound = false; // Reset

            if (eventArgs.Path.GetParent().Equals("mappe"))
                _subfolderIsJustProcessed = true;
        }

        private class N5_14_ArchivePart : ArchivePart { }
    }
}

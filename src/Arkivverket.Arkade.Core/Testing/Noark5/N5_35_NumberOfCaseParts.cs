using System.Collections.Generic;
using Arkivverket.Arkade.Core.Base;
using Arkivverket.Arkade.Core.Base.Noark5;
using Arkivverket.Arkade.Core.Resources;
using Arkivverket.Arkade.Core.Util;

namespace Arkivverket.Arkade.Core.Testing.Noark5
{
    public class N5_35_NumberOfCaseParts : Noark5XmlReaderBaseTest
    {
        private string _currentArchivePartSystemId;
        private int _totalNumberOfCaseParts;
        private Archive archive;
        private readonly Dictionary<string, int> _casePartsPerArchivePart = new Dictionary<string, int>();
        private readonly TestId _id;

        private string GetTestVersion()
        {
            string standardVersion = archive.Details.ArchiveStandard;

            if (standardVersion.Equals("5.5"))
            {
                return "5.5";
            }

            return standardVersion;
        }

        public N5_35_NumberOfCaseParts(Archive archive)
        {
            this.archive = archive;
            _id = new TestId(TestId.TestKind.Noark5, 35, GetTestVersion());
        }

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
                new TestResult(ResultType.Success, new Location(string.Empty),
                    string.Format(Noark5Messages.TotalResultNumber, _totalNumberOfCaseParts.ToString()))
            };

            if (_casePartsPerArchivePart.Count > 1)
            {
                foreach (KeyValuePair<string, int> casePartCount in _casePartsPerArchivePart)
                {
                    if (casePartCount.Value > 0)
                    {
                        var testResult = new TestResult(ResultType.Success, new Location(string.Empty),
                            string.Format(Noark5Messages.NumberOf_PerArchivePart, casePartCount.Key,
                                casePartCount.Value));

                        testResults.Add(testResult);
                    }
                }
            }

            return testResults;
        }


        protected override void ReadStartElementEvent(object sender, ReadElementEventArgs eventArgs)
        {
        }

        protected override void ReadAttributeEvent(object sender, ReadElementEventArgs eventArgs)
        {
        }

        protected override void ReadElementValueEvent(object sender, ReadElementEventArgs eventArgs)
        {
            if (eventArgs.Path.Matches("systemID", "arkivdel"))
            {
                _currentArchivePartSystemId = eventArgs.Value;
                _casePartsPerArchivePart.Add(_currentArchivePartSystemId, 0);
            }
        }

        protected override void ReadEndElementEvent(object sender, ReadElementEventArgs eventArgs)
        {
            if (archive.Details.ArchiveStandard.Equals("5.5"))
            {
                CountPartsForVersion5_5(eventArgs);
            }

            CountCaseParts(eventArgs);
        }

        private void CountPartsForVersion5_5(ReadElementEventArgs eventArgs)
        {
            if (eventArgs.NameEquals("part"))
            {
                Count();
            }
        }

        private void CountCaseParts(ReadElementEventArgs eventArgs)
        {
            if (eventArgs.NameEquals("sakspart"))
            {
                Count();
            }
        }

        private void Count()
        {
            _totalNumberOfCaseParts++;

            if (_casePartsPerArchivePart.Count > 0)
            {
                if (_casePartsPerArchivePart.ContainsKey(_currentArchivePartSystemId))
                    _casePartsPerArchivePart[_currentArchivePartSystemId]++;
            }
        }
    }
}
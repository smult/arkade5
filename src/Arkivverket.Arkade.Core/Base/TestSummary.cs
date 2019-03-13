using System.Numerics;

namespace Arkivverket.Arkade.Core.Base
{
    public class TestSummary
    {
        public string NumberOfProcessedFiles { get; }
        public string NumberOfProcessedRecords { get; }
        public string NumberOfProcessedRecordsLocal { get; }
        public string NumberOfTestsRun { get; }

        public TestSummary(int numberOfProcessedFiles, int numberOfProcessedRecords, int numberOfProcessedRecordsLocal,
            int numberOfTestsRun)
        {
            NumberOfProcessedFiles = numberOfProcessedFiles.ToString();
            NumberOfProcessedRecords= numberOfProcessedRecords.ToString();
            NumberOfProcessedRecordsLocal = numberOfProcessedRecordsLocal.ToString();
            NumberOfTestsRun = numberOfTestsRun.ToString();
        }

    }
}
using System.Collections.Generic;
using Arkivverket.Arkade.Core;

namespace Arkivverket.Arkade.Tests
{
    public interface ITestProvider
    {
        List<ITest> GetTestsForArchive(Archive archive);
    }
}
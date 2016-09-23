using System;
using System.Xml;
using Arkivverket.Arkade.Core;

namespace Arkivverket.Arkade.Tests.Noark5
{
    public class NumberOfArchives : BaseTest
    {
        public NumberOfArchives(IArchiveContentReader archiveReader) : base(TestType.Content, archiveReader)
        {
        }
        public override string GetName()
        {
            return this.GetType().Name;
        }

        protected override void Test(Archive archive)
        {
            using (var reader = XmlReader.Create(archive.GetContentDescriptionFileName()))
            {
                int counter = 0;
                while (reader.ReadToNextSibling("arkiv"))
                {
                    counter++;
                }
                Console.WriteLine("Number of archives: " + counter);
                TestSuccess($"Found {counter} archives.");
            }
        }

    }
}

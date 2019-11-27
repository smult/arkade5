﻿using System;
using System.Collections.Generic;
using System.IO;
using Arkivverket.Arkade.Core.Base;
using Arkivverket.Arkade.Core.Base.Noark5;
using Arkivverket.Arkade.Core.Resources;
using Arkivverket.Arkade.Core.Util;

namespace Arkivverket.Arkade.Core.Testing.Noark5
{
    public class N5_30_DocumentFilesChecksumControl : Noark5XmlReaderBaseTest
    {
        private readonly TestId _id = new TestId(TestId.TestKind.Noark5, 30);

        private N5_30_ArchivePart _currentArchivePart = new N5_30_ArchivePart();
        private string _currentDocumentDescriptionSystemId;
        private DocumentObject _currentDocumentObject;
        private readonly List<TestResult> _testResults;
        private readonly DirectoryInfo _contentDirectory;


        public N5_30_DocumentFilesChecksumControl(Archive archive)
        {
            _contentDirectory = archive.WorkingDirectory.Content().DirectoryInfo();

            _testResults = new List<TestResult>();
        }

        public override TestId GetId()
        {
            return _id;
        }

        public override TestType GetTestType()
        {
            return TestType.ContentControl;
        }

        protected override List<TestResult> GetTestResults()
        {
            return _testResults;
        }

        protected override void ReadStartElementEvent(object sender, ReadElementEventArgs eventArgs)
        {
            if (eventArgs.NameEquals("dokumentobjekt"))
            {
                _currentDocumentObject = new DocumentObject
                {
                    ArchivePart = _currentArchivePart,
                    DocumentDescriptionSystemId = _currentDocumentDescriptionSystemId
                };
            }
        }

        protected override void ReadAttributeEvent(object sender, ReadElementEventArgs eventArgs)
        {
        }

        protected override void ReadElementValueEvent(object sender, ReadElementEventArgs eventArgs)
        {
            if (eventArgs.Path.Matches("systemID", "arkivdel"))
                _currentArchivePart.SystemId = eventArgs.Value;

            if (eventArgs.Path.Matches("tittel", "arkivdel"))
                _currentArchivePart.Name = eventArgs.Value;

            if (eventArgs.Path.Matches("systemID", "dokumentbeskrivelse"))
                _currentDocumentDescriptionSystemId = eventArgs.Value;

            if (eventArgs.Path.Matches("referanseDokumentfil", "dokumentobjekt"))
                _currentDocumentObject.DocumentFileReference = eventArgs.Value;

            if (eventArgs.Path.Matches("sjekksum", "dokumentobjekt"))
                _currentDocumentObject.Checksum = eventArgs.Value;

            if (eventArgs.Path.Matches("sjekksumAlgoritme", "dokumentobjekt"))
                _currentDocumentObject.ChecksumAlgorithm = eventArgs.Value;
        }

        protected override void ReadEndElementEvent(object sender, ReadElementEventArgs eventArgs)
        {
            if (eventArgs.NameEquals("dokumentobjekt"))
            {
                try
                {
                    if (!ActualAndDocumentedFileChecksumsMatch(_currentDocumentObject))
                    {
                        _testResults.Add(new TestResult(ResultType.Error, new Location(string.Empty),
                            string.Format(Noark5Messages.DocumentFilesChecksumControlMessage,
                                _currentArchivePart.SystemId,
                                _currentArchivePart.Name,
                                _currentDocumentObject.DocumentFileReference,
                                _currentDocumentObject.DocumentDescriptionSystemId
                            )));
                    }
                }
                catch (Exception)
                {
                    _testResults.Add(new TestResult(ResultType.Error, new Location(string.Empty),
                        string.Format(Noark5Messages.FileNotFound, _currentDocumentObject.DocumentFileReference)));
                }

                _currentDocumentObject = null;
                _currentDocumentDescriptionSystemId = null;
            }

            if (eventArgs.NameEquals("arkivdel"))
                _currentArchivePart = new N5_30_ArchivePart();
        }

        private bool ActualAndDocumentedFileChecksumsMatch(DocumentObject documentObject)
        {
            string documentFileName = Path.DirectorySeparatorChar == '/'
                ? documentObject.DocumentFileReference.Replace('\\', '/')
                : documentObject.DocumentFileReference;

            var filePath = Path.Combine(_contentDirectory.FullName, documentFileName);
            
            var actualFileCheckSum = GenerateChecksumForFile(filePath, documentObject.ChecksumAlgorithm);
            return ChecksumsMatch(documentObject.Checksum, actualFileCheckSum);
        }

        private static string GenerateChecksumForFile(string filename, string checksumAlgorithm)
        {
            var generator = new ChecksumGeneratorFactory().GetGenerator(checksumAlgorithm);
            return generator.GenerateChecksum(filename);
        }

        private static bool ChecksumsMatch(string checksumA, string checkSumB)
        {
            return checksumA.ToUpper().Equals(checkSumB.ToUpper());
        }

        private class DocumentObject
        {
            public N5_30_ArchivePart ArchivePart { get; set; }
            public string DocumentDescriptionSystemId { get; set; }
            public string DocumentFileReference { get; set; }
            public string Checksum { get; set; }
            public string ChecksumAlgorithm { get; set; }
        }

        private class N5_30_ArchivePart : ArchivePart { }
    }
}

using System.IO;
using System.Security.Cryptography;
using Arkivverket.Arkade.Core.Base;

namespace Arkivverket.Arkade.Core.Util
{
    public class Sha256ChecksumGenerator : IChecksumGenerator
    {
        private const string AlgorithmName = "SHA256";

        /// <summary>
        ///     Generates a checksum with the SHA256 algorithm
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>The computed hash as a hex string</returns>
        public string GenerateChecksum(string filePath)
        {
            var hashAlgorithm = (HashAlgorithm) CryptoConfig.CreateFromName(AlgorithmName);

            if (hashAlgorithm == null)
                throw new ArkadeException($"Checksum algorithm {AlgorithmName} not supported.");

            byte[] hashValue;

            using (FileStream fileStream = new FileInfo(filePath).OpenRead())
                hashValue = hashAlgorithm.ComputeHash(fileStream);

            return Hex.ToHexString(hashValue);
        }
    }
}

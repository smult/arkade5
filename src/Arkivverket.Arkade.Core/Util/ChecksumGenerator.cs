using System.IO;
using System.Security.Cryptography;
using Arkivverket.Arkade.Core.Base;

namespace Arkivverket.Arkade.Core.Util
{
    public class ChecksumGenerator : IChecksumGenerator
    {
        /// <summary>
        ///     Generates a checksum with the given algorithm
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="algorithmName"></param>
        /// <returns>The computed hash as a hex string</returns>
        public string GenerateChecksum(string filePath, string algorithmName)
        {
            var hashAlgorithm = (HashAlgorithm) CryptoConfig.CreateFromName(algorithmName);

            if (hashAlgorithm == null)
                throw new ArkadeException($"Checksum algorithm {algorithmName} not supported.");

            byte[] hashValue;

            using (FileStream fileStream = new FileInfo(filePath).OpenRead())
                hashValue = hashAlgorithm.ComputeHash(fileStream);

            return Hex.ToHexString(hashValue);
        }
    }
}

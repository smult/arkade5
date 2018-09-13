namespace Arkivverket.Arkade.Core.Util
{
    public class Sha256ChecksumGenerator : ChecksumGenerator
    {
        private const string AlgorithmName = "SHA256";

        /// <summary>
        ///     Generates a checksum with the SHA256 algorithm
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>The computed hash as a hex string</returns>
        public string GenerateChecksum(string filePath)
        {
            return GenerateChecksum(filePath, AlgorithmName);
        }
    }
}

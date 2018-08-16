using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SecurityCourse.Exercise.Console
{
    public class HashGenerator
    {
        private readonly int _proofOfWorkDifficulty;

        public HashGenerator(int proofOfWorkDifficulty)
        {
            _proofOfWorkDifficulty = proofOfWorkDifficulty;
        }

        public void HashWithProofOfWork(Block block)
        {
            block.Nounce = 0;

            var hash = ComputeHash(block);

            while (!IsProofOfWork(hash))
            {
                block.Nounce++;
                hash = ComputeHash(block);
            }
        }

        public bool IsProofOfWork(string hash)
        {
            return hash.ToCharArray().Take(_proofOfWorkDifficulty).All(@char => @char == '0');
        }

        public string ComputeHash(Block block)
        {
            if (block == null)
                return string.Empty;

            using (var sha256 = new SHA256Managed())
            {
                var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(block.ToString()));

                var forattedHash = string.Join(string.Empty, hashBytes.Select(@byte => @byte.ToString("x2")));

                return forattedHash;
            }
        }
    }
}
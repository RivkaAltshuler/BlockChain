using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace SecurityCourse.Exercise.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var blocksRepository = new BlocksRepository<Block>();

            var blockChain = new BlockChain(proofOfWorkDifficulty: 4,
                                         durationBetweenBlocksCreation: TimeSpan.FromMinutes(1),
                                            blocksRepository: blocksRepository);

            System.Console.WriteLine("append data...");

            blockChain.Add(new Data("doctor A gave drug B to patient C"));
            blockChain.Add(new Data("doctor A gave drug B to patient D"));
            blockChain.Add(new Data("doctor B gave drug B to patient E"));

            System.Console.WriteLine("wait to the block to be closed...");
            Thread.Sleep(TimeSpan.FromMinutes(2));

            System.Console.WriteLine("append more data (for the next block)...");
            blockChain.Add(new Data("doctor A gave drug B to patient C"));
            blockChain.Add(new Data("doctor A gave drug B to patient D"));
            blockChain.Add(new Data("doctor B gave drug B to patient E"));

            System.Console.WriteLine("wait to the block to be closed...");
            Thread.Sleep(TimeSpan.FromMinutes(2));

            System.Console.WriteLine("verify the integrity (that the data has not been alerted)...");
            VerifyIntegrity(blockChain);

            System.Console.WriteLine("change data...");
            ChangeDataInBlock(0, "patient C", "patient V");

            System.Console.WriteLine("verify that the lack of the integrity has been identified...");
            VerifyBrokenIntegrity(blockChain);

            System.Console.WriteLine("hash the updated block with PoW...");
            var hashGenerator = new HashGenerator(proofOfWorkDifficulty: 4);
            MakeProofOfWork(blocksRepository, hashGenerator);

            System.Console.WriteLine("verify that the lack of the integrity has been identified...");
            VerifyBrokenIntegrity(blockChain);
        }

      

        private static void MakeProofOfWork(BlocksRepository<Block> blocksRepository, HashGenerator hashGenerator)
        {
            var firstBlock = blocksRepository.First();
            var existsNounce = firstBlock.Nounce;
            hashGenerator.HashWithProofOfWork(firstBlock);
            ChangeDataInBlock(0, existsNounce.ToString(), firstBlock.Nounce.ToString());
        }

        private static void VerifyBrokenIntegrity(BlockChain blockChain)
        {
            try
            {
                blockChain.Verify();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }

        }

        private static void VerifyIntegrity(BlockChain blockChain)
        {
            try
            {
                blockChain.Verify();
                System.Console.WriteLine("verified");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
        }

        private static void ChangeDataInBlock(int blockNumber, string replace, string with)
        {
            var path = $@"blocks/{blockNumber}";
            var s = File.ReadAllText(path);
            s = s.Replace(replace, with);
            File.WriteAllText(path, s);
        }


    }
}

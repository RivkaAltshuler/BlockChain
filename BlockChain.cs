using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SecurityCourse.Exercise.Console
{
    public class BlockChain : IEnumerable<Block>
    {
        private readonly IBlocksRepository<Block> _blocksRepository;
        private readonly HashGenerator _hashGenerator;
        private List<Data> _dataForTheNextBlock = new List<Data>();
        private Block _currentBlock;
        

        public BlockChain(int proofOfWorkDifficulty, TimeSpan durationBetweenBlocksCreation, IBlocksRepository<Block> blocksRepository)
        {
            _hashGenerator = new HashGenerator(proofOfWorkDifficulty);
            _blocksRepository = blocksRepository;

            AppendBlocksEndlessly(durationBetweenBlocksCreation);
        }

        public IEnumerator<Block> GetEnumerator()
        {
            return _blocksRepository.GetEnumerator();
        }

        public void Add(Data data)
        {
            _dataForTheNextBlock.Add(data);
        }

        public void Verify()
        {
            var currentBlock = new LinkedList<Block>(_blocksRepository).First;
            int blockNumber = 0;

            string hash = string.Empty;

            while (currentBlock != null)
            {
                if (currentBlock.Value.PreviousHash != hash)
                    throw new Exception($"block {blockNumber} does not hold the currect previous hash");

                hash = _hashGenerator.ComputeHash(currentBlock.Value);

                if (!_hashGenerator.IsProofOfWork(hash))
                    throw new Exception($"Proof Of Work Missing in block {blockNumber}");

                currentBlock = currentBlock.Next;
                blockNumber++;
            }
        }

        private void AppendBlocksEndlessly(TimeSpan durationBetweenBlocksCreation)
        {
            Observable.Interval(durationBetweenBlocksCreation)
                .Subscribe(o => AppendBlock());
        }

        private void AppendBlock()
        {
            lock (_dataForTheNextBlock)
            {
                var previousBlock = _currentBlock;

                _currentBlock = new Block { Nounce = 0, Data = _dataForTheNextBlock, PreviousHash = _hashGenerator.ComputeHash(previousBlock) };

                _hashGenerator.HashWithProofOfWork(_currentBlock);

                _blocksRepository.Append(_currentBlock);

                _dataForTheNextBlock = new List<Data>();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
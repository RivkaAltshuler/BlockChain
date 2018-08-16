using System.Collections.Generic;

namespace SecurityCourse.Exercise.Console
{
    public interface IBlockChain
    {
        void Add(Data data);
        IEnumerator<Block> GetEnumerator();
        void Verify();
    }
}
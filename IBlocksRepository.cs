using System.Collections.Generic;

namespace SecurityCourse.Exercise.Console
{
    public interface IBlocksRepository<T> : IEnumerable<T>
    {
        void Append(T block);
    }
}
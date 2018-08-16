using System.Collections.Generic;

namespace SecurityCourse.Exercise.Console
{
    public class Block
    {
        public int Nounce { get; set; }
        public string PreviousHash { get; set; }
        public IEnumerable<Data> Data { get; set; }

        public override string ToString()
        {
            return Nounce + PreviousHash  + string.Join("", Data);
        }
    }
}
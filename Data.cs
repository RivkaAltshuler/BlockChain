namespace SecurityCourse.Exercise.Console
{
    public class Data
    {
        public string SomeData { get; set; }

        public Data(string someData)
        {
            SomeData = someData;
        }

        public override string ToString()
        {
            return SomeData;
        }
    }
}
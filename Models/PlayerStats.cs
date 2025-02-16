namespace GeminiSdkTest.Models
{
    public class PlayerStats
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public FormatStats Odi { get; set; }
        public FormatStats Test { get; set; }
        public FormatStats T20 { get; set; }
        public FormatStats Ipl { get; set; }
        public FormatStats Domestic { get; set; }
    }

    public class FormatStats
    {
        public int Matches { get; set; }
        public long Runs { get; set; }
        public int Hundreds { get; set; }
        public int Fifties { get; set; }
    }

}

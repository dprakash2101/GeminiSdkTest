namespace GeminiSdkTest.Models
{
    public class RocketStats
    {
        public string RocketName { get; set; }
        public int NumberOfEngines { get; set; }
        public string EngineType { get; set; } // More descriptive than just "engine"
        public List<RocketStage> Stages { get; set; } // Array of RocketStage objects
        public int NumberOfTimesLaunched { get; set; }
        public string? AverageCostPerLaunch { get; set; } // Use decimal for currency
        public string LaunchPads { get; set; }
        public string CountryOfOrigin { get; set; }
        public string Company { get; set; }
        public string status {  get; set; }
        public bool IsOperational { get; set; } // Or use an enum for more detailed status (e.g., Active, Retired, UnderDevelopment)
        public PayloadToOrbit payloadToOrbit { get; set;}
        public string IsReusable { get; set; }

    }
    public class PayloadToOrbit
    {
        public string leo { get; set; }
        public string gto { get; set; }
        public string moonInjection { get; set; }
        public string mars { get; set; }
    }


        public class RocketStage
    {
        public string StageName { get; set; }  // e.g., "Stage 1", "Booster"
        public string EngineName { get; set; }
        public int NumberOfEngines { get; set; }
        public string EngineCombustionCycleType { get; set; }
        public string ThrustPerEngine { get; set; } // Assuming thrust is a numerical value; use decimal or double
        public string ThrustPerWholeStage { get; set; }
        public string propellentType { get; set; }
        public string oxidizer { get; set; }
        public string fuel { get; set; }
    }
}
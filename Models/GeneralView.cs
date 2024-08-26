using System.ComponentModel;

namespace ManagementOfMossadAgentsMVC.Models
{
    public class GeneralView
    {
        public int CountAgent { get; set; }
        public int AgentActive { get; set; }
        public int CountTarget { get; set; }
        public int TargetLive { get; set; }
        public int CountMission { get; set; }
        public int MissionActive { get; set; }

        [DisplayName("Relation of agents to goals")]
        public Double RadioBetweenAgentToTarget { get; set; }

        [DisplayName("Relation of agents to missions")]
        public Double RadioBetweenAgentToMission { get; set; }
    }
}

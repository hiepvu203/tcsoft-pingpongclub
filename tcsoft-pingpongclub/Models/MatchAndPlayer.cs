namespace tcsoft_pingpongclub.Models
{
    public class MatchAndPlayer
    {
        public int IdMatch { get; set; }
        public int IdPlayer1{ get; set; }
        public int IdPlayer2 { get; set; }

        public int IdTournament { get; set; }

        public string? PlayerName1 { get; set; } 
    
        public string? PlayerName2 { get; set; }
        public DateTime? TimeStart { get; set; }

        public int? IdGroupstage { get; set; }
        public List<Set> ListSet { get; set; }

        public int Points1 { get; set; }
        public int Points2 { get; set; }

    }
}

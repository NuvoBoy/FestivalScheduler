namespace FestivalScheduler.Data
{
    public class ChoirNode
    {
        /// <summary>
        /// global id
        /// schema: ChoirNode/{ShortCut}
        /// </summary>
        public string NodeId { get; set; }
        /// <summary>
        /// internal keyword for this choir
        /// </summary>
        public string ShortCut { get; set; }
        /// <summary>
        /// name of the choir
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// nationality
        /// </summary>
        public string Nationality { get; set; }
        /// <summary>
        /// count of all choir members
        /// </summary>
        public int MemberCount { get; set; }
        /// <summary>
        /// place of the accommodation
        /// </summary>
        public string AccommodationPlace { get; set; }
    }
}

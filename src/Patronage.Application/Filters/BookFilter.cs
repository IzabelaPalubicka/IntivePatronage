namespace Patronage.Application.Filters
{
    public class BookFilter
    {
        public string? Title { get; set; }
        public decimal? Rating { get; set; }
        public string? ISBN { get; set; }
        public DateTime? PublicationDateStartPeriod { get; set; }
        public DateTime? PublicationDateEndPeriod { get; set; }
    }
}
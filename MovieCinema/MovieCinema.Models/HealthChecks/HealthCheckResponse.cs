namespace MovieCinema.Models.HealthChecks
{
    public class HealthCheckResponse
    {
        public string Status { get; init; }

        public IEnumerable<IndividualHealthCheckResponse> HealthChecks { get; init; }

        public TimeSpan HealthCheckDuration { get; init; }
    }
}
namespace api.Services
{

    public interface ISmokerConnectionService
    {
        bool IsSmokerConnected { get; }        
    }

    public class SmokerConnectionService : ISmokerConnectionService
    {
        public bool IsSmokerConnected { get; set; }
    }
}
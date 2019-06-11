using BuildingBlocks.EventBus.Events;

namespace Core.Service.Api.Models
{
    public class EventObjectStructureModel : IntegrationEvent
    {
        public int Account { get; set; }
        public int ServiceProvider { get; set; }
        public int ServiceType { get; set; }
        public string ServerTime { get; set; }
        public string PanelTime { get; set; }
        public string Id { get; set; }
        public Event Event { get; set; }
    }

    public class Event
    {
        public int Code { get; set; }
        public int Severity { get; set; }
        public Details Details { get; set; }
        public bool IsMobile { get; set; }
        public Location Location { get; set; }
    }

    public class Location
    {
        public long Latitude { get; set; }
        public long Longitude { get; set; }
        public long HorizontalAccuracy { get; set; }
    }

    public class Details
    {
        public int DeviceId { get; set; }
        public int DeviceType { get; set; }
        public string DeviceDescription { get; set; }
        public int RadioLevel { get; set; }
    }
}

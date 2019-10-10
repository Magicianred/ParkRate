using System.IO;
using System.Xml.Serialization;

namespace ParkRate.ViewModel
{
    public class ParkRateConfig
    {
        public ParkRateConfig()
        {
            PaySlackTime = true;
            PayInAdvance = true;
            PayEveryMinutes = 15;
            PayAmountPerHour = 3;
            SlackTime = 90;
        }

        public bool PaySlackTime { get; set; }
        public bool PayInAdvance { get; set; }
        public int PayEveryMinutes { get; set; }
        public decimal PayAmountPerHour { get; set; }
        public int SlackTime { get; set; }

        public static ParkRateConfig FromXml(string filePath)
        {
            var reader = new XmlSerializer(typeof(ParkRateConfig));
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            using (var streamreader = new StreamReader(stream))
            {
                var config = (ParkRateConfig)reader.Deserialize(streamreader);
                return config;
            }
        }

        public void ToXml(string filePath)
        {
            var serializer = new XmlSerializer(typeof(ParkRateConfig));
            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                serializer.Serialize(stream, this);
            }
        }
    }
}
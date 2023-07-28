using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Base
{
	public class EventBusConfig
	{
		public int				ConnectionRetrycount											{ get; set; } = 5;
        public string			DefaultTopicName												{ get; set; } = "SellingProduct";
        public string			EventBusConnectionString										{ get; set; } = string.Empty;
		public string			SubscriberClientappName											{ get; set; } = string.Empty;
		public string			EventNamePrefix													{ get; set; } = string.Empty;
		public string			EventNameSuffix													{ get; set; } = "IntegraitonEvent";
		public EventBusType		EventBusType													{ get; set; } = EventBusType.RabbitMQ;
		public object			Connection														{ get; set; }

		public bool				DeleteEventPrefix => !String.IsNullOrEmpty(EventNamePrefix);
		public bool				DeleteEventSuffix => !String.IsNullOrEmpty(EventNameSuffix);
	}

	public enum EventBusType
	{
		RabbitMQ = 0,
		AzureServiceBus = 1
	}
}

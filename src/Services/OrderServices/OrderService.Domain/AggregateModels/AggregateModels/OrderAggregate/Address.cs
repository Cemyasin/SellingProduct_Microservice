using OrderService.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Domain.AggregateModels.AggregateModels.OrderAggregate
{
	public record Address  //: ValueObject
	{
		public string City { get; set; }

		public string Street { get; set; }

		public string State { get; set; }

		public string Country { get; set; }

		public string ZipCode { get; set; }

		public Address()
		{
		}

		public Address(string city, string street, string state, string country, string zipCode)
		{
			City	= city;
			Street	= street;
			State	= state;
			Country = country;
			ZipCode = zipCode;
		}

		//protected override IEnumerable<object> GetEqualityComponents()
		//{
		//	//Using yield return statement to return each element one at a time
		//	yield return Street;
		//	yield return City;
		//	yield return State;
		//	yield return Country;
		//	yield return ZipCode;
		//}
	}
}

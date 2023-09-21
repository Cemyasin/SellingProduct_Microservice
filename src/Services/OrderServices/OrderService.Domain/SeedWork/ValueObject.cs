using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Domain.SeedWork
{
	public abstract class ValueObject
	{

		public static bool EqualOperator(BaseEntity left, BaseEntity right)
		{
			if (Equals(left, null) ^ ReferenceEquals(right, null))
			{
				return false;
			}
			else
				return ReferenceEquals(left, null) || ReferenceEquals(right, null);	
		}


		public static bool NotEqualOperator(BaseEntity left, BaseEntity right)
		{
			return !EqualOperator(left, right);
		}

		protected abstract IEnumerable<object> GetEqualityComponents();

		public override bool Equals(object obj)
		{
			if (obj == null || GetType() != obj.GetType())
			{
				return false;
			}

			var other = (ValueObject) obj;

			return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
		}

		public override int GetHashCode()
		{
			return GetEqualityComponents()
				.Select(x => x != null ? x.GetHashCode() : 0)
				.Aggregate((x, y) => x ^ y);
		}

		public ValueObject GetCopy()
		{
			return MemberwiseClone() as ValueObject;
		}


	}
}

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace DEG.SC.HoursOfOperation.Web.Models
{
	[Serializable]
	[XmlRoot(ElementName = "day")]
	[XmlType(TypeName = "day")]
	public class DayOfOperation : IEquatable<DayOfOperation>, IEqualityComparer<DayOfOperation>
	{
		[XmlElement(ElementName = "isclosed")]
		public bool IsClosed { get; set; }

		[XmlElement(ElementName = "openingtime",IsNullable = true)]
		public string OpeningTime { get; set; }

		[XmlElement(ElementName = "closingtime")]
		public string ClosingTime { get; set; }

		[XmlElement(ElementName = "dayofweek")]
		public DayOfWeek DayOfWeek { get; set; }

		public bool Equals(DayOfOperation day)
		{
			if (day.IsClosed && IsClosed)
				return true;
			return IsClosed == day.IsClosed
			       && OpeningTime == day.OpeningTime
			       && ClosingTime == day.ClosingTime;
		}

		public bool Equals(DayOfOperation x, DayOfOperation y)
		{
			return x.Equals(y);
		}

		public int GetHashCode(DayOfOperation obj)
		{
			return obj.GetHashCode();
		}
	}
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace DEG.SC.HoursOfOperation.Web.Models
{
	[Serializable]
	[XmlRoot("hoursofoperationmodel")]
	public class HoursOfOperationModel
	{
		[XmlArray(ElementName = "days")]
		public List<DayOfOperation> Days { get; set; }

		[XmlElement(ElementName = "opentwentyfourhours")]
		public bool OpenTwentyFourHours { get; set; }

		public HoursOfOperationModel (string rawXmlValue)
		{
			Days = new List<DayOfOperation>(7);

			if (string.IsNullOrWhiteSpace(rawXmlValue))
				return;

			var sanitizedXml = SanitizeRawXml(rawXmlValue);
			var stringReader = new StringReader(sanitizedXml);
			var serializer = new XmlSerializer(typeof(HoursOfOperationModel));

			var model = serializer.Deserialize(stringReader) as HoursOfOperationModel;

			if(model == null)
				return;			
			
			OpenTwentyFourHours = model.OpenTwentyFourHours;
			Days = model.Days;
		}

		public HoursOfOperationModel()
		{
			Days = new List<DayOfOperation>(7);
		}		

		public DayOfOperation this[DayOfWeek day]
		{
			get { return Days.FirstOrDefault(x => x.DayOfWeek == day); }
		}

		protected string SanitizeRawXml(string rawXml)
		{
			return rawXml.Replace("<!--", "<").Replace("-->", ">");
		}
	}
}

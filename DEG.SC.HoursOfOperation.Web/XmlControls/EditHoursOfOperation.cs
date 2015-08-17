using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using DEG.SC.HoursOfOperation.Web.Models;
using Sitecore;
using Sitecore.Data;
using Sitecore.Web;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.Pages;
using Sitecore.Web.UI.Sheer;

namespace DEG.SC.HoursOfOperation.Web.XmlControls
{
	public class EditHoursOfOperation : DialogForm
	{
		protected Edit OpenTwentyFourHoursRaw;

		protected Edit SundayOpeningTimeSelectRaw;
		protected Edit SundayClosingTimeSelectRaw;
		protected Edit SundayClosedCheckboxRaw;

		protected Edit MondayOpeningTimeSelectRaw;
		protected Edit MondayClosingTimeSelectRaw;
		protected Edit MondayClosedCheckboxRaw;

		protected Edit TuesdayOpeningTimeSelectRaw;
		protected Edit TuesdayClosingTimeSelectRaw;
		protected Edit TuesdayClosedCheckboxRaw;

		protected Edit WednesdayOpeningTimeSelectRaw;
		protected Edit WednesdayClosingTimeSelectRaw;
		protected Edit WednesdayClosedCheckboxRaw;

		protected Edit ThursdayOpeningTimeSelectRaw;
		protected Edit ThursdayClosingTimeSelectRaw;
		protected Edit ThursdayClosedCheckboxRaw;

		protected Edit FridayOpeningTimeSelectRaw;
		protected Edit FridayClosingTimeSelectRaw;
		protected Edit FridayClosedCheckboxRaw;

		protected Edit SaturdayOpeningTimeSelectRaw;
		protected Edit SaturdayClosingTimeSelectRaw;
		protected Edit SaturdayClosedCheckboxRaw;
		
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);


			if (Context.ClientPage.IsEvent) return;

			var itemUri = WebUtil.GetQueryString("itemUri");
			var fieldName = WebUtil.GetQueryString("fieldName");

			if(!ItemUri.IsItemUri(itemUri) || string.IsNullOrWhiteSpace(fieldName))
				return;
				
			var item = Database.GetItem(ItemUri.Parse(itemUri));

			if(item == null)
				return;

			var model = new HoursOfOperationModel(item[fieldName]);

			OpenTwentyFourHoursRaw.Value = model.OpenTwentyFourHours ? "1" : "0";			

			if (model[DayOfWeek.Monday] != null)
			{
				MondayOpeningTimeSelectRaw.Value = model[DayOfWeek.Monday].OpeningTime;
				MondayClosingTimeSelectRaw.Value = model[DayOfWeek.Monday].ClosingTime;
				MondayClosedCheckboxRaw.Value = model[DayOfWeek.Monday].IsClosed ? "1" : "0";
			}

			if (model[DayOfWeek.Tuesday] != null)
			{
				TuesdayOpeningTimeSelectRaw.Value = model[DayOfWeek.Tuesday].OpeningTime;
				TuesdayClosingTimeSelectRaw.Value = model[DayOfWeek.Tuesday].ClosingTime;
				TuesdayClosedCheckboxRaw.Value = model[DayOfWeek.Tuesday].IsClosed ? "1" : "0";
			}

			if (model[DayOfWeek.Wednesday] != null)
			{
				WednesdayOpeningTimeSelectRaw.Value = model[DayOfWeek.Wednesday].OpeningTime;
				WednesdayClosingTimeSelectRaw.Value = model[DayOfWeek.Wednesday].ClosingTime;
				WednesdayClosedCheckboxRaw.Value = model[DayOfWeek.Wednesday].IsClosed ? "1" : "0";
			}

			if (model[DayOfWeek.Thursday] != null)
			{
				ThursdayOpeningTimeSelectRaw.Value = model[DayOfWeek.Thursday].OpeningTime;
				ThursdayClosingTimeSelectRaw.Value = model[DayOfWeek.Thursday].ClosingTime;
				ThursdayClosedCheckboxRaw.Value = model[DayOfWeek.Thursday].IsClosed ? "1" : "0";
			}

			if (model[DayOfWeek.Friday] != null)
			{
				FridayOpeningTimeSelectRaw.Value = model[DayOfWeek.Friday].OpeningTime;
				FridayClosingTimeSelectRaw.Value = model[DayOfWeek.Friday].ClosingTime;
				FridayClosedCheckboxRaw.Value = model[DayOfWeek.Friday].IsClosed ? "1" : "0";
			}

			if (model[DayOfWeek.Saturday] != null)
			{
				SaturdayOpeningTimeSelectRaw.Value = model[DayOfWeek.Saturday].OpeningTime;
				SaturdayClosingTimeSelectRaw.Value = model[DayOfWeek.Saturday].ClosingTime;
				SaturdayClosedCheckboxRaw.Value = model[DayOfWeek.Saturday].IsClosed ? "1" : "0";
			}

			if (model[DayOfWeek.Sunday] != null)
			{
				SundayOpeningTimeSelectRaw.Value = model[DayOfWeek.Sunday].OpeningTime;
				SundayClosingTimeSelectRaw.Value = model[DayOfWeek.Sunday].ClosingTime;
				SundayClosedCheckboxRaw.Value = model[DayOfWeek.Sunday].IsClosed ? "1" : "0";
			}
		}		

		protected override void OnOK(object sender, EventArgs args)
		{
			var value = SerializeModelToRawValue(CreateModelFromDialogValues());
			SheerResponse.SetDialogValue(RemoveSelfClosingTags(value));

			base.OnOK(sender, args);
		}

		private string RemoveSelfClosingTags(string value)
		{
			var selfClosingTagRegex = new Regex(@"<(?<tag>\w*) \s* />", RegexOptions.IgnorePatternWhitespace);
			if (selfClosingTagRegex.IsMatch(value))
			{
				foreach (Match match in selfClosingTagRegex.Matches(value))
				{
					value = value.Replace(match.Value, string.Format("<{0}></{0}>", match.Groups["tag"]));
				}
			}
			return value;
		}

		private string SerializeModelToRawValue(HoursOfOperationModel model)
		{
			var serializer = new XmlSerializer(typeof(HoursOfOperationModel));
			var stringwriter = new StringWriter();

			serializer.Serialize(stringwriter, model);

			return stringwriter.ToString();
		}

		private HoursOfOperationModel CreateModelFromDialogValues()
		{
			var model = new HoursOfOperationModel
				            {
					            OpenTwentyFourHours = OpenTwentyFourHoursRaw != null && OpenTwentyFourHoursRaw.Value == "1",
				            };

			model.Days.Add(new DayOfOperation
				               {
					               ClosingTime =
						               MondayClosingTimeSelectRaw != null
							               ? MondayClosingTimeSelectRaw.Value
							               : "",
					               IsClosed =
						               MondayClosedCheckboxRaw != null &&
						               MondayClosedCheckboxRaw.Value == "1",
					               OpeningTime =
						               MondayOpeningTimeSelectRaw != null
							               ? MondayOpeningTimeSelectRaw.Value
							               : "",
					               DayOfWeek = DayOfWeek.Monday
				               });

			model.Days.Add(new DayOfOperation
				               {
					               ClosingTime =
						               TuesdayClosingTimeSelectRaw != null
							               ? TuesdayClosingTimeSelectRaw.Value
							               : "",
					               IsClosed =
						               TuesdayClosedCheckboxRaw != null &&
						               TuesdayClosedCheckboxRaw.Value == "1",
					               OpeningTime =
						               TuesdayOpeningTimeSelectRaw != null
							               ? TuesdayOpeningTimeSelectRaw.Value
							               : "",
					               DayOfWeek = DayOfWeek.Tuesday
				               });

			model.Days.Add(new DayOfOperation
				               {
					               ClosingTime =
						               WednesdayClosingTimeSelectRaw != null
							               ? WednesdayClosingTimeSelectRaw.Value
							               : "",
					               IsClosed =
						               WednesdayClosedCheckboxRaw != null &&
						               WednesdayClosedCheckboxRaw.Value == "1",
					               OpeningTime =
						               WednesdayOpeningTimeSelectRaw != null
							               ? WednesdayOpeningTimeSelectRaw.Value
							               : "",
					               DayOfWeek = DayOfWeek.Wednesday
				               });

			model.Days.Add(new DayOfOperation
				               {
					               ClosingTime =
						               ThursdayClosingTimeSelectRaw != null
							               ? ThursdayClosingTimeSelectRaw.Value
							               : "",
					               IsClosed =
						               ThursdayClosedCheckboxRaw != null &&
						               ThursdayClosedCheckboxRaw.Value == "1",
					               OpeningTime =
						               ThursdayOpeningTimeSelectRaw != null
							               ? ThursdayOpeningTimeSelectRaw.Value
							               : "",
					               DayOfWeek = DayOfWeek.Thursday
				               });

			model.Days.Add(new DayOfOperation
				               {
					               ClosingTime =
						               FridayClosingTimeSelectRaw != null
							               ? FridayClosingTimeSelectRaw.Value
							               : "",
					               IsClosed =
						               FridayClosedCheckboxRaw != null &&
						               FridayClosedCheckboxRaw.Value == "1",
					               OpeningTime =
						               FridayOpeningTimeSelectRaw != null
							               ? FridayOpeningTimeSelectRaw.Value
							               : "",
					               DayOfWeek = DayOfWeek.Friday
				               });

			model.Days.Add(new DayOfOperation
				               {
					               ClosingTime =
						               SaturdayClosingTimeSelectRaw != null
							               ? SaturdayClosingTimeSelectRaw.Value
							               : "",
					               IsClosed =
						               SaturdayClosedCheckboxRaw != null &&
						               SaturdayClosedCheckboxRaw.Value == "1",
					               OpeningTime =
						               SaturdayOpeningTimeSelectRaw != null
							               ? SaturdayOpeningTimeSelectRaw.Value
							               : "",
					               DayOfWeek = DayOfWeek.Saturday
				               });

			model.Days.Add(new DayOfOperation
				               {
					               ClosingTime =
						               SundayClosingTimeSelectRaw != null
							               ? SundayClosingTimeSelectRaw.Value
							               : "",
					               IsClosed =
						               SundayClosedCheckboxRaw != null &&
						               SundayClosedCheckboxRaw.Value == "1",
					               OpeningTime =
						               SundayOpeningTimeSelectRaw != null
							               ? SundayOpeningTimeSelectRaw.Value
							               : "",
					               DayOfWeek = DayOfWeek.Sunday
				               });
			
			return model;
		}
	}
}
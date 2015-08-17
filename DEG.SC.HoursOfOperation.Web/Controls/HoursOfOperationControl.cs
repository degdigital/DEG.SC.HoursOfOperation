using System;
using System.Web.UI.WebControls;
using DEG.SC.HoursOfOperation.Web.Models;
using Sitecore;
using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.Text;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.Sheer;
using Memo = Sitecore.Web.UI.HtmlControls.Memo;

namespace DEG.SC.HoursOfOperation.Web.Controls
{
	public class HoursOfOperation : Input 
	{
		public string DialogUrl
		{
			get { return "control:EditHoursOfOperation"; }
		}

		public string ItemId
		{
			get { return GetViewStateString("ItemID"); }
			set
			{
				Assert.ArgumentNotNullOrEmpty(value, "value");
				SetViewStateString("ItemID", value);
			}
		}

		public string FieldId
		{
			get { return GetViewStateString("FieldID"); }
			set
			{
				Assert.ArgumentNotNullOrEmpty(value, "value");
				SetViewStateString("FieldID", value);
			}
		}		

		protected override void CreateChildControls()
		{
			base.CreateChildControls();
			const int multilinePixelHeight = 400;
			const int mutlilinePixelWidth = 500;
			if (FindControl(GetID("HoursOfOperationTextBox")) as Memo == null)
			{
				Controls.Add(new Memo
					             {
						             ID = GetID("HoursOfOperationTextBox"),
						             Enabled = false,
						             ReadOnly = true,
						             Height = new Unit(multilinePixelHeight),
						             Width = new Unit(mutlilinePixelWidth)
					             });
			}			
		}

		public override void HandleMessage(Message message)
		{
			base.HandleMessage(message);

			if (ShouldHandleMessage(message))
				DoHandleMessage(message);			
		}

		public void Edit(ClientPipelineArgs args)
		{
			if (args.IsPostBack)
			{
				if (!args.HasResult)
					return;

				Value = args.Result;
				SetModified();
				SheerResponse.Refresh(this);
			}
			else
			{
				Assert.IsNotNull(DialogUrl, "Dialog URL");

				var urlString = new UrlString(UIUtil.GetUri(DialogUrl));
				var itemUri = new ItemUri(ItemId, Sitecore.Configuration.Factory.GetDatabase("master"));
				var fieldItem = Sitecore.Context.ContentDatabase.GetItem(FieldId);


				urlString["itemUri"] = itemUri.ToString();
				urlString["fieldName"] = fieldItem.Name;

				Sitecore.Context.ClientPage.ClientResponse.ShowModalDialog(urlString.ToString(), "650px", "700px", string.Empty, true);

				args.WaitForPostBack();
			}
		}

		protected virtual bool ShouldHandleMessage(Message message)
		{
			return IsCurrentControl(message)
				   && !string.IsNullOrWhiteSpace(message.Name);
		}

		protected void DoHandleMessage(Message message)
		{
			var command = message.Name.Split(':');
			Assert.IsTrue(command.Length > 1, "Expected message format is control:message");

			switch (command[1].ToLower())
			{
				case "edit":
					Sitecore.Context.ClientPage.Start(this, "Edit");
					break;
				default:
					base.HandleMessage(message);
					break;
			}
		}
	
		protected virtual bool IsCurrentControl(Message message)
		{
			return string.Equals(message["id"], ID, StringComparison.InvariantCultureIgnoreCase);
		}
		
		protected override void SetModified()
		{
			base.SetModified();

			if (TrackModified)
				Sitecore.Context.ClientPage.Modified = true;
		}

		protected override void OnLoad(EventArgs e)
		{
			if (!Sitecore.Context.ClientPage.IsEvent)
			{
				//get the current value
				var model = new HoursOfOperationModel(Value);

				//set the values on the textbox control
				var hoursOfOperationTextBox = FindControl(GetID("HoursOfOperationTextBox")) as Memo;
				hoursOfOperationTextBox.Value = GetDisplayValueFromModel(model);
			}

			base.OnLoad(e);
		}

		private string GetDisplayValueFromModel(HoursOfOperationModel model)
		{
			var monday = model[DayOfWeek.Monday] ?? new DayOfOperation();
			var tuesday = model[DayOfWeek.Tuesday] ?? new DayOfOperation();
			var wednesday = model[DayOfWeek.Wednesday] ?? new DayOfOperation();
			var thursday = model[DayOfWeek.Thursday] ?? new DayOfOperation();
			var friday = model[DayOfWeek.Friday] ?? new DayOfOperation();
			var saturday = model[DayOfWeek.Saturday] ?? new DayOfOperation();
			var sunday = model[DayOfWeek.Sunday] ?? new DayOfOperation();

			return string.Format("Open 24 Hours: {0} \n\n" +			                     

			                     "Monday {15} \n\t" +
			                     "Open: {3} \n\t" +
			                     "Close: {4} \n" +

			                     "Tuesday {16} \n\t" +
			                     "Open: {5} \n\t" +
			                     "Close: {6} \n" +

			                     "Wednesday {17} \n\t" +
			                     "Open: {7} \n\t" +
			                     "Close: {8} \n" +

								 "Thursday {18} \n\t" +
			                     "Open: {9} \n\t" +
			                     "Close: {10}\n" +

								 "Friday {19} \n\t" +
			                     "Open: {11} \n\t" +
			                     "Close: {12} \n" +

								 "Saturday {20} \n\t" +
			                     "Open: {13} \n\t" +
			                     "Close: {14} \n" +

								 "Sunday {21} \n\t" +
			                     "Open: {1} \n\t" +
			                     "Close: {2} \n"

			                     , model.OpenTwentyFourHours
								 , sunday.IsClosed ? "" : sunday.OpeningTime
								 , sunday.IsClosed ? "" : sunday.ClosingTime
								 , monday.IsClosed ? "" : monday.OpeningTime
								 , monday.IsClosed ? "" : monday.ClosingTime
								 , tuesday.IsClosed ? "" : tuesday.OpeningTime
								 , tuesday.IsClosed ? "" : tuesday.ClosingTime
								 , wednesday.IsClosed ? "" : wednesday.OpeningTime
								 , wednesday.IsClosed ? "" : wednesday.ClosingTime
								 , thursday.IsClosed ? "" : thursday.OpeningTime
								 , thursday.IsClosed ? "" : thursday.ClosingTime
								 , friday.IsClosed ? "" : friday.OpeningTime
								 , friday.IsClosed ? "" : friday.ClosingTime
								 , saturday.IsClosed ? "" : saturday.OpeningTime
								 , saturday.IsClosed ? "" : saturday.ClosingTime
								 , monday.IsClosed ? "(Closed)" : ""
								 , tuesday.IsClosed ? "(Closed)" : ""
								 , wednesday.IsClosed ? "(Closed)" : ""
								 , thursday.IsClosed ? "(Closed)" : ""
								 , friday.IsClosed ? "(Closed)" : ""
								 , saturday.IsClosed ? "(Closed)" : ""
								 , sunday.IsClosed ? "(Closed)" : "");
		}		
	}
}

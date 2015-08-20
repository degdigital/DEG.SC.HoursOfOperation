# DEG.SC.HoursOfOperation
This field type provides an easy to use interface for your content authors to select which days and times that their company is open.  The value is saved as XML which makes it easy for you to parse and display on the site.

#Installation

This project uses [TDS](http://www.hhogdev.com/products/team-development-for-sitecore/overview.aspx "TDS") to bring all of your Sitecore items into Visual Studio.  Once you pull down the solution, please follow these steps:

1. Add a reference to Sitecore.Kernel under DEG.SC.HoursOfOperation.Web.  This will allow the solution to build properly.
2. Update the 'Sitecore Web Url' and 'Sitecore Deploy Folder' for DEG.SC.HoursOfOperation.TDS.Core. This will allow you to add the needed items to your Sitecore instance.
3. Update the associated publish profile(s) for your development environment.
4. Sync items in DEG.SC.HoursOfOperation.TDS.Core with your local Sitecore instance.
5. Publish the solution

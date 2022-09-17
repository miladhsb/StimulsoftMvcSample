using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;
using StimulsoftMvcSample.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace StimulsoftMvcSample.Controllers
{
    public class HomeController : Controller
    {
        private readonly List<Person> people = new();

        public HomeController()
        {
            people.Add(new Person("milad", "miladi", 25));
            people.Add(new Person("saeed", "saeedi", 29));
            people.Add(new Person("vahid", "vahidi", 12));

            #region Active 
           // Stimulsoft.Base.StiLicense.LoadFromString("6vJhGtLLLz2GNviWmUTrhSqnOItdDwjBylQzQcAOiHl2AD0gPVknKsaW0un+3PuM6TTcPMUAWEURKXNso0e5OFPaZYasFtsxNoDemsFOXbvf7SIcnyAkFX/4u37NTfx7g+0IqLXw6QIPolr1PvCSZz8Z5wjBNakeCVozGGOiuCOQDy60XNqfbgrOjxgQ5y/u54K4g7R/xuWmpdx5OMAbUbcy3WbhPCbJJYTI5Hg8C/gsbHSnC2EeOCuyA9ImrNyjsUHkLEh9y4WoRw7lRIc1x+dli8jSJxt9C+NYVUIqK7MEeCmmVyFEGN8mNnqZp4vTe98kxAr4dWSmhcQahHGuFBhKQLlVOdlJ/OT+WPX1zS2UmnkTrxun+FWpCC5bLDlwhlslxtyaN9pV3sRLO6KXM88ZkefRrH21DdR+4j79HA7VLTAsebI79t9nMgmXJ5hB1JKcJMUAgWpxT7C7JUGcWCPIG10NuCd9XQ7H4ykQ4Ve6J2LuNo9SbvP6jPwdfQJB6fJBnKg4mtNuLMlQ4pnXDc+wJmqgw25NfHpFmrZYACZOtLEJoPtMWxxwDzZEYYfT");
            #endregion

        }

      
        public IActionResult Index()
        {


          

            return View();
        }

        //جهت اجرای پیش نایش گزارش
        public IActionResult ReportView()
        {
            var viewerOptions = new StiNetCoreViewerOptions();
            viewerOptions.Actions.GetReport = "GetReport";
            viewerOptions.Actions.ViewerEvent = "ViewerEvent";
            // viewerOptions.Appearance.FullScreenMode = true;
            viewerOptions.Toolbar.ShowDesignButton = true;
            viewerOptions.Actions.DesignReport = "DesignReport";
          
            viewerOptions.Toolbar.Zoom = 75;
           // viewerOptions.Toolbar.PrintDestination = Stimulsoft.Report.Web.StiPrintDestination.Pdf;
           
            viewerOptions.Toolbar.ViewMode = Stimulsoft.Report.Web.StiWebViewMode.MultiplePages;
            viewerOptions.Theme = Stimulsoft.Report.Web.StiViewerTheme.Office2007Black;


            // Passing options via ViewBag
            ViewBag.ViewerOptions = viewerOptions;



            return View();
        }

       [Route("[Controller]/[Action]/{Reportname?}")]
        public async Task<IActionResult> GetReport(string Reportname)
        {
           // var Reportname = "Report";
            var report = StiReport.CreateNewReport();
            if (!string.IsNullOrEmpty(Reportname))
            {
                var path = StiNetCoreHelper.MapPath(this, $"wwwroot/{Reportname}.mrt");
                report.Load(path);

            }
            report.Dictionary.Databases.Clear();
            report.DataSources.Clear();
            var DataSourceName = people.GetType().GetGenericArguments().First().Name;
            report.RegData(DataSourceName, people);
            await report.Dictionary.SynchronizeAsync();
             return await StiNetCoreViewer.GetReportResultAsync(this, report);

           
        }
        //جهت اجرای دیزاینر از داخل ریپورت ویو
        public IActionResult DesignReport()

        {

            StiReport report = StiNetCoreViewer.GetReportObject(this);

            ViewBag.ReportName = report.ReportName;

            return View("Designer");

        }

        public IActionResult ViewerEvent()
        {
           
            return StiNetCoreViewer.ViewerEventResult(this);
        }

        //جهت ایجاد خروجی اکسل از منوی سایت
        public async Task<IActionResult> ExportExel()
        {
            var Reportname = "Report";
            var report = StiReport.CreateNewReport();
            if (!string.IsNullOrEmpty(Reportname))
            {
                var path = StiNetCoreHelper.MapPath(this, $"wwwroot/{Reportname}.mrt");
                report.Load(path);

            }
            report.Dictionary.Databases.Clear();
            report.DataSources.Clear();
            var DataSourceName = people.GetType().GetGenericArguments().First().Name;
            report.RegData(DataSourceName, people);
            await report.Dictionary.SynchronizeAsync();
          

            return Stimulsoft.Report.Mvc.StiNetCoreReportResponse.ResponseAsXls(report);
        }

        //جهت اجرای خروجی پی دی اف از منوی سایت
        public async Task<IActionResult> PrintPdf()
        {
            var Reportname = "Report";
            var report = StiReport.CreateNewReport();
            if (!string.IsNullOrEmpty(Reportname))
            {
                var path = StiNetCoreHelper.MapPath(this, $"wwwroot/{Reportname}.mrt");
                report.Load(path);

            }
            report.Dictionary.Databases.Clear();
            report.DataSources.Clear();
            var DataSourceName = people.GetType().GetGenericArguments().First().Name;
            report.RegData(DataSourceName, people);
            await report.Dictionary.SynchronizeAsync();
          


            return Stimulsoft.Report.Mvc.StiNetCoreReportResponse.PrintAsPdf(report);
        }

   

        //جهت اجرای دیزاینر از منوی سایت
        public IActionResult Designer()
        {
          
            return View();
        }

        //جهت اجرای دیزاینر از منوی سایت
        [Route("[Controller]/[Action]/{Reportname?}")]
        public async Task<IActionResult> GetDesignerReport(string Reportname)
        {
         
            var report = StiReport.CreateNewReport();
            if (!string.IsNullOrEmpty(Reportname))
            {
                var path = StiNetCoreHelper.MapPath(this, $"wwwroot/{Reportname}.mrt");
               
               
                report.Load(path);
                
            }
            report.Dictionary.Databases.Clear();
            report.DataSources.Clear();
            var DataSourceName = people.GetType().GetGenericArguments().First().Name;
            report.RegData(DataSourceName, people);
            await report.Dictionary.SynchronizeAsync();
            return StiNetCoreDesigner.GetReportResult(this, report);
        }

        public IActionResult SaveReport()
        {
            var report = StiNetCoreDesigner.GetReportObject(this);
               
            //ذخیره به صورت انکریپت شده
            report.SavePackedReport(report.ReportFile);
            //or //ذخیره به صورت ایکس ام ال 
             report.Save(report.ReportFile);


            return StiNetCoreDesigner.SaveReportResult(this);
        }
        public IActionResult DesignerEvent()
        {
            return StiNetCoreDesigner.DesignerEventResult(this);
        }



    }
}

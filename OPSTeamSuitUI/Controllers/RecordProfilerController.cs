using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using OPSTeamSuitUI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
namespace OPSTeamSuitUI.UploadedFiles
{
    public class RecordProfilerController : Controller
    {
        IOS objIos;
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult UploadFile()
        {
            return View("RecorderProfile");
        }

        //   [HttpPost]
        public IActionResult RecorderProfile(IFormFile file)
        {
            try
            {
                List<NewResultModel> resObj = new List<NewResultModel>();
                if (file != null)
                {
                    var result = new StringBuilder();
                    using (var reader = new StreamReader(file.OpenReadStream()))
                    {
                        while (reader.Peek() >= 0)
                            result.AppendLine(reader.ReadLine());
                    }
                    resObj = DeserializeJSONIOS(result.ToString());
                    return View(resObj);
                }
                else
                {
                    return View("Index");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.ToString();
                return View();
            }
        }
        public List<NewResultModel> DeserializeJSONIOS(string jsonStrin)
        {
            try
            {
                objIos = new IOS();
                objIos = JsonConvert.DeserializeObject<IOS>(jsonStrin);
                if (objIos != null)
                {
                    CheckFIleStatus();
                    double record_time_start = objIos.recordingInfo.recordingStartTime;
                    double record_time_end = objIos.recordingInfo.recordingEndTime;
                    EpochTimeConverstion(record_time_start, record_time_end);
                    ViewData["manufacturer"] = objIos.recordingInfo.manufacturer;
                    ViewData["deviceModel"] = objIos.recordingInfo.deviceModel;
                    if (objIos.recordingInfo.os != null)
                        ViewData["os"] = objIos.recordingInfo.os;
                    else
                        ViewData["os"] = "Not found";
                    ViewData["osVersion"] = objIos.recordingInfo.osVersion.ToString();
                    ViewData["recordingDuration"] = objIos.recordingInfo.recordingDuration.ToString();
                    ViewData["notes"] = objIos.optionalNotes.ToString();
                    if (objIos.recordingInfo.recorderVersion != null)
                        ViewData["appversion"] = objIos.recordingInfo.recorderVersion.ToString();
                    else
                        ViewData["appversion"] = "Not found";
                    var data = objIos.beaconData.GroupBy(p => new { p.uuid, p.major })
     .GroupBy(i => i.Key.uuid)
     .Select(k => new ResultModel
     {
         UUid = k.Key,
         Data = k.Select(j => new Data
         {
             MajorId = j.Key.major,
             Minors = j.Select(l => l.minor).Distinct().OrderBy(i => i).ToList(),
             manufacturer = objIos.recordingInfo.manufacturer,
             os = objIos.recordingInfo.os,
             recordType = objIos.recordingInfo.deviceModel,
             recorderVersion = objIos.recordingInfo.recorderVersion,
             devicemodel = objIos.recordingInfo.deviceModel,
             osVersion = objIos.recordingInfo.osVersion,

             recordingDuration = objIos.recordingInfo.recordingDuration,
             notes = objIos.optionalNotes.ToString(),
         }).ToList()
     })
     .ToList();
                    var result = minorSequenceCalculation(data);
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<NewResultModel> minorSequenceCalculation(List<ResultModel> data)
        {
            {
                int minorIncrement = 0;
                Boolean next_minor = false;
                int start_range = 0;
                StringBuilder minorContinuList = new StringBuilder();
                int Sequence_minor = 0;
                List<NewResultModel> objnewModel = new List<NewResultModel>();
                foreach (var items in data)
                {
                    foreach (var minor in items.Data)
                    {
                        foreach (var item in minor.Minors)
                        {
                            if (item < 0)
                            {
                                Sequence_minor = 65536 + item;
                            }
                            else
                                Sequence_minor = item;

                            if (minorIncrement != 0)
                            {  
                                if (Sequence_minor == (minorIncrement + 1))
                                {
                                    minorIncrement = Sequence_minor;
                                    next_minor = true;
                                }
                                else
                                {
                                    if (minorIncrement != start_range)
                                    {
                                        minorContinuList.Append(" " + start_range + " to " + minorIncrement + " ,").AppendLine();
                                        minorIncrement = Sequence_minor;
                                        start_range = Sequence_minor;
                                    }
                                    else
                                    {
                                        minorContinuList.Append("  " + start_range + ", ").AppendLine();
                                        minorIncrement = Sequence_minor;
                                        start_range = Sequence_minor;
                                    }
                                }

                            }
                            else
                            {
                                minorIncrement = Sequence_minor;
                                start_range = Sequence_minor;
                                next_minor = true;
                            }
                        }
                        if (next_minor)
                        {
                            minorContinuList.Append(" " + start_range + " to " + minorIncrement + "  ,").AppendLine();
                        }
                        objnewModel.Add(new NewResultModel { major = minor.MajorId.ToString(), UUid = items.UUid, minorList = minorContinuList.ToString() });
                        minorContinuList.Clear();
                        minorIncrement = 0;
                        next_minor = false;
                        start_range = 0;
                    }
                }
                return objnewModel;
            }
        }
            public bool CheckFIleStatus()
            {
                if (objIos == null)
                {
                    ViewData["Summary"] = "Recordings not good.No data found";
                    return false;
                }
                if (objIos.gpsData.Count() > 0)
                {
                    ViewData["GPSData"] = objIos.gpsData.Count().ToString();
                }
                if (objIos.sensorData.Count() > 10)
                {
                    ViewData["SensorData"] = objIos.sensorData.Count().ToString();
                }
                if (objIos.sensorData.Count() < 10 && objIos.sensorData.Count() > 0)
                {
                    ViewData["SensorData"] = objIos.sensorData.Count().ToString();
                    ViewData["Summary"] = "Recordings contains very less no of Snsor data";
                }
                if (objIos.sensorData.Count() == 0)
                {
                    ViewData["SensorData"] = 0;
                    ViewData["Summary"] = "Recordings not good.No Sensor data found";
                }
                if (objIos.beaconData.Count() == 0)
                {
                    ViewData["Summary"] = "Recordings not good.No Beacon data found";
                    return false;
                }
                if (objIos.beaconData.Count() < 11)
                {
                    ViewData["Summary"] = "Recordings not good.Few Beacon data found";
                    return false;
                }
                else
                {
                    ViewData["Summary"] = "Recordings are ok to proceed";
                    return true;
                }
            }



            public void EpochTimeConverstion(double record_start_time, double record_end_time)
            {
                try
                {
                    DateTime result_time;
                    DateTime unixStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                    //start time conversion  
                    long unixTimeStampInTicks = (long)(record_start_time * TimeSpan.TicksPerSecond);
                    if (unixTimeStampInTicks < 0)
                    {
                        result_time = unixStart.AddMilliseconds(record_start_time);
                    }
                    else
                    {
                        result_time = new DateTime(unixStart.Ticks + unixTimeStampInTicks, System.DateTimeKind.Utc);
                    }
                    ViewData["start_time"] = result_time;

                    //end time conversion 
                    DateTime unixEnd = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                    unixTimeStampInTicks = (long)(record_end_time * TimeSpan.TicksPerSecond);
                    if (unixTimeStampInTicks < 0)
                    {
                        result_time = unixEnd.AddMilliseconds(record_end_time);
                    }
                    else
                    {
                        result_time = new DateTime(unixStart.Ticks + unixTimeStampInTicks, System.DateTimeKind.Utc);
                    }
                    ViewData["end_time"] = result_time;
                }
                catch (Exception)
                {
                    ViewData["start_time"] = record_start_time;
                    ViewData["end_time"] = record_end_time;
                }

            }
            public void DeserializeJSONAndroid(string jsonStrin)
            {
                try
                {
                    Android objAndroid = new Android();
                    objAndroid = JsonConvert.DeserializeObject<Android>(jsonStrin);
                }
                catch (Exception)
                {

                }
            }


        }
    }
 
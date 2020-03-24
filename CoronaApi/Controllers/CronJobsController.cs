﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CoronaApi.Models;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CoronaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CronJobsController : ControllerBase
    {
        private readonly CoronaStatsDbContext coronaStatsDbContext;
        private readonly IWebHostEnvironment env;

        public CronJobsController(CoronaStatsDbContext coronaStatsDbContext, IWebHostEnvironment env)
        {
            this.coronaStatsDbContext = coronaStatsDbContext;
            this.env = env;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Call()
        {
            try
            {
                // call go link
                var dataOfStat = await GetCoronaDetailsFromGovLink();

                // check changes 
                if (dataOfStat == null)
                {
                    return StatusCode(500, "link response is error");
                }

                var previousRecord = await UpdateStatChanges(dataOfStat);

                // send notification to firebase
                if (previousRecord.CasesCount != dataOfStat.local_total_cases)
                {
                    var title = "කොරෝනා රොගීන් වැඩිවීමක්.";
                    var messageBody = "සෞඛ්‍ය ප්‍රවර්ධන කාර්යංශයට අනුව නව රෝගීන් " + (dataOfStat.local_total_cases - previousRecord.CasesCount).ToString() + " ක්  හඳුනා ගනී .";
                    var result = await SendNotificationAsync(title, messageBody);
                    return Ok(result);
                }

                // send response
                return Ok(false);
            }
            catch (Exception e)
            {
                return Ok("Error" + e.Message);
            }
        }

        private async Task<CoronaRecord> UpdateStatChanges(Data data)
        {
            // get current data row using date            
            var previousRecord = await this.coronaStatsDbContext.CoronaRecords.OrderByDescending(o => o.RecordDate.Date).Take(1).FirstOrDefaultAsync();

            if (previousRecord.RecordDate.Date == DateTime.Now.Date)
            {
                // update
                if (previousRecord.CasesCount != data.local_total_cases)
                {
                    previousRecord.RecoverCount = data.local_recovered;
                    previousRecord.DeathCount = data.local_deaths;
                    previousRecord.CasesCount = data.local_total_cases;
                    previousRecord.SuspectCount = data.local_total_number_of_individuals_in_hospitals;
                    this.coronaStatsDbContext.CoronaRecords.Update(previousRecord);
                    await this.coronaStatsDbContext.SaveChangesAsync();
                }
            }
            else
            {
                // insert
                var newRecord = new CoronaRecord()
                {
                    RecordDate = DateTime.Now.Date,
                    CasesCount = data.local_total_cases,
                    RecoverCount = data.local_recovered,
                    DeathCount = data.local_deaths,
                    SuspectCount = data.local_total_number_of_individuals_in_hospitals
                };
                await this.coronaStatsDbContext.CoronaRecords.AddAsync(newRecord);
                await this.coronaStatsDbContext.SaveChangesAsync();
            }
            
            return previousRecord;
        }

        private async Task<Data> GetCoronaDetailsFromGovLink()
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://hpb.health.gov.lk/api/get-current-statistical"))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        var govRes = JsonConvert.DeserializeObject<CoronaGovResponse>(apiResponse);
                        return govRes.data;
                    }
                }

                return null;
            }
        }

        private async Task<string> SendNotificationAsync(string title, string body)
        {
            var path = env.ContentRootPath;
            path = path + "\\Auth.json";
            FirebaseApp app = null;
            try
            {
                app = FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile(path)
                }, "situation-sl");
            }
            catch (Exception ex)
            {
                app = FirebaseApp.GetInstance("situation-sl");
            }

            var fcm = FirebaseAdmin.Messaging.FirebaseMessaging.GetMessaging(app);
            Message message = new Message()
            {
                Notification = new Notification
                {
                    Title = title,
                    Body = body
                },
                Data = new Dictionary<string, string>()
                {
                    { "click_action ", "FLUTTER_NOTIFICATION_CLICK" },
                },

                Topic = "allUsers"
            };

            var result = await fcm.SendAsync(message);
            return result;
        }
    }
}
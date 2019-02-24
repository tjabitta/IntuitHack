﻿using Nexmo.Api;
using System.Web.Mvc;

namespace NexmoDotNetQuickStarts.Controllers
{
    public class VerifyController : Controller
    {
        public Client Client { get; set; }

        public VerifyController()
        {
            Client = new Client(creds: new Nexmo.Api.Request.Credentials
            {
                ApiKey = "7825fda6",
                ApiSecret = "n0IbvACFT1eSzF4l",
            });
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Start()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Start(string to)
        {
            var RECIPIENT_NUMBER = to;

            var start = Client.NumberVerify.Verify(new NumberVerify.VerifyRequest
            {
                number = RECIPIENT_NUMBER,
                brand = "AcmeInc"
            });

            Session["requestID"] = start.request_id;

            return RedirectToAction("Check");
        }

        [HttpGet]
        public ActionResult Check()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Check(string code)
        {
            var result = Client.NumberVerify.Check(new NumberVerify.CheckRequest
            {
                request_id = Session["requestID"].ToString(),
                code = code
            });

            if (result.status == "0")
            {
                ViewBag.Message = "Verification Sucessful";
            }
            else
            {
                ViewBag.Message = result.error_text;
            }
            return View();

        }

        [HttpGet]
        public ActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Search(string requestID)
        {
            var search = Client.NumberVerify.Search(new NumberVerify.SearchRequest
            {
                request_id = requestID
            });
            ViewBag.error_text = search.error_text;
            ViewBag.status = search.status;

            return View();
        }

        [HttpGet]
        public ActionResult Cancel()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Cancel(string requestID)
        {
            var results = Client.NumberVerify.Control(new NumberVerify.ControlRequest
            {
                request_id = requestID,
                cmd = "cancel"
            });
            ViewBag.status = results.status;
            return View();
        }

        [HttpGet]
        public ActionResult TriggerNext()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TriggerNext(string requestID)
        {
            var results = Client.NumberVerify.Control(new NumberVerify.ControlRequest
            {
                request_id = requestID,
                cmd = "trigger_next_event"
            });
            ViewBag.status = results.status;
            return View();
        }
    }
}
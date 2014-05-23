﻿using IAmRich.Website.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IAmRich.Website.Controllers
{
	public class PayPalController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}
		public ActionResult RedirectFromPaypal()
		{
			return View();
		}
		public ActionResult CancelFromPaypal()
		{
			return View();
		}
		public ActionResult NotifyFromPaypal()
		{
			return View();
		}
		public ActionResult ValidateCommand(string product, string totalPrice)
		{
			bool useSandbox = Convert.ToBoolean(ConfigurationManager.AppSettings["IsSandbox"]);
			var paypal = new PayPalModel(useSandbox);
			paypal.item_name = product;
			paypal.amount = totalPrice;
			return View(paypal);
		}
	}
}
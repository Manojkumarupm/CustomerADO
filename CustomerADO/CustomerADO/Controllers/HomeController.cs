using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CustomerADO.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            CustomerContext cc = new CustomerContext();
            IEnumerable<Customer> customers = cc.GetCustomer();
            return View(customers);
        }
        [HttpGet]
        public ActionResult SearchCustomer(DateTime DOB)
        {
            CustomerContext cc = new CustomerContext();
            IEnumerable<Customer> customers = cc.GetCustomerYoungerByDOB(DOB);
            return View(customers);
        }
        [HttpGet]
        public ActionResult CreateCustomer()
        {
            return View();
        }
        [HttpPost]
        [ActionName("CreateCustomer")]
        public ActionResult CreateCustomer_Post()
        {
            if (ModelState.IsValid)
            {
                Customer c = new Customer();
                TryUpdateModel(c);
                CustomerContext cc = new CustomerContext();
                cc.AddCustomer(c);
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpGet]
        public ActionResult EditCustomer(int Id)
        {
            CustomerContext cc = new CustomerContext();
            Customer c = cc.SearchCustomer(Id).ToList().FirstOrDefault();
            return View(c);
        }
        [HttpPost]
        [ActionName("EditCustomer")]
        public ActionResult EditCustomer_Post()
        {
            if (ModelState.IsValid)
            {
                Customer c = new Customer();
                TryUpdateModel(c);
                CustomerContext cc = new CustomerContext();
                cc.UpdateCustomer(c);
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpGet]
        public ActionResult DetailsCustomer(int Id)
        {
            CustomerContext cc = new CustomerContext();
            Customer c = cc.SearchCustomer(Id).ToList().FirstOrDefault();
            return View(c);
        }
        [HttpGet]
        public ActionResult DeleteCustomer(int Id)
        {
            CustomerContext cc = new CustomerContext();
            cc.DeleteCustomerDetails(Id);
            return RedirectToAction("Index");
        }
    }
}
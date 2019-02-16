using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CustomerADO.Controllers
{
    public class CustomerController : Controller
    {
        CustomerDetails cd = new CustomerDetails();
        public ActionResult MyIndex()
        {
            IEnumerable<Customer> customers = cd.GetCustomerList();
            return View(customers);
        }
        [HttpGet]
        public ActionResult CreateMyCustomer()
        {
            return View();
        }
        [HttpPost]
        [ActionName("CreateMyCustomer")]
        public ActionResult CreateMyCustomer_Post()
        {
            if (ModelState.IsValid)
            {
                Customer c = new Customer();
                TryUpdateModel(c);

                cd.AddCustomer(c);
                return RedirectToAction("MyIndex");
            }
            return View();
        }
        [HttpGet]
        public ActionResult EditMyCustomer(int Id)
        {
            Customer c = cd.SearchCustomer(Id);
            return View(c);
        }
        [HttpPost]
        [ActionName("EditMyCustomer")]
        public ActionResult EditCustomer_Post()
        {
            if (ModelState.IsValid)
            {
                Customer c = new Customer();
                TryUpdateModel(c);
                cd.UpdateCustomer(c);
                return RedirectToAction("MyIndex");
            }
            return View();
        }
        [HttpGet]
        public ActionResult DetailsMyCustomer(int Id)
        {

            Customer c = cd.SearchCustomer(Id);
            return View(c);
        }
        [HttpGet]
        public ActionResult DeleteMyCustomer(int Id)
        {
            cd.DeleteCustomerDetails(Id);
            return RedirectToAction("MyIndex");
        }
    }
}
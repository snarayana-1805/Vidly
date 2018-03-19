using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Vidly.Models;
using Vidly.Models.ManageViewModels;
using Vidly.Data;
using Pomelo.EntityFrameworkCore.MySql;
using Pomelo.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Vidly.ViewModels;

namespace Vidly.Controllers
{
    public class CustomersController : Controller
    {

       public ApplicationDbContext DbContext { get; set; }

        private readonly ApplicationDbContext _context;

        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
            
         }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        public IActionResult Index()
        {
            var customers = _context.Customers.Include(b => b.MembershipType).ToList();
           // var customers = _context.Customers.ToList();

            return View(customers);

        }

        public IActionResult New()
        {
            var membershipTypes = _context.MembershipType.ToList();
            var viewModel = new CustomerFormViewModel
            {
                MembershipTypes = membershipTypes
            };

            return View("CustomerForm", viewModel);

        }

        [HttpPost]
        public IActionResult Save(Customer customer)
        {
            if (customer.Id == 0)
                _context.Customers.Add(customer);
            else
            {
                var customerInDb = _context.Customers.Single(c => c.Id == customer.Id);
                customerInDb.Name = customer.Name;
                customerInDb.Birthdate = customer.Birthdate;
                customerInDb.MembershipTypeId = customer.MembershipTypeId;
                customerInDb.IsSubsribedToNewsletter = customer.IsSubsribedToNewsletter;
            }

            _context.SaveChanges();

            return RedirectToAction("Index", "Customers");

        }



        public IActionResult Details(int id)
        {
            var customer = _context.Customers.Include(b => b.MembershipType).SingleOrDefault(c=> c.Id==id);

            if (customer == null)
                return StatusCode(418);

            return View(customer);

        }

        public IActionResult Edit(int id)
        {
            var customer = _context.Customers.SingleOrDefault(c => c.Id == id);

            if (customer == null)
                return StatusCode(418);


            var viewModel = new CustomerFormViewModel
            {

                Customer = customer,
                MembershipTypes = _context.MembershipType.ToList()
            };
            return View("CustomerForm", viewModel);

        }



    }
}
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
using AutoMapper;
using Vidly.Dtos;
using System.Net.Http;


namespace Vidly.Controllers
{

    [Route("api/[controller]")]


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



        // GET /api/customers
        [HttpGet]
        public IEnumerable<Customer> GetCustomers(string query = null)
        {
            var customersQuery = _context.Customers
                .Include(c => c.MembershipType);

            //if (!String.IsNullOrWhiteSpace(query))
            //    customersQuery = customersQuery.Where(c => c.Name.Contains(query));

            //var customerDtos = customersQuery
            //    .ToList()
            //    .Select(Mapper.Map<Customer, CustomerDto>);

            var customers = customersQuery
                 .ToList();

            return customers;
        }


        // GET /api/customers/1

        [HttpGet("{id}", Name = "Customer")]
        public IActionResult GetCustomer(int id)
        {
            var customer = _context.Customers.SingleOrDefault(c => c.Id == id);

            if (customer == null)
                return NotFound();

            return new ObjectResult(customer);
        }


        [HttpPost]
        public IActionResult CreateCustomer([FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            //var customer = Mapper.Map<CustomerDto, Customer>(customerDto);
            _context.Customers.Add(customer);
            _context.SaveChanges();

            customer.Id = customer.Id;
            //request.requestURI not found.. need to investigate
            return CreatedAtRoute("Customer", new { id = customer.Id }, customer);
        }



            public ViewResult Index()
        {
            // var customers = _context.Customers.Include(b => b.MembershipType).ToList();

            // var customers = _context.Customers.ToList();

            return View();

        }

        public ViewResult ShowGrid()
        {
            // var customers = _context.Customers.Include(b => b.MembershipType).ToList();

            // var customers = _context.Customers.ToList();

            return View();

        }

        // PUT /api/customers/1
        [HttpPut("{id}")]
        public IActionResult UpdateCustomer(int id, [FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var customerInDb = _context.Customers.SingleOrDefault(c => c.Id == id);

            if (customerInDb == null)
                return NotFound();

            Mapper.Map(customer, customerInDb);
            
            _context.SaveChanges();

            return Ok();
        }

        // DELETE /api/customers/1
        [HttpDelete("{id}")]
        public IActionResult DeleteCustomer(int id)
        {
            var customerInDb = _context.Customers.SingleOrDefault(c => c.Id == id);

            if (customerInDb == null)
                return NotFound();

            _context.Customers.Remove(customerInDb);
            _context.SaveChanges();

            return Ok();
        }

        //public IActionResult New()
        //{
        //    var membershipTypes = _context.MembershipType.ToList();
        //    var viewModel = new CustomerFormViewModel
        //    {
        //        MembershipTypes = membershipTypes
        //    };

        //    return View("CustomerForm", viewModel);

        //}

        //[HttpPost]
        //public IActionResult Save(Customer customer)
        //{
        //    if (customer.Id == 0)
        //        _context.Customers.Add(customer);
        //    else
        //    {
        //        var customerInDb = _context.Customers.Single(c => c.Id == customer.Id);
        //        customerInDb.Name = customer.Name;
        //        customerInDb.Birthdate = customer.Birthdate;
        //        customerInDb.MembershipTypeId = customer.MembershipTypeId;
        //        customerInDb.IsSubsribedToNewsletter = customer.IsSubsribedToNewsletter;
        //    }

        //    _context.SaveChanges();

        //    return RedirectToAction("Index", "Customers");

        //}



        //public IActionResult Details(int id)
        //{
        //    var customer = _context.Customers.Include(b => b.MembershipType).SingleOrDefault(c=> c.Id==id);

        //    if (customer == null)
        //        return StatusCode(418);

        //    return View(customer);

        //}

        //public IActionResult Edit(int id)
        //{
        //    var customer = _context.Customers.SingleOrDefault(c => c.Id == id);

        //    if (customer == null)
        //        return StatusCode(418);


        //    var viewModel = new CustomerFormViewModel
        //    {

        //        Customer = customer,
        //        MembershipTypes = _context.MembershipType.ToList()
        //    };
        //    return View("CustomerForm", viewModel);

        //}



    }
}
using CRUDwithADO.Net.Data;
using CRUDwithADO.Net.Models;
using Microsoft.AspNetCore.Mvc;

namespace CRUDwithADO.Net.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _db;
        public EmployeeController(ApplicationDbContext applicationDb) {
            _db = applicationDb;
        }
        public IActionResult Index()
        {
            List<Employee> employees = new List<Employee>();
            employees = _db.GetEmployeesList();
            return View(employees);
        }

        [HttpGet]
        public IActionResult Create() {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Employee employee) {
            if (ModelState.IsValid)
            {
            bool checkFlag = _db.AddNewEmployee(employee);
                if (checkFlag)
                {
                    TempData["InsertMsg"] = "Data has been inserted successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(employee);
                }
            }
            return View(employee);
            
        }

        [HttpGet]
        public IActionResult Delete(int id) { 
            bool check = _db.DeleteEmployeeByID(id);
            if (check)
            {
                TempData["DeletionUnsuccess"] = "Deletion Successfull";
                return RedirectToAction("Index");
            }
            TempData["DeletionUnsuccess"] = "Deletion Unsuccessfull";
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            Employee employee = _db.GetEmployeeByID(id);
            return View(employee);
        }
        [HttpPost]
        //todo
        public IActionResult Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                bool checkFlag = _db.UpdateEmployeeByID(employee);
                if (checkFlag)
                {
                    TempData["InsertMsg"] = "Data updated successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(employee);
                }
            }
            return View(employee);
        }
    }
}

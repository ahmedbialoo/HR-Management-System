using Microsoft.AspNetCore.Mvc;
using HRSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Web;
using ReflectionIT.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;



namespace HRSystem.Controllers
{
   
    public class AdminController : Controller
    {
        HR_SystemContext hr;
       
        
        public AdminController(HR_SystemContext hr)
        {
            this.hr = hr;
            
        }

        //--------------Cookie function ---------------//
        public void Set(string key, string value, int? expireTime)
        {
            CookieOptions co = new CookieOptions();
            if (expireTime.HasValue)
                co.Expires = DateTime.Now.AddSeconds(expireTime.Value);
            else
                co.Expires = DateTime.Now.AddMilliseconds(10);
            Response.Cookies.Append(key, value, co);
        }

        //--------------Login GET ---------------//
        public IActionResult Login()

        {
            if (Request.Cookies["ID"] != null)

            {
                HttpContext.Session.SetString("user", Request.Cookies["ID"].ToString());
                return RedirectToAction("Index");
            }
            else
            {
                if (hr.Users.Where(x => x.Username == "admin" && x.Password == "1234").FirstOrDefault() == null)
                {
                    Group gr = new Group { Group_Name = "Admin", Emp_Edit = true, User_Edit = true, Attend_Edit = true, Group_Edit = true, Emp_Add = true, Emp_Delete = true, Emp_show = true, Group_Add = true, Group_Delete = true, Group_show = true, Attend_Add = true, Attend_Delete = true, Attend_show = true, User_Add = true, User_Delete = true, User_show = true };
                    hr.SaveChanges();
                    User def = new User { Username = "admin", Password = "1234", User_Fname = "admin", User_Email = "a@a.com", GroupId=1};
                    hr.Groups.Add(gr);
                    hr.Users.Add(def);
                    hr.SaveChanges();

                }

                return View();
                
            }
         
        }

        //--------------Login Post ---------------//

        [HttpPost]
        public IActionResult Login (User u, bool remember)
        {

           

                User s = hr.Users.Where(x => x.Username == u.Username && x.Password == u.Password).FirstOrDefault();

                if (s != null)
                {
               
                    HttpContext.Session.SetString("user", s.Username);

                if (remember == true)
                    {
                        Set("ID", HttpContext.Session.GetString("user"), int.MaxValue);
                        

                }

                return RedirectToAction("Index");


            }
            else
                {
                    ViewBag.Status = "Please, Insert right username & password";
                    return View();
                }
            
        }

        //--------------Logout Action ---------------//

        public IActionResult Logout()
        {
            Response.Cookies.Delete("ID");
            HttpContext.Session.Remove("user");
            return RedirectToAction("Login");
        }

        //--------------Index Action ---------------//

        public IActionResult index()
        {
            #region Validation 
            //------------- Validation variables----------- //
            string y = HttpContext.Session.GetString ("user");
            User y1 = hr.Users.Where(n=>n.Username==y).FirstOrDefault();
            

            if ( y1 == null)
            {

                return RedirectToAction("Login");
            }


            //---------- End Of variables -----------//
            #endregion
            else
            {
                Group gry = hr.Groups.Where(x => x.Group_Id == y1.GroupId).FirstOrDefault();
                ViewBag.validGroup = gry;
                #region Handling the percent of daily employee attendence

                int current_attend = hr.Attendances.ToList().FindAll(n => n.Date.ToShortDateString() == DateTime.Now.ToShortDateString()).Count;
                int total_emp = hr.Employees.ToList().Count;
                double double_percent = (double)current_attend / total_emp;
                int att_percent = (int)(double_percent * 100);
                string status;
                if (att_percent > 0 && att_percent < 50) status = "Low";
                else if (att_percent >= 50 && att_percent < 100) status = "Good";
                else if (att_percent == 0) status = "Absence";
                else status = "Complete";
                ViewBag.status = status;
                ViewBag.current_percent = (int)(double_percent * 100);

                #endregion

                return View(hr.AttendenceEmployees.Include(n => n.Attendances).ToList());
            }
        }

        //--------------Add Group GET ---------------//

        public IActionResult addgroup()
        {
            #region Validation 
            //------------- Validation variables----------- //
            string y = HttpContext.Session.GetString("user");
            User y1 = hr.Users.Where(n => n.Username == y).FirstOrDefault();


            if (y1 == null)
            {

                return RedirectToAction("Login");
            }
            else
            {
                Group gry = hr.Groups.Where(x => x.Group_Id == y1.GroupId).FirstOrDefault();
                ViewBag.validGroup = gry;

                //---------- End Of variables -----------//
                #endregion

                if (gry.Group_Add == false)
                {
                    return RedirectToAction("Error");
                }
                return View();
            }
        }

        //--------------Add Group Post ---------------//

        [HttpPost]
        public IActionResult addgroup(Group gr)
        {
            hr.Groups.Add(gr);
            hr.SaveChanges();
            return RedirectToAction("permissions");
        }



        //--------------  Users   Action ---------------//
     


        public async Task<IActionResult> users(string sortOrder, string currentFilter, string searchString, int? pageNumber, int pageSize = 3)
        {
            #region Validation 
            //------------- Validation variables----------- //
            string y = HttpContext.Session.GetString("user");
            User y1 = hr.Users.Where(n => n.Username == y).FirstOrDefault();


            if (y1 == null)
            {

                return RedirectToAction("Login");
            }
            Group gry = hr.Groups.Where(x => x.Group_Id == y1.GroupId).FirstOrDefault();
            ViewBag.validGroup = gry;

            //---------- End Of variables -----------//
            #endregion

            if (gry.User_show == false)
            {
                return RedirectToAction("Error");
            }
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var users = from s in hr.Users
                        select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(s => s.User_Fname.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    users = users.OrderByDescending(s => s.User_Fname);
                    break;

                default:
                    users = users.OrderBy(s => s.User_Fname);
                    break;
            }
            var users2 = hr.Users.Include(u => u.Group).ToList();


            return View(await PaginatedList<User>.CreateAsync(users.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        //-------------- Add User  GET ---------------//


        public IActionResult adduser()
        {
            #region Validation 
            //------------- Validation variables----------- //
            string y = HttpContext.Session.GetString("user");
            User y1 = hr.Users.Where(n => n.Username == y).FirstOrDefault();
            Group gry = hr.Groups.Where(x => x.Group_Id == y1.GroupId).FirstOrDefault();
            ViewBag.validGroup = gry;

            if (gry == null)
            {

                return RedirectToAction("Login");
            }


            //---------- End Of variables -----------//
            #endregion

            if (gry.User_Add == false)
            {
                return RedirectToAction("Error");
            }
            ViewBag.users = hr.Users.ToList();
            ViewBag.group = new SelectList(hr.Groups.ToList(), "Group_Id", "Group_Name");
            return View();
        }

        //-------------- Add User  Post ---------------//

        [HttpPost]
        public IActionResult adduser([Bind] User u)
        {
            TempData["EmailError"]=null;
            TempData["UserError"]=null;
            TempData["userfname"] = null;
            TempData["useremail"] = null;
            TempData["username"] = null;

            User x = hr.Users.Where(n => n.User_Email == u.User_Email).FirstOrDefault();
            User y = hr.Users.Where(n => n.Username == u.Username).FirstOrDefault();

            if ( x == null && y == null)
            {
                

                if (ModelState.IsValid)
                {

                    hr.Users.Add(u);
                    hr.SaveChanges();
                    return RedirectToAction("users");

                }
                else
                {
                    ViewBag.group = new SelectList(hr.Groups.ToList(), "Group_Id", "Group_Name");

                    return View();
                }

                
            }
            else 
            {
                if (x != null) TempData["EmailError"] = "value2";
                if (y != null) TempData["UserError"] = "value";
                TempData["userfname"]= u.User_Fname;
                TempData["useremail"]= u.User_Email;
                TempData["username"]= u.Username;

                return RedirectToAction("adduser");
            }
           
            
        }

        //--------------  User  Details ---------------//

        public IActionResult userdetails(int id)
        {
            #region Validation 
            //------------- Validation variables----------- //
            string y = HttpContext.Session.GetString("user");
            User y1 = hr.Users.Where(n => n.Username == y).FirstOrDefault();


            if (y1 == null)
            {

                return RedirectToAction("Login");
            }
            Group gry = hr.Groups.Where(x => x.Group_Id == y1.GroupId).FirstOrDefault();
            ViewBag.validGroup = gry;

            //---------- End Of variables -----------//
            #endregion
            if (gry.User_show == false)
            {
                return RedirectToAction("Error");
            }
            ViewBag.group = new SelectList(hr.Groups.ToList(), "Group_Id", "Group_Name");

            return View(hr.Users.Find(id));
        }

        //-------------- Edit User  GET ---------------//

        public IActionResult edituser(int id)
        {
            #region Validation 
            //------------- Validation variables----------- //
            string y = HttpContext.Session.GetString("user");
            User y1 = hr.Users.Where(n => n.Username == y).FirstOrDefault();


            if (y1 == null)
            {

                return RedirectToAction("Login");
            }
            Group gry = hr.Groups.Where(x => x.Group_Id == y1.GroupId).FirstOrDefault();
            ViewBag.validGroup = gry;

            //---------- End Of variables -----------//
            #endregion
            if (gry.User_Edit == false)
            {
                return RedirectToAction("Error");
            }
            ViewBag.group = new SelectList(hr.Groups.ToList(), "Group_Id", "Group_Name");

            return View(hr.Users.Find(id));
        }

        //-------------- Edit User  Post ---------------//

        [HttpPost]
        public IActionResult edituser(User s)
        {
            #region Validation 
            //------------- Validation variables----------- //
            string y2 = HttpContext.Session.GetString("user");
            User y1 = hr.Users.Where(n => n.Username == y2).FirstOrDefault();


            if (y1 == null)
            {

                return RedirectToAction("Login");
            }
            Group gry = hr.Groups.Where(x => x.Group_Id == y1.GroupId).FirstOrDefault();
            ViewBag.validGroup = gry;

            //---------- End Of variables -----------//
            #endregion
            if (gry.User_Edit == false)
            {
                return RedirectToAction("Error");
            }
            TempData["EmailError"] = null;
            TempData["UserError"] = null;
            TempData["Ok"] = null;



            User x = hr.Users.Where(n => n.User_Email == s.User_Email).FirstOrDefault();
            User y = hr.Users.Where(n => n.Username == s.Username).FirstOrDefault();

            if (x == null && y == null)
            {
                hr.Users.Update(s);
                hr.SaveChanges();
                TempData["Ok"] = "User has been saved";
                return View();

            }
            else
            {
                if (x != null) TempData["EmailError"] = "value2";
                if (y != null) TempData["UserError"] = "value";
               

                return RedirectToAction("edituser");
            }
        }

        //-------------- Delete User   ---------------//

        public IActionResult deleteuser(int id)

        {
            #region Validation 
            //------------- Validation variables----------- //
            string y = HttpContext.Session.GetString("user");
            User y1 = hr.Users.Where(n => n.Username == y).FirstOrDefault();


            if (y1 == null)
            {

                return RedirectToAction("Login");
            }
            Group gry = hr.Groups.Where(x => x.Group_Id == y1.GroupId).FirstOrDefault();
            ViewBag.validGroup = gry;

            //---------- End Of variables -----------//
            #endregion
            if (gry.Group_Add == false)
            {
                return RedirectToAction("Error");
            }

            User s = hr.Users.Find(id);
            hr.Remove(s);
            hr.SaveChanges();
            return RedirectToAction("users");
        }


        //--------------Attendance  GET ---------------//


        public async Task<IActionResult> attendancedeparture(string searchString, DateTime date_to, DateTime date_from)
        {
            #region Validation 
            //------------- Validation variables----------- //
            string y = HttpContext.Session.GetString("user");
            User y1 = hr.Users.Where(n => n.Username == y).FirstOrDefault();


            if (y1 == null)
            {

                return RedirectToAction("Login");
            }

            Group gry = hr.Groups.Where(x => x.Group_Id == y1.GroupId).FirstOrDefault();
            ViewBag.validGroup = gry;

            //---------- End Of variables -----------//
            #endregion
            if (gry.Attend_show == false)
            {
                return RedirectToAction("Error");
            }

            IQueryable<AttendenceEmployee> emps = hr.AttendenceEmployees.Include(n => n.Attendances);
            if (!String.IsNullOrEmpty(searchString))
            {
                emps = emps.Where(n => n.Employees.Emp_FullName.Contains(searchString));
            }
            if (date_from != null)
            {
                emps = emps.Where(n => n.Attendances.Date >= date_from && n.Attendances.Date <= date_to);
            }
            ViewBag.date_to = date_to;
            ViewBag.date_from = date_from;
            ViewBag.sts_emp = new SelectList(hr.Employees.ToList(), "Emp_Ssn", "Emp_FullName");
            ViewBag.emps = hr.Employees.ToList();
            ViewBag.ats = await emps.Include(n => n.Attendances).ToListAsync();
            return View();
        }



        //--------------Attendance  Post ---------------//

        [HttpPost]
        public IActionResult attendancedeparture([Bind] AttendenceEmployee atn, DateTime date)
        {
            atn.Attendances.order = date;
            hr.AttendenceEmployees.Add(atn);
            hr.SaveChanges();
            return RedirectToAction("Index");
        }

        //-------------- Delete Attendance  Action ---------------//

        public IActionResult deleteattendancedeparture(int id)
        {
            #region Validation 
            //------------- Validation variables----------- //
            string y = HttpContext.Session.GetString("user");
            User y1 = hr.Users.Where(n => n.Username == y).FirstOrDefault();


            if (y1 == null)
            {

                return RedirectToAction("Login");
            }
            Group gry = hr.Groups.Where(x => x.Group_Id == y1.GroupId).FirstOrDefault();
            ViewBag.validGroup = gry;

            //---------- End Of variables -----------//
            #endregion
            if (gry.Attend_Delete == false)
            {
                return RedirectToAction("Error");
            }
            var ae = hr.AttendenceEmployees.Where(n => n.Att_id == id).SingleOrDefault();
            var a = hr.Attendances.Where(n => n.att_id == id).SingleOrDefault();
            hr.AttendenceEmployees.Remove(ae);
            hr.Attendances.Remove(a);
            hr.SaveChanges();
            return RedirectToAction("attendancedeparture");
        }



        //--------------  Employees  Action ---------------//


        public async Task<IActionResult> employees(string sortOrder,string currentFilter,string searchString,int? pageNumber , int pageSize = 3)
        {
            #region Validation 
            //------------- Validation variables----------- //
            string y = HttpContext.Session.GetString("user");
            User y1 = hr.Users.Where(n => n.Username == y).FirstOrDefault();


            if (y1 == null)
            {

                return RedirectToAction("Login");
            }
            Group gry = hr.Groups.Where(x => x.Group_Id == y1.GroupId).FirstOrDefault();
            ViewBag.validGroup = gry;

            //---------- End Of variables -----------//
            #endregion
            if (gry.Emp_show == false)
            {
                return RedirectToAction("Error");
            }
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var employees = from s in hr.Employees
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                employees = employees.Where(s => s.Emp_FullName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    employees = employees.OrderByDescending(s => s.Emp_FullName);
                    break;
                case "Date":
                    employees = employees.OrderBy(s => s.BirthDate);
                    break;
                case "date_desc":
                    employees = employees.OrderByDescending(s => s.BirthDate);
                    break;
                default:
                    employees = employees.OrderBy(s => s.Emp_FullName);
                    break;
            }

            
            return View(await PaginatedList<Employee>.CreateAsync(employees.AsNoTracking(), pageNumber ?? 1, pageSize));
        }


        //--------------Add Employee GET ---------------//

        public IActionResult addemployee()
        {
            #region Validation 
            //------------- Validation variables----------- //
            string y = HttpContext.Session.GetString("user");
            User y1 = hr.Users.Where(n => n.Username == y).FirstOrDefault();


            if (y1 == null)
            {

                return RedirectToAction("Login");
            }
            Group gry = hr.Groups.Where(x => x.Group_Id == y1.GroupId).FirstOrDefault();
            ViewBag.validGroup = gry;

            //---------- End Of variables -----------//
            #endregion
            if (gry.Emp_Add == false)
            {
                return RedirectToAction("Error");
            }
            return View();
        }

        //--------------Add Employee Post ---------------//

        [HttpPost]
        public IActionResult addemployee([Bind] Employee em)
        {
            TempData["EmailError"] = null;
            TempData["PhoneError"] = null;
            TempData["SSNError"] = null;

          

            Employee x = hr.Employees.Where(n => n.Emp_Email == em.Emp_Email).FirstOrDefault();
            Employee t = hr.Employees.Where(n => n.Phone == em.Phone).FirstOrDefault();
            Employee z = hr.Employees.Where(n => n.Emp_Ssn == em.Emp_Ssn).FirstOrDefault();

            if (x == null && t == null &&z == null)
            {
                #region Validation 
                //------------- Validation variables----------- //
                string y = HttpContext.Session.GetString("user");
                User y1 = hr.Users.Where(n => n.Username == y).FirstOrDefault();


                if (y1 == null)
                {

                    return RedirectToAction("Login");
                }
                Group gry = hr.Groups.Where(x => x.Group_Id == y1.GroupId).FirstOrDefault();
                ViewBag.validGroup = gry;

                //---------- End Of variables -----------//
                #endregion
                if (gry.Emp_Add == false)
                {
                    return RedirectToAction("Error");
                }


                if (em.HireDate.Year < 2008)
                {
                    ModelState.IsValid.Equals(false);
                    ModelState.AddModelError("HireDate", "HireDate must not be before 2008");
                    ModelState.GetFieldValidationState("HireDate");
                    ViewBag.emp1 = "HireDate must not be before 2008";
                }
                if (DateTime.Now - em.BirthDate < new DateTime(2020, 1, 1) - new DateTime(2000, 1, 1))
                {
                    ModelState.IsValid.Equals(false);
                    ModelState.AddModelError("BirthDate", "Employee age must equal or more than 20 years!");
                    ModelState.GetFieldValidationState("BirthDate");
                    ViewBag.emp2 = "Employee age must equal or more than 20 years!";
                }
                if (ModelState.IsValid)
                {
                    hr.Employees.Add(em);
                    hr.SaveChanges();
                    return RedirectToAction("employees");
                }
                return View();
            }
            else
            {

                if (x != null) TempData["EmailError"] = "value2";
                if (t != null) TempData["PhoneError"] = "value";
                if (z != null) TempData["SSNError"] = "value3";

                TempData["EmailError"] = em.Emp_Email;
                TempData["PhoneError"] = em.Phone;
                TempData["SSNError"] = em.Emp_Ssn;

                return RedirectToAction("addemployee");
            }

        }


        //--------------  Edit Employees  GET ---------------//

        public IActionResult editemployee(int id )
        {
            #region Validation 
            //------------- Validation variables----------- //
            string y = HttpContext.Session.GetString("user");
            User y1 = hr.Users.Where(n => n.Username == y).FirstOrDefault();


            if (y1 == null)
            {

                return RedirectToAction("Login");
            }
            Group gry = hr.Groups.Where(x => x.Group_Id == y1.GroupId).FirstOrDefault();
            ViewBag.validGroup = gry;

            //---------- End Of variables -----------//
            #endregion
            if (gry.Emp_Edit == false)
            {
                return RedirectToAction("Error");
            }
            return View(hr.Employees.Find(id));
        }


        //--------------  Edit Employees  Post ---------------//


        [HttpPost] 

        public IActionResult editemployee(Employee emp)
        {

            hr.Employees.Update(emp);
            hr.SaveChanges();

            return RedirectToAction("employees");
        }

        //--------------  Delete  Employee  Action ---------------//

        public IActionResult deleteemployee(int id)

        {
            #region Validation 
            //------------- Validation variables----------- //
            string y = HttpContext.Session.GetString("user");
            User y1 = hr.Users.Where(n => n.Username == y).FirstOrDefault();


            if (y1 == null)
            {

                return RedirectToAction("Login");
            }
            Group gry = hr.Groups.Where(x => x.Group_Id == y1.GroupId).FirstOrDefault();
            ViewBag.validGroup = gry;

            //---------- End Of variables -----------//
            #endregion
            if (gry.Emp_Delete == false)
            {
                return RedirectToAction("Error");
            }
            Employee emp = hr.Employees.Find(id);
            hr.Remove(emp);
            hr.SaveChanges();
            return RedirectToAction("employees");
        }




        //--------------  Invoice  Action ---------------//

        public IActionResult invoice(int id,
             string name,
             string phone,
             int salary,
             int attendance,
             int absent,
             int extra_hrs,
             int discount_hrs,
             int overtime,
             int discount,
             int total)
        { 
            #region Validation 
        //------------- Validation variables----------- //
        string y = HttpContext.Session.GetString("user");
            User y1 = hr.Users.Where(n => n.Username == y).FirstOrDefault();


            if (y1 == null)
            {

                return RedirectToAction("Login");
            }
            Group gry = hr.Groups.Where(x => x.Group_Id == y1.GroupId).FirstOrDefault();
            ViewBag.validGroup = gry;

            //---------- End Of variables -----------//
            #endregion

            ViewBag.ssn = id;
            ViewBag.name = name;
            ViewBag.phone = phone;
            ViewBag.salary = salary;
            ViewBag.attendance = attendance;
            ViewBag.absent = absent;
            ViewBag.extra_hrs = extra_hrs;
            ViewBag.discount_hrs = discount_hrs;
            ViewBag.overtime = overtime;
            ViewBag.discount = discount;
            ViewBag.total = total;
            return View();
        }


        //--------------  Permissions  Action ---------------//

        public async Task<IActionResult> permissions(string sortOrder, string currentFilter, string searchString, int? pageNumber, int pageSize = 3)
        {
            #region Validation 
            //------------- Validation variables----------- //
            string y = HttpContext.Session.GetString("user");
            User y1 = hr.Users.Where(n => n.Username == y).FirstOrDefault();


            if (y1 == null)
            {

                return RedirectToAction("Login");
            }
            Group gry = hr.Groups.Where(x => x.Group_Id == y1.GroupId).FirstOrDefault();
            ViewBag.validGroup = gry;

            //---------- End Of variables -----------//
            #endregion
            if (gry.Group_show == false)
            {
                return RedirectToAction("Error");
            }
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var permissions = from s in hr.Groups
                              select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                permissions = permissions.Where(s => s.Group_Name.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    permissions = permissions.OrderByDescending(s => s.Group_Name);
                    break;

                default:
                    permissions = permissions.OrderBy(s => s.Group_Name);
                    break;
            }


            return View(await PaginatedList<Group>.CreateAsync(permissions.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        //--------------  Delete Group  Action ---------------//

        public IActionResult deletegroup(int id)
        {
            #region Validation 
            //------------- Validation variables----------- //
            string y = HttpContext.Session.GetString("user");
            User y1 = hr.Users.Where(n => n.Username == y).FirstOrDefault();


            if (y1 == null)
            {

                return RedirectToAction("Login");
            }
            Group gry = hr.Groups.Where(x => x.Group_Id == y1.GroupId).FirstOrDefault();
            ViewBag.validGroup = gry;

            //---------- End Of variables -----------//
            #endregion
            hr.Groups.Remove(hr.Groups.Find(id));
            hr.SaveChanges();
            return RedirectToAction("permissions");
        }

        //--------------  Edit Group  GET ---------------//

        public IActionResult editgroup(int id)
        {
            #region Validation 
            //------------- Validation variables----------- //
            string y = HttpContext.Session.GetString("user");
            User y1 = hr.Users.Where(n => n.Username == y).FirstOrDefault();


            if (y1 == null)
            {

                return RedirectToAction("Login");
            }
            Group gry = hr.Groups.Where(x => x.Group_Id == y1.GroupId).FirstOrDefault();
            ViewBag.validGroup = gry;

            //---------- End Of variables -----------//
            #endregion
            if (gry.Group_Edit == false)
            {
                return RedirectToAction("Error");
            }

            return View(hr.Groups.Find(id));
        }

        //--------------  Edit Group  Post ---------------//

        [HttpPost]
        public IActionResult editgroup(Group gr)
        {
            hr.Groups.Update(gr);
            hr.SaveChanges();
            return RedirectToAction("permissions");
        }

        //--------------  Salary  Action ---------------//

        public IActionResult salaryreport()
        {
            #region Validation 
            //------------- Validation variables----------- //
            string y = HttpContext.Session.GetString("user");
            User y1 = hr.Users.Where(n => n.Username == y).FirstOrDefault();


            if (y1 == null)
            {

                return RedirectToAction("Login");
            }
            Group gry = hr.Groups.Where(x => x.Group_Id == y1.GroupId).FirstOrDefault();
            ViewBag.validGroup = gry;

            //---------- End Of variables -----------//
            #endregion
            return View();
        }


        //--------------  Salary  Details Partial View ---------------//

        public IActionResult salaryDetails(int year, int month)
        {
            ViewBag.year = year;
            ViewBag.month = month;

            List<string> month_days_names = new List<string>();

            List<DateTime> annual_vac_in_month_dates = new List<DateTime>();
            var month_days_dates = month_days_func(year, month);
            foreach (var item in month_days_dates)
            {
                var day_name = item.DayOfWeek.ToString();
                month_days_names.Add(day_name);
            }

            foreach (var day_in_month in month_days_dates)
            {
                foreach (var annual_vac in hr.AnnualVacations.ToList())
                {
                    if (annual_vac.date == day_in_month)
                    {
                        annual_vac_in_month_dates.Add(day_in_month);
                    }
                }

            }


            ViewBag.annual_vac_in_month_dates = annual_vac_in_month_dates;
            ViewBag.month_days_names = month_days_names;
            ViewBag.month_days_dates = month_days_dates;
            ViewBag.annual = hr.AnnualVacations.ToList();
            ViewBag.emps = hr.Employees.ToList();
            ViewBag.ats = hr.AttendenceEmployees.Include(n => n.Attendances).ToList();
            return PartialView();
        }

        public static List<DateTime> month_days_func(int year, int month)
        {
            return Enumerable.Range(1, DateTime.DaysInMonth(year, month)).Select(n => new DateTime(year, month, n)).ToList();
        }



        //--------------  Annual Vacation  GET ---------------//

        public IActionResult annualvacation()
        {
            #region Validation 
            //------------- Validation variables----------- //
            string y = HttpContext.Session.GetString("user");
            User y1 = hr.Users.Where(n => n.Username == y).FirstOrDefault();


            if (y1 == null)
            {

                return RedirectToAction("Login");
            }
            else
            {
                Group gry = hr.Groups.Where(x => x.Group_Id == y1.GroupId).FirstOrDefault();
                ViewBag.validGroup = gry;

                //---------- End Of variables -----------//
                #endregion
                if (gry.Group_Add == false)
                {
                    return RedirectToAction("Error");
                }
                if (hr.AnnualVacations.ToList() != null)
                {
                    ViewBag.annual = hr.AnnualVacations.ToList();
                }
                return View();
            }
        }


        //--------------  Annual Vacation  Post ---------------//

        [HttpPost]
        public IActionResult annualvacation([Bind] AnnualVacation ann)
        {
            if (ModelState.IsValid)
            {

                hr.AnnualVacations.Add(ann);
                hr.SaveChanges();
                ViewBag.annual = hr.AnnualVacations.ToList();

                return RedirectToAction("annualvacation");
            }
            else
            {

                return View();
            }
        }





        //--------------    Delete Annual Vacation ---------------//

        public IActionResult deleteannualvacation(int id)
        {
            #region Validation 
            //------------- Validation variables----------- //
            string y = HttpContext.Session.GetString("user");
            User y1 = hr.Users.Where(n => n.Username == y).FirstOrDefault();


            if (y1 == null)
            {

                return RedirectToAction("Login");
            }
            Group gry = hr.Groups.Where(x => x.Group_Id == y1.GroupId).FirstOrDefault();
            ViewBag.validGroup = gry;

            //---------- End Of variables -----------//
            #endregion
            if (gry.Group_Add == false)
            {
                return RedirectToAction("Error");
            }
            var del = hr.AnnualVacations.Where(n => n.id == id).FirstOrDefault();
            hr.AnnualVacations.Remove(del);
            hr.SaveChanges();
            return RedirectToAction("annualvacation");
        }
   
    

        public IActionResult Error()
    {
        return View();
    }


        public IActionResult slider()
        {
            #region Validation 
            //------------- Validation variables----------- //
            string y = HttpContext.Session.GetString("user");
            User y1 = hr.Users.Where(n => n.Username == y).FirstOrDefault();


            if (y1 == null)
            {

                return RedirectToAction("Login");
            }
            Group gry = hr.Groups.Where(x => x.Group_Id == y1.GroupId).FirstOrDefault();
            ViewBag.validGroup = gry;

            //---------- End Of variables -----------//
            #endregion
            return PartialView();
        }
    }

    
}

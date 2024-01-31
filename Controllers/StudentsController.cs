
using ISPRGG2_Finals_Dy_Lim_Esguerra.Models;
using Microsoft.AspNetCore.Mvc;

namespace ISPRGG2_Finals.Controllers
{
    public class StudentsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(StudentModelLogin model)
        {
                bool Connected;

            if(ModelState.IsValid)
            {
                if (Connected = model.LogIn())
                {
                   // return View("StudentIndex");
                    return RedirectToAction("StudentIndex");

                }

                else
                {
                    TempData["AlertMessage"] = "Incorrect username or password";
                    return RedirectToAction("Index");

                }
            }
            else
            {
                return View("Index");
            }

        }

       
        public IActionResult RegisterPage()
        {
            return View("Register");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(StudentModelRegister model)
        {
            bool Connected;

            if (ModelState.IsValid)
            {
                if (Connected = model.RegisterOne())
                {

                    model.RegisterTwo();
                    TempData["AlertMessageS"] = " Account Registered Succesfully!";
                    return RedirectToAction("RegisterPage");

                }
                else
                {
                    TempData["AlertMessageF"] = "Username taken!";
                    return RedirectToAction("RegisterPage");
                   

                }
            }
            else
            {
                return View("Register");
            }
        }

        public IActionResult ChangePass()
        {
            return View("Password");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Password(StudentModelChange model)
        {

            if (ModelState.IsValid)
            {
                model.ChangePass();
                TempData["AlertMessageSP"] = " Password Changed Succesfully!";
                return RedirectToAction("ChangePass");
            }
            else
            {
                return View("Password");
            }
        }

        public IActionResult StudentIndex(string search = "")
        {
            StudentModelRecords model = new StudentModelRecords();
            List<StudentModelRecords> slist = model.Get(search);
            ViewBag.Search = search;
            return View(slist);
        }

        [HttpGet]
        public IActionResult AddPage()
        {
            return View("Add");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(StudentModelRecords model)
        {
            try
            {
                model.Add();
                return RedirectToAction("StudentIndex");
            }
            catch (Exception ex)
            {
                return View(model);
            }
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(StudentModelRecords model)
        {
            model.Delete();

            return RedirectToAction("StudentIndex");
        }


        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("StudentIndex");
            }

            StudentModelRecords s = new();
            s = s.Get((int)id);
            if (s.StudentID == 0)
            {
                return RedirectToAction("StudentIndex");
            }

            return View(s);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(StudentModelRecords model)
        {
            try
            {
                model.Update();
                return RedirectToAction("StudentIndex");
            }
            catch (Exception ex)
            {
                return View(model);
            }
        }
    }
}

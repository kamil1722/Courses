using Courses.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Courses.Controllers
{
    public class HomeController : Controller
    {
        private DBCtx Context { get; }
        public HomeController(DBCtx _context)
        {
            this.Context = _context;
        }

        public string SearchCourse { get; set; }
        public SelectList LisGenres { get; set; }
        public SelectList LisSubjects { get; set; }
        public SelectList ListGrades { get; set; }
        public string SelectGenre { get; set; }
        public string SelectSubjects { get; set; }
        public string SelectGrades { get; set; }

        public  IActionResult Index()
        {
            //Box attributes
            LisGenres = new SelectList(Context.Courses
                .Select(x => x.Genre).Distinct().ToList());
            LisSubjects = new SelectList(Context.Courses
                .Select(x => x.Subject).Distinct().ToList());
            ListGrades = new SelectList(Context.Courses
                .Select(x => x.Grade).Distinct().ToList());

            ViewBag.LisGenres = LisGenres;
            ViewBag.LisSubjects = LisSubjects;
            ViewBag.ListGrades = ListGrades;

            //Serialize and filtering to JSON string. 
            if (!string.IsNullOrEmpty(SelectGenre))
            {
                ViewBag.Json = JsonConvert.SerializeObject(myJson()
                    .Where(x => x.genre == SelectGenre));
            }
            if (!string.IsNullOrEmpty(SelectSubjects))
            {
                ViewBag.Json = JsonConvert.SerializeObject(myJson()
                    .Where(x => x.subjects == SelectSubjects));
            }
            if (!string.IsNullOrEmpty(SelectGrades))
            {
                ViewBag.Json = JsonConvert.SerializeObject(myJson()
                    .Where(x => x.subjects == SelectSubjects));
            }
            else
                ViewBag.Json =  JsonConvert.SerializeObject(myJson());
            return View();
        }
        public  List<TreeViewNode> myJson()
        {
            List<TreeViewNode> json = new List<TreeViewNode>();

            //Loop and add the Parent Nodes.
            foreach (Models.Courses type in this.Context.Courses)
            {
                json.Add(new TreeViewNode
                {
                    id = type.Id.ToString(),
                    parent = "#",
                    text = type.Title,
                    genre = type.Genre,
                    grade = type.Grade,
                    subjects = type.Subject
                });
            }

            //////Loop and add the Child Nodes.
            foreach (Modules subType in this.Context.Modules)
            {
                if (subType.ParentId == 0)
                {
                    json.Add(new TreeViewNode
                    {
                        id = subType.Id.ToString(),
                        parent = subType.CourseId.ToString(),
                        text = subType.Num + " " + subType.Title
                    });
                }
                else
                    json.Add(new TreeViewNode
                    {
                        id = subType.Id.ToString(),
                        parent = subType.ParentId.ToString(),
                        text = subType.Num + "  " + subType.Title
                    });
            }
            return json;
        }
    }
}

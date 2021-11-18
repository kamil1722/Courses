using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Courses.Models
{

    public class HomeController : Controller
    {
        private DBCtx Context { get; }
        public HomeController(DBCtx _context)
        {
            this.Context = _context;
        }
        public string SearchCourse { get; set; }
        [BindProperty]
        public string SelectGenre { get; set; }
        [BindProperty]
        public string SelectSubjects { get; set; }
        [BindProperty]
        public string SelectGrades { get; set; }

        public async Task<IActionResult> Index()
        {
            List<TreeViewNode> json = myJson();
            //Box attributes

            ViewBag.LisGenres = new SelectList(await Context.Courses
                .Select(x => x.Genre).Distinct().ToListAsync());
            ViewBag.LisSubjects = new SelectList(await Context.Courses
                .Select(x => x.Subject).Distinct().ToListAsync());
            ViewBag.ListGrades =  new SelectList(await Context.Courses
                .Select(x => x.Grade).Distinct().ToListAsync());

            //Serialize and filtering to JSON string. 
            ViewBag.Json = JsonConvert.SerializeObject(json);
            if (SelectGenre != null)
            {
                ViewBag.Json = await Task.Run(() => JsonConvert.SerializeObject(myJson().
                   Where(x=>x.subParent == "27")));
            }
            if (SelectSubjects != null)
            {
                //ViewBag.Json = await Task.Run(() => JsonConvert.SerializeObject(json.
                //  Where(x => x.subjects == SelectSubjects.Select(x=>x.Genre) )));
            }
            if (SelectGrades != null)
            {
                //ViewBag.Json = await Task.Run(() => JsonConvert.SerializeObject(json.
                //   Where(x => x.grade == SelectGrades)));
            }     
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
                    subjects = type.Subject,
                    subParent = type.Id.ToString()
                    
                });
            }

            //////Loop and add the Child Nodes.
            foreach (Modules subType in this.Context.Modules)
            {
                //var a = JsonConvert.SerializeObject(Context.Courses.Where(x => x.Id == subType.Id).Select(x => x.Grade));

                if (subType.ParentId == 0)
                {

                    json.Add(new TreeViewNode
                    {
                        id = subType.Id.ToString(),
                        parent = subType.CourseId.ToString(),
                        text = subType.Num + " " + subType.Title,
                    });

                }
                else

                    json.Add(new TreeViewNode
                    {
                        id = subType.Id.ToString(),
                        parent = subType.ParentId.ToString(),
                        text = subType.Num + "  " + subType.Title,
                        subParent = subType.CourseId.ToString()
                    });
                  
            }
            return json;
        }
    }
}

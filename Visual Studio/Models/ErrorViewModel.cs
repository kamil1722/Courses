using System;
using System.Collections.Generic;

namespace Courses.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
    public class Courses
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Subject { get; set; }
        public string Grade { get; set; }
        public string Genre { get; set; }


    }
    public class Modules
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int ParentId { get; set; }
        public int Order { get; set; }
        public string Title { get; set; }
        public string Num { get; set; }

    }
    public class TreeViewNode
    {
        public string id { get; set; }
        public string parent { get; set; }
        public string text { get; set; }
        public string genre { get; set; }
        public string subjects { get; set; }
        public string grade { get; set; }

    }
}
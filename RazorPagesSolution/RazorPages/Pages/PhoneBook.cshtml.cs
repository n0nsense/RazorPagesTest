using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataModels;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using LinqToDB.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Splat;

namespace RazorPages.Pages
{
    public class PhoneBookModel : PageModel
    {
        private readonly ActionLogger logger;

        [BindProperty]
        public RazorBaseDB razorBaseDB { get; set; }
        public JsonResult OnGetListAll()
        {  
            using(var db = new RazorBaseDB())
            {
                return new JsonResult(db.PhoneBooks.ToList());
            }
            
        }
        public void OnPostAddItem(int id)
        {
            using (var db = new RazorBaseDB())
            {
                //return new JsonResult(db.PhoneBooks.ToList());
            }
        }
    }
}

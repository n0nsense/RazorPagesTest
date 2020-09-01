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

namespace RazorPages.Pages
{
    public class PhoneBookModel : PageModel
    {
        public JsonResult OnGetListAll()
        {  
            using(var db = new RazorBaseDB())
            {
                return new JsonResult(db.PhoneBooks.ToList());
            }
            
        }
        public void OnPostInsertItem()
        {
            using (var db = new RazorBaseDB())
            {
                //return new JsonResult(db.PhoneBooks.ToList());
            }
        }
    }
}

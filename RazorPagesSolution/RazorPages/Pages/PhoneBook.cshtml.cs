using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataModels;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using LinqToDB.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Splat;

namespace RazorPages.Pages
{
    public class PhoneBookModel : PageModel
    {
        readonly RazorBaseDB _context;
        public PhoneBookModel(RazorBaseDB context)
        {
            _context = context;
        }
        public IActionResult OnGetGridData(DataSourceLoadOptions loadOptions)
        {  
            using(var db = new RazorBaseDB())
            {
                return new JsonResult(db.PhoneBooks.ToList());
            }
            
        }
        public IActionResult OnPutGridRow(int key, string values)
        {
            //using (var db = new RazorBaseDB("PhoneBook"))
            //{
                var model = _context.PhoneBooks.FirstOrDefault(item => item.IdPb == key);
                var _values = JsonConvert.DeserializeObject<IDictionary>(values);
                //model.IdPb = values["IdPb"].;

                if (!TryValidateModel(model))
                    return BadRequest("Validation failed");

                //db.PhoneBooks.
                return new OkResult();
            //}
        }
    }
}

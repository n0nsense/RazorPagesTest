using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using DataModels;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using LinqToDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Splat;

namespace RazorPages.Pages
{
    public class PhoneBookModel : PageModel
    {
        private readonly RazorBaseDB _context;
        
        public PhoneBookModel(RazorBaseDB context)
        {
            _context = context;
        }
        public IActionResult OnGetGridData()
        {
            return new JsonResult(_context.PhoneBooks.ToList());        
        }

        public IActionResult OnPutGridRow(int key, string values)
        {
            try
            {
                var transaction = _context.BeginTransaction();

                var existingRecord = _context.PhoneBooks.FirstOrDefault(i => i.IdPb.Equals(key));
                var _values = JsonConvert.DeserializeObject<DataModels.PhoneBook>(values);

                if (!TryValidateModel(existingRecord))
                    return BadRequest("Validation failed");

                var updated = _context.PhoneBooks.Where(i => i.IdPb.Equals(key))
                   .Set(w => w.IdPb, _values.IdPb == 0 ? existingRecord.IdPb : _values.IdPb)
                   .Set(w => w.Name, _values.Name ?? existingRecord.Name)
                   .Set(w => w.Patronymic, _values.Patronymic ?? existingRecord.Patronymic)
                   .Set(w => w.Phone, _values.Phone ?? existingRecord.Phone)
                   .Set(w => w.Surname, _values.Surname ?? existingRecord.Surname)
                   .Set(w => w.Sex, _values.Sex ?? existingRecord.Sex)
                   .Update();

                if (updated != 1)
                    return BadRequest("Ошибка при сохранении записи.");
                else
                {
                    int idpbh = 0;
                    var query = from p in _context.PhoneBookHistories orderby p.IdPbh descending select p;
                    var queryresult = query.FirstOrDefault();
                    if (queryresult != null)
                    {
                        idpbh = queryresult.IdPbh;
                    }
                    
                    _context.PhoneBookHistories
                        .Value(p => p.Date, DateTime.Now)
                        .Value(p => p.IdPb, _values.IdPb == 0 ? existingRecord.IdPb : _values.IdPb)
                        .Value(p => p.IdPbh, idpbh + 1)
                        .Value(p => p.Name, _values.Name ?? existingRecord.Name)
                        .Value(p => p.Patronymic, _values.Patronymic ?? existingRecord.Patronymic)
                        .Value(p => p.Phone, _values.Phone ?? existingRecord.Phone)
                        .Value(p => p.Surname, _values.Surname ?? existingRecord.Surname)
                        .Value(p => p.Sex, _values.Sex ?? existingRecord.Sex)
                        .Insert();
                }
                transaction.Commit();
                return new OkResult();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.ToString());
            }
        }

        public IActionResult OnPostGridRowAdd(string values)
        {
            try
            {
                var transaction = _context.BeginTransaction();
                int idpb = 0;
                var qPhoneBooks = from p in _context.PhoneBooks orderby p.IdPb descending select p;
                var resultPhoneBooks = qPhoneBooks.FirstOrDefault();
                if (resultPhoneBooks != null)
                {
                    idpb = resultPhoneBooks.IdPb + 1;
                }
                var _values = JsonConvert.DeserializeObject<DataModels.PhoneBook>(values);
                _values.IdPb = idpb;

                var inserted = _context.Insert(_values);

                if (inserted != 1)
                    return BadRequest("Ошибка при добавлении записи.");
                else
                {
                    int idpbh = 0;
                    var qPhoneBookHistories = from p in _context.PhoneBookHistories orderby p.IdPbh descending select p;
                    var resultPhoneBookHistories = qPhoneBookHistories.FirstOrDefault();
                    if (resultPhoneBookHistories != null)
                    {
                        idpbh = resultPhoneBookHistories.IdPbh;
                    }

                    _context.PhoneBookHistories
                        .Value(p => p.Date, DateTime.Now)
                        .Value(p => p.IdPb, _values.IdPb)
                        .Value(p => p.IdPbh, idpbh + 1)
                        .Value(p => p.Name, _values.Name)
                        .Value(p => p.Patronymic, _values.Patronymic)
                        .Value(p => p.Phone, _values.Phone)
                        .Value(p => p.Surname, _values.Surname)
                        .Value(p => p.Sex, _values.Sex)
                        .Insert();
                }
                transaction.Commit();
                return new OkResult();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.ToString());
            }
            //return new OkResult();
        }

        public class DataGridController : Controller
        {
            public ActionResult PopupEditing()
            {
                return View();
            }
        }
    }
}

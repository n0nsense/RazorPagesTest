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
using Newtonsoft.Json;
using Splat;

namespace RazorPages.Pages
{
    public class PhoneBookModel : PageModel
    {
        private RazorBaseDB _context;
        public PhoneBookModel(RazorBaseDB context)
        {
            _context = context;
        }
        public IActionResult OnGetGridData(DataSourceLoadOptions loadOptions)
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
                    .Set(w => w.Name, _values.Name == null ? existingRecord.Name : _values.Name)
                    .Set(w => w.Patronymic, _values.Patronymic == null ? existingRecord.Patronymic : _values.Patronymic)
                    .Set(w => w.Phone, _values.Phone == null ? existingRecord.Phone : _values.Phone)
                    .Set(w => w.Surname, _values.Surname == null ? existingRecord.Surname : _values.Surname)
                    .Set(w => w.Sex, _values.Sex == null ? existingRecord.Sex : _values.Sex)
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
                        .Value(p => p.Name, _values.Name == null ? existingRecord.Name : _values.Name)
                        .Value(p => p.Patronymic, _values.Patronymic == null ? existingRecord.Patronymic : _values.Patronymic)
                        .Value(p => p.Phone, _values.Phone == null ? existingRecord.Phone : _values.Phone)
                        .Value(p => p.Surname, _values.Surname == null ? existingRecord.Surname : _values.Surname)
                        .Value(p => p.Sex, _values.Sex == null ? existingRecord.Sex : _values.Sex)
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

    }
}

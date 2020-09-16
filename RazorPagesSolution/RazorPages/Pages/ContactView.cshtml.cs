using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataModels;
using LinqToDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPages.Pages
{
    public class ContactViewModel : PageModel
    {
        private readonly RazorBaseDB _context;
        [BindProperty]
        public PhoneBook PBook { get; set; }

        public ContactViewModel(DataModels.RazorBaseDB context)
        {
            _context = context;
        }
        public IActionResult OnGet(int id)
        {
            PBook = _context.PhoneBooks.FirstOrDefault(i => i.IdPb == id);
            if (PBook == null)
                return Redirect("Error");
            else
                return Page();
        }

        public IActionResult OnGetListHistory(int id)
        {
            return new JsonResult(_context.PhoneBookHistories.ToList().Where(i => i.IdPb == id));
        }

        public IActionResult OnPost(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                var transaction = _context.BeginTransaction();
                PBook.IdPb = id;
                var updated = _context.Update(PBook);

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
                        .Value(p => p.IdPb, PBook.IdPb)
                        .Value(p => p.IdPbh, idpbh + 1)
                        .Value(p => p.Name, PBook.Name)
                        .Value(p => p.Patronymic, PBook.Patronymic)
                        .Value(p => p.Phone, PBook.Phone)
                        .Value(p => p.Surname, PBook.Surname)
                        .Value(p => p.Sex, PBook.Sex)
                        .Insert();
                }
                transaction.Commit();
                return Redirect("/");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.ToString());
            }
        }
    }
}

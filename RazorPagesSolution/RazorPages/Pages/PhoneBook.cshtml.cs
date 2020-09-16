using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
        [BindProperty]
        public int DataCount { get; set; }
        public PhoneBookModel(RazorBaseDB context)
        {
            _context = context;
        }
        public IActionResult OnGetGridData()
        {
            return new JsonResult(_context.PhoneBooks.ToList());        
        }
        public async Task<IActionResult> OnPostAsync(int DataCount)
        {
            try
            {
                await GenerateRandomDataAsync(DataCount);
                return Redirect("/");
                
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            
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
                //
                var _values = JsonConvert.DeserializeObject<DataModels.PhoneBook>(values);
                InsertNewData(_values);
                transaction.Commit();
                return new OkResult();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.ToString());
            }
            //return new OkResult();
        }

        public void InsertNewData(PhoneBook phoneBook)
        {

            int idpb = 0;
            var qPhoneBooks = from p in _context.PhoneBooks orderby p.IdPb descending select p;
            var resultPhoneBooks = qPhoneBooks.FirstOrDefault();
            if (resultPhoneBooks != null)
            {
                idpb = resultPhoneBooks.IdPb + 1;
            }

            phoneBook.IdPb = idpb;

            var inserted = _context.Insert(phoneBook);

            if (inserted != 1)
                throw new Exception("Не получилось добавить новую запись");
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
                    .Value(p => p.IdPb, phoneBook.IdPb)
                    .Value(p => p.IdPbh, idpbh + 1)
                    .Value(p => p.Name, phoneBook.Name)
                    .Value(p => p.Patronymic, phoneBook.Patronymic)
                    .Value(p => p.Phone, phoneBook.Phone)
                    .Value(p => p.Surname, phoneBook.Surname)
                    .Value(p => p.Sex, phoneBook.Sex)
                    .Insert();
            }
        }

        public async Task GenerateRandomDataAsync(int maxCount)
        {
            //Мужские имена в массив
            string MaleNamesPath = "OtherFiles\\MaleNames.txt";
            string[] MaleNamesArray;
            using (StreamReader sr = new StreamReader(MaleNamesPath))
            {
                string result = await sr.ReadToEndAsync();
                MaleNamesArray = result.Split("\r\n");
            }

            //Женские имена в массив
            string FemaleNamesPath = "OtherFiles\\FemaleNames.txt";
            string[] FemaleNamesArray;
            using (StreamReader sr = new StreamReader(FemaleNamesPath))
            {
                string result = await sr.ReadToEndAsync();
                FemaleNamesArray = result.Split("\r\n");
            }
            //Отчества в массив
            string PatronymicNamesPath = "OtherFiles\\patronymic.txt";
            string[] PatronymicArray;
            using (StreamReader sr = new StreamReader(PatronymicNamesPath))
            {
                string result = await sr.ReadToEndAsync();
                PatronymicArray = result.Split("\r\n");
            }

            //Фамилии в массив
            string SurnamesNamesPath = "OtherFiles\\surnames.txt";
            string[] SurnamesArray;
            using (StreamReader sr = new StreamReader(SurnamesNamesPath))
            {
                string result = await sr.ReadToEndAsync();
                SurnamesArray = result.Split("\r\n");
            }
            //пол
            string[] sexArray = { "f", "m" };
            
            while(maxCount !=0)
            {
                
                Random rnd = new Random();
                PhoneBook phoneBook = new PhoneBook();
                int sexChoice = rnd.Next(0, 2);
                if(sexChoice == 0)
                {
                    phoneBook.Sex = "f";
                    phoneBook.Name = FemaleNamesArray[rnd.Next(0, FemaleNamesArray.Length-1)];
                }
                else
                {
                    phoneBook.Sex = "m";
                    phoneBook.Name = MaleNamesArray[rnd.Next(0, MaleNamesArray.Length - 1)];
                }
                phoneBook.Surname = SurnamesArray[rnd.Next(0, SurnamesArray.Length - 1)];
                phoneBook.Patronymic = PatronymicArray[rnd.Next(0, PatronymicArray.Length - 1)];
                phoneBook.Phone = "+7" + rnd.Next(100, 999).ToString() + rnd.Next(1000000, 9999999).ToString();

                try
                {
                    var transaction = _context.BeginTransaction();

                    InsertNewData(phoneBook);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }

                maxCount += -1;
            }
        }

    }
}

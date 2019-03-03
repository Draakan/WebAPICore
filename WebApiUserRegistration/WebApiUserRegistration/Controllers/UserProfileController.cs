using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApiUserRegistration.Models;

namespace WebApiUserRegistration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;
        private AuthenticationContext _db;

        public UserProfileController(
            UserManager<ApplicationUser> userManager,
            AuthenticationContext db
            )
        {
            _userManager = userManager;
            _db = db;
        }

        [HttpGet]
        [Authorize]
        public async Task<Object> GetUserProfile()
        {
            var user = await GetUser();
            return new { user.FullName };
        }

        [HttpGet]
        [Authorize]
        [Route("UserBill")]
        public async Task<Object> GetUserBill()
        {
            var user = await GetUser();

            return _db.Bills
                .Where(x => x.Id == user.Id)
                .Select(x => new { x.Value, x.Currency })
                .First();
        }

        [HttpPost]
        [Authorize]
        [Route("AddCategory")]
        public async Task<Object> PostAddCategory(Category category)
        {
            var user = await GetUser();
            Category newCategory = new Category() { Name = category.Name, Capacity = category.Capacity, ApplicationUserId = user.Id };

            await _db.Categories.AddAsync(newCategory);
            await _db.SaveChangesAsync();

            return category;
        }

        [HttpPost]
        [Authorize]
        [Route("AddEvent")]
        public async Task<Object> PostAddEvent(Event @event)
        {
            var user = await GetUser();

            try
            {
                Event newEvent = new Event()
                {
                    Type = @event.Type,
                    Amount = @event.Amount,
                    CategoryId = @event.CategoryId,
                    Description = @event.Description,
                    Date = @event.Date,
                    ApplicationUserId = user.Id
                };

                await _db.Events.AddAsync(newEvent);
                await _db.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut]
        [Authorize]
        [Route("UpdateBill")]
        public async Task<Object> PutUpdateBill(Bill bill)
        {
            var user = await GetUser();

            _db.Bills.Where(x => x.Id == user.Id).First().Value = bill.Value;
            await _db.SaveChangesAsync();

            return Ok();
        }

        [HttpGet]
        [Authorize]
        [Route("UserCategory")]
        public async Task<Object> GetUserCategory()
        {
            var user = await GetUser();

            return _db.Categories
                .Where(x => x.ApplicationUserId == user.Id)
                .Select(x => new { x.Name, x.Capacity, x.Id, x.ApplicationUserId })
                .ToList();
        }

        [HttpGet]
        [Authorize]
        [Route("UserCategoryId/{id}")]
        public async Task<Object> GetUserCategoryId(string id)
        {
            var user = await GetUser();

            return _db.Categories
                .Where(x => x.ApplicationUserId == user.Id)
                .Where(x => x.Id == id )
                .Select(x => new { x.Name, x.Capacity, x.Id, x.ApplicationUserId })
                .First();
        }

        [HttpGet]
        [Authorize]
        [Route("UserEvents")]
        public async Task<Object> GetUserEvents()
        {
            var user = await GetUser();

            return _db.Events
                .Where(x => x.ApplicationUserId == user.Id)
                .Select(x => new { x.Type, x.Amount, x.CategoryId, x.Date, x.Description, x.Id })
                .ToList();
        }

        [HttpGet]
        [Authorize]
        [Route("EventById/{id}")]
        public async Task<Object> GetEventById(string id)
        {
            var user = await GetUser();

            return _db.Events
                .Where(x => x.ApplicationUserId == user.Id)
                .Where(x => x.Id == id)
                .Select(x => new { x.Type, x.Amount, x.CategoryId, x.Date, x.Description, x.Id })
                .First();
        }


        [HttpGet]
        [Route("Exchange")]
        public async Task<Object> GetExchange()
        {
            System.Net.Http.HttpClient http = new System.Net.Http.HttpClient();

            return await http.GetAsync("https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?json").Result.Content.ReadAsStringAsync();
        }

        private async Task<ApplicationUser> GetUser() => await _userManager.FindByIdAsync(User.Claims.First(c => c.Type == "UserID").Value);
    }
}

//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using AmlaProductCatalog.Data;
//using AmlaProductCatalog.Models;

//namespace AmlaProductCatalog.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class AmlaProductCatalogController : ControllerBase
//    {
//        private readonly ApplicationDbContext _context;

//        public AmlaProductCatalogController(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        // ✅ 1. SAVE USER + REQUEST + RESPONSE
//        [HttpPost("save1")]
//        public async Task<IActionResult> SaveUserRequest1(IFormFile responsePdf, string email, string templatName)
//        {
//            return null;
//        }
//        [HttpPost("save")]
//        public async Task<IActionResult> SaveUserRequest([FromBody] AmlaProductCatalogModel input)
//        {
//            if (input == null || string.IsNullOrEmpty(input.Email))
//                return BadRequest("Invalid request");

//            try
//            {
//                // Get or create user
//                var user = await _context.Users
//                    .FirstOrDefaultAsync(x => x.Email == input.Email);

//                if (user == null)
//                {
//                    user = new User
//                    {
//                        Email = input.Email
//                    };

//                    await _context.Users.AddAsync(user);
//                    await _context.SaveChangesAsync();
//                }

//                // Save request
//                var userRequest = new UserRequest
//                {
//                    UserId = user.UserId,
//                    TemplateName = input.TemplateName,
//                    Request = input.Request,
//                    Response = input.Response
//                };

//                await _context.UserRequests.AddAsync(userRequest);
//                await _context.SaveChangesAsync();

//                return Ok(new
//                {
//                    message = "Request & response saved successfully",
//                    email = user.Email,
//                    userId = user.UserId,
//                    requestId = userRequest.Id
//                });
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, new
//                {
//                    message = "Something went wrong",
//                    error = ex.Message
//                });
//            }
//        }

//        // ✅ 2. GET ALL REQUESTS
//        [HttpGet("all")]
//        public async Task<IActionResult> GetAllRequests()
//        {
//            var data = await _context.UserRequests
//                .OrderByDescending(x => x.CreatedDate)
//                .ToListAsync();

//            return Ok(data);
//        }

//        // ✅ 3. GET REQUESTS BY USER ID
//        [HttpGet("user/{userId}")]
//        public async Task<IActionResult> GetRequestsByUser(int userId)
//        {
//            var data = await _context.UserRequests
//                .Where(x => x.UserId == userId)
//                .OrderByDescending(x => x.CreatedDate)
//                .ToListAsync();

//            if (!data.Any())
//                return NotFound("No records found");

//            return Ok(data);
//        }

//        // ✅ 4. GET REQUESTS BY EMAIL
//        [HttpGet("email/{email}")]
//        public async Task<IActionResult> GetRequestsByEmail(string email)
//        {
//            var user = await _context.Users
//                .FirstOrDefaultAsync(x => x.Email == email);

//            if (user == null)
//                return NotFound("User not found");

//            var data = await _context.UserRequests
//                .Where(x => x.UserId == user.UserId)
//                .ToListAsync();

//            return Ok(data);
//        }

//        // ✅ 5. UPDATE RESPONSE
//        [HttpPut("update-response/{requestId}")]
//        public async Task<IActionResult> UpdateResponse(int requestId, [FromBody] string response)
//        {
//            var request = await _context.UserRequests
//                .FirstOrDefaultAsync(x => x.Id == requestId);

//            if (request == null)
//                return NotFound("Request not found");

//            request.Response = response;

//            await _context.SaveChangesAsync();

//            return Ok(new
//            {
//                message = "Response updated successfully",
//                requestId = requestId
//            });
//        }

//        // ✅ 6. DELETE REQUEST
//        [HttpDelete("delete/{requestId}")]
//        public async Task<IActionResult> DeleteRequest(int requestId)
//        {
//            var request = await _context.UserRequests
//                .FirstOrDefaultAsync(x => x.Id == requestId);

//            if (request == null)
//                return NotFound("Request not found");

//            _context.UserRequests.Remove(request);
//            await _context.SaveChangesAsync();

//            return Ok(new
//            {
//                message = "Request deleted successfully",
//                requestId = requestId
//            });
//        }
//    }
//}


using Microsoft.AspNetCore.Mvc;
using AmlaProductCatalog.Models;

namespace AmlaProductCatalog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AmlaProductCatalogController : ControllerBase
    {
        private readonly JsonFileService _jsonService;

        public AmlaProductCatalogController(JsonFileService jsonService)
        {
            _jsonService = jsonService;
        }

        // ✅ SAVE
        [HttpPost("save")]
        public async Task<IActionResult> SaveUserRequest([FromBody] AmlaProductCatalogModel input)
        {
            if (input == null)
                return BadRequest("Invalid request");

            var data = await _jsonService.GetAsync();

            var newItem = new UserRequest
            {
                Id = data.Count + 1,
                TemplateName = input.TemplateName,
                Request = input.Request,
                Response = input.Response,
                CreatedDate = DateTime.UtcNow
            };

            data.Add(newItem);

            await _jsonService.SaveAsync(data);

            return Ok(newItem);
        }

        // ✅ GET ALL
        [HttpGet("all")]
        public async Task<IActionResult> GetAllRequests()
        {
            var data = await _jsonService.GetAsync();
            return Ok(data.OrderByDescending(x => x.CreatedDate));
        }

        // ✅ GET BY ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _jsonService.GetAsync();
            var item = data.FirstOrDefault(x => x.Id == id);

            if (item == null)
                return NotFound();

            return Ok(item);
        }

        // ✅ UPDATE RESPONSE
        [HttpPut("update-response/{id}")]
        public async Task<IActionResult> UpdateResponse(int id, [FromBody] string response)
        {
            var data = await _jsonService.GetAsync();
            var item = data.FirstOrDefault(x => x.Id == id);

            if (item == null)
                return NotFound();

            item.Response = response;

            await _jsonService.SaveAsync(data);

            return Ok("Updated successfully");
        }

        // ✅ DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _jsonService.GetAsync();
            var item = data.FirstOrDefault(x => x.Id == id);

            if (item == null)
                return NotFound();

            data.Remove(item);

            await _jsonService.SaveAsync(data);

            return Ok("Deleted successfully");
        }
    }
}
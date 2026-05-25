using BusinessLayer;
using LMS.DataAccess.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace APIs
{
    [Route("api/members")]
    [ApiController]
    public class MemberController
    {
        [HttpGet("create")]
        public async Task<Profile> Create(Profile profile)
        {
            Member member = new Member()
             
        }
    }
}

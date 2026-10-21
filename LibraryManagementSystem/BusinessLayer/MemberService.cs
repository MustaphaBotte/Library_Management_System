using BCrypt;
using LMS.DataAccess;
using LMS.DTOs;
using LMS.Models;
using Microsoft.Data.SqlClient;
namespace LMS.BusinessLayer
{
    public class MemberService
    {
        private readonly MemberRepository _memberRepository;
        public MemberService(MemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public async Task<int> RegisterAsync(RegisterMemberDto dto)
        {
            var person = new Person
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };
            if (_memberRepository.GetByEmail(dto.Email)!=null)
            {
                throw new Exceptions.EmailAlreadyExistsException("Email already in use"); 
            }
            return await _memberRepository.AddAsync(person);
        }     
    }
}

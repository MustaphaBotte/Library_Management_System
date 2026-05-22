using LMS.BusinessLayer;
using System;

namespace BusinessLayer
{
    public class Member : Person
    {
    
        public uint MemberId { get; private set; } = 0;
        public DateTime ExpiredAt { get; set; } = new DateTime();
        public bool IsBanned { get; set; } = false;
        public int MemberShipStatusId { get; set; } = 0; 
        public uint LibraryId { get; set; } = 0;
        public string Notes { get; set; } = "";

        public Member(Profile memberProfile) : base(memberProfile)
        {
           

            
        }

        public MemberDTO MemberDTO => new MemberDTO(PersonId, MemberId, ExpiredAt, IsBanned, MemberShipStatusId, LibraryId, Notes);

        public async static Task<Member?> GetMemberByIdAsync(uint MemberID)
        {
            var memberDTO = await MemberRepository.GetMemberAsync(MemberID);
            if (memberDTO == null) return null;

            var personDTO = await PersonRepository.GetPersonAsync(memberDTO.PersonId);
            if (personDTO == null) return null;

            return new Member(new Profile(memberDTO.MemberId, personDTO.FirstName,personDTO.SecondName,personDTO.Email??"", "",
                personDTO.PhoneNumber , personDTO.DateOfBirth,
                personDTO.Gender,personDTO.CountryID ,memberDTO.LibraryId));

        }

        public override async Task<bool> Save()
        {
            var memberDTO = new MemberDTO(PersonId, MemberId, ExpiredAt, false, this.MemberShipStatusId, LibraryId, Notes);

            if (await base.Save())
            {
               int insertedMemberID= await MemberRepository.AddNewMemberAsync(memberDTO);
                if (insertedMemberID > 0)
                {
                    this.MemberId =(uint)insertedMemberID;
                    return true;
                }
            }
            return false;

        }



    }
}

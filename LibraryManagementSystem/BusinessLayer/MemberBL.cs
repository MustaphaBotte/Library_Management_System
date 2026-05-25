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

        public Member(Profile personInfo,MemberDTO member) : base(personInfo)
        {
            this.LibraryId = LibraryId;
            IsBanned = false;
            ExpiredAt = DateTime.Now.AddYears(1);
            MemberShipStatusId = 1;
        }

        public MemberDTO MemberDTO => new MemberDTO(base.PersonId, MemberId, ExpiredAt, IsBanned, MemberShipStatusId, LibraryId);

        public async static Task<Profile?> GetMemberByIdAsync(uint MemberID)
        {
            var memberDTO = await MemberRepository.GetMemberAsync(MemberID);
            if (memberDTO == null) return null;

            var personDTO = await PersonRepository.GetPersonAsync(memberDTO.PersonId);
            if (personDTO == null) return null;

            return new Profile(memberDTO.MemberId, personDTO.FirstName, personDTO.SecondName, personDTO.Email ?? "", "",
                                          personDTO.PhoneNumber, personDTO.DateOfBirth,
                                           personDTO.Gender, personDTO.CountryID, memberDTO.LibraryId,personDTO.Notes);

        }

        public override async Task<bool> Save()
        {
            switch (base._Mode)
            {
                case EnMode.Add:
                    if (await base.Save())
                    {
                        int insertedMemberID = await MemberRepository.AddNewMemberAsync(MemberDTO);
                        if (insertedMemberID > 0)
                        {
                            this.MemberId = (uint)insertedMemberID;
                            return true;
                        }
                    }
                    break;

                case EnMode.Update:
                    {
                        await base.Save();
                        return await MemberRepository.UpdateMemberAsync(MemberDTO);
                    }


            }
            return false;
        }

        public static async Task<bool> DeleteMemberAsync(uint MemberID)
        {
            return await MemberRepository.DeleteMemberAsync(MemberID);
        }

    }
}

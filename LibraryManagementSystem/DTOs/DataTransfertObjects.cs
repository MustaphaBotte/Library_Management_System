namespace LMS.DTOs
{
    public class PersonDto
    {
        public uint PersonId { private set; get; } = 0;
        public string FirstName { set; get; } = "";
        public string LastName { set; get; } = "";
        public string Email { set; get; } = "";
        public string PhoneNumber { set; get; } = "";
        public DateTime DateOfBirth { set; get; } = DateTime.Now.AddYears(-18);
        public char Gender { set; get; } = '?';
        public DateTime CreatedAt { set; get; } = DateTime.Now;
        public DateTime UpdatedAt { set; get; } = DateTime.Now;
        public int CreatedBy { set; get; } = -1;
        public int CountryID { set; get; } = -1;
        public string ProfilePicturePath { set; get; } = "";

        public bool IsDeleted { set; get; } = false;

        public PersonDto(uint personID, string firstName, string lastName, string email,string phonenumber, DateTime dateOfBirth, char gender,
                    DateTime createdAt, DateTime updatedAt, int createdBy, int countryId, string profilePicturePath, bool isDeleted)
        {
            PersonId = personID;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phonenumber;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            CreatedBy = createdBy;
            CountryID = countryId;
            ProfilePicturePath = profilePicturePath;
            IsDeleted = isDeleted;
        }
    }
    public class MemberDto
    {
        public MemberDto(uint PersonId, uint memberId, uint personId, string username, string passwordHash, string passwordSalt, DateTime joinedAt,
                        DateTime expiredAt, bool isBanned, int memberShipStatusId, DateTime lastBorrowAt, uint libraryId, string notes)
        {
            this.PersonId = PersonId ;
            this.MemberId = memberId;
            this.Username = username;
            this.PasswordHash = passwordHash;
            this.PasswordSalt = passwordSalt;
            this.JoinedAt = joinedAt;
            this.ExpiredAt = expiredAt;
            this.IsBanned = isBanned;
            this.MemberShipStatusId = memberShipStatusId;
            this.LastBorrowAt = lastBorrowAt;
            this.LibraryId = libraryId;
            this.Notes = notes;
        }
        public uint PersonId { private set; get; } = 0;
        public uint MemberId { private set; get; } = 0;
        public string Username { set; get; } = "";
        public string PasswordHash { set; get; } = "";
        public string PasswordSalt { set; get; } = "";
        public DateTime JoinedAt { set; get; } = DateTime.Now;
        public DateTime ExpiredAt { set; get; } = DateTime.Now.AddYears(1);
        public bool IsBanned { set; get; } = false;
        public int MemberShipStatusId { set; get; } = -1;
        public DateTime LastBorrowAt { set; get; } = DateTime.Now.AddYears(1);
        public uint LibraryId { set; get; } = 0;
        public string Notes { set; get; } = "";
    }


}
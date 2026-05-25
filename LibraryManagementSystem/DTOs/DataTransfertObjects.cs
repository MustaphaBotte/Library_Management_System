namespace LMS.DataAccess.DTOs
{
    public class PersonDTO
    {
        public uint PersonId { private set; get; } = 0;
        public string FirstName { set; get; } = "";
        public string SecondName { set; get; } = "";
        public string? Email { set; get; } = "";
        public string PasswordHash { set; get; } = "";
        public string PhoneNumber { set; get; } = "";
        public DateTime DateOfBirth { set; get; }
        public char Gender { set; get; } = '?';
        public DateTime CreatedAt { set; get; } = DateTime.Now;
        public DateTime UpdatedAt { set; get; } = DateTime.Now;
        public int? CreatedBy { set; get; } =null;
        public uint CountryID { set; get; } = 0;
        public string? ProfilePicturePath { set; get; } = "";
        public bool IsDeleted { set; get; } = false;
        public string? Notes { set; get; } = "";

        public PersonDTO(uint personID, string firstName, string secondName,  string? email, string phonenumber, string passwordHash, DateTime dateOfBirth, char gender,
                    DateTime createdAt, DateTime updatedAt, int? createdBy, uint countryId, string? profilePicturePath, bool isDeleted ,string? notes)
        {
            PersonId = personID;
            SecondName = secondName;
            FirstName = firstName;
            Email = email;
            PasswordHash = passwordHash;
            Notes = notes;
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
    public class MemberDTO
    {
        public MemberDTO(uint personId, uint memberId, DateTime expiredAt, bool isBanned,
            int memberShipStatusId,uint libraryId)
        {
            this.PersonId = personId;
            this.MemberId = memberId;
            this.ExpiredAt = expiredAt;
            this.IsBanned = isBanned;
            this.MemberShipStatusId = memberShipStatusId;
            this.LibraryId = libraryId;
        }
        public uint PersonId { private set; get; } = 0;
        public uint MemberId { private set; get; } = 0;
        public DateTime ExpiredAt { set; get; } = DateTime.Now.AddYears(1);
        public bool IsBanned { set; get; } = false;
        public int MemberShipStatusId { set; get; } = -1;
        public uint LibraryId { set; get; } = 0;
    }

    // for the api 
    public class Profile

    {
        public Profile(uint memberID , string firstName, string lastName, string email, string plainPassword, string phoneNumber, 
                                 DateTime dateOfBirth, char gender, uint countryId, uint libraryId,string ? notes)
        {
            MemberId = memberID;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PlainPassword = plainPassword;
            PhoneNumber = phoneNumber;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            CountryId = countryId;
            LibraryId = libraryId;
            Notes = notes;
        }
        public uint MemberId { private set; get; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public string PlainPassword { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public DateTime DateOfBirth { get; set; }
        public char Gender { get; set; }
        public uint CountryId { get; set; }
        public uint LibraryId { get; set; }

        public string? Notes { set; get; } = "";

    }
    public class CountryDTO
    {
        public CountryDTO(uint countryId, string countryName)
        {
            CountryId = countryId;
            CountryName = countryName;
        }
        public uint CountryId { get; }
        public string CountryName { get; }
    }

    public class EmployeeDTO
    {
        public EmployeeDTO(uint personId, uint employeeId, uint managerId, uint libraryId, 
                           DateTime hireDate, DateTime contractEndDate, bool isActive)
        {
            PersonId = personId;
            EmployeeId = employeeId;
            ManagerId = managerId;
            LibraryId = libraryId;
            HireDate = hireDate;
            ContractEndDate = contractEndDate;
            IsActive = isActive;
            IsDeleted = false;
        }

        public uint PersonId { private set; get; } = 0;
        public uint EmployeeId { private set; get; } = 0;
        public uint ManagerId { private set; get; } = 0;
        public uint LibraryId { private set; get; } = 0;
        public DateTime HireDate { set; get; }
        public DateTime ContractEndDate { set; get; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

    }

}
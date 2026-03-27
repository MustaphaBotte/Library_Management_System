using LMS.DTOs;
using System.Net.Mail;
using System.Text.RegularExpressions;
using LMS.DataAccess;
using System.Data;
using static LMS.DataAccess.Exceptions;
using System.Numerics;

namespace LMS.BusinessLayer
{
    public class Person
    {
        public class PersonException:Exception
        {
            public override string Message {get; }
            public PersonException(string message)
            {
                Message = message;
            }

        }
        private enum EnMode {Add=1 , Update = 2 }
        EnMode _Mode = EnMode.Add;

        public uint PersonId { get; private set; } = 0;

        private string _firstName = "";
        public string FirstName
        {
            get => _firstName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException(nameof(FirstName), "The First Name cannot be empty!");
                if (!Regex.IsMatch(value, @"^[A-Za-zÀ-ÿ\s'-]+$"))
                    throw new ArgumentException("The First Name contains invalid characters.");
                _firstName = value;
            }
        }

        private string _lastName = "";
        public string LastName
        {
            get => _lastName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException(nameof(LastName), "The Last Name cannot be empty!");
                if (!Regex.IsMatch(value, @"^[A-Za-zÀ-ÿ\s'-]+$"))
                    throw new ArgumentException("The Last Name contains invalid characters.");
                _lastName = value;
            }
        }

        private string? _email = "";
        public string? Email
        {
            get => _email;
            set
            {
                if (MailAddress.TryCreate(value, out _))
                {
                    _email = value;
                }else
                    throw new ArgumentException("Invalid email format.");

            }
        }

        private string _phoneNumber = "";
        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    if (!Regex.IsMatch(value, @"^\+?[0-9\s\-]{7,15}$"))
                        throw new ArgumentException("Invalid Phone Number format.");
                }
                _phoneNumber = value;
            }
        }

        public DateTime DateOfBirth { get; set; } = DateTime.Now.AddYears(-18);

        private char _gender = ' ';
        public char Gender
        {
            get => _gender;
            set
            {
                if (value != 'M' && value != 'F')
                    throw new ArgumentException("Gender must be 'M', 'F'");
                _gender = value;
            }
        }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public int? CreatedBy { get; set; } = null;
        public int CountryID { get; set; } = -1;
        public string? ProfilePicturePath { get; set; } = "";

        public Person() 
        {
            this._Mode = EnMode.Add;
        }

        public Person(PersonDto dto)
        {
            PersonId = dto.PersonId;
            FirstName = dto.FirstName;
            LastName = dto.LastName;
            Email = dto.Email;
            PhoneNumber = dto.PhoneNumber;
            DateOfBirth = dto.DateOfBirth;
            Gender = dto.Gender;
            CreatedAt = dto.CreatedAt;
            UpdatedAt = dto.UpdatedAt;
            CreatedBy = dto.CreatedBy;
            CountryID = dto.CountryID;
            ProfilePicturePath = dto.ProfilePicturePath;
            this._Mode = EnMode.Update;
        }
        public static async Task<Person?> GetPersonAsync(uint PersonID)
        {
           var personDto =await PersonRepository.GetPersonAsync(PersonID);
            if(personDto!=null)
            {
                return new Person(personDto);
            }
            return null;
        }

        public static async Task<bool> IsPersonExists(uint PersonID)
        {
           return await PersonRepository.IsPersonExists(PersonID);
          
        }
        public static async Task<bool> IsEmailExists(string Email)
        {
            return await PersonRepository.IsEmailExists(Email);           
        }
        public static async Task<bool> IsPhoneNumberExists(string PhoneNumber)
        {
            return await PersonRepository.IsPhoneNumberExists(PhoneNumber);
        }
        public static async Task<DataTable?> GetPeopleAsync(int LastId, int Rows = 10)
        {
            var People = await PersonRepository.GetPeopleAsync(LastId,Rows);
            if (People != null)
            {
                return People;
            }
            return null;
        }
        public async Task<bool> Save()
        {
            
            var PersonDto = new PersonDto(PersonId, FirstName, LastName, Email, PhoneNumber, DateOfBirth, Gender, CreatedAt, UpdatedAt,
                    CreatedBy, CountryID, ProfilePicturePath, false);

            try
            {
                switch (this._Mode)
                {
                    case EnMode.Add:
                    int InsertedId = await PersonRepository.AddNewPersonAsync(PersonDto);
                    if (InsertedId > 0)
                    {
                        this.PersonId = (uint)InsertedId;
                        return true;
                    }
                        break;

                    case EnMode.Update:                 
                    bool IsSuccess = await PersonRepository.UpdatePersonAsync(PersonDto);
                    return IsSuccess;
                  
                }
            }
            catch (Exceptions.CannotInsertNullException)
            {
                throw new PersonException("Some required fields are missing. Please fill all mandatory fields");
            }
            catch (Exceptions.OperationTimeoutException)
            {
                throw new PersonException("The operation timed out. Please try again in a few moments.");
            }
            catch (Exceptions.ForeignKeyViolationException)
            {
                throw new PersonException("Cannot complete this operation because related data does not exist.");
            }
            catch (Exceptions.StringTruncationException)
            {
                throw new PersonException("One of the input values is too long. Please shorten it and try again.");
            }
            catch (Exceptions.UniqueConstraintViolation)
            {
                throw new PersonException("A record with the same unique data already exists. Please check your input.");
            }   
            catch (Exception e)
            {
                throw new PersonException("An unexpected error occurred. Please contact support.");
            }
            return false;           
        }



    }
}
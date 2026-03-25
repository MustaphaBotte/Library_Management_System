namespace LMS.BusinessLayer
{
    public class PersonService
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
        EnMode _Mode = EnMode.Add ;

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
                if (!string.IsNullOrEmpty(value) && MailAddress.TryCreate(value, out _))
                {
                    _email = value;
                }
                else throw new ArgumentException("Invalid email format.");

            }
        }

        private string _phoneNumber = "";
        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                if (!string.IsNullOrWhiteSpace(value) && !Regex.IsMatch(value, @"^\+?[0-9\s\-]{7,15}$"))
                {
                    _phoneNumber = value;
                }
                else throw new ArgumentException("Invalid Phone Number format.");
                  
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
        public string? ProfilePicturePath { get; set; } = "";

        public (uint CountryId, string CountryName) Country; 
        public PersonService() 
        {
            this._Mode = EnMode.Add;
        }

        public PersonService(PersonEntity dto)
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
            ProfilePicturePath = dto.ProfilePicturePath;
            this._Mode = EnMode.Update;
        }
        public static async Task<PersonService?> GetPersonAsync(uint PersonID)
        {
           var PersonEntity =await PersonRepository.GetPersonAsync(PersonID);
            if(PersonEntity!=null)
            {
                
                var Person = new PersonService(PersonEntity);
                var Country =await CountryRepository.GetCountryById(PersonEntity.CountryID);
                if(Country!=null)
                {
                    
                    Person.Country.CountryId = Country.CountryId;
                    Person.Country.CountryName = Country.CountryName;
                }
                return Person;
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
            return await PersonRepository.GetPeopleAsync(LastId,Rows);        
        }
        public async Task<bool> Save()
        {
            
            var PersonEntity = new PersonEntity(PersonId, FirstName, LastName, Email, PhoneNumber, DateOfBirth, Gender, CreatedAt, UpdatedAt,
                    CreatedBy,Country.CountryId, ProfilePicturePath, false);

            try
            {
                switch (this._Mode)
                {
                    case EnMode.Add:
                    int InsertedId = await PersonRepository.AddNewPersonAsync(PersonEntity);
                    if (InsertedId > 0)
                    {
                        this.PersonId = (uint)InsertedId;
                        return true;
                    }
                        break;

                    case EnMode.Update:                 
                    bool IsSuccess = await PersonRepository.UpdatePersonAsync(PersonEntity);
                    return IsSuccess;
                  
                }
            }
            catch (CannotInsertNullException)
            {
                throw new PersonException("Some required fields are missing. Please fill all mandatory fields");
            }
            catch (OperationTimeoutException)
            {
                throw new PersonException("The operation timed out. Please try again in a few moments.");
            }
            catch (ForeignKeyViolationException)
            {
                throw new PersonException("Cannot complete this operation because related data does not exist.");
            }
            catch (StringTruncationException)
            {
                throw new PersonException("One of the input values is too long. Please shorten it and try again.");
            }
            catch (UniqueConstraintViolation)
            {
                throw new PersonException("A record with the same unique data already exists. Please check your input.");
            }   
            catch (Exception)
            {
                throw new PersonException("An unexpected error occurred. Please contact support.");
            }
            return false;           
        }



    }
}
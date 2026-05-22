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

        private string _secondName = "";
        public string SecondName
        {
            get => _secondName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException(nameof(_secondName), "The Last Name cannot be empty!");
                if (!Regex.IsMatch(value, @"^[A-Za-zÀ-ÿ\s'-]+$"))
                    throw new ArgumentException("The Last Name contains invalid characters.");
                _secondName = value;
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
                if (string.IsNullOrWhiteSpace(value)||!Regex.IsMatch(value, @"^\+?[0-9\s\-]{7,15}$"))              
                    throw new ArgumentException("Invalid Phone Number format.");
                
                _phoneNumber = value;
            }
        }

        private string _passwordHash = "";
        private string Password {
            set
            {
                if (!Regex.IsMatch(value, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]).{8,64}$"))
                    throw new ArgumentException("PasswordHash must be 8-64 characters and include uppercase, lowercase, number, and special character.");

                _passwordHash = BCrypt.Net.BCrypt.HashPassword(value, workFactor: 12);
            }
        }
      
        private DateTime _dateOfBirth { get; set; } = DateTime.Now.AddYears(-18);
        public DateTime DateOfBirth { get => _dateOfBirth;
            set
            {
                if (value > DateTime.Now.AddYears(-18))
                    throw new ArgumentException("Member must be at least 18 years old.");
                _dateOfBirth = value;
            }
        }

        private char _gender = ' ';
        public char Gender
        {
            get => _gender;
            set
            {
                char toUpperCase = char.ToUpper(value);
                if (toUpperCase != 'M' && toUpperCase != 'F')
                {
                    throw new ArgumentException("Gender must be 'M', 'F'");
                }
                _gender = toUpperCase;
            }
        }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public int? CreatedBy { get; set; } = null;
        public string? ProfilePicturePath { get; set; } = "";

        public uint CountryID { set; get; } = 0;

        public CountryDTO? Country = null;      
        public bool IsDeleted { set; get; } = false;

        public Person(Profile dto)
        {
          
            FirstName = dto.FirstName;
            _secondName = dto.LastName;
            Email = dto.Email;
            Password = dto.PlainPassword;
            PhoneNumber = dto.PhoneNumber;
            DateOfBirth = dto.DateOfBirth;
            Gender = dto.Gender;
            CountryID = dto.CountryId;
            this._Mode = EnMode.Add;
        }  
        private Person(PersonDTO dto)
        {
            PersonId = dto.PersonId;
            FirstName = dto.FirstName;
            SecondName = dto.SecondName;
            Email = dto.Email;
            _passwordHash = dto.PasswordHash;
            PhoneNumber = dto.PhoneNumber;
            DateOfBirth = dto.DateOfBirth;
            Gender = dto.Gender;
            CountryID = dto.CountryID;
            CreatedAt = dto.CreatedAt;
            UpdatedAt = dto.UpdatedAt;
            CreatedBy = dto.CreatedBy;
            ProfilePicturePath = dto.ProfilePicturePath;
            this._Mode = EnMode.Update;
        }

        private PersonDTO personDTO => new PersonDTO(PersonId,FirstName, SecondName, Email,
                                                 PhoneNumber,_passwordHash, DateOfBirth, Gender, CreatedAt, UpdatedAt, CreatedBy,CountryID,ProfilePicturePath, IsDeleted);
        
        
        public static async Task<PersonDTO?> GetPersonAsync(uint PersonID)
        {
            var PersonDTO =await PersonRepository.GetPersonAsync(PersonID);
            if(PersonDTO!=null)
            {
                
                var Person = new Person(PersonDTO);
                var Country =await CountryRepository.GetCountryById(PersonDTO.CountryID);
                if(Country!=null)
                {
                    Person.Country = Country;
                }
                return Person.personDTO;
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
        public static async Task<List<PersonDTO>?> GetPeopleAsync(int LastId, int Rows = 10)
        {
            Rows = Rows > 10 || Rows < 1 ? 10 : Rows; // only fetch from 1 to 10 rows at a time
            return await PersonRepository.GetPeopleAsync(LastId,Rows);        
        }

        private async Task<int> _AddPersonAsync()
        {
            CountryDTO? country = await CountryRepository.GetCountryById(CountryID);
            if(country==null)
                throw new ArgumentException("Country not found");

            if (await Person.IsEmailExists(Email??""))
                throw new ArgumentException("Email Already In Use");

            if (await Person.IsPhoneNumberExists(PhoneNumber))
                throw new ArgumentException("Phone Number Already In Use");

            int personID = await PersonRepository.AddNewPersonAsync(this.personDTO);
            return personID;
        }

        /// <summary>
        /// Exceptions are intentionally not caught here.
        /// SQL exceptions are handled and translated at the repository layer (PersonRepository).
        /// </summary>
        public virtual async Task<bool> Save()
        {
            
           
            switch (this._Mode)
            {
                case EnMode.Add:
                 int insertedId = await _AddPersonAsync();
                 if (insertedId > 0)
                 {
                    this._Mode = EnMode.Update;
                    PersonId = (uint)insertedId;
                    return true;
                 }
                 break;

                case EnMode.Update:
                    return await PersonRepository.UpdatePersonAsync(this.personDTO);
            }     
            return false;           
        }



    }
}
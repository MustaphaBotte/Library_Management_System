namespace LMS.Models
{   
        // --- 1. User & Identity Models --
        public class Person
        {
            public int PersonID { get; set; }
            public string FirstName { get; set; } = string.Empty;
            public string LastName { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string PasswordHash { get; set; } = string.Empty;
            public string? ProfilePicturePath { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime? DeletedAt { get; set; }
        }

        public class Member
        {
            public int MemberID { get; set; }
            public int PersonID { get; set; }
            public string MembershipStatus { get; set; } = "Pending";
            public Person? Person { get; set; } = new Person();
        }

        public class Staff
        {
            public int StaffID { get; set; }
            public int PersonID { get; set; }
            public decimal Salary { get; set; }
            public int Permissions { get; set; } = 0;
        }


        // --- 2. Catalog Models ---

        public class Category
        {
            public int CategoryID { get; set; }
            public string Name { get; set; } = string.Empty;
            public string? Description { get; set; }
        }

        public class Book
        {
            public int BookID { get; set; }
            public int CategoryID { get; set; }
            public string? CoverImagePath { get; set; }
            public string ISBN { get; set; } = string.Empty;
            public string Title { get; set; } = string.Empty;
            public string? Description { get; set; }
            public short? PublicationYear { get; set; }
            public int? Pages { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime? DeletedAt { get; set; }
        }

        public class Author
        {
            public int AuthorID { get; set; }
            public string FirstName { get; set; } = string.Empty;
            public string LastName { get; set; } = string.Empty;
            public string? Bio { get; set; }
            public string? PicturePath { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime? DeletedAt { get; set; }
        }

        public class BookAuthor
        {
            public int BookID { get; set; }
            public int AuthorID { get; set; }
        }


        // --- 3. Inventory & Operations Models ---

        public class BookCopy
        {
            public int CopyID { get; set; }
            public int BookID { get; set; }
            public string Status { get; set; } = "Available";
            public string? Condition { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime? DeletedAt { get; set; }
        }

        public class Reservation
        {
            public int ReservationID { get; set; }
            public int BookID { get; set; }
            public int MemberID { get; set; }
            public DateTime ReservationDate { get; set; }
            public DateTime MaxPickUpDate { get; set; }
            public string Status { get; set; } = "ReadyForPickup";
            public DateTime CreatedAt { get; set; }
        }

        public class Borrow
        {
            public int BorrowID { get; set; }
            public int BookCopyID { get; set; }
            public int ReservationID { get; set; }
            public DateTime BorrowDate { get; set; }
            public DateTime DueDate { get; set; }
            public DateTime? ActualReturnDate { get; set; }
        }

        public class Fee
        {
            public int FeeID { get; set; }
            public int MemberID { get; set; }
            public int BorrowID { get; set; }
            public decimal Amount { get; set; }
            public string FeeType { get; set; } = string.Empty;
            public string Status { get; set; } = "Unpaid";
            public DateTime CreatedAt { get; set; }
            public DateTime? ResolvedAt { get; set; }
        }
    
}


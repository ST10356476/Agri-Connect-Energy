using System.ComponentModel.DataAnnotations;

namespace Agri_Energy_Connect.ViewModels
{
    /// <summary>
    /// View model for user profile management
    /// </summary>
    public class ProfileViewModel
    {
        /// <summary>
        /// The user's identifier
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// The user's username (cannot be changed)
        /// </summary>
        [Display(Name = "Username")]
        public string Username { get; set; }

        /// <summary>
        /// The user's email address
        /// </summary>
        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        /// <summary>
        /// The user's first name
        /// </summary>
        [Required(ErrorMessage = "First name is required")]
        [StringLength(100, ErrorMessage = "First name cannot exceed 100 characters")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        /// <summary>
        /// The user's last name
        /// </summary>
        [Required(ErrorMessage = "Last name is required")]
        [StringLength(100, ErrorMessage = "Last name cannot exceed 100 characters")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        /// <summary>
        /// Full name (first name + last name)
        /// </summary>
        public string? FullName => $"{FirstName} {LastName}";

        /// <summary>
        /// The user's phone number
        /// </summary>
        [Phone(ErrorMessage = "Invalid phone number")]
        [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// The user's role in the system
        /// </summary>
        [Display(Name = "Role")]
        public string Role { get; set; }

        /// <summary>
        /// URL to the user's profile image
        /// </summary>
        public string ProfileImageUrl { get; set; }

        /// <summary>
        /// Indicates whether the user wishes to receive email notifications
        /// </summary>
        [Display(Name = "Receive Notifications")]
        public bool ReceiveNotifications { get; set; } = true;

        /// <summary>
        /// The date when the user account was created
        /// </summary>
        [Display(Name = "Member Since")]
        [DisplayFormat(DataFormatString = "{0:MMMM d, yyyy}")]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// The date when the user last logged in
        /// </summary>
        [Display(Name = "Last Login")]
        [DisplayFormat(DataFormatString = "{0:MMMM d, yyyy HH:mm}")]
        public DateTime? LastLoginDate { get; set; }

        /// <summary>
        /// File upload for profile image
        /// </summary>
        [Display(Name = "Profile Image")]
        public IFormFile ProfileImage { get; set; }

        // For password change (typically handled separately)

        /// <summary>
        /// Current password (for verification when changing password)
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string CurrentPassword { get; set; }

        /// <summary>
        /// New password
        /// </summary>
        [StringLength(100, ErrorMessage = "Password must be between {2} and {1} characters", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        /// <summary>
        /// Confirm new password
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}

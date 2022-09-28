using System.ComponentModel.DataAnnotations;

namespace MessagingApp.Models
{
    public class UserModel
    {
        [Required]
        public long Id { get; set; }
        [Required]
        [MaxLength(MessageModel.UsernameMaxLength, ErrorMessage = "Your username can't be longer than {1} characters.")]
        public string Username { get; set; } = default!;
    }
}

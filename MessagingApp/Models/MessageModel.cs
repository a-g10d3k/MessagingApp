using System.ComponentModel.DataAnnotations;

namespace MessagingApp.Models
{
    public class MessageModel
    {
        public const int UsernameMaxLength = 50;
        public const int TitleMaxLength = 50;
        public const int MessageMaxLength = 300;
        [Required]
        public long Id { get; set; }
        [Required]
        [MaxLength(UsernameMaxLength, ErrorMessage = "Your username can't be longer than {1} characters.")]
        public string Sender { get; set; } = default!;
        [Required]
        [MaxLength(UsernameMaxLength, ErrorMessage = "The recipent's username can't be longer than {1} characters.")]
        public string Recipent { get; set; } = default!;
        [Required]
        [MaxLength(TitleMaxLength, ErrorMessage = "The title can't be longer than {1} characters.")]
        public string Title { get; set; } = default!;
        [Required]
        [MaxLength(MessageMaxLength, ErrorMessage = "The message can't be longer than {1} characters.")]
        public string Body { get; set; } = default!;
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }
    }
}

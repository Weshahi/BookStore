using System.ComponentModel.DataAnnotations;

namespace BookStore.API.Dtos
{
    public class UserForRegisterationDto
    {
        [Required]
        public string UserName { get; set; }
        [StringLength(8,MinimumLength=4,ErrorMessage="Sorry Password should be at least 4 and not exceeds 8")]
        public string Password { get; set; }
    }
}
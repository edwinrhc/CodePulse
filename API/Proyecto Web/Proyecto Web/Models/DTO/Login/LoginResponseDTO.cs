namespace Proyecto_Web.Models.DTO.Login
{
    public class LoginResponseDTO
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public List<string> Roles { get; set; }
    }
}

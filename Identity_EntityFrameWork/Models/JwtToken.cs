namespace Identity_EntityFrameWork.Models
{
    public class JWTToken
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        
    }
}

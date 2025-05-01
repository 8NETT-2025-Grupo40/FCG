namespace FCG.Application.Modules.Login
{
    public class LoginAppResultDTO
    {
        public bool IsSuccess { get; set; }
        public string Token { get; set; }
        public string message { get; set; }

        public static LoginAppResultDTO Success(string token){
            return new(){
                IsSuccess = true,
                Token = token
            };
        }

        public static LoginAppResultDTO Fail(string message){
            return new(){ 
                IsSuccess = false, 
                message = message};
        }
    }
}
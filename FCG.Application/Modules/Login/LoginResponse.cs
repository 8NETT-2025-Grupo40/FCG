namespace FCG.Application.Modules.Login
{
    public class LoginResponse
    {
        public bool IsSuccess { get; set; }
        public string Token { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;

        public static LoginResponse Success(string token, string message){
            return new(){
                IsSuccess = true,
                Token = token,
                Message = message
            };
        }

        public static LoginResponse Fail(string message){
            return new(){
                IsSuccess = false,
                Message = message};
        }
    }
}
namespace FCG.Application.Modules.Login
{
    public class LoginAppResultDTO
    {
        public bool IsSuccess { get; set; }
        public string Token { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;

        public static LoginAppResultDTO Success(string token, string message){
            return new(){
                IsSuccess = true,
                Token = token,
                Message = message
            };
        }

        public static LoginAppResultDTO Fail(string message){
            return new(){
                IsSuccess = false,
                Message = message};
        }
    }
}
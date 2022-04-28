namespace Domain.Core
{
    public class Result
    {
        public bool IsSuccess { get; set; }
        public bool IsNotFound { get; set; }
        public object Value { get; set; }
        public string ErrorMessage { get; set; }
        public static Result Success(object value) => new Result { IsSuccess = true, Value = value, IsNotFound = value == null };
        public static Result SuccessNotFound() => new Result { IsSuccess = true, IsNotFound = true };
        public static Result SuccessNoContent() => new Result { IsSuccess = true };
        public static Result Error(string error) => new Result { IsSuccess = false, ErrorMessage = error };
    }
}

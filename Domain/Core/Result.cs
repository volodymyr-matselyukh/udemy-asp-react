namespace Domain.Core
{
    public class Result<T>
    {
        public bool IsSuccess { get; set; }
        public bool IsNotFound { get; set; }
        public T Value { get; set; }
        public string ErrorMessage { get; set; }
        public static Result<T> Success(T value) => new Result<T> { IsSuccess = true, Value = value, IsNotFound = value == null };
        public static Result<T> SuccessNotFound() => new Result<T> { IsSuccess = true, IsNotFound = true };
        public static Result<T> SuccessNoContent() => new Result<T> { IsSuccess = true };
        public static Result<T> Error(string error) => new Result<T> { IsSuccess = false, ErrorMessage = error };
    }

    public class Result: Result<object>
    {
    }
}

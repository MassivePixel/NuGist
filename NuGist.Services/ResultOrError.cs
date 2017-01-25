namespace NuGist.Services
{
    public enum CommonErrors
    {
        NoError = 0,
        NotFound
    }

    public class ResultOrError<T>
    {
        public T Result { get; private set; }
        public string Error { get; private set; }
        public CommonErrors CommonError { get; private set; }

        public bool IsError => Error != null || CommonError != CommonErrors.NoError;

        public static implicit operator ResultOrError<T>(T result)
            => new ResultOrError<T>
            {
                Result = result
            };

        public static implicit operator ResultOrError<T>(string error)
            => new ResultOrError<T>
            {
                Error = error
            };

        public static implicit operator ResultOrError<T>(CommonErrors error)
             => new ResultOrError<T>
             {
                 CommonError = error
             };
    }
}

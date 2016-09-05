namespace Gorman.API.Framework {
    using System;
    using API.Domain;

    public class ResponseValidator
        : IResponseValidator {
        public T Validate<T>(Response<T> response) {
            if (!response.IsSuccessful())
                throw new Exception();

            return response.Data;
        }
    }

    public interface IResponseValidator {
        T Validate<T>(Response<T> response);
    }
}
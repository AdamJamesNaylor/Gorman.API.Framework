namespace Gorman.API.Framework.Validators {
    using System;
    using System.Net;
    using System.Runtime.Serialization;
    using API.Domain;
    using RestSharp;

    public interface IResponseValidator {
        T Validate<T>(IRestResponse<Response<T>> response);
    }

    public class ResponseValidator
        : IResponseValidator {
        public T Validate<T>(IRestResponse<Response<T>> response) {
            if (response.ResponseStatus != ResponseStatus.Completed)
                throw new ResponseValidationException(
                    $"There was a problem completing the request to {response.Request?.Resource}. {response.ErrorMessage}");

            if (response.StatusCode == HttpStatusCode.NotFound)
                throw new NotFoundResponseValidationException($"The request to {response.Request?.Resource} returned a 404 not found response.");

            if (!response.Data.IsSuccessful())
                throw new ResponseValidationException(
                    $"The endpoint {response.Request.Resource} returned the error {response.Data.Error}.");

            if (response.Data.Data == null)
                throw new ResponseValidationException(
                    $"There was a problem deserialising the response '{response.Content}' to type {typeof (Response<T>).FullName}.");

            return response.Data.Data;
        }
    }

    [Serializable]
    public class ResponseValidationException : Exception {

        public ResponseValidationException() {
        }

        public ResponseValidationException(string message) : base(message) {
        }

        public ResponseValidationException(string message, Exception inner) : base(message, inner) {
        }

        protected ResponseValidationException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) {
        }
    }

    public class NotFoundResponseValidationException
        : ResponseValidationException {
        public NotFoundResponseValidationException() {
        }

        public NotFoundResponseValidationException(string message) : base(message) {
        }

        public NotFoundResponseValidationException(string message, Exception inner) : base(message, inner) {
        }

        protected NotFoundResponseValidationException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) {
        }
    }
}
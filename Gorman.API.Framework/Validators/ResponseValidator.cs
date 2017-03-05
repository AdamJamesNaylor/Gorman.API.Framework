namespace Gorman.API.Framework.Validators {
    using System;
    using System.Net;
    using System.Runtime.Serialization;
    using API.Domain;
    using Newtonsoft.Json;
    using RestSharp;

    public interface IResponseValidator {
        T Validate<T>(IRestResponse<T> response);
    }

    public class ResponseValidator
        : IResponseValidator {
        public T Validate<T>(IRestResponse<T> response) {
            if (response.ResponseStatus != ResponseStatus.Completed)
                throw new ResponseValidationException(
                    $"There was a problem completing the request to '{response.Request?.Resource}'. '{response.ErrorMessage}'");

            switch (response.StatusCode) {
                case HttpStatusCode.InternalServerError:
                    var error = JsonConvert.DeserializeObject<ServerError>(response.Content);
                    throw new ServerErrorResponseValidationException(
                        $"The request to '{response.Request?.Resource}' returned a 500 server error. '{error.Message}");
                case HttpStatusCode.NotFound:
                    throw new NotFoundResponseValidationException(
                        $"The request to '{response.Request?.Resource}' returned a 404 not found response.");
            }

            if (response.Data == null)
                throw new ResponseValidationException(
                    $"The request to '{response.Request?.Resource}' resulted in a problem deserialising the response '{response.Content}' to type {typeof (T).FullName}.");

            return response.Data;
        }
    }

    public class ServerError {
        public string Message { get; set; }
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

    [Serializable]
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

    [Serializable]
    public class ServerErrorResponseValidationException
        : ResponseValidationException {
        public ServerErrorResponseValidationException() {
        }

        public ServerErrorResponseValidationException(string message) : base(message) {
        }

        public ServerErrorResponseValidationException(string message, Exception inner) : base(message, inner) {
        }

        protected ServerErrorResponseValidationException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) {
        }
    }
}
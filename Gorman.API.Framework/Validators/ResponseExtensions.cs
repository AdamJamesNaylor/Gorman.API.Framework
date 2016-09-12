namespace Gorman.API.Framework.Validators {
    using API.Domain;

    public static class IRestResponseExtensions {
        public static bool IsSuccessful<T>(this Response<T> response) {
            return response.Error == null;
        }
    }
}
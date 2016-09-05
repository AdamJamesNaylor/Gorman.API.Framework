namespace Gorman.API.Framework {
    using API.Domain;

    public static class ResponseExtensions {
        public static bool IsSuccessful<T>(this Response<T> response) {
            return response.Error == null;
        }
    }
}
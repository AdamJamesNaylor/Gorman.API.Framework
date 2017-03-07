namespace Gorman.API.Framework {
    using RestSharp;

    public static class IRestRequestExtensions {
        public static IRestRequest RemoveUrlSegment(this IRestRequest operand, string parameterName) {
            operand.AddParameter(parameterName, "", ParameterType.UrlSegment);
            return operand;
        }
    }
}
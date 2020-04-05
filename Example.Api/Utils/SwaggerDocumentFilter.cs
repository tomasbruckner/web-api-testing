using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace Example.Api.Utils
{
    public static class SwaggerDocumentFilter
    {
        public static string SortSwagger(ApiDescription apiDescription)
        {
            var x = apiDescription.RelativePath;
            var template = apiDescription.ActionDescriptor.AttributeRouteInfo.Template ?? "";
            var routeValues = apiDescription.ActionDescriptor.RouteValues;

            return
                $"{routeValues["controller"]}_{template} _{SortHttpMethods(apiDescription.HttpMethod)}";
        }

        private static string SortHttpMethods(string httpMethod)
        {
            return httpMethod.ToLower() switch
            {
                // cannot use Microsoft.AspNetCore.Http.HttpMethods because it is not constant but static readonly
                "get" => "001",
                "post" => "002",
                "put" => "003",
                "patch" => "004",
                "delete" => "005",
                _ => "006"
            };
        }
    }
}

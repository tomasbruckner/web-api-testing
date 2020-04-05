using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using Example.Api.Tests.Support.Constants;

namespace Example.Api.Tests.Support.Utils
{
    public static class AuthorizationSupport
    {
        private const string AuthorizationHeader = "Authorization";

        public static readonly ReadOnlyCollection<(Action<HttpRequestMessage>, string)> AllRoles =
            new List<(Action<HttpRequestMessage>, string)>
            {
                (AddAdminHeaders, "Admin"),
                (AddUserHeaders, "User"),
            }.AsReadOnly();

        public static readonly ReadOnlyCollection<(Action<HttpRequestMessage>, string)> AllRolesAndAnonymous =
            new List<(Action<HttpRequestMessage>, string)>(AllRoles)
            {
                (AddAnonymousHeaders, "Anonymous")
            }.AsReadOnly();

        public static void AddAdminHeaders(HttpRequestMessage request)
        {
            request.Headers.Add(AuthorizationHeader, $"Bearer {AuthenticationTestConstants.AdminJwtToken}");
        }

        public static void AddUserHeaders(HttpRequestMessage request)
        {
            request.Headers.Add(AuthorizationHeader, $"Bearer {AuthenticationTestConstants.UserJwtToken}");
        }

        /**
         * <summary>
         * This is a helper function for easier iterating requests that should work even without authentication
         * </summary>
         */
        public static void AddAnonymousHeaders(HttpRequestMessage request)
        {
        }
    }
}

#region Dapplo 2018 - GNU Lesser General Public License

//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2018 Dapplo
// 
//  For more information see: http://dapplo.net/
//  Dapplo repositories are hosted on GitHub: https://github.com/dapplo
// 
//  This file is part of Dapplo.Nice.Perform
// 
//  Dapplo.Nice.Perform is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  Dapplo.Nice.Perform is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
// 
//  You should have a copy of the GNU Lesser General Public License
//  along with Dapplo.Nice.Perform. If not, see <http://www.gnu.org/licenses/lgpl.txt>.

#endregion

using System.Linq;
using System.Net;
using Dapplo.HttpExtensions;
using Dapplo.Log;
using Dapplo.Nice.Perform.Entities;

namespace Dapplo.Nice.Perform.Internals
{
    /// <summary>
    /// Extensions for the HttpResponse object
    /// </summary>
    internal static class HttpResponseExtensions
    {
        private static readonly LogSource Log = new LogSource();

        /// <summary>
        /// Helper method to log the error
        /// </summary>
        /// <param name="httpStatusCode">HttpStatusCode</param>
        /// <param name="error">Error</param>
        private static void LogError(HttpStatusCode httpStatusCode, Error error = null)
        {
            // Log all error information
            Log.Warn().WriteLine("Http status code: {0} ({1}). Response from server: {2}", httpStatusCode.ToString(), (int)httpStatusCode, error?.Message ?? httpStatusCode.ToString());
        }

        /// <summary>
        ///     Helper method for handling errors in the response, if the response has an error an exception is thrown.
        ///     Else the real response is returned.
        /// </summary>
        /// <typeparam name="TResponse">Type for the ok content</typeparam>
        /// <param name="expectedHttpStatusCodes">optional HttpStatusCode(s) to expect</param>
        /// <param name="response">TResponse</param>
        /// <returns>TResponse</returns>
        public static TResponse HandleErrors<TResponse>(this HttpResponse<TResponse, string> response, params HttpStatusCode[] expectedHttpStatusCodes)
            where TResponse : class
        {
            if (expectedHttpStatusCodes != null && expectedHttpStatusCodes.Any(code => code == response.StatusCode))
            {
                return response.Response;
            }
            if (!response.HasError)
            {
                return response.Response;
            }

            // Log all error information
            Log.Warn().WriteLine("Http status code: {0} ({1}). Response from server: {2}", response.StatusCode.ToString(), (int)response.StatusCode, response.ErrorResponse ?? response.StatusCode.ToString());
            throw new NicePerformException(response.StatusCode, response.ErrorResponse);
        }

        /// <summary>
        ///     Helper method for handling errors in the response, if the response has an error an exception is thrown.
        ///     Else the real response is returned.
        /// </summary>
        /// <typeparam name="TResponse">Type for the ok content</typeparam>
        /// <typeparam name="TError">Type for the error content</typeparam>
        /// <param name="expectedHttpStatusCodes">optional HttpStatusCode(s) to expect</param>
        /// <param name="response">TResponse</param>
        /// <returns>TResponse</returns>
        public static TResponse HandleErrors<TResponse, TError>(this HttpResponse<TResponse, TError> response, params HttpStatusCode[] expectedHttpStatusCodes)
            where TResponse : class where TError : Error
        {
            if (expectedHttpStatusCodes != null && expectedHttpStatusCodes.Any(code => code == response.StatusCode))
            {
                return response.Response;
            }
            if (!response.HasError)
            {
                return response.Response;
            }

            // Log all error information
            LogError(response.StatusCode, response.ErrorResponse);
            throw new NicePerformException(response.StatusCode, response.ErrorResponse);
        }

        /// <summary>
        ///     Helper method for handling errors in the response, if the response doesn't have the expected status code an
        ///     exception is thrown.
        ///     Else the real response is returned.
        /// </summary>
        /// <typeparam name="TResponse">Type for the ok content</typeparam>
        /// <param name="expectedHttpStatusCodes">HttpStatusCode(s) to expect</param>
        /// <param name="response">TResponse</param>
        /// <returns>TResponse</returns>
        public static TResponse HandleErrors<TResponse>(this HttpResponse<TResponse> response, params HttpStatusCode[] expectedHttpStatusCodes)
            where TResponse : class
        {
            if (expectedHttpStatusCodes == null || expectedHttpStatusCodes.Length == 0)
            {
                expectedHttpStatusCodes = new[] { HttpStatusCode.OK };
            }
            if (expectedHttpStatusCodes.Any(code => code == response.StatusCode))
            {
                return response.Response;
            }
            LogError(response.StatusCode);
            throw new NicePerformException(response.StatusCode);
        }

        /// <summary>
        ///     Helper method for handling errors in the response, if the response doesn't have the expected status code an
        ///     exception is thrown.
        /// </summary>
        /// <param name="expectedHttpStatusCodes">HttpStatusCode(s) to expect</param>
        /// <param name="response">TResponse</param>
        public static void HandleStatusCode(this HttpResponse response, params HttpStatusCode[] expectedHttpStatusCodes)
        {
            if (expectedHttpStatusCodes == null || expectedHttpStatusCodes.Length == 0)
            {
                expectedHttpStatusCodes = new[] { HttpStatusCode.OK };
            }
            if (expectedHttpStatusCodes.Any(code => code == response.StatusCode))
            {
                return;
            }
            LogError(response.StatusCode);
            throw new NicePerformException(response.StatusCode);
        }

        /// <summary>
        ///     Helper method for handling errors in the response, if the response doesn't have the expected status code an
        ///     exception is thrown.
        /// </summary>
        /// <typeparam name="TError">Type for the error</typeparam>
        /// <param name="expectedHttpStatusCodes">HttpStatusCode(s) to expect</param>
        /// <param name="response">TResponse</param>
        public static void HandleStatusCode<TError>(this HttpResponseWithError<TError> response, params HttpStatusCode[] expectedHttpStatusCodes) where TError : Error
        {
            if (expectedHttpStatusCodes == null || expectedHttpStatusCodes.Length == 0)
            {
                expectedHttpStatusCodes = new[] { HttpStatusCode.OK };
            }
            if (expectedHttpStatusCodes.Any(code => code == response.StatusCode))
            {
                return;
            }
            LogError(response.StatusCode, response.ErrorResponse);
            throw new NicePerformException(response.StatusCode, response.ErrorResponse);
        }

        /// <summary>
        ///     Helper method for handling errors in the response, if the response doesn't have the expected status code an
        ///     exception is thrown.
        /// </summary>
        /// <param name="expectedHttpStatusCodes">HttpStatusCode(s) to expect</param>
        /// <param name="response">TResponse</param>
        public static void HandleStatusCode(this HttpResponseWithError<string> response, params HttpStatusCode[] expectedHttpStatusCodes)
        {
            if (expectedHttpStatusCodes == null || expectedHttpStatusCodes.Length == 0)
            {
                expectedHttpStatusCodes = new[] { HttpStatusCode.OK };
            }
            if (expectedHttpStatusCodes.Any(code => code == response.StatusCode))
            {
                return;
            }
            Log.Warn().WriteLine("Http status code: {0}. Response from server: {1}", response.StatusCode, response.ErrorResponse);
            throw new NicePerformException(response.StatusCode, response.ErrorResponse);
        }
    }
}

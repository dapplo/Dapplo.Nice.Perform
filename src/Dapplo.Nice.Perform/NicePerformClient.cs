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


#region using

using System;
using Dapplo.HttpExtensions;
using Dapplo.HttpExtensions.JsonSimple;

#if NET45 || NET46
using System.Collections.Generic;
using System.Net.Cache;
using System.Net.Http;
using Dapplo.HttpExtensions.Extensions;
#endif

#endregion

namespace Dapplo.Nice.Perform
{
    /// <summary>
    ///     A Nice Perform client build by using Dapplo.HttpExtensions
    /// </summary>
    public class NicePerformClient : INicePerformClientPlugins
    {
        /// <summary>
        ///     Password for the basic authentication
        /// </summary>
        private string _password;

        /// <summary>
        ///     User for the basic authentication
        /// </summary>
        private string _user;

        /// <summary>
        ///     Create the NicePerformClient object, here the HttpClient is configured
        /// </summary>
        /// <param name="nicePerformUri">Base URL, e.g. https://yourniceperformserver</param>
        /// <param name="httpSettings">IHttpSettings or null for default</param>
        private NicePerformClient(Uri nicePerformUri, IHttpSettings httpSettings = null)
        {
            NicePerformUri = nicePerformUri ?? throw new ArgumentNullException(nameof(nicePerformUri));
            ConfluenceApiUri = nicePerformUri.AppendSegments("rest", "api");

            Behaviour = ConfigureBehaviour(new HttpBehaviour(), httpSettings);
        }

        /// <summary>
        ///     The IHttpBehaviour for this Confluence instance
        /// </summary>
        public IHttpBehaviour HttpBehaviour => Behaviour;

        /// <summary>
        ///     Store the specific HttpBehaviour, which contains a IHttpSettings and also some additional logic for making a
        ///     HttpClient which works with Confluence
        /// </summary>
        public IHttpBehaviour Behaviour { get; protected set; }

        /// <summary>
        ///     Plugins dock to this property by implementing an extension method to IConfluenceClientPlugins
        /// </summary>
        public INicePerformClientPlugins Plugins => this;

        /// <summary>
        ///     Set Basic Authentication for the current client
        /// </summary>
        /// <param name="user">username</param>
        /// <param name="password">password</param>
        public void SetBasicAuthentication(string user, string password)
        {
            _user = user;
            _password = password;
        }

        /// <summary>
        ///     The base URI for your Nice Perform server api calls
        /// </summary>
        public Uri ConfluenceApiUri { get; }

        /// <summary>
        ///     The base URI for your Nice Perform server downloads
        /// </summary>
        public Uri NicePerformUri { get; }
        
        /// <summary>
        ///     Helper method to configure the IChangeableHttpBehaviour
        /// </summary>
        /// <param name="behaviour">IChangeableHttpBehaviour</param>
        /// <param name="httpSettings">IHttpSettings</param>
        /// <returns>the behaviour, but configured as IHttpBehaviour </returns>
        protected IHttpBehaviour ConfigureBehaviour(IChangeableHttpBehaviour behaviour, IHttpSettings httpSettings = null)
        {
            behaviour.HttpSettings = httpSettings ?? HttpExtensionsGlobals.HttpSettings;
#if NET45 || NET46
            // Disable caching, if no HTTP settings were provided.
            // This is needed as was detected here: https://github.com/dapplo/Dapplo.Confluence/issues/11
            if (httpSettings == null)
            {
                behaviour.HttpSettings.RequestCacheLevel = RequestCacheLevel.NoCacheNoStore;
            }
#endif
            // Using our own Json Serializer, implemented with SimpleJson
            behaviour.JsonSerializer = new SimpleJsonSerializer();

            behaviour.OnHttpRequestMessageCreated = httpMessage =>
            {
                httpMessage?.Headers.TryAddWithoutValidation("X-Atlassian-Token", "no-check");
                if (!string.IsNullOrEmpty(_user) && _password != null)
                {
                    httpMessage?.SetBasicAuthorization(_user, _password);
                }
                return httpMessage;
            };
            return behaviour;
        }

        /// <summary>
        ///     Factory method to create a NicePerformClient
        /// </summary>
        /// <param name="nicePerformUri">Uri to your Nice Perform server</param>
        /// <param name="httpSettings">IHttpSettings used if you need specific settings</param>
        /// <returns>IConfluenceClient</returns>
        public static INicePerformClient Create(Uri nicePerformUri, IHttpSettings httpSettings = null)
        {
            return new NicePerformClient(nicePerformUri, httpSettings);
        }

        /// <summary>
        ///     This makes sure that the HttpBehavior is promoted for the following Http call.
        /// </summary>
        public void PromoteContext()
        {
            Behaviour.MakeCurrent();
        }
    }
}
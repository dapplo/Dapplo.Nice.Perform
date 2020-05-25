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

#endregion

namespace Dapplo.Nice.Perform
{
	/// <summary>
	///     The is the interface to the base client functionality of the Nice Perform API
	/// </summary>
	public interface INicePerformClient
	{
		/// <summary>
		///     The base URI for your Nice Perform server api calls
		/// </summary>
		Uri ConfluenceApiUri { get; }

		/// <summary>
		///     The base URI for your Nice Perform server downloads
		/// </summary>
		Uri NicePerformUri { get; }

		/// <summary>
		///     Extensions of the client dock to this property, so typing "confluenceClient.Plugins." should show your extension.
		/// </summary>
		INicePerformClientPlugins Plugins { get; }

		/// <summary>
		///     Enables basic authentication for every request following this call
		/// </summary>
		/// <param name="user">string with the confluence user</param>
		/// <param name="password">string with the password for the Nice Perform user</param>
		void SetBasicAuthentication(string user, string password);
	}

	/// <summary>
	///     Interface of all the Nice Perform interfaces
	/// </summary>
	public interface INicePerformDomain : INicePerformClient
	{
        /// <summary>
        /// The IHttpBehaviour of the Confluence client
        /// </summary>
		IHttpBehaviour Behaviour { get; }
	}
}
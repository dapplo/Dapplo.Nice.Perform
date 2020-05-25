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

using System.Runtime.Serialization;

#endregion

namespace Dapplo.Nice.Perform.Entities
{
	/// <summary>
	///     Error information
	/// </summary>
	[DataContract]
	public class Error
	{
		/// <summary>
		///     Error message from Confluence
		/// </summary>
		[DataMember(Name = "message", EmitDefaultValue = false)]
		public string Message { get; set; }

		/// <summary>
		///     Confluence status code
		/// </summary>
		[DataMember(Name = "statusCode", EmitDefaultValue = false)]
		public int StatusCode { get; set; }
	}
}
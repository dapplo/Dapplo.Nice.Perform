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

#region Usings

using System;
using System.IO;
using System.Threading.Tasks;
using Dapplo.Log;
using Dapplo.Log.XUnit;
using Xunit;
using Xunit.Abstractions;

#endregion

namespace Dapplo.Nice.Perform.Tests
{
	/// <summary>
	///     Tests
	/// </summary>
	public class NicePerformClientTests
	{
		public NicePerformClientTests(ITestOutputHelper testOutputHelper)
		{
			LogSettings.RegisterDefaultLogger<XUnitLogger>(LogLevels.Verbose, testOutputHelper);
			_nicePerformClient = NicePerformClient.Create(TestConfluenceUri);

			var username = Environment.GetEnvironmentVariable("confluence_test_username");
			var password = Environment.GetEnvironmentVariable("confluence_test_password");
			if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
			{
				_nicePerformClient.SetBasicAuthentication(username, password);
			}
		}

		// Test against a well known Confluence
		private static readonly Uri TestConfluenceUri = new Uri("https://greenshot.atlassian.net/wiki");


		private readonly INicePerformClient _nicePerformClient;

		/// <summary>
		///     Test only works on Nice Perform and later
		/// </summary>
		/// <returns></returns>
		[Fact]
		public async Task TestCurrentUserAndPicture()
		{
			var currentUser = await _nicePerformClient.
			Assert.NotNull(currentUser);
			Assert.NotNull(currentUser.ProfilePicture);

			var bitmapSource = await _nicePerformClient.Misc.GetPictureAsync<MemoryStream>(currentUser.ProfilePicture);
			Assert.NotNull(bitmapSource);
		}
	}
}
using Weather.Api.Core.Models;

namespace Weather.Integration.Tests.Common;

internal sealed record FakeUser(string UserName, string Email, string Password, Role[] Roles);

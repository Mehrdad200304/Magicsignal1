using Microsoft.EntityFrameworkCore;
using Moq;
using MagicSignal.Modules.Accounts.Application.DTOs.Auth;
using MagicSignal.Modules.Accounts.Application.Interfaces.Authentication;
using MagicSignal.Modules.Accounts.Application.Interfaces.Services;
using MagicSignal.Modules.Accounts.Infrastructure.Services;
using MagicSignal.Modules.Accounts.Domain.Entities;
using MagicSignal.Modules.Accounts.Infrastructure.Persistence;

namespace MagicSignal.Modules.Accounts.Tests;

public class AuthServiceTests
{
    [Fact]
    public void SimpleTest()
    {
        Assert.True(true);
    }
}
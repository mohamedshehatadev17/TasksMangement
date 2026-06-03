using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskMangement.Domain.Models;

namespace TaskMangement.Application.Abstractions.Authentication
{
    public interface IJwtTokenGenerator
    {
        Task<string> GenerateTokenAsync(User user);
    }
}

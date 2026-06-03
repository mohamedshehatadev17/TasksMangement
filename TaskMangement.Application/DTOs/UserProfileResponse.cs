using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMangement.Application.DTOs
{
    public sealed class UserProfileResponse
    {
        public Guid Id { get; init; }

        public string Name { get; init; } = string.Empty;

        public string Email { get; init; } = string.Empty;

        public IList<string> Roles { get; set; } = new List<string>();

        public DateTime CreatedAt { get; init; }
    }
}

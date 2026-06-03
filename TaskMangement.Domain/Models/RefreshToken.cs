using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TaskMangement.Domain.Models
{
    public class RefreshToken
    {
        public Guid Id { get; private set; }

        public string Token { get; private set; } = string.Empty;

        public DateTime ExpiresAt { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public bool IsRevoked { get; private set; }

        public Guid UserId { get; private set; }

        public User User { get; private set; } = null!;

        public void Revoke()
        {
            IsRevoked = true;
        }
        public static RefreshToken Generate(Guid userId, int daysToExpire = 7)
        {
            var bytes = RandomNumberGenerator.GetBytes(64);

            return new RefreshToken
            {
                Token = Convert.ToBase64String(bytes),
                ExpiresAt = DateTime.UtcNow.AddDays(daysToExpire),
                CreatedAt = DateTime.UtcNow,
                UserId = userId
            };
        }
    }
}
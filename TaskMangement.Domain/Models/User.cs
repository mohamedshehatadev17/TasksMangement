
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace TaskMangement.Domain.Models
{
    [Table("AspNetUsers")]
    public class User :IdentityUser<Guid>
    {
        public string Name { get; private set; } = string.Empty;
        public DateTime CreatedAt { get; private set; }
        public bool IsDeleted { get; private set; }
        public ICollection<RefreshToken> RefreshTokens { get; private set; }=[];
        public ICollection<Task> Tasks { get; private set; } = [];

        public User(string name, string email)
        {
            Name = name;
            Email = email;
            UserName = email;
            NormalizedEmail = email.ToUpperInvariant();
            NormalizedUserName = email.ToUpperInvariant();
            CreatedAt = DateTime.UtcNow;
        }
        public void MarkAsDeleted()
        {
            IsDeleted = true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMangement.Domain.Models
{
    public class BaseEntity
    {
        public Guid Id { get; protected set; }= Guid.NewGuid();
        public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
        public bool IsDeleted { get;protected set; } =false;


        public void MarkAsDeleted()
        {
            IsDeleted = true;
        }
    }
}

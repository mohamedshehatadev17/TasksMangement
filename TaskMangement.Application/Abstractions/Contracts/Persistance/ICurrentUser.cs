using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMangement.Application.Abstractions.Contracts.Persistance
{
    public interface ICurrentUser
    {
        public Guid UserId { get; }

    }
}

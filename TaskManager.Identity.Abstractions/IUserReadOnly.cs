using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Identity.Abstractions;
public interface IUserReadOnly
{
    Task<UserDto?> GetUserByIdAsync(string id, CancellationToken ct = default);
    Task<UserDto?> FindByUsernameAsync(string username, CancellationToken ct = default);
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Identity.Abstractions;
public record UserDto(string Id, string Email, string Username, string FullName);

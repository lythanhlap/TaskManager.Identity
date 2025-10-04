using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Identity.Abstractions;
public record RegisterRequest(string Email, string Username, string Password, string FullName);

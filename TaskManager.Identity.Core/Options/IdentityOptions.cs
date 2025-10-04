using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Identity.Core.Options;

public sealed class IdentityOptions
{
    public string JwtIssuer { get; set; } = "TaskManager";
    public string JwtAudience { get; set; } = "TaskManager.Web";
    public string JwtSigningKey { get; set; } = "CHANGE_THIS_32+CHARS";
    public int JwtExpiresMinutes { get; set; } = 120;
}

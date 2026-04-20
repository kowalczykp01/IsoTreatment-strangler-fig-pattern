using System;

namespace Application.DTOs;

public sealed record JwtDto
{
    public string AccessToken { get; set; }
}

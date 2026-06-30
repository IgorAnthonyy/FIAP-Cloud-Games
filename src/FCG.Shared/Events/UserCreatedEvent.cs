using System;

namespace FCG.Shared.Events;

public record UserCreatedEvent(
    Guid UserId,
    string Name,
    string Email,
    bool IsAdmin,
    string? TemporaryPassword
);

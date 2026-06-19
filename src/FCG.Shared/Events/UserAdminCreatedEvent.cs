using System;

namespace FCG.Shared.Events;

public record UserAdminCreatedEvent(
    Guid Id,
    string Name,
    string Email,
    string TemporaryPassword
);

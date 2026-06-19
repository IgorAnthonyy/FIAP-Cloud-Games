using System;

namespace FCG.Shared.Events;

public record UserDefaultCreatedEvent(
    Guid Id,
    string Name,
    string Email
);

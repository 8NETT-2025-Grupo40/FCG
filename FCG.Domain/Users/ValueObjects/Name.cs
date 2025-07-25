﻿using FCG.Domain.Common;

namespace FCG.Domain.Users.ValueObjects;

public record Name
{
    public string Value { get; } = null!;

    public Name(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException("Name is required.");
        }

        Value = value.Trim();
    }

    public override string ToString() => Value;

    public static implicit operator Name(string name)
    {
        return new Name(name);
    }

    protected Name() { }
}
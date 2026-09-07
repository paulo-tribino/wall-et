// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using SharedKernel;

namespace Application.Errors
{
    public sealed class ValidationErrors
    {
        public static Error BadRequest(string description) => new("Validation.BadRequest", description);
    }
}

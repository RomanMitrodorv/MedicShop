﻿namespace Ordering.Infastructure.Idempotency;

public interface IRequestManager
{
    Task<bool> ExistAsync(Guid id);
    Task CreateRequestForCommandAsync<T>(Guid id);
}


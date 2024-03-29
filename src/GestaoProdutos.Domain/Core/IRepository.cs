﻿using System;

namespace GestaoProdutos.Domain.Core
{
    public interface IRepository<T> : IDisposable where T : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
﻿using System;
using System.Collections.Generic;

namespace DDDPPP.Chap19.MicroORM.Application.Infrastructure
{
    public abstract class Entity<TId>
    {
        public TId Id { get; protected set; }
    }
}

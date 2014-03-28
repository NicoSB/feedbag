﻿using System.Collections.Generic;

namespace KaVE.Model.Names
{
    public interface IGenericName : IName
    {
        // TODO what's the difference between these two?
        bool IsGenericType { get; }
        bool HasTypeParameters { get; }
        IList<ITypeName> TypeParameters { get; }
    }
}

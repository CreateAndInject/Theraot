﻿#if FAT

namespace Theraot.Factories
{
    public interface IFactory<out TOutput>
    {
        TOutput Create();
    }
}

#endif
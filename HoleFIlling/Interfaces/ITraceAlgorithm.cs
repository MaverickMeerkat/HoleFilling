﻿using HoleFilling.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoleFilling
{
    public interface ITraceAlgorithm
    {
        List<Pixel> Trace(ImageMatrix img);
    }
}
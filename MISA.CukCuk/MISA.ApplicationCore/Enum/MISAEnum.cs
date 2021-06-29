using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.ApplicationCore.Enum
{
    public enum MISACode
    {
        Valid = 100,
        Invalid = 900,
        Success = 200
    }

    public enum EntityState
    {
        Add = 1,
        Update = 2,
        Delete = 3
    }
}

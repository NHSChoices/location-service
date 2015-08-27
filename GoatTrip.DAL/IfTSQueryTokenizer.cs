using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace GoatTrip.DAL
{
    public interface IfTSQueryTokenizer
    {
        string LastToken { get; }
        string[] Tokens { get; }
        string GetMatchQuery();
        string GetLikeQuery();
    }
}

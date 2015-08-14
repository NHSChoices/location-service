using System;
using System.Linq;

namespace GoatTrip.DAL
{
    public class fTSQueryTokenizer : IfTSQueryTokenizer
    {
        public fTSQueryTokenizer(string locationQuery)
        {
            Tokens = locationQuery.Split();
        }

        public string[] Tokens { get; private set; }

        public string LastToken
        {
            get
            {
                if(Tokens.Length > 0)
                    return Tokens.Last();
                return "";
            }
        }

        public string GetMatchQuery()
        {
            if (Tokens.Length > 1) return String.Join(" ",Tokens.Take(Tokens.Length -1));
            if (Tokens.Length == 1) return Tokens.First() + "*";
            return "";
        }

        public string GetLikeQuery()
        {
            if (Tokens.Length > 1) return "%" + Tokens.Last() + "%";
            throw new InvalidOperationException("Like query not requres for single or zero tokens");
        }

    }
}
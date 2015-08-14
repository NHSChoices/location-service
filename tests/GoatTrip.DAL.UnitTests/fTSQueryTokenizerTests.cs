using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoatTrip.DAL;
using Xunit;
namespace GoatTrip.DAL.Tests
{
    public class fTSQueryTokenizerTests
    {

        [Fact()]
        public void fTSQueryTokenizer_Correct_Tokens_Test()
        {
            var searchQuery = "Multiple tokens Query";
            var tokenizer = new fTSQueryTokenizer(searchQuery);

            Assert.Equal(3, tokenizer.Tokens.Length);
            Assert.Equal("Multiple", tokenizer.Tokens[0]);
            Assert.Equal("tokens", tokenizer.Tokens[1]);
            Assert.Equal("Query", tokenizer.Tokens[2]);
        }

        [Fact()]
        public void GetMatchQuery_Single_Token_Test()
        {
            var searchQuery = "SingleToken";
            var tokenizer = new fTSQueryTokenizer(searchQuery);

            Assert.Equal("SingleToken*", tokenizer.GetMatchQuery());
        }

        [Fact()]
        public void GetMatchQuery_Multiple_Tokens_Test()
        {
            var searchQuery = "Multiple Tokens Test";
            var tokenizer = new fTSQueryTokenizer(searchQuery);

            Assert.Equal("Multiple Tokens", tokenizer.GetMatchQuery());
        }

        [Fact()]
        public void GetLikeQuery_Multiple_Tokens_Test()
        {
            var searchQuery = "Multiple Tokens Test";
            var tokenizer = new fTSQueryTokenizer(searchQuery);

            Assert.Equal("%Test%", tokenizer.GetLikeQuery());
        }

        [Fact()]
        public void GetLikeQuery_Single_Token_Thows_Exeption_Test()
        {
            var searchQuery = "SingleToken";
            var tokenizer = new fTSQueryTokenizer(searchQuery);

            Assert.Throws<InvalidOperationException>(() => tokenizer.GetLikeQuery());
        }
    }
}

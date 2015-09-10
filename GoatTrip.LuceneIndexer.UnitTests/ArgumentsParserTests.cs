using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoatTrip.LucenceIndexer;
using Xunit;
namespace GoatTrip.LuceneIndexer.UnitTests
{
    public class ArgumentsParserTests
    {
        [Fact()]
        public void ArgumentsParser_Singlearg_UnwrappedByQuotes_Test()
        {
            var result = "";

            var expected = "testarg";
            var testArgument = "t=" + expected;

            var argsParser = new ArgumentsParser(
               new Dictionary<string, Action<string>>() { { "t", s => result = s } });
            argsParser.ParseArgs(new string[]{testArgument});
            Assert.Equal(expected, result);
        }

        [Fact()]
        public void ArgumentsParser_Singlearg_WrappedByQuotes_Test()
        {
            var result = "";

            var expected = "testarg";
            var testArgument = "t=\"" + expected + "\"";

            var argsParser = new ArgumentsParser(
               new Dictionary<string, Action<string>>() { { "t", s => result = s } });
            argsParser.ParseArgs(new string[] { testArgument });
            Assert.Equal(expected, result);
        }

        [Fact()]
        public void ArgumentsParser_Singlearg_Seperated_Test()
        {
            var result = "";

            var expected = "testarg";

            var argsParser = new ArgumentsParser(
               new Dictionary<string, Action<string>>() { { "t", s => result = s } });
            argsParser.ParseArgs(new string[] { "t", expected });
            Assert.Equal(expected, result);
        }
    }
}

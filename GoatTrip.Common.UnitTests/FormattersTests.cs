using GoatTrip.Common.Formatters;
using Moq;
using Xunit;

namespace GoatTrip.Common.UnitTests
{
    public class FormattersTests
    {
        [Fact()]
        public void ConditionalFormatter()
        {
            var mockFormatter = new Mock<IFormatter<string>>();
            var mockFormatConditions = new Mock<IFormatConditions<string>>();

            mockFormatter.Setup(r => r.Format(It.IsAny<string>())).Returns<string>(a => a.ToUpper());
            mockFormatConditions.Setup(r => r.ShouldFormat(It.Is<string>(f => f == ("yes")))).Returns(true);
            mockFormatConditions.Setup(r => r.ShouldFormat(It.Is<string>(f => f == ("no")))).Returns(false);

            IConditionalFormatter<string,string> sut = new ConditionalFormatter<string, string>(mockFormatter.Object,mockFormatConditions.Object);
            
            Assert.Equal("FRED", sut.DetermineConditionsAndFormat("fred", "yes"));
            Assert.Equal("dave", sut.DetermineConditionsAndFormat("dave", "no"));
            Assert.Equal(null, sut.DetermineConditionsAndFormat(null, "no"));
            Assert.Equal(null, sut.DetermineConditionsAndFormat(null, "yes"));
            Assert.Equal("", sut.DetermineConditionsAndFormat("", "yes"));
            Assert.Equal("stefan", sut.DetermineConditionsAndFormat("stefan", null));
        }

        [Fact()]
        public void TitleCaseFormatter()
        {
            IFormatter<string> sut = new TitleCaseFormatter();
            Assert.Equal("Upper",sut.Format("UPPER"));
            Assert.Equal("Lower",sut.Format("lower"));
            Assert.Equal("Mixed", sut.Format("mIxEd"));
            Assert.Equal("Numb3rs", sut.Format("numb3rs"));
            Assert.Equal("", sut.Format(""));
            Assert.Equal(null, sut.Format(null));
        }
    }
}

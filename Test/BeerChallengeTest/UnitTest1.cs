using Microsoft.VisualStudio.TestTools.UnitTesting;
using RA;
using RA.Extensions;
using System.Globalization;
using System.Linq;

namespace Test1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
           var result = new RestAssured()
                .Given()
                    .Name("BeerChallengeTestABV")
                .When()
                    .Get("https://api.punkapi.com/v2/beers?brewed_after=12-2015")
                .Then()
                .Retrieve(x => x);
            //.TestBody("AllBeers", x => x.abv > 4);
            var jsonResult = result as Newtonsoft.Json.Linq.JArray;

            NumberStyles style = NumberStyles.Number;

            jsonResult.ForEach(x => Assert.IsTrue(double.TryParse((string)x["abv"], style, new CultureInfo(string.Empty), out _), $"Failed for {(string)x["id"]}" ));
            jsonResult.ForEach(x => Assert.IsTrue((double)x["abv"] > 4.0, $"Failed for ID {(string)x["id"]}. Abv value [{x["abv"]}] lower than expected."));
        }
    }
}

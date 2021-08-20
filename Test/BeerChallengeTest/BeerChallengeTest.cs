using Microsoft.VisualStudio.TestTools.UnitTesting;
using RA;
using RA.Extensions;
using System.Globalization;
using System.Linq;

namespace BeerChallengeTest
{
    [TestClass]
    public class Challenge_BeersMadeAfter122015
    {
        [TestMethod]
        public void Abv_NotEmptyNumericAndLargerThan4()
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

            //ABV empty string test, double test, also not null test - double is never null
            //try to parse and check, if not parsed or error, it won't be a double
            //locale issue - CultureInfo needed to solve the problem
            jsonResult.ForEach(x => Assert.IsTrue(double.TryParse((string)x["abv"], style, new CultureInfo(string.Empty), out _), $"Failed for {(string)x["id"]}"));

            //ABV 4 or more test
            //parse each element in collection and assert
            //log to find where the test fails
            jsonResult.ForEach(x => Assert.IsTrue((double)x["abv"] > 4.0, $"Failed for ID {(string)x["id"]}. Abv value [{x["abv"]}] lower than expected."));
        }

        [TestMethod]
        public void Name_NotEmptyAndNotNull()
        {
            var result = new RestAssured()
                 .Given()
                     .Name("BeerChallengeTestNames")
                 .When()
                     .Get("https://api.punkapi.com/v2/beers?brewed_after=12-2015")
                 .Then()
                 .Retrieve(e => e);

            var jsonResult = result as Newtonsoft.Json.Linq.JArray;

            //name not null test
            jsonResult.ForEach(e => Assert.IsNotNull("name", $"Failed for ID {(string)e["id"]}. Name is null."));

            //name not empty string test
            jsonResult.ForEach(e => Assert.AreNotEqual("", "name", $"Failed for ID {(string)e["id"]}. Name is empty."));
        }

        [TestMethod]
        public void TestBeerExtras_ElaspedTimeAndStatus()
        {
            var result = new RestAssured()
                 .Given()
                     .Name("BeerChallengeTestTimeResponse")
                 .When()
                     .Get("https://api.punkapi.com/v2/beers?brewed_after=12-2015")
                 .Then()
                 .TestElaspedTime("Time Test", x => x < 500)
                 .TestStatus("Response Test", x => x == 200);

        }

    }
}

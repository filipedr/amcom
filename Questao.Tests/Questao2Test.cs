using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Questao2.Tests
{
    public class TotalScoredGoalsTests
    {

        [Fact]
        public async Task Get_Paris_TotalGoals()
        {
            string teamName = "Paris Saint-Germain";
            int year = 2013;

            int totalGoals = await Program.getTotalScoredGoals(teamName, year);
            Assert.Equal(62, totalGoals); // 62 valor esperado
        }

        [Fact]
        public async Task Get_Chelsea_TotalGoals()
        {
            string teamName = "Chelsea";
            int year = 2014;

            int totalGoals = await Program.getTotalScoredGoals(teamName, year);
            Assert.Equal(47, totalGoals); // 47 valor esperado
        }


    }
}
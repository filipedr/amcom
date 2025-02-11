using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Program
{
    public static async Task Main()
    {
        string teamName = "Paris Saint-Germain";
        int year1 = 2012;
        int totalGoals1 = await getTotalScoredGoals(teamName, year1);
        int year2 = 2013;
        int totalGoals2 = await getTotalScoredGoals(teamName, year2);
        //Soma para completar os Gols da temporada 2012/2013
        int totalGoals = totalGoals1 + totalGoals2;

        Console.WriteLine("Team "+ teamName +" scored "+ totalGoals.ToString() + " goals in " + year1 + "/" + year2);

        teamName = "Chelsea";
        year1 = 2013;
        totalGoals1 = await getTotalScoredGoals(teamName, year1);
        year2 = 2014;
        totalGoals2 = await getTotalScoredGoals(teamName, year2);
        //Soma para completar os Gols da temporada 2013/2014
        totalGoals = totalGoals1 + totalGoals2;

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year1 +"/" + year2);

        // Output expected:
        // Team Paris Saint - Germain scored 109 goals in 2013
        // Team Chelsea scored 92 goals in 2014
    }

    public static async Task<int> getTotalScoredGoals(string team, int year)
    {

        using (HttpClient client = new HttpClient())
        {  
            int totalGoals = 0;
            int page = 1;
            bool totalPages = true;

            while (totalPages)
            {
                string url = $"https://jsonmock.hackerrank.com/api/football_matches?year={year}&team1={team.Replace(" ", "%20")}&page={page}";
                var response = await client.GetStringAsync(url);
                var json = JObject.Parse(response);

                foreach (var match in json["data"])
                {
                    totalGoals += (int)match["team1goals"];
                }

                // Verifica se há mais páginas
                totalPages = page < (int)json["total_pages"];
                page++;

            }

            return totalGoals;
        }
    }

}
using System.Collections.Generic;
using SearchFight.Logic;
using System;
using System.Threading.Tasks;

namespace SearchFight
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                RunAsync(args).Wait();
            }
            else 
            {
                Console.WriteLine("Please, enter languages when call to exe file");
                Console.WriteLine("");
                Console.WriteLine("Loading initial languages");
                string[] queries = new string[] { ".net", "java", "python", "javacript", "php" };
                RunAsync(queries).Wait();                
            }
            
            Console.ReadLine();
        }


        static async Task RunAsync(string[] queries)
        {
            SearchFightLogic searchFightLogic = new SearchFightLogic();
            
            long maxResultGoogle = 0, maxResultBing = 0;
            int idxMajorGoogle = 0, idxMajorBing = 0;
            long resultGoogle = 0, resultBing = 0;

            List<string> resuts = new List<string>();

            for (int i = 0; i < queries.Length; i++)
            {
                resultGoogle = await searchFightLogic.FindWithGoogle(queries[i]);
                resultBing = await searchFightLogic.FindWithBing(queries[i]);
                resuts.Add(string.Format("{0},{1},{2}", queries[i], resultGoogle, resultBing));

                Console.WriteLine(string.Format("{0}: Google: {1} Bing: {2}", queries[i], resultGoogle, resultBing));

                if (resultGoogle > maxResultGoogle) 
                {
                    maxResultGoogle = resultGoogle;
                    idxMajorGoogle = i;
                }

                if (resultBing > maxResultBing)
                {
                    maxResultBing = resultBing;
                    idxMajorBing = i;
                }
            }

            Console.WriteLine("Google winner: " + queries[idxMajorGoogle]);
            Console.WriteLine("Bing winner: " + queries[idxMajorBing]);
            Console.WriteLine("Total winner: " + searchFightLogic.GetWinner(resuts));

        }

        
    }
}

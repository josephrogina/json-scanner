using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JSONScanner.Models;
using Newtonsoft.Json;

namespace JSONScanner.Controllers
{
    public class OrdersController : Controller
    {
        // GET: Orders
        public ActionResult Index()
        {
            // TODO: Setup for user selection/input?
            string jsonUrl = "http://files.olo.com/pizzas.json";

            // Get the JSON data from URL.
            string jsonString = DownloadSerializedJsonDataFromUrl(jsonUrl);

            // Pull the JSON data into a list of objects.
            var listOfToppingsFromOrders = JsonConvert.DeserializeObject<List<OrdersModels>>(jsonString);

            // Sort the objects as requested.
            var sortedListOfToppingOrders = SortToppingOrdersByNumberOfTimesOrdered(listOfToppingsFromOrders); 

            // Return only the top 20 results.
            return View(sortedListOfToppingOrders);
        }

        /*
         * Sort the list of orders by most frequently ordered pizza configurations, listing the toppings 
         * for each along with the number of times that pizza configuration has been ordered.
         */
        private object SortToppingOrdersByNumberOfTimesOrdered(List<OrdersModels> listOfToppingsFromOrders)
        {
            // TODO: Optimize code for speed and add better error detection.

            // Build a list of just toppings as comma seperated strings and group them together for the counts.
            var listOfToppings = listOfToppingsFromOrders.Select(x => string.Join(", ", x.toppings.OrderBy(y => y)));
            var groupsOfToppings = listOfToppings.GroupBy(x => x);

            // Build the list of toppings ordered with counts.
            var listOfOrdersForViewModels = new List<OrdersViewModels>();
            foreach (var toppingGroup in groupsOfToppings)
            {
                listOfOrdersForViewModels.Add(new OrdersViewModels { toppings = toppingGroup.Key, OrderCount = toppingGroup.Count() });
            }

            // Sort the list of toppings by the order count and return the top 20 records.
            var sortedListOfOrdersForViewModels = listOfOrdersForViewModels.OrderByDescending(x => x.OrderCount);
            var top20listOfOrdersForViewModels = sortedListOfOrdersForViewModels.Take(20);
            return top20listOfOrdersForViewModels;
        }

        /*
         * Grab the JSON data from a URL and return as a string.
         * - Once exception handling is added, this can be used for multiple types of data retrieval.
         */
        public string DownloadSerializedJsonDataFromUrl(string url)
        {
            using (var webClient = new WebClient())
            {
                var json_data = string.Empty;
                
                // Try to download JSON data as a string
                try
                {
                    json_data = webClient.DownloadString(url);
                }
                catch (Exception)
                {
                    // TODO: Add exception handling for a clean recovery.
                }

                // TODO: Check string with JSON data is not empty before return.
                return json_data;
            }
        }
    }
}

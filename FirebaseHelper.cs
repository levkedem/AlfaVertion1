using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Database;
using Firebase.Database.Query;
using Xamarin.Essentials;

namespace AlfaVertion1
{
    public class FirebaseHelper
    {
        public static FirebaseClient client= new FirebaseClient("https://cohesive-geode-337809-default-rtdb.firebaseio.com/");
        private static string database = "locationsDb";       

        public static async Task<List<Exercise>> GetAll()
        {
            var respone = await client.Child(database).OnceAsync<Exercise>();
            return (respone).Select(item => new Exercise
                {
                    id = item.Object.id,
                    parts = item.Object.parts,
                    date = item.Object.date,
                    name=item.Object.name,
                    timeForThisEx= item.Object.timeForThisEx,
                    distanceForThisExKM = item.Object.distanceForThisExKM,
                }).ToList();
        }

        public static async Task Add(Exercise exercise)
        {
            await client
                .Child(database)
                .PostAsync(exercise);
        }

        public static async Task<Exercise> Get(string name)
        {
            var allPersons = await GetAll();
            await client
              .Child(database)
              .OnceAsync<Exercise>();
            return allPersons.Where(a => a.name == name).FirstOrDefault();
        }

        public static async Task Update(Exercise state)
        {
            var toUpdatePerson = (await client
              .Child(database)
              .OnceAsync<Exercise>()).Where(a => a.Object.name == state.name).FirstOrDefault();

            await client
              .Child(database)
              .Child(toUpdatePerson.Key)
              .PutAsync(state);
        }
        public static async Task Delete(string name)
        {
            var toDeletePerson = (await client
              .Child(database)
              .OnceAsync<Exercise>()).Where(a => a.Object.name == name).FirstOrDefault();
            await client.Child(database).Child(toDeletePerson.Key).DeleteAsync();

        }
    }
}
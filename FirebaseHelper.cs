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
        public static FirebaseClient client= new FirebaseClient("https://runningappv3-default-rtdb.firebaseio.com/");
        private static string database = "locationsDb2";


        public static async Task Add(Exercise exercise)
        {
            await client
                .Child(database)
                .PostAsync(exercise);

        }
        public static async Task<List<Exercise>> GetAll()
        {  
            var response = await client.Child(database).OnceAsync<Exercise>();
            //var cast = response.Select(item => (FirebaseObject<DbExercise>)item);
            var cast = response.Select(item => (Exercise)item.Object).ToList();

            return (response).Select(item => new Exercise
            {
                    user=item.Object.user,
                    isPublic=item.Object.isPublic,
                    parts = item.Object.parts,
                    date = item.Object.date,
                    name=item.Object.name,
                    timeForThisEx= item.Object.timeForThisEx,
                    distanceForThisExKM = item.Object.distanceForThisExKM,

                }).ToList();
        }
       
        
        /*public static async Task<List<Exercise>> GetAllNormalExerecise()
        {
            List<DbExercise> dBList =await GetAll();
              List<Exercise> eXListT = new List<Exercise>();
              for (int i = 0; i < dBList.Count; i++)
              {
                  eXListT.Add(dBList[i].ConvertToNormal());
              }
              return eXListT;
        }*/

        

        public static async Task<Exercise> Get(string name)
        {
            var allPersons = await GetAll();
            await client
              .Child(database)
              .OnceAsync<DbExercise>();
            return allPersons.Where(a => a.name == name).FirstOrDefault();
        }
        public static async Task<List<Exercise>> GetAllPublic()
        {
            var allExercises = await GetAll();

            string mac = MainActivity.userName.GetString("UserName", "0");

            return allExercises.Where(a => a.user != mac && a.isPublic==true).ToList();
        }
        public static async Task<List<Exercise>> GetAllPersonalExercises()
        {
            var allExercises = await GetAll();

            string mac = MainActivity.userName.GetString("UserName", "0");

            return allExercises.Where(a => a.user == mac).ToList();
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
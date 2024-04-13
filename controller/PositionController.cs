using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using testapi.Models; //για το sql database
using System;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace MyWebApi.ControllerBase
{
    [ApiController]
    [Route("[Controller]")]
    
    public class PositionController : Controller
    {
        public static double ToRadians(double coordinates){  //Μετατροπή σε radians που θα χρησιμοποιηθεί παρακάτω για την απόσταση Haversine
    
        return Math.PI * coordinates / 180.0;
        }
        private readonly MyDbContext _db;

        public PositionController(MyDbContext db)
        {
            _db = db; //Χρήση constructor για να μπορεί να χρησιμοποιηθεί παρακάτω χωρίς φόβο για error.
        }
        [HttpPost]
        [Route("insert")] //position/add (url για τεστινγκ μέσω swagger)

        //1ο endpoint για προσθήκη αντικειμένου Position στη βάση
        public IActionResult InsertPosition([FromBody] Position position) //[FromBody] γιατί θέλουμε να διαβάζει από το http request body
        {
            
            _db.Positions.Add(position);
            _db.SaveChanges(); //εισαγωγή στο Position table
            return Ok("Position added successfully");
        }

        [HttpGet]
        [Route("get")] //position/get (url για τεστινγκ μέσω swagger)

        //2ο endpoint για προβολή της κάθε μεταβλητής που περιέχει ο πίνακας.
        public IActionResult GetPositions()
        {
            var table= _db.Positions
            .FromSql($"SELECT * FROM dbo.Position");
            // Retrieve and return positions
            return Ok(table);
        }
        // τρίτο endpoint για τον υπολογισμό της απόστασης haversine
        [HttpGet()]
        [Route("calculate/{name1}/{name2}")] //position/calculate
        public  double  Calculation(string name1,string name2){
            // https://stackoverflow.com/questions/63217053/how-to-get-request-a-specific-id-from-database-in-a-c-sharp-net-api
            var FirstPosition= _db.Positions
            .Where(p => p.Name == name1)
            .Select(p=>new Position(){
                Latitude=p.Latitude,
                Longtitude=p.Longtitude
            }).FirstOrDefault();

            var SecondPosition = _db.Positions
            .Where(p=>p.Name==name2)
            .Select(p=>new Position(){
                Latitude=p.Latitude,
                Longtitude=p.Longtitude
            }).FirstOrDefault();

            //μετατροπή συντεταγμένων σε rads
            FirstPosition.Latitude=ToRadians(FirstPosition.Latitude);
            FirstPosition.Longtitude=ToRadians(FirstPosition.Longtitude);
            SecondPosition.Latitude=ToRadians(SecondPosition.Latitude);
            SecondPosition.Longtitude=ToRadians(SecondPosition.Longtitude);

            //calculate the distance between 2 points
            double Final_Latitude = FirstPosition.Latitude-SecondPosition.Latitude;
            double Final_Longtitude = FirstPosition.Longtitude-SecondPosition.Longtitude;
            double a = Math.Pow(Math.Sin(Final_Latitude / 2), 2) + Math.Cos(FirstPosition.Latitude) * Math.Cos(FirstPosition.Longtitude) * Math.Pow(Math.Sin(Final_Longtitude / 2), 2);
            double c = 2 * Math.Asin(Math.Sqrt(a));

            double radius = 6371;

            return radius * c;
        }
    }
}

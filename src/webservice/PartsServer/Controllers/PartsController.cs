using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PartsService.Models;
using System.Net;
using System.Text.Json;

namespace PartsService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartsController : BaseController
    {
        [HttpGet]
        public ActionResult Get()
        {
            var authorized = CheckAuthorization();
            if (!authorized)
            {
                return Unauthorized();
            }
            Console.WriteLine("GET /api/cars");
            return new JsonResult(UserCars);
        }

        [HttpGet("{carid}")]
        public ActionResult Get(string carid)
        {
            var authorized = CheckAuthorization();
            if (!authorized)
            {
                return Unauthorized();
            }

            if (string.IsNullOrEmpty(carid))
                return this.BadRequest();

            carid = carid.ToUpperInvariant();
            Console.WriteLine($"GET /api/cars/{carid}");
            var userCars = UserCars;
            var car = userCars.SingleOrDefault(x => x.CarID == carid);

            if (car == null)
            {
                return this.NotFound();
            }
            else
            {
                return this.Ok(car);
            }
        }

        [HttpPut("{carid}")]
        public HttpResponseMessage Put(string carid, [FromBody] Car car)
        {
            try
            {
                var authorized = CheckAuthorization();
                if (!authorized)
                {
                    return new HttpResponseMessage(HttpStatusCode.Unauthorized);
                }

                if (!ModelState.IsValid)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                if (string.IsNullOrEmpty(car.CarID))
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                Console.WriteLine($"PUT /api/cars/{carid}");
                Console.WriteLine(JsonSerializer.Serialize(car));


                var userCars = UserCars;
                var existingParts = userCars.SingleOrDefault(x => x.CarID == carid);
                if (existingParts != null)
                {
                    existingParts.Marka = car.Marka;
                    existingParts.Model = car.Model;
                    existingParts.Cena = car.Cena;
                    existingParts.Rocznik = car.Rocznik;
                }

                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public ActionResult Post([FromBody] Car car)
        {
            try
            {
                var authorized = CheckAuthorization();
                if (!authorized)
                {
                    return this.Unauthorized();
                }

                if (!string.IsNullOrWhiteSpace(car.CarID))
                {
                    return this.BadRequest();
                }
                Console.WriteLine($"POST /api/cars");
                Console.WriteLine(JsonSerializer.Serialize(car));

                car.CarID = PartsFactory.CreateCarID();

                if (!ModelState.IsValid)
                {
                    return this.BadRequest();
                }

                var userCars = UserCars;

                if (userCars.Any(x => x.CarID == car.CarID))
                {
                    return this.Conflict();
                }

                userCars.Add(car);

                return this.Ok(car);
            }
            catch (Exception ex)
            {
                return this.Problem("Internal server error");
            }
        }

        [HttpDelete]
        [Route("{carid}")]
        public HttpResponseMessage Delete(string carid)
        {
            try
            {
                var authorized = CheckAuthorization();
                if (!authorized)
                {
                    return new HttpResponseMessage(HttpStatusCode.Unauthorized);
                }

                var userCars = UserCars;
                var existingParts = userCars.SingleOrDefault(x => x.CarID == carid);

                if (existingParts == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.NotFound);
                }
                Console.WriteLine($"POST /api/cars/{carid}");
                userCars.RemoveAll(x => x.CarID == carid);

                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
    }
}

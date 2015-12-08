using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using TheWorld.Models;
using System.Net;
using TheWorld.ViewModels;
using AutoMapper;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TheWorld.Controllers.Api
{
    [Route("api/trips")]
    public class TripController : Controller
    {
        private IWorldRepository _repository;

        public TripController(IWorldRepository repository)
        {
            _repository = repository;
        }

        // GET: api/values
        [HttpGet]
        public JsonResult Get()
        {
            var results = Mapper.Map<IEnumerable<TripViewModel>>(_repository.GetAllTripsWithStops());

            return Json(results);
        }

        //// GET api/values/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/values
        [HttpPost]
        public JsonResult Post([FromBody]TripViewModel vm)
        {
            if(ModelState.IsValid)
            {
                //Map the view model to the Trip object
                var newTrip = Mapper.Map<Trip>(vm);

                _repository.AddTrip(newTrip);

                if (_repository.SaveAll()) {
                    Response.StatusCode = (int)HttpStatusCode.Created;
                    return Json(Mapper.Map<TripViewModel>(newTrip));
                }
            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json("Failed");
        }

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}

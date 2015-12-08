using AutoMapper;
using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TheWorld.Models;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Api
{
    [Route("api/trips/{tripName}/stops")]
    public class StopController : Controller
    {
        private IWorldRepository _repository;

        public StopController(IWorldRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public JsonResult Get(string tripName)
        {
            try {
                var results = _repository.GetTripByName(tripName);

                if(results == null)
                {
                    return Json(null);
                }

                return Json(Mapper.Map<IEnumerable<Stop>>(results.Stops.OrderBy(s => s.Order)));
           }
            catch(Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Post(string tripName, [FromBody]StopViewModel vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Map the view model to the Trip object
                    var newStop = Mapper.Map<Stop>(vm);

                    _repository.AddStop(tripName, newStop);

                    if (_repository.SaveAll())
                    {
                        Response.StatusCode = (int)HttpStatusCode.Created;
                        return Json(Mapper.Map<StopViewModel>(newStop));
                    }
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Validation failed on new stop");
            }
        }
    }
}

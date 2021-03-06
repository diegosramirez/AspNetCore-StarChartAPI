﻿using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [ApiController]
    [Route("")]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var celestialObject = _context.CelestialObjects.Find(id);
            if (celestialObject == null)
                return NotFound();
            celestialObject.Satellites = _context.CelestialObjects.Where(c => c.Id == celestialObject.Id).ToList();
            return Ok(celestialObject);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celestialObjects = _context.CelestialObjects.Where(c => c.Name == name).ToList();
            if (!celestialObjects.Any())
                return NotFound();
            celestialObjects.ForEach(
                celestialObject => celestialObject.Satellites = _context.CelestialObjects.Where(
                    satellite => satellite.Id == celestialObject.Id).ToList());
            return Ok(celestialObjects);
        }

        [HttpGet()]
        public IActionResult GetAll()
        {
            var celestialObjects = _context.CelestialObjects.ToList();
            celestialObjects.ForEach(
                celestialObject => celestialObject.Satellites = _context.CelestialObjects.Where(
                    satellite => satellite.Id == celestialObject.Id).ToList());
            return Ok(celestialObjects);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CelestialObject celestialObject)
        {
            _context.CelestialObjects.Add(celestialObject);
            _context.SaveChanges();
            return CreatedAtRoute("GetById", new
            {
                id = celestialObject.Id                
            }, celestialObject);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject celestialObject)
        {
            var celestialObj = _context.CelestialObjects.FirstOrDefault(c => c.Id == id);
            if (celestialObj == null)
                return NotFound();

            celestialObj.Name = celestialObject.Name;
            celestialObj.OrbitalPeriod = celestialObject.OrbitalPeriod;
            celestialObj.OrbitedObjectId = celestialObject.OrbitedObjectId;

            _context.CelestialObjects.Update(celestialObj);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var celestialObject = _context.CelestialObjects.FirstOrDefault(c => c.Id == id);
            if (celestialObject == null)
                return NotFound();

            celestialObject.Name = name;

            _context.Update(celestialObject);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var celestialObject = _context.CelestialObjects.Where(c => c.Id == id || c.OrbitedObjectId == id).ToList();
            if (!celestialObject.Any())
                return NotFound();
            
            _context.CelestialObjects.RemoveRange(celestialObject);
            _context.SaveChanges();

            return NoContent();
        }
    }
}

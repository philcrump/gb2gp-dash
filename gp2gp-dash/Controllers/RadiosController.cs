﻿using gp2gp_dash.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace gp2gp_dash.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RadiosController : ControllerBase
    {
        static List<RadioState> states = new List<RadioState>();

        [HttpPost]
        public void PostState([FromBody]List<RadioState> inp)
        {
            lock (states)
            {
                foreach (var state in inp)
                {
                    var existing = states.SingleOrDefault(rs => rs.ID == state.ID);

                    if (existing != null)
                    {
                        states.Remove(existing);
                    }

                    states.Add(state);
                }
            }
        }

        [HttpGet]
        public List<RadioState> GetStates()
        {
            lock (states)
            {
                return states.Where(rs => rs.LastUpdated.IsYoungerThan(TimeSpan.FromMinutes(1))).OrderBy(rs => rs.Frequency).ToList();
            }
        }
    }

    public static class Extensions
    {
        public static bool IsYoungerThan(this DateTime dt, TimeSpan thisLong)
        {
            return DateTime.Now - dt < thisLong;
        }
    }
}
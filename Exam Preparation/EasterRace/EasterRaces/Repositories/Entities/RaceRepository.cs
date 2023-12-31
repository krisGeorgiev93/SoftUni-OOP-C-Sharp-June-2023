﻿using EasterRaces.Models.Races.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasterRaces.Repositories.Entities
{
    public class RaceRepository : Repository<IRace>
    {
        private ICollection<IRace> races;
        public RaceRepository()
        {
            races = new List<IRace>();
        }
    }
}

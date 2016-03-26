﻿using DpControl.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.IRepository
{
    public interface IUserLocationRepository:IBaseRepository<UserLocationAddModel,UserLocationUpdateModel,UserLocationSearchModel>
    {
    }
}

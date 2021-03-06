﻿using DpControl.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.IRepository
{
    public interface IDeviceRepository:IBaseRepository<DeviceAddModel,DeviceUpdateModel,DeviceSearchModel>
    {
        #region Relations
        Task<IEnumerable<LocationSubSearchModel>> GetLocationsByDeviceIdAsync(int deviceId);
        #endregion
    }
}

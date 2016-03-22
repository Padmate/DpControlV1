﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using DpControl.Domain.Entities;
using DpControl.Domain.Models;
using DpControl.Domain.IRepository;
using DpControl.Domain.EFContext;
using DpControl.Domain.Execptions;

namespace DpControl.Domain.Repository
{
    public class LocationRepository : ILocationRepository
    {
        ShadingContext _context;
        private readonly IUserInfoRepository _userInfo;

        #region
        public LocationRepository()
        {

        }
        public LocationRepository(ShadingContext context)
        {
            _context = context;
        }

        public LocationRepository(ShadingContext context, IUserInfoRepository userInfo)
        {
            _context = context;
            _userInfo = userInfo;
        }
        #endregion

        public int Add(LocationAddModel mLocation)
        {
            var customer = _context.Projects.FirstOrDefault(l => l.ProjectId == mLocation.ProjectId);
            if (customer == null)
                throw new ExpectException("Could not find Project data which ProjectId equal to " + mLocation.ProjectId);

            var device = _context.Devices.FirstOrDefault(l => l.DeviceId == mLocation.DeviceId);
            if (device == null)
                throw new ExpectException("Could not find Device data which DeviceId equal to " + mLocation.DeviceId);

            //InstallationNumber must be unique
            var checkData = _context.Locations.Where(dl => dl.InstallationNumber == mLocation.InstallationNumber).ToList();
            if (checkData.Count > 0)
                throw new ExpectException("The data which InstallationNumber equal to '" + mLocation.InstallationNumber + "' already exist in system");

            //DeviceSerialNo must be unique
            checkData = _context.Locations.Where(dl => dl.DeviceSerialNo == mLocation.DeviceSerialNo).ToList();
            if (checkData.Count > 0)
                throw new ExpectException("The data which DeviceSerialNo equal to '" + mLocation.DeviceSerialNo + "' already exist in system");

            //Check Orientation
            if (!Enum.IsDefined(typeof(Orientation), mLocation.Orientation))
            {
                throw new ExpectException("Invalid Orientation.You can get correct Orientation values from API");
            }

            //Check DeviceType
            if (!Enum.IsDefined(typeof(DeviceType), mLocation.DeviceType))
            {
                throw new ExpectException("Invalid DeviceType.You can get correct DeviceType values from API");
            }

            //Check CommMode
            if (!Enum.IsDefined(typeof(CommMode), mLocation.CommMode))
            {
                throw new ExpectException("Invalid CommMode.You can get correct CommMode values from API");
            }

            //Get UserInfo
            var user = _userInfo.GetUserInfo();

            var model = new Location
            {
                Building = mLocation.Building,
                CommAddress = mLocation.CommAddress,
                CommMode = mLocation.CommMode,
                CurrentPosition = mLocation.CurrentPosition,
                Description = mLocation.Description,
                DeviceSerialNo = mLocation.DeviceSerialNo,
                DeviceId = mLocation.DeviceId,
                DeviceType = mLocation.DeviceType,
                FavorPositionFirst = mLocation.FavorPositionFirst,
                FavorPositionrSecond = mLocation.FavorPositionrSecond,
                FavorPositionThird = mLocation.FavorPositionThird,
                Floor = mLocation.Floor,
                InstallationNumber = mLocation.InstallationNumber,
                Orientation = mLocation.Orientation,
                ProjectId = mLocation.ProjectId,
                RoomNo = mLocation.RoomNo,
                Creator = user.UserName,
                CreateDate = DateTime.Now
            };
            _context.Locations.Add(model);
            _context.SaveChangesAsync();
            return model.LocationId;
        }

        public async Task<int> AddAsync(LocationAddModel mLocation)
        {
            var customer = _context.Projects.FirstOrDefault(l=>l.ProjectId == mLocation.ProjectId);
            if (customer == null)
                throw new ExpectException("Could not find Project data which ProjectId equal to " + mLocation.ProjectId);

            if (mLocation.DeviceId != null)
            {
                var device = _context.Devices.FirstOrDefault(l => l.DeviceId == mLocation.DeviceId);
                if (device == null)
                    throw new ExpectException("Could not find Device data which DeviceId equal to " + mLocation.DeviceId);

            }
            //InstallationNumber must be unique
            var checkData = await _context.Locations.Where(dl=>dl.InstallationNumber == mLocation.InstallationNumber).ToListAsync();
            if (checkData.Count > 0)
                throw new ExpectException("The data which InstallationNumber equal to '" + mLocation.InstallationNumber + "' already exist in system");

            //DeviceSerialNo must be unique
            checkData = await _context.Locations.Where(dl => dl.DeviceSerialNo == mLocation.DeviceSerialNo).ToListAsync();
            if (checkData.Count > 0)
                throw new ExpectException("The data which DeviceSerialNo equal to '" + mLocation.DeviceSerialNo + "' already exist in system");

            //Check Orientation
            if (!Enum.IsDefined(typeof(Orientation),mLocation.Orientation))
            {
                throw new ExpectException("Invalid Orientation.You can get correct Orientation values from API");
            }

            //Check DeviceType
            if (!Enum.IsDefined(typeof(DeviceType), mLocation.DeviceType))
            {
                throw new ExpectException("Invalid DeviceType.You can get correct DeviceType values from API");
            }

            //Check CommMode
            if (!Enum.IsDefined(typeof(CommMode), mLocation.CommMode))
            {
                throw new ExpectException("Invalid CommMode.You can get correct CommMode values from API");
            }

            //Get UserInfo
            var user = await _userInfo.GetUserInfoAsync();

            var model = new Location
            {
                Building = mLocation.Building,
                CommAddress = mLocation.CommAddress,
                CommMode = mLocation.CommMode,
                CurrentPosition = mLocation.CurrentPosition,
                Description = mLocation.Description,
                DeviceSerialNo = mLocation.DeviceSerialNo,
                DeviceId = mLocation.DeviceId,
                DeviceType = mLocation.DeviceType,
                FavorPositionFirst = mLocation.FavorPositionFirst,
                FavorPositionrSecond = mLocation.FavorPositionrSecond,
                FavorPositionThird = mLocation.FavorPositionThird,
                Floor = mLocation.Floor,
                InstallationNumber = mLocation.InstallationNumber,
                Orientation =mLocation.Orientation,
                ProjectId = mLocation.ProjectId,
                RoomNo = mLocation.RoomNo,
                Creator = user.UserName,
                CreateDate = DateTime.Now
            };
            _context.Locations.Add(model);
            await _context.SaveChangesAsync();
            return model.LocationId;
        }

        public LocationSearchModel FindById(int locationId)
        {
            var result = _context.Locations.Where(v => v.LocationId == locationId);
            result = result.Include(l => l.GroupLocations).ThenInclude(gl => gl.Group);
            result = result.Include(l => l.Alarms);
            result = result.Include(l => l.Logs);

            var location = result.FirstOrDefault();
            var locationSearch = LocationOperator.SetLocationSearchModelCascade(location);

            return locationSearch;
        }

        public async Task<LocationSearchModel> FindByIdAsync(int locationId)
        {
            var result = _context.Locations.Where(v => v.LocationId == locationId);
            result = result.Include(l => l.GroupLocations).ThenInclude(gl => gl.Group);
            result = result.Include(l => l.Alarms);
            result = result.Include(l => l.Logs);

            var location = await result.FirstOrDefaultAsync();
            var locationSearch = LocationOperator.SetLocationSearchModelCascade(location);
            
            return locationSearch;
        }

        public IEnumerable<LocationSearchModel> GetAll(Query query)
        {
            var queryData = from L in _context.Locations
                            select L;

            var result = QueryOperate<Location>.Execute(queryData, query);
            result = ExpandRelatedEntities(result, query.expand);
            //以下执行完后才会去数据库中查询
            var locations =  result.ToList();
            var locationsSearch = LocationOperator.SetLocationSearchModelCascade(locations);


            return locationsSearch;
        }

        public async Task<IEnumerable<LocationSearchModel>> GetAllAsync(Query query)
        {
            var queryData = from L in _context.Locations
                            select L;

            var result = QueryOperate<Location>.Execute(queryData, query);
            result = ExpandRelatedEntities(result,query.expand);

            //以下执行完后才会去数据库中查询
            var locations = await result.ToListAsync();
            var locationsSearch = LocationOperator.SetLocationSearchModelCascade(locations);

            return locationsSearch;
        }
        
        public void RemoveById(int locationId)
        {
            var location = _context.Locations.FirstOrDefault(l => l.LocationId == locationId);
            if (location == null)
                throw new ExpectException("Could not find data which DeviceLocation equal to " + locationId);

            _context.Remove(location);
             _context.SaveChanges();
        }

        public async Task RemoveByIdAsync(int locationId)
        {
            var location = _context.Locations.FirstOrDefault(l=>l.LocationId == locationId);
            if (location == null)
                throw new ExpectException("Could not find data which LocationId equal to " + locationId);

            _context.Remove(location);
            await _context.SaveChangesAsync();
        }

        public int UpdateById(int locationId, LocationUpdateModel mLocation)
        {
            var location = _context.Locations.FirstOrDefault(l => l.LocationId == locationId);
            if (location == null)
                throw new ExpectException("Could not find data which LocationId equal to " + locationId);

            var device = _context.Devices.FirstOrDefault(l => l.DeviceId == mLocation.DeviceId);
            if (device == null)
                throw new ExpectException("Could not find Device data which DeviceId equal to " + mLocation.DeviceId);


            //InstallationNumber must be unique
            var checkData = _context.Locations
                .Where(dl => dl.InstallationNumber == mLocation.InstallationNumber
                && dl.LocationId != locationId).ToList();
            if (checkData.Count > 0)
                throw new ExpectException("The data which InstallationNumber equal to '" + mLocation.InstallationNumber + "' already exist in system");

            //DeviceSerialNo must be unique
            checkData = _context.Locations
                .Where(dl => dl.DeviceSerialNo == mLocation.DeviceSerialNo
                && dl.LocationId != locationId).ToList();
            if (checkData.Count > 0)
                throw new ExpectException("The data which DeviceSerialNo equal to '" + mLocation.DeviceSerialNo + "' already exist in system");

            //Check Orientation
            if (!Enum.IsDefined(typeof(Orientation), mLocation.Orientation))
            {
                throw new ExpectException("Invalid Orientation.You can get correct Orientation values from API");
            }

            //Check DeviceType
            if (!Enum.IsDefined(typeof(DeviceType), mLocation.DeviceType))
            {
                throw new ExpectException("Invalid DeviceType.You can get correct DeviceType values from API");
            }

            //Check CommMode
            if (!Enum.IsDefined(typeof(CommMode), mLocation.CommMode))
            {
                throw new ExpectException("Invalid CommMode.You can get correct CommMode values from API");
            }

            //Get UserInfo
            var user = _userInfo.GetUserInfo();

            location.Building = mLocation.Building;
            location.CommAddress = mLocation.CommAddress;
            location.CommMode = mLocation.CommMode;
            location.CurrentPosition = mLocation.CurrentPosition;
            location.Description = mLocation.Description;
            location.DeviceSerialNo = mLocation.DeviceSerialNo;
            location.DeviceId = mLocation.DeviceId;
            location.DeviceType = mLocation.DeviceType;
            location.FavorPositionFirst = mLocation.FavorPositionFirst;
            location.FavorPositionrSecond = mLocation.FavorPositionrSecond;
            location.FavorPositionThird = mLocation.FavorPositionThird;
            location.Floor = mLocation.Floor;
            location.InstallationNumber = mLocation.InstallationNumber;
            location.Orientation = mLocation.Orientation;
            location.RoomNo = mLocation.RoomNo;
            location.Modifier = user.UserName;
            location.ModifiedDate = DateTime.Now;

            _context.SaveChanges();
            return location.LocationId;
        }

        public async Task<int> UpdateByIdAsync(int locationId, LocationUpdateModel mLocation)
        {
            var location = _context.Locations.FirstOrDefault(l => l.LocationId == locationId);
            if (location == null)
                throw new ExpectException("Could not find data which DeviceLocation equal to " + locationId);

            var device = _context.Devices.FirstOrDefault(l => l.DeviceId == mLocation.DeviceId);
            if (device == null)
                throw new ExpectException("Could not find Device data which DeviceId equal to " + mLocation.DeviceId);


            //InstallationNumber must be unique
            var checkData = await _context.Locations
                .Where(dl => dl.InstallationNumber == mLocation.InstallationNumber
                && dl.LocationId != locationId).ToListAsync();
            if (checkData.Count > 0)
                throw new ExpectException("The data which InstallationNumber equal to '" + mLocation.InstallationNumber + "' already exist in system");

            //DeviceSerialNo must be unique
            checkData = await _context.Locations
                .Where(dl => dl.DeviceSerialNo == mLocation.DeviceSerialNo
                && dl.LocationId != locationId).ToListAsync();
            if (checkData.Count > 0)
                throw new ExpectException("The data which DeviceSerialNo equal to '" + mLocation.DeviceSerialNo + "' already exist in system");

            //Check Orientation
            if (!Enum.IsDefined(typeof(Orientation), mLocation.Orientation))
            {
                throw new ExpectException("Invalid Orientation.You can get correct Orientation values from API");
            }

            //Check DeviceType
            if (!Enum.IsDefined(typeof(DeviceType), mLocation.DeviceType))
            {
                throw new ExpectException("Invalid DeviceType.You can get correct DeviceType values from API");
            }

            //Check CommMode
            if (!Enum.IsDefined(typeof(CommMode), mLocation.CommMode))
            {
                throw new ExpectException("Invalid CommMode.You can get correct CommMode values from API");
            }

            //Get UserInfo
            var user = await _userInfo.GetUserInfoAsync();

            location.Building = mLocation.Building;
            location.CommAddress = mLocation.CommAddress;
            location.CommMode = mLocation.CommMode;
            location.CurrentPosition = mLocation.CurrentPosition;
            location.Description = mLocation.Description;
            location.DeviceSerialNo = mLocation.DeviceSerialNo;
            location.DeviceId = mLocation.DeviceId;
            location.DeviceType = mLocation.DeviceType;
            location.FavorPositionFirst = mLocation.FavorPositionFirst;
            location.FavorPositionrSecond = mLocation.FavorPositionrSecond;
            location.FavorPositionThird = mLocation.FavorPositionThird;
            location.Floor = mLocation.Floor;
            location.InstallationNumber = mLocation.InstallationNumber;
            location.Orientation = mLocation.Orientation;
            location.RoomNo = mLocation.RoomNo;
            location.Modifier = user.UserName;
            location.ModifiedDate = DateTime.Now;

            await _context.SaveChangesAsync();
            return location.LocationId;
        }

        /// <summary>
        /// Expand Related Entities
        /// </summary>
        /// <param name="result"></param>
        /// <param name="expandParams"></param>
        /// <returns></returns>
        private IQueryable<Location> ExpandRelatedEntities(IQueryable<Location> result, string[] expandParams)
        {
            var needExpandGroups = ExpandOperator.NeedExpand("Groups", expandParams);
            var needExpandAlarms = ExpandOperator.NeedExpand("Alarms", expandParams);
            var needExpandLogs = ExpandOperator.NeedExpand("Logs", expandParams);

            if (needExpandGroups)
                result = result.Include(l => l.GroupLocations).ThenInclude(gl => gl.Group);
            if (needExpandAlarms)
                result = result.Include(l => l.Alarms);
            if (needExpandLogs)
                result = result.Include(l => l.Logs);

            return result;
        }
    }
}

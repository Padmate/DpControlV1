﻿using DpControl.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DpControl.Domain.Models;
using DpControl.Domain.EFContext;
using DpControl.Domain.Execptions;
using Microsoft.Data.Entity;
using DpControl.Domain.Entities;

namespace DpControl.Domain.Repository
{
    public class LogDescriptionRepository : ILogDescriptionRepository
    {

        ShadingContext _context;
        private readonly IUserInfoRepository _userInfo;

        #region
        public LogDescriptionRepository()
        {

        }
        public LogDescriptionRepository(ShadingContext context)
        {
            _context = context;
        }
        
        #endregion

        public int Add(LogDescriptionAddModel mLogDescription)
        {
            //DescriptionCode must be unique
            var checkData = _context.LogDescriptions
                .Where(c => c.DescriptionCode == mLogDescription.DescriptionCode).ToList();
            if (checkData.Count > 0)
                throw new ExpectException("The data which DescriptioncODE equal to " + mLogDescription.DescriptionCode + " already exist in system");


            var model = new LogDescription
            {
                DescriptionCode = mLogDescription.DescriptionCode,
                Description = mLogDescription.Description
            };
            _context.LogDescriptions.Add(model);
            _context.SaveChanges();
            return model.LogDescriptionId;
        }

        public async Task<int> AddAsync(LogDescriptionAddModel mLogDescription)
        {
            //DescriptionCode must be unique
            var checkData = await _context.LogDescriptions
                .Where(c => c.DescriptionCode == mLogDescription.DescriptionCode).ToListAsync();
            if (checkData.Count > 0)
                throw new ExpectException("The data which DescriptioncODE equal to " + mLogDescription.DescriptionCode + " already exist in system");


            var model = new LogDescription
            {
                DescriptionCode = mLogDescription.DescriptionCode,
                Description = mLogDescription.Description
            };
            _context.LogDescriptions.Add(model);
            await _context.SaveChangesAsync();
            return model.LogDescriptionId;
        }

        public LogDescriptionSearchModel FindById(int logDescriptionId)
        {
            var logDescription = _context.LogDescriptions
              .Where(v => v.LogDescriptionId == logDescriptionId)
              .Select(v => new LogDescriptionSearchModel()
              {
                  LogDescriptionId = v.LogDescriptionId,
                  DescriptionCode = v.DescriptionCode,
                  Description = v.Description,
                  Logs = v.Logs.Select(l => new LogSubSearchModel()
                  {
                      LogId = l.LogId,
                      Comment = l.Comment,
                      LogDescriptionId = l.LogDescriptionId,
                      LocationId = l.LocationId,
                      Creator = l.Creator,
                      CreateDate = l.CreateDate,
                      Modifier = l.Modifier,
                      ModifiedDate = l.ModifiedDate
                  })

              }).FirstOrDefault();

            return logDescription;
        }

        public async Task<LogDescriptionSearchModel> FindByIdAsync(int logDescriptionId)
        {
            var logDescription = await _context.LogDescriptions
               .Where(v => v.LogDescriptionId == logDescriptionId)
               .Select(v => new LogDescriptionSearchModel()
               {
                   LogDescriptionId = v.LogDescriptionId,
                   DescriptionCode = v.DescriptionCode,
                   Description = v.Description,
                   Logs = v.Logs.Select(l => new LogSubSearchModel()
                   {
                       LogId = l.LogId,
                       Comment = l.Comment,
                       LogDescriptionId = l.LogDescriptionId,
                       LocationId = l.LocationId,
                       Creator = l.Creator,
                       CreateDate = l.CreateDate,
                       Modifier = l.Modifier,
                       ModifiedDate = l.ModifiedDate
                   })

               }).FirstOrDefaultAsync();

            return logDescription;
        }

        public IEnumerable<LogDescriptionSearchModel> GetAll(Query query)
        {
            var queryData = from L in _context.LogDescriptions
                            select L;

            var result = QueryOperate<LogDescription>.Execute(queryData, query);

            var needExpandLogs = ExpandOperator.NeedExpand("Logs", query.expand);
            if (needExpandLogs)
                result = result.Include(l => l.Logs);

            //以下执行完后才会去数据库中查询
            var logDescriptions = result.ToList();

            var logDescriptionsSearch = logDescriptions.Select(v => new LogDescriptionSearchModel
            {
                LogDescriptionId = v.LogDescriptionId,
                DescriptionCode = v.DescriptionCode,
                Description = v.Description,
                Logs = v.Logs.Select(l => new LogSubSearchModel()
                {
                    LogId = l.LogId,
                    Comment = l.Comment,
                    LogDescriptionId = l.LogDescriptionId,
                    LocationId = l.LocationId,
                    Creator = l.Creator,
                    CreateDate = l.CreateDate,
                    Modifier = l.Modifier,
                    ModifiedDate = l.ModifiedDate
                })
            });

            return logDescriptionsSearch;
        }

        public async Task<IEnumerable<LogDescriptionSearchModel>> GetAllAsync(Query query)
        {
            var queryData = from L in _context.LogDescriptions
                            select L;

            var result = QueryOperate<LogDescription>.Execute(queryData, query);

            var needExpandLogs = ExpandOperator.NeedExpand("Logs", query.expand);
            if (needExpandLogs)
                result = result.Include(l => l.Logs);

            //以下执行完后才会去数据库中查询
            var logDescriptions = await result.ToListAsync();

            var logDescriptionsSearch = logDescriptions.Select(v => new LogDescriptionSearchModel
            {
                LogDescriptionId = v.LogDescriptionId,
                DescriptionCode = v.DescriptionCode,
                Description = v.Description,
                Logs = v.Logs.Select(l => new LogSubSearchModel()
                {
                    LogId = l.LogId,
                    Comment = l.Comment,
                    LogDescriptionId = l.LogDescriptionId,
                    LocationId = l.LocationId,
                    Creator = l.Creator,
                    CreateDate = l.CreateDate,
                    Modifier = l.Modifier,
                    ModifiedDate = l.ModifiedDate
                })
            });

            return logDescriptionsSearch;
        }

        public void RemoveById(int logDescriptionId)
        {
            var logDescription = _context.LogDescriptions.FirstOrDefault(c => c.LogDescriptionId == logDescriptionId);
            if (logDescription == null)
                throw new ExpectException("Could not find data which LogDescriptionId equal to " + logDescriptionId);

            _context.Remove(logDescription);
            _context.SaveChanges();
        }

        public async Task RemoveByIdAsync(int logDescriptionId)
        {
            var logDescription = _context.LogDescriptions.FirstOrDefault(c => c.LogDescriptionId == logDescriptionId);
            if (logDescription == null)
                throw new ExpectException("Could not find data which LogDescriptionId equal to " + logDescriptionId);

            _context.Remove(logDescription);
            await _context.SaveChangesAsync();
        }

        public int UpdateById(int logDescriptionId, LogDescriptionUpdateModel mLogDescription)
        {
            var logDescription = _context.LogDescriptions.FirstOrDefault(c => c.LogDescriptionId == logDescriptionId);
            if (logDescription == null)
                throw new ExpectException("Could not find data which LogDescriptionId equal to " + logDescriptionId);

            //DescriptionCode must be unique
            var checkData = _context.LogDescriptions
                .Where(c => c.DescriptionCode == mLogDescription.DescriptionCode
                && c.LogDescriptionId != logDescriptionId).ToList();
            if (checkData.Count > 0)
                throw new ExpectException("The data which DescriptionCode equal to " + mLogDescription.DescriptionCode + " already exist in system");



            logDescription.DescriptionCode = mLogDescription.DescriptionCode;
            logDescription.Description = mLogDescription.Description;

            _context.SaveChanges();
            return logDescription.LogDescriptionId;
        }

        public async Task<int> UpdateByIdAsync(int logDescriptionId, LogDescriptionUpdateModel mLogDescription)
        {
            var logDescription = _context.LogDescriptions.FirstOrDefault(c => c.LogDescriptionId == logDescriptionId);
            if (logDescription == null)
                throw new ExpectException("Could not find data which LogDescriptionId equal to " + logDescriptionId);

            //DescriptionCode must be unique
            var checkData = await _context.LogDescriptions
                .Where(c => c.DescriptionCode == mLogDescription.DescriptionCode
                && c.LogDescriptionId != logDescriptionId).ToListAsync();
            if (checkData.Count > 0)
                throw new ExpectException("The data which DescriptionCode equal to " + mLogDescription.DescriptionCode + " already exist in system");



            logDescription.DescriptionCode = mLogDescription.DescriptionCode;
            logDescription.Description = mLogDescription.Description;

            await _context.SaveChangesAsync();
            return logDescription.LogDescriptionId;
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DpControl.Domain.Models;
using DpControl.Domain.IRepository;
using DpControl.Domain.Entities;
using DpControl.Domain.EFContext;
using Microsoft.Data.Entity;
using DpControl.Domain.Execptions;

namespace DpControl.Domain.Repository
{
    public class GroupRepository : IGroupRepository
    {
        private ShadingContext _context;
        private readonly IUserInfoRepository _userInfo;

        #region Constructors
        public GroupRepository()
        {
        }

        public GroupRepository(ShadingContext dbContext)
        {
            _context = dbContext;
        }
        public GroupRepository(ShadingContext dbContext, IUserInfoRepository userInfo)
        {
            _context = dbContext;
            _userInfo = userInfo;
        }
        #endregion

        public int Add(GroupAddModel group)
        {
            var project = _context.Projects.FirstOrDefault(p => p.ProjectId == group.ProjectId);
            if (project == null)
                throw new ExpectException("Could not find Project data which ProjectId equal to " + group.ProjectId);
            
            //If SceneId not null,check whether corresponding Scenes data existed
            if (group.SceneId != null)
            {
                var scene = _context.Scenes.FirstOrDefault(p => p.SceneId == group.SceneId);
                if (scene == null)
                    throw new ExpectException("Could not find Scenes data which SceneId equal to " + group.SceneId);
            }

            //GroupName must be unique
            var checkData = _context.Groups.Where(g => g.GroupName == group.GroupName).ToList();
            if (checkData.Count > 0)
                throw new ExpectException("The data which GroupName equal to '" + group.GroupName + "' already exist in system.");

            //Get UserInfo
            var user = _userInfo.GetUserInfo();

            var model = new Group
            {
                GroupName = group.GroupName,
                ProjectId = group.ProjectId,
                SceneId = group.SceneId,
                Creator = user.UserName,
                CreateDate = DateTime.Now
            };
            _context.Groups.Add(model);
            _context.SaveChanges();
            return model.GroupId;
        }

        public async Task<int> AddAsync(GroupAddModel group)
        {
            //Chech whether the Foreign key ProjectId data exist
            var project = _context.Projects.FirstOrDefault(p => p.ProjectId == group.ProjectId);
            if (project == null)
                throw new ExpectException("Could not find Project data which ProjectId equal to " + group.ProjectId);

            //If SceneId not null,check whether corresponding Scenes data existed
            if (group.SceneId !=null)
            {
                var scene = _context.Scenes.FirstOrDefault(p => p.SceneId == group.SceneId);
                if (scene == null)
                    throw new ExpectException("Could not find Scenes data which SceneId equal to " + group.SceneId);
            }

            //GroupName must be unique
            var checkData =await _context.Groups.Where(g => g.GroupName == group.GroupName).ToListAsync();
            if (checkData.Count > 0)
                throw new ExpectException("The data which GroupName equal to '"+group.GroupName + "' already exist in system.");

            //Get UserInfo
            var user = await _userInfo.GetUserInfoAsync();

            var model = new Group
            {
                GroupName = group.GroupName,
                ProjectId = group.ProjectId,
                SceneId = group.SceneId,
                Creator = user.UserName,
                CreateDate = DateTime.Now
            };
            _context.Groups.Add(model);
            await _context.SaveChangesAsync();
            return model.GroupId;
        }

        public GroupSearchModel FindById(int groupId)
        {
            var result = _context.Groups.Where(v => v.GroupId == groupId);
            result = result.Include(g => g.GroupLocations).ThenInclude(gl => gl.Location);
            var group = result.FirstOrDefault();
            var groupSearch = GroupOperator.SetGroupSearchModelCascade(group);

            return groupSearch;
        }

        public async Task<GroupSearchModel> FindByIdAsync(int groupId)
        {
            var result = _context.Groups.Where(v => v.GroupId == groupId);
            result = result.Include(g=>g.GroupLocations).ThenInclude(gl=>gl.Location);
            var group = await result.FirstOrDefaultAsync();
            var groupSearch = GroupOperator.SetGroupSearchModelCascade(group);

            return groupSearch;
        }

        public IEnumerable<GroupSearchModel> GetAll(Query query)
        {
            var queryData = from G in _context.Groups
                            select G;

            var result = QueryOperate<Group>.Execute(queryData, query);
            result = ExpandRelatedEntities(result, query.expand);
            //以下执行完后才会去数据库中查询
            var groups =  result.ToList();
            var groupsSearch = GroupOperator.SetGroupSearchModelCascade(groups);

            return groupsSearch;
        }

        public async Task<IEnumerable<GroupSearchModel>> GetAllAsync(Query query)
        {
            var queryData = from G in _context.Groups
                            select G;

            var result = QueryOperate<Group>.Execute(queryData, query);
            result = ExpandRelatedEntities(result,query.expand);

            //以下执行完后才会去数据库中查询
            var groups = await result.ToListAsync();
            var groupsSearch = GroupOperator.SetGroupSearchModelCascade(groups);

            return groupsSearch;
        }

        

        public void RemoveById(int groupId)
        {
            var group = _context.Groups.FirstOrDefault(c => c.GroupId == groupId);
            if (group == null)
                throw new ExpectException("Could not find data which GroupId equal to " + groupId);

            _context.Groups.Remove(group);
            _context.SaveChanges();
        }

        public async Task RemoveByIdAsync(int groupId)
        {
            var group = _context.Groups.FirstOrDefault(c => c.GroupId == groupId);
            if (group == null)
                throw new ExpectException("Could not find data which GroupId equal to " + groupId);

            _context.Groups.Remove(group);
           await _context.SaveChangesAsync();
        }

        public int UpdateById(int groupId, GroupUpdateModel mgroup)
        {
            var group = _context.Groups.FirstOrDefault(c => c.GroupId == groupId);
            if (group == null)
                throw new ExpectException("Could not find data which GroupId equal to " + groupId);
            //If SceneId not null,check whether corresponding Scenes data existed
            if (mgroup.SceneId != null)
            {
                var scene = _context.Scenes.FirstOrDefault(p => p.SceneId == mgroup.SceneId);
                if (scene == null)
                    throw new ExpectException("Could not find Scenes data which SceneId equal to " + mgroup.SceneId);
            }
            //GroupName must be unique
            var checkData = _context.Groups.Where(g => g.GroupName == mgroup.GroupName
                                                        && g.GroupId != groupId).ToList();
            if (checkData.Count > 0)
                throw new ExpectException("The data which GroupName equal to '" + mgroup.GroupName + "' already exist in system.");


            //Get UserInfo
            var user = _userInfo.GetUserInfo();

            group.GroupName = mgroup.GroupName;
            group.SceneId = mgroup.SceneId;
            group.Modifier = user.UserName;
            group.ModifiedDate = DateTime.Now;

            _context.SaveChanges();
            return group.GroupId;
        }

        public async Task<int> UpdateByIdAsync(int groupId, GroupUpdateModel mgroup)
        {
            var group = _context.Groups.FirstOrDefault(c => c.GroupId == groupId);
            if (group == null)
                throw new ExpectException("Could not find data which GroupId equal to " + groupId);
            //If SceneId not null,check whether corresponding Scenes data existed
            if (mgroup.SceneId != null)
            {
                var scene = _context.Scenes.FirstOrDefault(p => p.SceneId == mgroup.SceneId);
                if (scene == null)
                    throw new ExpectException("Could not find Scenes data which SceneId equal to " + mgroup.SceneId);
            }
            //GroupName must be unique
            var checkData = await _context.Groups.Where(g => g.GroupName == mgroup.GroupName
                                                        && g.GroupId != groupId).ToListAsync();
            if (checkData.Count > 0)
                throw new ExpectException("The data which GroupName equal to '" + mgroup.GroupName + "' already exist in system.");


            //Get UserInfo
            var user = await _userInfo.GetUserInfoAsync();

            group.GroupName = mgroup.GroupName;
            group.SceneId = mgroup.SceneId;
            group.Modifier = user.UserName;
            group.ModifiedDate = DateTime.Now;

            await _context.SaveChangesAsync();
            return group.GroupId;
        }

        /// <summary>
        /// Expand Related Entities
        /// </summary>
        /// <param name="result"></param>
        /// <param name="expandParams"></param>
        /// <returns></returns>
        private IQueryable<Group> ExpandRelatedEntities(IQueryable<Group> result, string[] expandParams)
        {
            var needExpandLocations = ExpandOperator.NeedExpand("Locations", expandParams);
            if (needExpandLocations)
                result = result.Include(g => g.GroupLocations).ThenInclude(gl => gl.Location);

            return result;
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LandBankManagement.Data;
using LandBankManagement.Models;

namespace LandBankManagement.Services
{
   public interface ITalukService
    {
        Task<TalukModel> AddTalukAsync(TalukModel model);
        Task<TalukModel> GetTalukAsync(long id);
        Task<IList<TalukModel>> GetTaluksAsync(DataRequest<Taluk> request);
        Task<IList<TalukModel>> GetTaluksAsync(int skip, int take, DataRequest<Taluk> request);
        Task<int> GetTaluksCountAsync(DataRequest<Taluk> request);
        Task<TalukModel> UpdateTalukAsync(TalukModel model);
        Task<Result> DeleteTalukAsync(TalukModel model);
    }
}

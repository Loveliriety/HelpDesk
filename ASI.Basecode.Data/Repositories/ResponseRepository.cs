using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Repositories
{
    public class ResponseRepository : BaseRepository, IResponseRepository
    {
        private readonly AsiBasecodeDBContext _dbContext;

        public ResponseRepository(AsiBasecodeDBContext dBContext, UnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = dBContext; 
        }

        public Response GetResponseById(int responseId)
        {
            return _dbContext.Set<Response>().Find(responseId);
        }

        public IEnumerable<Response> GetAllResponses()
        {
            return _dbContext.Responses.ToList();
        }

        public void AddResponse(Response response)
        {
            _dbContext.Responses.Add(response);
            _dbContext.SaveChanges();
        }

        public void UpdateResponse(Response response)
        { 
            _dbContext.Set<Response>().Update(response);
            _dbContext.SaveChanges();
        }

        public void DeleteResponse(int responseId)
        {
            var response = _dbContext.Set<Response>().Find(responseId);
            if (response != null)
            {
                _dbContext.Set<Response>().Remove(response);
                _dbContext.SaveChanges();
            }
        }
    }
}

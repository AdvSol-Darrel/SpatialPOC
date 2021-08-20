using SpatialPOC.Context;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AutoMapper;
using SpatialPOC.Models;
using Microsoft.EntityFrameworkCore;
using SpatialPOC.Dto;

namespace SpatialPOC.Services
{
    class ZogLineService
    {
        private MyDbContext _dbContext;
        private IMapper _mapper;

        public ZogLineService(MyDbContext context, IMapper mapper)
        {
            _dbContext = context;
            _mapper = mapper;
        }

        public IEnumerable<LineDto> GetLines()
        {
            DbSet<ZogLine> dbSet = _dbContext.Set<ZogLine>();

            return _mapper.Map<IEnumerable<LineDto>>(dbSet.ToList());
        }
    }
}

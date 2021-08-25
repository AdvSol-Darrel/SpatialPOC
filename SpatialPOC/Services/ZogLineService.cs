using SpatialPOC.Context;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SpatialPOC.Models;
using Microsoft.EntityFrameworkCore;
using SpatialPOC.Dto;

namespace SpatialPOC.Services
{
    class ZogLineService
    {
        private AppDbContext _dbContext;
        private IMapper _mapper;

        public ZogLineService(AppDbContext context, IMapper mapper)
        {
            _dbContext = context;
            _mapper = mapper;
        }

        public IEnumerable<LineDto> GetTransformedLines()
        {
            DbSet<ZogGeometry> dbSet = _dbContext.Set<ZogGeometry>();

            //return _mapper.Map<IEnumerable<LineDto>>(dbSet.ToList());
            return _mapper.Map<IEnumerable<LineDto>>(dbSet.Select(l => new LineDto
            {
                LineId = l.Id,
                Geometry = EF.Functions.Transform(l.Geometry, l.SRId)
            })
            .ToList());
        }

        public IEnumerable<LineDto> GetLines()
        {
            DbSet<ZogGeometry> dbSet = _dbContext.Set<ZogGeometry>();

            return _mapper.Map<IEnumerable<LineDto>>(dbSet.ToList());
        }

        public IEnumerable<LineLength> GetST_3DLength()
        {
            var result = _dbContext.LineLengths.FromSqlRaw("SELECT id As Id, ST_3DLength(geometry) as Length from gis_test.zog_geometry");
            
            return result;
        }

        public CalculatedGeometry LineInterpolate()
        {
            var result = _dbContext.CalculatedGeometries.FromSqlRaw("SELECT ST_LineInterpolatePoints('LINESTRING(25 50, 100 125, 150 190)', 0.20) as geom")
                .FirstOrDefault();

            return result;
        }
    }
}

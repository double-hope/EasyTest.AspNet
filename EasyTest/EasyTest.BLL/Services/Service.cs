using AutoMapper;
using EasyTest.DAL;

namespace EasyTest.BLL.Services
{
	public abstract class Service
	{
		private protected readonly ApplicationDbContext _context;
		private protected readonly IMapper _mapper;

		public Service(ApplicationDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}
	}
}

using AutoMapper;
using EasyTest.DAL;
using EasyTest.DAL.Repository.IRepository;

namespace EasyTest.BLL.Services
{
	public abstract class Service
	{
		private protected readonly IUnitOfWork _unitOfWork;
		private protected readonly IMapper _mapper;

		public Service(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}
	}
}

using AutoMapper;
using EasyTest.DAL.Repository.IRepository;
using EasyTest.Shared.Constants;
using EasyTest.Shared.DTO.Response;

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

		public Response SuccessResponse<T>(T data, string message)
		{
            return new Response
            {
                Status = ResponseStatusCodes.Success,
                Message = message,
				Data = data
            };
        }

        public Response ErrorResponse(string message, List<string>? errors = null)
        {
			return new Response
			{
				Status = ResponseStatusCodes.Error,
				Message = message,
				Errors = errors
			};
        }
    }
}
